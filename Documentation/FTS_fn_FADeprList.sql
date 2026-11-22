/* =====================================================================================
   FTS_fn_FADeprList - assets to include in Run Depreciation (DeprnWidget)
   -------------------------------------------------------------------------------------
   31 parameters, in the same order ButtonController.RunDeprn_Execute passes them:
     DateTo, F/A Code from/to, F/A Group from/to, F/A Class from/to,
     Project from/to, Profit Centre (Dim 1) from/to, Dim 2..5 from/to,
     Indicator 1..6 from/to (indicator CODE, FTS11OIND.U_IndCode)
   Empty string ('') = no filter for that field.
   A "from" or "to" value filters on its own (from only = ">= from", to only = "<= to").
   When a filter is used, assets with that field blank are excluded.

   Run on EVERY company FA database. Safe to run more than once.
   ===================================================================================== */

IF OBJECT_ID('dbo.FTS_fn_FADeprList') IS NULL
    EXEC ('CREATE FUNCTION dbo.FTS_fn_FADeprList() RETURNS TABLE AS RETURN (SELECT 0 AS FAKey, N'''' AS U_FACode)');
GO

ALTER FUNCTION [dbo].[FTS_fn_FADeprList]
(
    @DateTo        DATETIME,
    @FACodFrm      NVARCHAR(100),
    @FACodTo       NVARCHAR(100),
    @FAGrpFrm      NVARCHAR(100),
    @FAGrpTo       NVARCHAR(100),
    @FAClsFrm      NVARCHAR(100),
    @FAClsTo       NVARCHAR(100),

    -- Project / Dimensions
    @ProjectFrm    NVARCHAR(100),
    @ProjectTo     NVARCHAR(100),
    @ProfitCodeFrm NVARCHAR(100),
    @ProfitCodeTo  NVARCHAR(100),
    @OcrCode2Frm   NVARCHAR(100),
    @OcrCode2To    NVARCHAR(100),
    @OcrCode3Frm   NVARCHAR(100),
    @OcrCode3To    NVARCHAR(100),
    @OcrCode4Frm   NVARCHAR(100),
    @OcrCode4To    NVARCHAR(100),
    @OcrCode5Frm   NVARCHAR(100),
    @OcrCode5To    NVARCHAR(100),

    -- Indicator keys (indicator code)
    @IndKey1Frm    NVARCHAR(100),
    @IndKey1To     NVARCHAR(100),
    @IndKey2Frm    NVARCHAR(100),
    @IndKey2To     NVARCHAR(100),
    @IndKey3Frm    NVARCHAR(100),
    @IndKey3To     NVARCHAR(100),
    @IndKey4Frm    NVARCHAR(100),
    @IndKey4To     NVARCHAR(100),
    @IndKey5Frm    NVARCHAR(100),
    @IndKey5To     NVARCHAR(100),
    @IndKey6Frm    NVARCHAR(100),
    @IndKey6To     NVARCHAR(100)
)
RETURNS TABLE
AS
RETURN
(
    SELECT T0.DocEntry AS [FAKey], T0.U_FACode
    FROM FTS11OFAM T0
    INNER JOIN FTS11OFAG T1 ON T0.U_FAGrpKey = T1.DocEntry
    LEFT OUTER JOIN FTS11OIND I1 ON I1.DocEntry = T0.U_IndKey1
    LEFT OUTER JOIN FTS11OIND I2 ON I2.DocEntry = T0.U_IndKey2
    LEFT OUTER JOIN FTS11OIND I3 ON I3.DocEntry = T0.U_IndKey3
    LEFT OUTER JOIN FTS11OIND I4 ON I4.DocEntry = T0.U_IndKey4
    LEFT OUTER JOIN FTS11OIND I5 ON I5.DocEntry = T0.U_IndKey5
    LEFT OUTER JOIN FTS11OIND I6 ON I6.DocEntry = T0.U_IndKey6
    WHERE (T0.U_EffDate <= @DateTo OR @DateTo = '19000101')
    AND (T0.U_FACode   >= @FACodFrm OR @FACodFrm = '') AND (T0.U_FACode   <= @FACodTo OR @FACodTo = '')
    AND (T0.U_FAGrpCod >= @FAGrpFrm OR @FAGrpFrm = '') AND (T0.U_FAGrpCod <= @FAGrpTo OR @FAGrpTo = '')
    AND (T1.U_FAClsCod >= @FAClsFrm OR @FAClsFrm = '') AND (T1.U_FAClsCod <= @FAClsTo OR @FAClsTo = '')

    -- Project / Dimensions
    AND (T0.U_Project    >= @ProjectFrm    OR @ProjectFrm    = '') AND (T0.U_Project    <= @ProjectTo    OR @ProjectTo    = '')
    AND (T0.U_ProfitCode >= @ProfitCodeFrm OR @ProfitCodeFrm = '') AND (T0.U_ProfitCode <= @ProfitCodeTo OR @ProfitCodeTo = '')
    AND (T0.U_OcrCode2   >= @OcrCode2Frm   OR @OcrCode2Frm   = '') AND (T0.U_OcrCode2   <= @OcrCode2To   OR @OcrCode2To   = '')
    AND (T0.U_OcrCode3   >= @OcrCode3Frm   OR @OcrCode3Frm   = '') AND (T0.U_OcrCode3   <= @OcrCode3To   OR @OcrCode3To   = '')
    AND (T0.U_OcrCode4   >= @OcrCode4Frm   OR @OcrCode4Frm   = '') AND (T0.U_OcrCode4   <= @OcrCode4To   OR @OcrCode4To   = '')
    AND (T0.U_OcrCode5   >= @OcrCode5Frm   OR @OcrCode5Frm   = '') AND (T0.U_OcrCode5   <= @OcrCode5To   OR @OcrCode5To   = '')

    -- Indicator keys
    AND (I1.U_IndCode >= @IndKey1Frm OR @IndKey1Frm = '') AND (I1.U_IndCode <= @IndKey1To OR @IndKey1To = '')
    AND (I2.U_IndCode >= @IndKey2Frm OR @IndKey2Frm = '') AND (I2.U_IndCode <= @IndKey2To OR @IndKey2To = '')
    AND (I3.U_IndCode >= @IndKey3Frm OR @IndKey3Frm = '') AND (I3.U_IndCode <= @IndKey3To OR @IndKey3To = '')
    AND (I4.U_IndCode >= @IndKey4Frm OR @IndKey4Frm = '') AND (I4.U_IndCode <= @IndKey4To OR @IndKey4To = '')
    AND (I5.U_IndCode >= @IndKey5Frm OR @IndKey5Frm = '') AND (I5.U_IndCode <= @IndKey5To OR @IndKey5To = '')
    AND (I6.U_IndCode >= @IndKey6Frm OR @IndKey6Frm = '') AND (I6.U_IndCode <= @IndKey6To OR @IndKey6To = '')

    AND T0.SQuantity > 0 AND T0.SNBVLC >= 0
)
GO
