/* =====================================================================================
   Depreciation Run Log (FTS11ODRL) - "block posting into the month being depreciated"
   -------------------------------------------------------------------------------------
   One row per Run Depreciation: period, progress, who, when, result.
   While a row is Running, the app blocks posting / cancelling documents dated inside
   PrdFrom..PrdTo, data import of assets/transactions, and a second depreciation run.

   The app also creates this table automatically on the first Run Depreciation,
   so running this script is optional (use it if the app login has no CREATE TABLE right).
   Safe to run more than once.

   Status: R Running, F Finished, E Error/stopped, X Interrupted (no progress for 10 min)
   ===================================================================================== */

IF OBJECT_ID('dbo.FTS11ODRL') IS NULL
BEGIN
    CREATE TABLE dbo.FTS11ODRL (
        DocEntry  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_FTS11ODRL PRIMARY KEY,
        DateFrm   DATETIME NOT NULL,
        DateTo    DATETIME NOT NULL,
        PrdFrom   DATETIME NOT NULL,
        PrdTo     DATETIME NOT NULL,
        Status    NVARCHAR(1) NOT NULL,
        Total     INT NOT NULL DEFAULT 0,
        Done      INT NOT NULL DEFAULT 0,
        Generated INT NOT NULL DEFAULT 0,
        Skipped   INT NOT NULL DEFAULT 0,
        ErrCnt    INT NOT NULL DEFAULT 0,
        StartBy   NVARCHAR(50) NULL,
        StartDate DATETIME NOT NULL,
        LastBeat  DATETIME NOT NULL,
        EndDate   DATETIME NULL,
        Note      NVARCHAR(254) NULL
    );
    CREATE INDEX iStatus_FTS11ODRL ON dbo.FTS11ODRL (Status);
    PRINT 'FTS11ODRL created on ' + DB_NAME();
END
ELSE
    PRINT 'FTS11ODRL already exists on ' + DB_NAME();

/* ---- Useful queries ----

-- Run history
SELECT DocEntry, PrdFrom, PrdTo, Status, Total, Done, Generated, Skipped, ErrCnt, StartBy, StartDate, EndDate, Note
FROM dbo.FTS11ODRL ORDER BY DocEntry DESC;

-- Release a stuck run manually (normally not needed: it expires after 10 minutes without progress)
UPDATE dbo.FTS11ODRL SET Status = 'X', EndDate = GETDATE(), Note = 'Released manually' WHERE Status = 'R';
*/
