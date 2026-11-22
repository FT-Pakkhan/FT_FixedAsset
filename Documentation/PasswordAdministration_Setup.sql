/* ==========================================================================
   Password Administration - database setup (run once in EVERY company DB:
   FA_HTH, FA_HTH_Test, FA_APM, FA_BD, FA_HCSB, FA_HSSB, FA_HTVB ...)
   Safe to run more than once. Adds:
     - FTS11OPPA  : Password Administration settings (1 row, 0 = rule off)
     - FTS11OUPH  : password history (salted hashes)
     - SystemUsers: LastPwdChgOn, IsLocked, LockedDate, FailedLogonCount
   ========================================================================== */
SET NOCOUNT ON;

IF OBJECT_ID('dbo.FTS11OPPA') IS NULL
BEGIN
    CREATE TABLE dbo.FTS11OPPA (
        DocEntry        int IDENTITY(1,1) NOT NULL CONSTRAINT PK_FTS11OPPA PRIMARY KEY,
        PwdExp          numeric(19,6) NULL,
        PwdMinLen       numeric(19,6) NULL,
        MinUppers       numeric(19,6) NULL,
        MinLowCase      numeric(19,6) NULL,
        MinDigits       numeric(19,6) NULL,
        MinNonAlph      numeric(19,6) NULL,
        NumPrevPwd      numeric(19,6) NULL,
        NumAuthLoc      numeric(19,6) NULL,
        AccLocDuration  numeric(19,6) NULL,
        CreateBy        nvarchar(100) NULL,
        CreateDate      datetime NULL,
        UpdateBy        nvarchar(100) NULL,
        UpdateDate      datetime NULL,
        [Object]        nvarchar(100) NULL
    );
END;

-- the single settings row (all rules off until the administrator sets them)
IF NOT EXISTS (SELECT 1 FROM dbo.FTS11OPPA)
    INSERT INTO dbo.FTS11OPPA (PwdExp, PwdMinLen, MinUppers, MinLowCase, MinDigits, MinNonAlph, NumPrevPwd, NumAuthLoc, AccLocDuration,
                               CreateBy, CreateDate, UpdateBy, UpdateDate, [Object])
    VALUES (0, 0, 0, 0, 0, 0, 0, 0, 30, 'sa', GETDATE(), 'sa', GETDATE(), 'FTS11OPPA');

IF OBJECT_ID('dbo.FTS11OUPH') IS NULL
BEGIN
    CREATE TABLE dbo.FTS11OUPH (
        DocEntry   int IDENTITY(1,1) NOT NULL CONSTRAINT PK_FTS11OUPH PRIMARY KEY,
        UserOid    uniqueidentifier NULL,
        PrevPwd    nvarchar(200) NULL,
        ChangedOn  datetime NULL
    );
    CREATE INDEX iUserOid_FTS11OUPH ON dbo.FTS11OUPH (UserOid);
END;

IF COL_LENGTH('dbo.SystemUsers', 'LastPwdChgOn') IS NULL     ALTER TABLE dbo.SystemUsers ADD LastPwdChgOn datetime NULL;
IF COL_LENGTH('dbo.SystemUsers', 'IsLocked') IS NULL         ALTER TABLE dbo.SystemUsers ADD IsLocked bit NULL;
IF COL_LENGTH('dbo.SystemUsers', 'LockedDate') IS NULL       ALTER TABLE dbo.SystemUsers ADD LockedDate datetime NULL;
IF COL_LENGTH('dbo.SystemUsers', 'FailedLogonCount') IS NULL ALTER TABLE dbo.SystemUsers ADD FailedLogonCount int NULL;

PRINT DB_NAME() + ': Password Administration ready.';
