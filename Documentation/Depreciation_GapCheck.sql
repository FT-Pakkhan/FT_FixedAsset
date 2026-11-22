/* =====================================================================================
   Depreciation gap check  (READ-ONLY - changes nothing)
   -------------------------------------------------------------------------------------
   Lists open, depreciating assets that have a posting period WITHOUT depreciation
   although the asset should have been depreciated in that period.

   Same rules as sp_FTS11_GenerateDeprn:
     - periods from the asset's effective date (first period starting on/after U_EffDate)
       up to @Before (exclusive)
     - a period counts as depreciated if a non-cancelled DEP journal (T9901/T9902) exists on its last day
     - a period is NOT expected (so not a gap) when, at that period:
         * quantity is zero, or
         * depreciation count already reached the useful life (fully depreciated), or
         * NBV is already at / below the minimum NBV (U_MBaseLC)

   Set @Before to the first day of the month you are about to run
   (e.g. '2026-11-01' before running November 2026).
   Run on each company FA database.
   ===================================================================================== */

SET NOCOUNT ON;

DECLARE @Before DATETIME = '2026-11-01';    -- <<< first day of the next run month

-- open, depreciating assets
SELECT T0.DocEntry AS FAKey, T0.U_FACode, T0.U_FAName, T0.U_EffDate, ISNULL(T0.U_MBaseLC, 0) AS MBaseLC
INTO #A
FROM FTS11OFAM T0
WHERE T0.U_Status = 'O' AND T0.U_DeprnYN = 'True';

-- periods each asset should have been depreciated in (same period rule as the procedure's check)
SELECT A.FAKey, P.T_RefDate
INTO #AP
FROM #A A
INNER JOIN FTS11_vw_OFPR P ON P.F_RefDate >= A.U_EffDate AND P.T_RefDate < @Before;

-- periods without a depreciation entry
SELECT AP.FAKey, AP.T_RefDate
INTO #M
FROM #AP AP
WHERE NOT EXISTS (SELECT 1 FROM FTS11JDT1 J INNER JOIN FTS11STRN S ON J.TransType = S.Code AND S.U_TrnsGrp = 'DEP'
                  WHERE J.FAKey = AP.FAKey AND J.DocDate = AP.T_RefDate AND J.TransStatus <> 'C');

CREATE CLUSTERED INDEX cx ON #M (FAKey, T_RefDate);

-- journal lines of those assets only
SELECT J.FAKey, J.DocDate, J.AcctCat, ISNULL(J.Quantity, 0) AS Quantity, ISNULL(J.UseLife, 0) AS UseLife,
       ISNULL(J.DeprnCnt, 0) AS DeprnCnt, ISNULL(J.AmtLC, 0) AS AmtLC
INTO #J
FROM FTS11JDT1 J
WHERE J.TransStatus <> 'C' AND J.AcctCat IN ('COST', 'ACCU')
  AND J.FAKey IN (SELECT DISTINCT FAKey FROM #M);

CREATE CLUSTERED INDEX cx ON #J (FAKey, DocDate);

-- keep only the periods where the asset was still depreciable (a real gap)
SELECT M.FAKey, M.T_RefDate
INTO #G
FROM #M M
CROSS APPLY (
    SELECT SUM(V.Qty) AS Qty, SUM(V.UseLife) AS UseLife, SUM(V.DeprnCnt) AS DeprnCnt, SUM(V.NBV) AS NBV
    FROM (
        SELECT CASE WHEN J.AcctCat = 'COST' THEN J.Quantity ELSE 0 END                                    AS Qty,
               CASE WHEN J.AcctCat = 'COST' THEN J.UseLife ELSE 0 END                                     AS UseLife,
               CASE WHEN J.AcctCat = 'ACCU' AND J.DocDate < M.T_RefDate THEN J.DeprnCnt ELSE 0 END        AS DeprnCnt,
               CASE WHEN J.AcctCat = 'COST' THEN J.AmtLC
                    WHEN J.AcctCat = 'ACCU' AND J.DocDate < M.T_RefDate THEN J.AmtLC ELSE 0 END           AS NBV
        FROM #J J
        WHERE J.FAKey = M.FAKey AND J.DocDate <= M.T_RefDate
    ) V
) X
INNER JOIN #A A ON A.FAKey = M.FAKey
WHERE X.Qty > 0 AND X.DeprnCnt < X.UseLife AND X.NBV > A.MBaseLC;

-- 1) summary
SELECT COUNT(DISTINCT FAKey) AS AssetsWithGaps, COUNT(*) AS MissingPeriods FROM #G;

-- 2) per asset
SELECT A.U_FACode AS [F/A Code], A.U_FAName AS [F/A Name], CONVERT(VARCHAR(10), A.U_EffDate, 103) AS [Effective Date],
       CONVERT(VARCHAR(10), MIN(G.T_RefDate), 103) AS [First Missing], CONVERT(VARCHAR(10), MAX(G.T_RefDate), 103) AS [Last Missing],
       COUNT(*) AS [Missing Periods]
FROM #G G INNER JOIN #A A ON A.FAKey = G.FAKey
GROUP BY A.U_FACode, A.U_FAName, A.U_EffDate
ORDER BY MIN(G.T_RefDate), A.U_FACode;

DROP TABLE #A, #AP, #M, #J, #G;
