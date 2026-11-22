/* =====================================================================================
   Depreciation speed-up  (run on EVERY company FA database, e.g. FA_HTH, FA_APM, ...)
   -------------------------------------------------------------------------------------
   Why it is slow:
     FTS11JDT1 (F/A journal) has NO index. sp_FTS11_GenerateDeprn reads FTS11JDT1 about
     15 times for every asset x every month, and each read scans the whole table.
     The table grows every month, so the run gets slower every month.

   What this does:
     Adds 2 indexes only. No table data, no procedure and no calculation is changed,
     so the depreciation amounts are exactly the same (verified: same rows, same checksum).

   Safe to run more than once. Can be undone with the DROP lines at the bottom.
   Build time: a few seconds (even with ~1M journal rows). Best run outside office hours.
   ===================================================================================== */

SET NOCOUNT ON;

-- 1) Per-asset lookups: cost / accumulated depreciation / quantity / location / deprn count
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.FTS11JDT1') AND name = 'IX_FTS11JDT1_FAKey')
    CREATE NONCLUSTERED INDEX IX_FTS11JDT1_FAKey
        ON dbo.FTS11JDT1 (FAKey, AcctCat, DocDate)
        INCLUDE (TransStatus, TransType, AmtLC, AmtSC, Quantity, UseLife, DeprnCnt, Location, ReconKey, DocEntry);

-- 2) "Next DocEntry" = MAX(DocEntry)+1 for every asset-month
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.FTS11JDT1') AND name = 'IX_FTS11JDT1_DocEntry')
    CREATE NONCLUSTERED INDEX IX_FTS11JDT1_DocEntry
        ON dbo.FTS11JDT1 (DocEntry);

PRINT 'Depreciation indexes OK on ' + DB_NAME();

/* ---- Undo (only if ever needed) ----
DROP INDEX IX_FTS11JDT1_FAKey    ON dbo.FTS11JDT1;
DROP INDEX IX_FTS11JDT1_DocEntry ON dbo.FTS11JDT1;
*/
