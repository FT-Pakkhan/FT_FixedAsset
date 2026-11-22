/* =====================================================================================
   Check whether Depreciation_SpeedUp.sql has been run
   -------------------------------------------------------------------------------------
   Read-only - changes nothing.
   Looks for the 2 indexes on FTS11JDT1 that Depreciation_SpeedUp.sql creates:
     IX_FTS11JDT1_FAKey, IX_FTS11JDT1_DocEntry

   IndexesFound = 2  -> speed-up already applied
   IndexesFound = 1  -> only partly applied (run Depreciation_SpeedUp.sql again, it is safe)
   IndexesFound = 0  -> not applied yet
   ===================================================================================== */

SET NOCOUNT ON;

/* ---------- Part 1: current database only ---------- */
SELECT DB_NAME() AS DatabaseName,
       COUNT(*)  AS IndexesFound,
       CASE COUNT(*) WHEN 2 THEN 'Applied' WHEN 0 THEN 'Not applied' ELSE 'Partly applied - run script again' END AS Status
FROM sys.indexes
WHERE object_id = OBJECT_ID('dbo.FTS11JDT1')
  AND name IN ('IX_FTS11JDT1_FAKey', 'IX_FTS11JDT1_DocEntry');


/* ---------- Part 2: every FA_ database on this server ---------- */
DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += CASE WHEN @sql = N'' THEN N'' ELSE N' UNION ALL ' END + N'
SELECT N''' + REPLACE(name, '''', '''''') + N''' AS DatabaseName,
       CASE WHEN OBJECT_ID(N''' + QUOTENAME(name) + N'.dbo.FTS11JDT1'') IS NULL THEN NULL
            ELSE (SELECT COUNT(*) FROM ' + QUOTENAME(name) + N'.sys.indexes
                  WHERE object_id = OBJECT_ID(N''' + QUOTENAME(name) + N'.dbo.FTS11JDT1'')
                    AND name IN (N''IX_FTS11JDT1_FAKey'', N''IX_FTS11JDT1_DocEntry'')) END AS IndexesFound'
FROM sys.databases
WHERE name LIKE 'FA[_]%'
  AND state_desc = 'ONLINE'
  AND HAS_DBACCESS(name) = 1;

IF @sql = N''
    PRINT 'No FA_ databases found on this server.';
ELSE
BEGIN
    SET @sql = N'SELECT DatabaseName, IndexesFound,
       CASE WHEN IndexesFound IS NULL THEN ''No FTS11JDT1 table''
            WHEN IndexesFound = 2 THEN ''Applied''
            WHEN IndexesFound = 0 THEN ''Not applied''
            ELSE ''Partly applied - run script again'' END AS Status
FROM (' + @sql + N') X
ORDER BY DatabaseName;';
    EXEC (@sql);
END
