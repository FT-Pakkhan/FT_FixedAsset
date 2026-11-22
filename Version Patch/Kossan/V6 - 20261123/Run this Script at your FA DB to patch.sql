/* =====================================================================================
   Fast Track Fixed Asset - KOSSAN Version Patch V6 (2026-11-23)
   -------------------------------------------------------------------------------------
   FOR KOSSAN ONLY (Kossan program). Standard clients use Version Patch\Fast Track instead.
   Run this whole script ONCE on EVERY Kossan company FA database, NOT on FA_COMMON.
   In SSMS: select the company database, open this file, press Execute (F5).
   Safe to run more than once (every part checks first or uses ALTER).

   Contents:
     0. Items from earlier versions, only if missing     (Company Setup columns, audit columns,
                                                          history tables, SBO_SP_PostTransactionNotice,
                                                          sp_FTS11_RegenerateView, SE_FASvrTyp)
     1. Password Administration tables / columns          (FTS11OPPA, FTS11OUPH, SystemUsers)
     2. Depreciation speed-up indexes                     (FTS11JDT1)
     3. Depreciation run log / progress / error list      (FTS11ODRL, FTS11DRL1)
     4. Run Depreciation asset list with all filters      (FTS_fn_FADeprList, 31 parameters)
     5. Posting Widget Project / Cost Centre D1-D5 filter (sp_FTS11_PrepareJEPreview)
     6. Corrected "Missing depreciation" check            (sp_FTS11_GenerateDeprn)
     7. Reset Database also clears the run log            (sp_FTS11_ResetDatabase, 2 lines added)
     8. Kossan captions in Company Setup                  (Project Code, Department, Area Code)

   BEFORE running:
     - Back up the database.
     - Run "Optional - Check missing depreciation before patch (read-only).sql" and fill
       any real gaps first (see Readme.txt). After part 6, an asset with a gap stops with
       "Missing depreciation on ..." until the gap is filled.
     - Parts 5 and 6 replace the whole procedure with the standard Fast Track version.
       If this client has a customised sp_FTS11_PrepareJEPreview or sp_FTS11_GenerateDeprn,
       compare first and merge by hand.
   ===================================================================================== */

SET NOCOUNT ON;
IF OBJECT_ID('dbo.FTS11JDT1') IS NULL OR OBJECT_ID('dbo.FTS11OFAM') IS NULL
BEGIN
    RAISERROR('This is not a Fixed Asset company database (FTS11JDT1 / FTS11OFAM not found). Select the company FA database and run again. Nothing was changed.', 16, 1);
    SET NOEXEC ON;
END
GO
PRINT 'Patching ' + DB_NAME() + ' ...';
GO

/* #####################################################################################
   0. Items from earlier versions (V5 and before) - only added if MISSING
   ##################################################################################### */
/* =====================================================================================
   A database that is fully on V5 already has everything below, and this part changes
   nothing there. An older company database (found on FA_NewIQ, 22/11/2026) misses these;
   without them the V5 / V6 program stops at log in with "Connection Error".
     0a. FTS11OADM  : 13 Company Setup columns (Disposal / Depreciation options, Qty decimals,
                      Cost Centre captions, Ref 1 / Ref 2)
     0b. FTS11FAG1, FTS11FAM2 : Created / Updated / Cancelled by + date, LogInst
     0c. FTS11AFAG1, FTS11AFAM2 : history tables (used by the standard procedures)
     0d. SBO_SP_PostTransactionNotice : only if it still has the old "INSERT INTO FTS11FAM2"
                      without a column list (that line fails once 0b adds the columns).
                      It is then replaced by the standard Fast Track version.
     0e. sp_FTS11_RegenerateView : G/L account list = all postable accounts (old version
                      only showed Level 5 accounts). Only the account filter is changed.
     0f. SE_FASvrTyp : adds "MSSQL 2022" to the SQL Server Type list.
   Client-specific objects (report functions, SE_FARptGrpOpt captions, extra tables) are
   NOT touched.
   ===================================================================================== */
SET NOCOUNT ON;

-- 0a. Company Setup columns
IF COL_LENGTH('dbo.FTS11OADM','U_ApportDepr') IS NULL   ALTER TABLE dbo.FTS11OADM ADD U_ApportDepr bit NULL;
IF COL_LENGTH('dbo.FTS11OADM','U_BFByNBV') IS NULL      ALTER TABLE dbo.FTS11OADM ADD U_BFByNBV bit NULL;
IF COL_LENGTH('dbo.FTS11OADM','U_DisFACode') IS NULL    ALTER TABLE dbo.FTS11OADM ADD U_DisFACode bit NULL;
IF COL_LENGTH('dbo.FTS11OADM','U_DisposeCost') IS NULL  ALTER TABLE dbo.FTS11OADM ADD U_DisposeCost bit NULL;
IF COL_LENGTH('dbo.FTS11OADM','U_DisposeWoQty') IS NULL ALTER TABLE dbo.FTS11OADM ADD U_DisposeWoQty bit NULL;
IF COL_LENGTH('dbo.FTS11OADM','U_QtyDec') IS NULL       ALTER TABLE dbo.FTS11OADM ADD U_QtyDec numeric(19,6) NULL;
IF COL_LENGTH('dbo.FTS11OADM','U_Ocr1Capt') IS NULL     ALTER TABLE dbo.FTS11OADM ADD U_Ocr1Capt nvarchar(100) NULL;
IF COL_LENGTH('dbo.FTS11OADM','U_Ocr2Capt') IS NULL     ALTER TABLE dbo.FTS11OADM ADD U_Ocr2Capt nvarchar(100) NULL;
IF COL_LENGTH('dbo.FTS11OADM','U_Ocr3Capt') IS NULL     ALTER TABLE dbo.FTS11OADM ADD U_Ocr3Capt nvarchar(100) NULL;
IF COL_LENGTH('dbo.FTS11OADM','U_Ocr4Capt') IS NULL     ALTER TABLE dbo.FTS11OADM ADD U_Ocr4Capt nvarchar(100) NULL;
IF COL_LENGTH('dbo.FTS11OADM','U_Ocr5Capt') IS NULL     ALTER TABLE dbo.FTS11OADM ADD U_Ocr5Capt nvarchar(100) NULL;
IF COL_LENGTH('dbo.FTS11OADM','U_Ref1') IS NULL         ALTER TABLE dbo.FTS11OADM ADD U_Ref1 nvarchar(100) NULL;
IF COL_LENGTH('dbo.FTS11OADM','U_Ref2') IS NULL         ALTER TABLE dbo.FTS11OADM ADD U_Ref2 nvarchar(100) NULL;
GO
-- defaults for the new settings = same behaviour as before (options off, 0 qty decimals)
UPDATE dbo.FTS11OADM SET
    U_ApportDepr   = ISNULL(U_ApportDepr, 0),
    U_BFByNBV      = ISNULL(U_BFByNBV, 0),
    U_DisFACode    = ISNULL(U_DisFACode, 0),
    U_DisposeCost  = ISNULL(U_DisposeCost, 0),
    U_DisposeWoQty = ISNULL(U_DisposeWoQty, 0),
    U_QtyDec       = ISNULL(U_QtyDec, 0),
    U_Ocr1Capt     = ISNULL(NULLIF(U_Ocr1Capt, ''), 'Cost Centre D1'),
    U_Ocr2Capt     = ISNULL(NULLIF(U_Ocr2Capt, ''), 'Cost Centre D2'),
    U_Ocr3Capt     = ISNULL(NULLIF(U_Ocr3Capt, ''), 'Cost Centre D3'),
    U_Ocr4Capt     = ISNULL(NULLIF(U_Ocr4Capt, ''), 'Cost Centre D4'),
    U_Ocr5Capt     = ISNULL(NULLIF(U_Ocr5Capt, ''), 'Cost Centre D5')
WHERE U_ApportDepr IS NULL OR U_BFByNBV IS NULL OR U_DisFACode IS NULL OR U_DisposeCost IS NULL OR U_DisposeWoQty IS NULL
   OR U_QtyDec IS NULL OR ISNULL(U_Ocr1Capt,'') = '' OR ISNULL(U_Ocr2Capt,'') = '' OR ISNULL(U_Ocr3Capt,'') = ''
   OR ISNULL(U_Ocr4Capt,'') = '' OR ISNULL(U_Ocr5Capt,'') = '';
GO

-- 0b. Audit columns on the F/A Group and F/A Master G/L account lines
IF COL_LENGTH('dbo.FTS11FAG1','CreateDate') IS NULL ALTER TABLE dbo.FTS11FAG1 ADD CreateDate datetime NULL;
IF COL_LENGTH('dbo.FTS11FAG1','UpdateDate') IS NULL ALTER TABLE dbo.FTS11FAG1 ADD UpdateDate datetime NULL;
IF COL_LENGTH('dbo.FTS11FAG1','U_CreateBy') IS NULL ALTER TABLE dbo.FTS11FAG1 ADD U_CreateBy nvarchar(100) NULL;
IF COL_LENGTH('dbo.FTS11FAG1','U_UpdateBy') IS NULL ALTER TABLE dbo.FTS11FAG1 ADD U_UpdateBy nvarchar(100) NULL;
IF COL_LENGTH('dbo.FTS11FAG1','LogInst') IS NULL    ALTER TABLE dbo.FTS11FAG1 ADD LogInst numeric(19,6) NULL;
IF COL_LENGTH('dbo.FTS11FAG1','U_CnclBy') IS NULL   ALTER TABLE dbo.FTS11FAG1 ADD U_CnclBy nvarchar(100) NULL;
IF COL_LENGTH('dbo.FTS11FAG1','U_CnclDate') IS NULL ALTER TABLE dbo.FTS11FAG1 ADD U_CnclDate datetime NULL;
IF COL_LENGTH('dbo.FTS11FAM2','CreateDate') IS NULL ALTER TABLE dbo.FTS11FAM2 ADD CreateDate datetime NULL;
IF COL_LENGTH('dbo.FTS11FAM2','UpdateDate') IS NULL ALTER TABLE dbo.FTS11FAM2 ADD UpdateDate datetime NULL;
IF COL_LENGTH('dbo.FTS11FAM2','U_CreateBy') IS NULL ALTER TABLE dbo.FTS11FAM2 ADD U_CreateBy nvarchar(100) NULL;
IF COL_LENGTH('dbo.FTS11FAM2','U_UpdateBy') IS NULL ALTER TABLE dbo.FTS11FAM2 ADD U_UpdateBy nvarchar(100) NULL;
IF COL_LENGTH('dbo.FTS11FAM2','LogInst') IS NULL    ALTER TABLE dbo.FTS11FAM2 ADD LogInst numeric(19,6) NULL;
IF COL_LENGTH('dbo.FTS11FAM2','U_CnclBy') IS NULL   ALTER TABLE dbo.FTS11FAM2 ADD U_CnclBy nvarchar(100) NULL;
IF COL_LENGTH('dbo.FTS11FAM2','U_CnclDate') IS NULL ALTER TABLE dbo.FTS11FAM2 ADD U_CnclDate datetime NULL;
GO

-- 0c. History tables
IF OBJECT_ID('dbo.FTS11AFAG1') IS NULL
    CREATE TABLE dbo.FTS11AFAG1 (
        DocEntry int NULL, [Object] nvarchar(100) NULL, U_FAGrpKey int NULL, U_LocationKey int NULL, U_Location nvarchar(100) NULL,
        U_AccCodA nvarchar(100) NULL, U_AccFrmA nvarchar(100) NULL, U_AccNamA nvarchar(100) NULL,
        U_AccCodD nvarchar(100) NULL, U_AccFrmD nvarchar(100) NULL, U_AccNamD nvarchar(100) NULL,
        U_AccCodC nvarchar(100) NULL, U_AccFrmC nvarchar(100) NULL, U_AccNamC nvarchar(100) NULL,
        U_AccCodW nvarchar(100) NULL, U_AccFrmW nvarchar(100) NULL, U_AccNamW nvarchar(100) NULL,
        U_AccCodQ nvarchar(100) NULL, U_AccFrmQ nvarchar(100) NULL, U_AccNamQ nvarchar(100) NULL,
        CreateDate datetime NULL, UpdateDate datetime NULL, U_CreateBy nvarchar(100) NULL, U_UpdateBy nvarchar(100) NULL,
        LogInst numeric(19,6) NULL, U_CnclBy nvarchar(100) NULL, U_CnclDate datetime NULL);

IF OBJECT_ID('dbo.FTS11AFAM2') IS NULL
    CREATE TABLE dbo.FTS11AFAM2 (
        DocEntry int NULL, [Object] nvarchar(100) NULL, U_FAKey int NULL, U_LocationKey int NULL, U_Location nvarchar(100) NULL,
        U_AccCodA nvarchar(100) NULL, U_AccFrmA nvarchar(100) NULL, U_AccNamA nvarchar(100) NULL,
        U_AccCodD nvarchar(100) NULL, U_AccFrmD nvarchar(100) NULL, U_AccNamD nvarchar(100) NULL,
        U_AccCodC nvarchar(100) NULL, U_AccFrmC nvarchar(100) NULL, U_AccNamC nvarchar(100) NULL,
        U_AccCodW nvarchar(100) NULL, U_AccFrmW nvarchar(100) NULL, U_AccNamW nvarchar(100) NULL,
        U_AccCodQ nvarchar(100) NULL, U_AccFrmQ nvarchar(100) NULL, U_AccNamQ nvarchar(100) NULL,
        U_Project nvarchar(100) NULL, U_ProfitCode nvarchar(100) NULL, U_OcrCode2 nvarchar(100) NULL, U_OcrCode3 nvarchar(100) NULL,
        U_OcrCode4 nvarchar(100) NULL, U_OcrCode5 nvarchar(100) NULL,
        U_IndKey1 int NULL, U_IndCod1 nvarchar(100) NULL, U_IndKey2 int NULL, U_IndCod2 nvarchar(100) NULL,
        U_IndKey3 int NULL, U_IndCod3 nvarchar(100) NULL, U_IndKey4 int NULL, U_IndCod4 nvarchar(100) NULL,
        U_IndKey5 int NULL, U_IndCod5 nvarchar(100) NULL, U_IndKey6 int NULL, U_IndCod6 nvarchar(100) NULL,
        CreateDate datetime NULL, UpdateDate datetime NULL, U_CreateBy nvarchar(100) NULL, U_UpdateBy nvarchar(100) NULL,
        LogInst numeric(19,6) NULL, U_CnclBy nvarchar(100) NULL, U_CnclDate datetime NULL);
GO

-- 0d. SBO_SP_PostTransactionNotice - only when it still has the old INSERT without a column list
IF REPLACE(REPLACE(REPLACE(REPLACE(OBJECT_DEFINITION(OBJECT_ID('dbo.SBO_SP_PostTransactionNotice')),' ',''),CHAR(9),''),CHAR(13),''),CHAR(10),'')
   LIKE '%INSERTINTOFTS11FAM2SELECT%'
BEGIN
    EXEC (N'/*

	- Correction to SBO_SP_PostTransactionNotice to copy FTS11FAG1 to FTS11FAM2 when add Fixed Asset Master
	- Modified Fixed Asset Transaction Listing - Move report format
	- Update Audit Check
	- Updated sp_FTS11_ReverseJDT1 to set Reversed Document status
*/
ALTER PROCEDURE [dbo].[SBO_SP_PostTransactionNotice]
@object_type nvarchar(20), 				-- SBO Object Type
@transaction_type nchar(1),			-- [A]dd, [U]pdate, [D]elete, [C]ancel, C[L]ose
@num_of_cols_in_key int,
@list_of_key_cols_tab_del nvarchar(255),
@list_of_cols_val_tab_del nvarchar(255)
AS
begin

--DECLARE @object_type nvarchar(20)
--DECLARE @transaction_type nchar(1)
--DECLARE @num_of_cols_in_key int
--DECLARE @list_of_key_cols_tab_del nvarchar(255)
--DECLARE @list_of_cols_val_tab_del nvarchar(255)
--SELECT @object_type = ''FTS11OFAM'', @transaction_type = ''A'', @num_of_cols_in_key = 1, @list_of_key_cols_tab_del = ''DocEntry'', @list_of_cols_val_tab_del = 5194

/*

	EXEC SBO_SP_PostTransactionNotice ''FTS11OFAM'', ''A'',1,''DocEntry'',5194
	Patch:
	2012-10-09	Include sp_FTS11_ResetOFAM(TRANSCNT) to reset TransCnt in FTS11OFAM
	2012-10-13	Delete FTS11JDT1 when FTS11OFAM delete
	2013-03-11	Correction & Standardization control structure for all transtype
	2013-09-16	To update FTS11FAM2 when FTS11FAG1 is updated, and delete when deleted
				To create FTS11FAM2 when FTS11OFAM is created, and delete when deleted
				To trigger FTS11OFAM journal reset when FTS11FAM2 is updated
	2013-10-21	Include FTS11T0304W
	2013-11-05	To trigger sp_FTS11_GenerateJDT1, sp_FTS11_GenerateFAM1 for FTS11T0304
	2014-01-07	Replace cursor with WHILE for better performance
	2014-05-07	Correction to Copy FTS11FAG1 to FTS11FAM2 when Add Fixed Asset Master
*/
	-- Return values
	declare @error  int				-- Result (0 for no error)
	declare @error_message nvarchar (200) 		-- Error string to be displayed
	select @error = 0
	select @error_message = N''Ok''

	DECLARE @LogNote NVARCHAR(254)
	EXEC sp_FTS11_DebugLog ''SBO_SP_PostTransactionNotice'', @object_type, @list_of_cols_val_tab_del, ''Begin Procedure. ''
	--------------------------------------------------------------------------------------------------------------------------------

	--	ADD	YOUR	CODE	HERE
	DECLARE @DocEntry INT
	DECLARE @LineId INT
	DECLARE @UserCode NVARCHAR(10)
	DECLARE @FAKey INT
	DECLARE @DocDate SMALLDATETIME
	DECLARE @PrdBegin SMALLDATETIME
	DECLARE @PrdEnd SMALLDATETIME
	DECLARE @ErrorMsg NVARCHAR(255)
	DECLARE @Changed INT -- 1: Critical Data Changed, 0: Critical Data Not Changed
	DECLARE @Result TABLE (ErrCode INT, ErrMsg NVARCHAR(200))

	--IF @object_type = ''FTS11OLOC'' -- Location - Settings
	--BEGIN
	--	IF @transaction_type = ''A'' OR @transaction_type = ''U''
	--	BEGIN			
	--		-- Copy Location setting to FA - Location with Sync FA Option
	--		UPDATE T0 SET T0.Project = CASE WHEN ISNULL(T2.Project,'''') = '''' THEN T0.Project ELSE T2.Project END,
	--		T0.ProfitCode = CASE WHEN ISNULL(T2.ProfitCode,'''') = '''' THEN T0.ProfitCode ELSE T2.ProfitCode END,
	--		T0.OcrCode2 = CASE WHEN ISNULL(T2.OcrCode2,'''') = '''' THEN T0.OcrCode2 ELSE T2.OcrCode2 END,
	--		T0.OcrCode3 = CASE WHEN ISNULL(T2.OcrCode3,'''') = '''' THEN T0.OcrCode3 ELSE T2.OcrCode3 END,
	--		T0.OcrCode4 = CASE WHEN ISNULL(T2.OcrCode4,'''') = '''' THEN T0.OcrCode4 ELSE T2.OcrCode4 END,
	--		T0.OcrCode5 = CASE WHEN ISNULL(T2.OcrCode5,'''') = '''' THEN T0.OcrCode5 ELSE T2.OcrCode5 END,
	--		T0.IndCod1 = CASE WHEN ISNULL(T2.IndCod1,'''') = '''' THEN T0.IndCod1 ELSE T2.IndCod1 END,
	--		T0.IndKey1 = CASE WHEN ISNULL(T2.IndCod1,'''') = '''' THEN T0.IndKey1 ELSE T2.IndKey1 END,
	--		T0.IndCod2 = CASE WHEN ISNULL(T2.IndCod2,'''') = '''' THEN T0.IndCod2 ELSE T2.IndCod2 END,
	--		T0.IndKey2 = CASE WHEN ISNULL(T2.IndCod2,'''') = '''' THEN T0.IndKey2 ELSE T2.IndKey2 END,
	--		T0.IndCod3 = CASE WHEN ISNULL(T2.IndCod3,'''') = '''' THEN T0.IndCod3 ELSE T2.IndCod3 END,
	--		T0.IndKey3 = CASE WHEN ISNULL(T2.IndCod3,'''') = '''' THEN T0.IndKey3 ELSE T2.IndKey3 END,
	--		T0.IndCod4 = CASE WHEN ISNULL(T2.IndCod4,'''') = '''' THEN T0.IndCod4 ELSE T2.IndCod4 END,
	--		T0.IndKey4 = CASE WHEN ISNULL(T2.IndCod4,'''') = '''' THEN T0.IndKey4 ELSE T2.IndKey4 END,
	--		T0.IndCod5 = CASE WHEN ISNULL(T2.IndCod5,'''') = '''' THEN T0.IndCod5 ELSE T2.IndCod5 END,
	--		T0.IndKey5 = CASE WHEN ISNULL(T2.IndCod5,'''') = '''' THEN T0.IndKey5 ELSE T2.IndKey5 END,
	--		T0.IndCod6 = CASE WHEN ISNULL(T2.IndCod6,'''') = '''' THEN T0.IndCod6 ELSE T2.IndCod6 END,
	--		T0.IndKey6 = CASE WHEN ISNULL(T2.IndCod6,'''') = '''' THEN T0.IndKey6 ELSE T2.IndKey6 END			
	--		FROM FTS11FAM2 T0 INNER JOIN FTS11OFAM T1 ON T0.U_FAKey = T1.DocEntry 
	--		INNER JOIN FTS11OLOC T2 ON T0.Location = T2.Code
	--		WHERE T1.FAGrpSyn = ''Y'' AND T2.DocEntry = @list_of_cols_val_tab_del
	--	END		
	--END
	
	
	--IF @object_type = ''FTS11OFAG'' -- Fixed Asset Group
	--BEGIN
	--	IF @transaction_type = ''U''
	--	BEGIN			
	--		-- If critical data updated
	--		IF dbo.fn_FTS11_IsChanged_OFAG(@list_of_cols_val_tab_del) = 1
	--		BEGIN
	--			EXEC sp_FTS11_ResetOFAG_OFAM @list_of_cols_val_tab_del 
	--		END 
	--	END		
	--END

	--IF @object_type = ''FTS11FAG1'' -- Fixed Asset Group - Settings
	--BEGIN
	--	IF @transaction_type = ''D''
	--	BEGIN						
	--		DELETE T0 FROM FTS11FAM2 T0 INNER JOIN FTS11OFAM T1 ON T0.U_FAKey = T1.DocEntry 
	--		INNER JOIN FTS11AFAG1 T3 ON T1.U_FAGrpKey = T3.U_FAGrpKey AND T0.Location = T3.Location
	--		WHERE T1.FAGrpSyn = ''Y'' AND T3.DocEntry = @list_of_cols_val_tab_del					
	--	END
			
	--	IF @transaction_type = ''A'' OR @transaction_type = ''U''
	--	BEGIN			
	--		IF EXISTS(SELECT 1 FROM FTS11OFAM T1 INNER JOIN FTS11FAG1 T3 ON T1.U_FAGrpKey = T3.U_FAGrpKey
	--		WHERE T1.U_FAGrpSyn = ''Y'' AND T3.DocEntry = @list_of_cols_val_tab_del
	--		AND (ISNULL(T3.U_AccCodA,'''') <> '''' OR ISNULL(T3.U_AccCodD,'''') <> '''' OR ISNULL(T3.U_AccCodC,'''') <> '''' OR ISNULL(T3.U_AccCodW,'''') <> '''' OR ISNULL(T3.U_AccCodQ,'''') <> ''''))
	--		BEGIN
	--			-- Loop through FA Group Sync Enabled FA and update / creaste FA - Location settings
	--			SELECT @FAKey = MIN(T0.DocEntry) FROM FTS11OFAM T0 INNER JOIN FTS11FAG1 T1 ON T0.U_FAGrpKey = T1.U_FAGrpKey 
	--				WHERE T0.U_FAGrpSyn = ''Y'' AND T1.DocEntry = @list_of_cols_val_tab_del
	--				AND (ISNULL(T1.U_AccCodA,'''') <> '''' OR ISNULL(T1.U_AccCodD,'''') <> '''' OR ISNULL(T1.U_AccCodC,'''') <> '''' OR ISNULL(T1.U_AccCodW,'''') <> '''' OR ISNULL(T1.U_AccCodQ,'''') <> '''');

	--			WHILE @FAKey IS NOT NULL
	--			BEGIN
	--				-- If FA - Location settings already exists
	--				SET @DocEntry = 0
	--				SELECT @DocEntry = T0.DocEntry FROM FTS11FAM2 T0 INNER JOIN FTS11OFAM T1 ON T0.U_FAKey = T1.DocEntry 
	--				INNER JOIN FTS11FAG1 T3 ON T1.U_FAGrpKey = T3.U_FAGrpKey AND T0.U_Location = T3.U_Location
	--				WHERE T1.U_FAGrpSyn = ''Y'' AND T3.DocEntry = @list_of_cols_val_tab_del AND T1.DocEntry = @FAKey
					
	--				IF @DocEntry > 0
	--				BEGIN		
	--					UPDATE T0 SET T0.U_AccCodA = CASE WHEN ISNULL(T3.U_AccCodA,'''') = '''' THEN T0.U_AccCodA ELSE T3.U_AccCodA END,
	--					T0.U_AccFrmA = CASE WHEN ISNULL(T3.U_AccCodA,'''') = '''' THEN T0.U_AccFrmA ELSE T3.U_AccFrmA END,
	--					T0.U_AccNamA = CASE WHEN ISNULL(T3.U_AccCodA,'''') = '''' THEN T0.U_AccNamA ELSE T3.U_AccNamA END,
	--					T0.U_AccCodD = CASE WHEN ISNULL(T3.U_AccCodD,'''') = '''' THEN T0.U_AccCodD ELSE T3.U_AccCodD END,
	--					T0.U_AccFrmD = CASE WHEN ISNULL(T3.U_AccCodD,'''') = '''' THEN T0.U_AccFrmD ELSE T3.U_AccFrmD END,
	--					T0.U_AccNamD = CASE WHEN ISNULL(T3.U_AccCodD,'''') = '''' THEN T0.U_AccNamD ELSE T3.U_AccNamD END,
	--					T0.U_AccCodC = CASE WHEN ISNULL(T3.U_AccCodC,'''') = '''' THEN T0.U_AccCodC ELSE T3.U_AccCodC END,
	--					T0.U_AccFrmC = CASE WHEN ISNULL(T3.U_AccCodC,'''') = '''' THEN T0.U_AccFrmC ELSE T3.U_AccFrmC END,
	--					T0.U_AccNamC = CASE WHEN ISNULL(T3.U_AccCodC,'''') = '''' THEN T0.U_AccNamC ELSE T3.U_AccNamC END,
	--					T0.U_AccCodW = CASE WHEN ISNULL(T3.U_AccCodW,'''') = '''' THEN T0.U_AccCodW ELSE T3.U_AccCodW END,
	--					T0.U_AccFrmW = CASE WHEN ISNULL(T3.U_AccCodW,'''') = '''' THEN T0.U_AccFrmW ELSE T3.U_AccFrmW END,
	--					T0.U_AccNamW = CASE WHEN ISNULL(T3.U_AccCodW,'''') = '''' THEN T0.U_AccCodW ELSE T3.U_AccNamW END,
	--					T0.U_AccCodQ = CASE WHEN ISNULL(T3.U_AccCodQ,'''') = '''' THEN T0.U_AccCodQ ELSE T3.U_AccCodQ END,
	--					T0.U_AccFrmQ = CASE WHEN ISNULL(T3.U_AccCodQ,'''') = '''' THEN T0.U_AccFrmQ ELSE T3.U_AccFrmQ END,
	--					T0.U_AccNamQ = CASE WHEN ISNULL(T3.U_AccCodQ,'''') = '''' THEN T0.U_AccNamQ ELSE T3.U_AccNamQ END			
	--					FROM FTS11FAM2 T0 INNER JOIN FTS11OFAM T1 ON T0.U_FAKey = T1.DocEntry 
	--					INNER JOIN FTS11OFAG T2 ON T1.U_FAGrpKey = T2.DocEntry
	--					INNER JOIN FTS11FAG1 T3 ON T2.DocEntry = T3.U_FAGrpKey AND T0.U_Location = T3.U_Location
	--					WHERE T0.DocEntry = @DocEntry	
						
	--					--EXEC sp_FTS11_RecordChangeLog ''FTS11FAM2'', 1, ''DocEntry'', @DocEntry, 1		
						
	--					SELECT @FAKey = U_FAKey FROM FTS11FAM2 WHERE DocEntry = @DocEntry						
	--					EXEC sp_FTS11_GenerateJDT1 ''FTS11OFAM'', ''U'', 1, ''DocEntry'', @FAKey
	--				END
	--				ELSE
	--				BEGIN		
	--					--EXEC sp_getnextskno ''FTS11FAM2'', @DocEntry OUTPUT
						
	--					INSERT INTO FTS11FAM2
	--					SELECT 
	--					T0.DocEntry, T1.U_LocationKey, T1.U_Location, 
	--					ISNULL(T1.U_AccCodA,''''), CASE WHEN ISNULL(T1.U_AccCodA,'''') = '''' THEN '''' ELSE T1.U_AccFrmA END, CASE WHEN ISNULL(T1.U_AccCodA,'''') = '''' THEN '''' ELSE T1.U_AccNamA END, 
	--					ISNULL(T1.U_AccCodD,''''), CASE WHEN ISNULL(T1.U_AccCodD,'''') = '''' THEN '''' ELSE T1.U_AccFrmD END, CASE WHEN ISNULL(T1.U_AccCodD,'''') = '''' THEN '''' ELSE T1.U_AccNamD END, 
	--					ISNULL(T1.U_AccCodC,''''), CASE WHEN ISNULL(T1.U_AccCodC,'''') = '''' THEN '''' ELSE T1.U_AccFrmC END, CASE WHEN ISNULL(T1.U_AccCodC,'''') = '''' THEN '''' ELSE T1.U_AccNamC END, 
	--					ISNULL(T1.U_AccCodW,''''), CASE WHEN ISNULL(T1.U_AccCodW,'''') = '''' THEN '''' ELSE T1.U_AccFrmW END, CASE WHEN ISNULL(T1.U_AccCodW,'''') = '''' THEN '''' ELSE T1.U_AccNamW END, 
	--					ISNULL(T1.U_AccCodQ,''''), CASE WHEN ISNULL(T1.U_AccCodQ,'''') = '''' THEN '''' ELSE T1.U_AccFrmQ END, CASE WHEN ISNULL(T1.U_AccCodQ,'''') = '''' THEN '''' ELSE T1.U_AccNamQ END,
	--					ISNULL(T2.Project,''''), ISNULL(T2.ProfitCode,''''), ISNULL(T2.OcrCode2,''''), ISNULL(T2.OcrCode3,''''), ISNULL(T2.OcrCode4,''''), ISNULL(T2.OcrCode5,''''), 
	--					ISNULL(T2.IndKey1,0), ISNULL(T2.IndCod1,''''), ISNULL(T2.IndKey2,0), ISNULL(T2.IndCod2,''''), ISNULL(T2.IndKey3,0), ISNULL(T2.IndCod3,''''), 
	--					ISNULL(T2.IndKey4,0), ISNULL(T2.IndCod4,''''), ISNULL(T2.IndKey5,0), ISNULL(T2.IndCod5,''''), ISNULL(T2.IndKey6,0), ISNULL(T2.IndCod6,''''),
	--					''FTS11FAM2''
	--					FROM FTS11OFAM T0 INNER JOIN FTS11FAG1 T1 ON T0.U_FAGrpKey = T1.U_FAGrpKey 
	--					LEFT OUTER JOIN FTS11OLOC T2 ON T1.U_Location = T2.Code 
	--					WHERE T1.DocEntry = @list_of_cols_val_tab_del AND T0.DocEntry = @FAKey
						
	--					--EXEC sp_FTS11_RecordChangeLog ''FTS11FAM2'', 1, ''DocEntry'', @DocEntry, 0				
	--					EXEC sp_FTS11_GenerateJDT1 ''FTS11OFAM'', ''U'', 1, ''DocEntry'', @FAKey
	--				END
					
	--				-- Next Fixed Asset
	--				SELECT @FAKey = MIN(T0.DocEntry) FROM FTS11OFAM T0 INNER JOIN FTS11FAG1 T1 ON T0.U_FAGrpKey = T1.U_FAGrpKey 
	--				WHERE T0.U_FAGrpSyn = ''Y'' AND T1.DocEntry = @list_of_cols_val_tab_del
	--				AND (ISNULL(T1.U_AccCodA,'''') <> '''' OR ISNULL(T1.U_AccCodD,'''') <> '''' OR ISNULL(T1.U_AccCodC,'''') <> '''' OR ISNULL(T1.U_AccCodW,'''') <> '''' OR ISNULL(T1.U_AccCodQ,'''') <> '''')
	--				AND T0.DocEntry > @FAKey
	--			END

	--		END
	--	END		
	--END
	
	--IF @object_type = ''FTS11FAM2''
	--BEGIN
	--	IF @transaction_type = ''U''
	--	BEGIN
	--		SET @FAKey = 0
	--		SELECT @FAKey = U_FAKey FROM FTS11FAM2 WHERE DocEntry = @list_of_cols_val_tab_del
	--		IF @FAKey > 0 EXEC sp_FTS11_GenerateJDT1 ''FTS11OFAM'', ''U'', 1, ''DocEntry'', @FAKey
	--	END
	--END

	IF @object_type = ''FTS11OFAM'' -- Fixed Asset Master
	BEGIN
		IF @transaction_type = ''D''
		BEGIN
			DELETE FTS11JDT1 WHERE fakey = @list_of_cols_val_tab_del
			DELETE FTS11FAM1 WHERE fakey = @list_of_cols_val_tab_del
			DELETE FTS11FAM2 WHERE U_fakey = @list_of_cols_val_tab_del
			EXEC sp_FTS11_SetIsFieldEditable @list_of_cols_val_tab_del
		END

		IF @transaction_type = ''A''
		BEGIN
			IF EXISTS(SELECT 1 FROM FTS11OFAM WHERE U_Status <> ''D'' AND DocEntry = @list_of_cols_val_tab_del)
			BEGIN
				EXEC sp_FTS11_GenerateJDT1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @list_of_cols_val_tab_del	
				EXEC sp_FTS11_GenerateFAM1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @list_of_cols_val_tab_del	
				EXEC sp_FTS11_SetIsFieldEditable @list_of_cols_val_tab_del
				-- Copy F/A Group - Location Settings to F/A 
				IF EXISTS(SELECT 1 FROM FTS11OFAM T0 INNER JOIN FTS11FAG1 T1 ON T0.U_FAGrpKey = T1.U_FAGrpKey 
					WHERE T0.U_FAGrpSyn = ''Y'' AND T0.DocEntry = @list_of_cols_val_tab_del
					AND (ISNULL(T1.U_AccCodA,'''') <> '''' OR ISNULL(T1.U_AccCodD,'''') <> '''' OR ISNULL(T1.U_AccCodC,'''') <> '''' OR ISNULL(T1.U_AccCodW,'''') <> '''' OR ISNULL(T1.U_AccCodQ,'''') <> ''''))
				BEGIN
					-- Loop through FA Group Sync Enabled FA and update / creaste FA - Location settings
					SELECT @LineId = MIN(T1.DocEntry) FROM FTS11OFAM T0 INNER JOIN FTS11FAG1 T1 ON T0.U_FAGrpKey = T1.U_FAGrpKey 
						WHERE T0.U_FAGrpSyn = ''Y'' AND T0.DocEntry = @list_of_cols_val_tab_del
						AND (ISNULL(T1.U_AccCodA,'''') <> '''' OR ISNULL(T1.U_AccCodD,'''') <> '''' OR ISNULL(T1.U_AccCodC,'''') <> '''' OR ISNULL(T1.U_AccCodW,'''') <> '''' OR ISNULL(T1.U_AccCodQ,'''') <> '''');

					WHILE @LineId IS NOT NULL
					BEGIN				
						--EXEC sp_getnextskno ''FTS11FAM2'', @DocEntry OUTPUT						
						
						INSERT INTO FTS11FAM2 ([Object] ,[U_FAKey] ,[U_LocationKey] ,[U_Location] 
						,[U_AccCodA] ,[U_AccFrmA] ,[U_AccNamA] ,[U_AccCodD] ,[U_AccFrmD] ,[U_AccNamD] ,[U_AccCodC] ,[U_AccFrmC] ,[U_AccNamC] 
						,[U_AccCodW] ,[U_AccFrmW] ,[U_AccNamW] ,[U_AccCodQ] ,[U_AccFrmQ] ,[U_AccNamQ] ,[U_Project] ,[U_ProfitCode] ,[U_OcrCode2] ,[U_OcrCode3] ,[U_OcrCode4],[U_OcrCode5] 
						,[U_IndKey1] ,[U_IndCod1] ,[U_IndKey2] ,[U_IndCod2] ,[U_IndKey3] ,[U_IndCod3] ,[U_IndKey4] ,[U_IndCod4] ,[U_IndKey5] ,[U_IndCod5] ,[U_IndKey6],[U_IndCod6])
						
						SELECT 
						''FTS11FAM2'', T0.DocEntry, T1.U_LocationKey, T1.U_Location, 
						ISNULL(T1.U_AccCodA,''''), CASE WHEN ISNULL(T1.U_AccCodA,'''') = '''' THEN NULL ELSE T1.U_AccFrmA END, CASE WHEN ISNULL(T1.U_AccCodA,'''') = '''' THEN '''' ELSE T1.U_AccNamA END, 
						ISNULL(T1.U_AccCodD,''''), CASE WHEN ISNULL(T1.U_AccCodD,'''') = '''' THEN NULL ELSE T1.U_AccFrmD END, CASE WHEN ISNULL(T1.U_AccCodD,'''') = '''' THEN '''' ELSE T1.U_AccNamD END, 
						ISNULL(T1.U_AccCodC,''''), CASE WHEN ISNULL(T1.U_AccCodC,'''') = '''' THEN NULL ELSE T1.U_AccFrmC END, CASE WHEN ISNULL(T1.U_AccCodC,'''') = '''' THEN '''' ELSE T1.U_AccNamC END, 
						ISNULL(T1.U_AccCodW,''''), CASE WHEN ISNULL(T1.U_AccCodW,'''') = '''' THEN NULL ELSE T1.U_AccFrmW END, CASE WHEN ISNULL(T1.U_AccCodW,'''') = '''' THEN '''' ELSE T1.U_AccNamW END, 
						ISNULL(T1.U_AccCodQ,''''), CASE WHEN ISNULL(T1.U_AccCodQ,'''') = '''' THEN NULL ELSE T1.U_AccFrmQ END, CASE WHEN ISNULL(T1.U_AccCodQ,'''') = '''' THEN '''' ELSE T1.U_AccNamQ END,
						ISNULL(T2.U_Project, NULL), ISNULL(T2.U_ProfitCode, NULL), ISNULL(T2.U_OcrCode2, NULL), ISNULL(T2.U_OcrCode3, NULL), ISNULL(T2.U_OcrCode4,NULL), ISNULL(T2.U_OcrCode5,NULL), 
						ISNULL(T2.U_IndKey1, NULL), ISNULL(T2.U_IndCod1,''''), ISNULL(T2.U_IndKey2, NULL), ISNULL(T2.U_IndCod2,''''), ISNULL(T2.U_IndKey3, NULL), ISNULL(T2.U_IndCod3,''''), 
						ISNULL(T2.U_IndKey4, NULL), ISNULL(T2.U_IndCod4,''''), ISNULL(T2.U_IndKey5, NULL), ISNULL(T2.U_IndCod5,''''), ISNULL(T2.U_IndKey6, NULL), ISNULL(T2.U_IndCod6,'''')
						FROM FTS11OFAM T0 INNER JOIN FTS11FAG1 T1 ON T0.U_FAGrpKey = T1.U_FAGrpKey 
						LEFT OUTER JOIN FTS11OLOC T2 ON T1.U_Location = T2.Code 
						WHERE T1.DocEntry = @LineId AND T0.DocEntry = @list_of_cols_val_tab_del
						
						EXEC sp_FTS11_SetIsFieldEditable @list_of_cols_val_tab_del
						--EXEC sp_FTS11_RecordChangeLog ''FTS11FAM2'', 1, ''DocEntry'', @DocEntry, 0

						-- Next F/A
						SELECT @LineId = MIN(T1.DocEntry) FROM FTS11OFAM T0 INNER JOIN FTS11FAG1 T1 ON T0.U_FAGrpKey = T1.U_FAGrpKey 
						WHERE T0.U_FAGrpSyn = ''Y'' AND T0.DocEntry = @list_of_cols_val_tab_del
						AND (ISNULL(T1.U_AccCodA,'''') <> '''' OR ISNULL(T1.U_AccCodD,'''') <> '''' OR ISNULL(T1.U_AccCodC,'''') <> '''' OR ISNULL(T1.U_AccCodW,'''') <> '''' OR ISNULL(T1.U_AccCodQ,'''') <> '''')
						AND T1.DocEntry > @LineId
					END
	
	
				END
			END
		END		
		
		IF @transaction_type IN (''U'', ''C'')
		BEGIN				
			IF @transaction_type = ''C''
			BEGIN
				IF EXISTS(SELECT 1 FROM FTS11OFAM T0 INNER JOIN FTS11AFAM T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.U_Status = ''C'' AND T1.U_Status = ''O'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11OFAM SET U_CnclBy = U_UpdateBy, U_CnclDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
					EXEC sp_FTS11_SetIsFieldEditable @list_of_cols_val_tab_del
				END		
			END
			
			IF @transaction_type = ''C'' OR dbo.fn_FTS11_IsChanged_OFAM(@list_of_cols_val_tab_del) = 1
			BEGIN
				EXEC sp_FTS11_GenerateJDT1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @list_of_cols_val_tab_del
				EXEC sp_FTS11_GenerateFAM1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @list_of_cols_val_tab_del
				EXEC sp_FTS11_SetIsFieldEditable @list_of_cols_val_tab_del
				--EXEC sp_FTS11_ResetOFAM @list_of_cols_val_tab_del, ''''
			END
		END			
	END

	IF @object_type = ''FTS11T0201'' -- Write Off
	BEGIN
		IF @transaction_type IN (''A'', ''U'', ''C'')
		BEGIN					
			IF @transaction_type = ''C''
			BEGIN
				IF EXISTS(SELECT 1 FROM FTS11T0201 T0 INNER JOIN FTS11A0201 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''O'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0201 SET U_CnclBy = U_UpdateBy, U_CnclDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
				IF EXISTS(SELECT 1 FROM FTS11T0201 T0 INNER JOIN FTS11A0201 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''P'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0201 SET U_RvrsBy = U_UpdateBy, U_RvrsDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
			END
			
			IF @transaction_type = ''C'' 
				OR (@transaction_type = ''U'' )--AND dbo.fn_FTS11_IsChanged_Doc(@object_type, @list_of_cols_val_tab_del) = 1)
				OR (@transaction_type = ''A'' AND EXISTS(SELECT 1 FROM FTS11T0201 WHERE Status = ''O'' AND DocEntry = @list_of_cols_val_tab_del))
			BEGIN
				-- Loop through each row in the document
				SELECT @LineId = MIN(T0.ResultLineID)
				FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0

				WHILE ISNULL(@LineId,0) <> 0
				BEGIN

					SELECT @FAKey = T0.ResultFAKey 
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID = @LineId

					SELECT @DocDate = T0.U_DocDate, @FAKey = T1.U_FAKey 
					FROM FTS11T0201 T0
					INNER JOIN FTS11T0201D T1 ON T0.DocEntry = T1.DocEntry 
					WHERE Object = @object_type AND T0.DocEntry = @list_of_cols_val_tab_del
					AND T1.Oid = @LineId AND T1.U_FAKey = @FAKey
				
					-- Recalculate Depreciation for period before to ensure correct ACCU				
					SELECT @PrdBegin = U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey
					SELECT @PrdBegin = ISNULL(MAX(T0.DocDate), @PrdBegin )
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus = ''P'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @PrdBegin

					EXEC sp_FTS11_GenerateDeprn @PrdBegin, @DocDate, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT	
							
					EXEC sp_FTS11_GenerateJDT1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey
					EXEC sp_FTS11_GenerateFAM1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey
			
					-- Recalculate Depreciation for period after					
					SET @PrdEnd = @DocDate				
					SELECT @PrdEnd = ISNULL(MAX(T0.DocDate), @PrdEnd)
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus <> ''C'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @DocDate
				
					EXEC sp_FTS11_GenerateDeprn @DocDate, @PrdEnd, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT
					EXEC sp_FTS11_ResetOFAM @FAKey, ''Status''	
					EXEC sp_FTS11_SetIsFieldEditable @FAKey

					-- Next row
					SELECT @LineId = MIN(T0.ResultLineID)
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID > @LineId
				END	

				DELETE T0 FROM FTS11T0201D T0 WHERE ISNULL(T0.DocEntry,0) = 0 
								
			END
		END			
	END -- Write Off

	IF @object_type = ''FTS11T0202'' -- Dispose (Sales)
	BEGIN
		IF @transaction_type IN (''A'', ''U'', ''C'')
		BEGIN					
			IF @transaction_type = ''C''
			BEGIN
				IF EXISTS(SELECT 1 FROM FTS11T0202 T0 INNER JOIN FTS11A0202 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''O'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0202 SET U_CnclBy = U_UpdateBy, U_CnclDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
				IF EXISTS(SELECT 1 FROM FTS11T0202 T0 INNER JOIN FTS11A0202 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''P'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0202 SET U_RvrsBy = U_UpdateBy, U_RvrsDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
			END
			
			IF @transaction_type = ''C'' 
				OR (@transaction_type = ''U'' )--AND dbo.fn_FTS11_IsChanged_Doc(@object_type, @list_of_cols_val_tab_del) = 1)
				OR (@transaction_type = ''A'' AND EXISTS(SELECT 1 FROM FTS11T0202 WHERE Status = ''O'' AND DocEntry = @list_of_cols_val_tab_del))
			BEGIN
				-- Loop through each row in the document
				SELECT @LineId = MIN(T0.ResultLineID)
				FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0

				WHILE ISNULL(@LineId,0) <> 0
				BEGIN

					SELECT @FAKey = T0.ResultFAKey 
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID = @LineId

					SELECT @DocDate = T0.U_DocDate, @FAKey = T1.U_FAKey 
					FROM FTS11T0202 T0
					INNER JOIN FTS11T0202D T1 ON T0.DocEntry = T1.DocEntry 
					WHERE Object = @object_type AND T0.DocEntry = @list_of_cols_val_tab_del
					AND T1.Oid = @LineId AND T1.U_FAKey = @FAKey
				
					-- Recalculate Depreciation for period before to ensure correct ACCU				
					SELECT @PrdBegin = U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey
					SELECT @PrdBegin = ISNULL(MAX(T0.DocDate), @PrdBegin )
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus = ''P'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @PrdBegin

					EXEC sp_FTS11_GenerateDeprn @PrdBegin, @DocDate, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT	
							
					EXEC sp_FTS11_GenerateJDT1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey
					EXEC sp_FTS11_GenerateFAM1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey				
				
					-- Recalculate Depreciation for period after					
					SET @PrdEnd = @DocDate				
					SELECT @PrdEnd = ISNULL(MAX(T0.DocDate), @PrdEnd)
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus <> ''C'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @DocDate

					EXEC sp_FTS11_GenerateDeprn @DocDate, @PrdEnd, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT	
					EXEC sp_FTS11_ResetOFAM @FAKey, ''Status''
					EXEC sp_FTS11_SetIsFieldEditable @FAKey

					-- Next row
					SELECT @LineId = MIN(T0.ResultLineID)
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID > @LineId
				END	
				
				DELETE T0 FROM FTS11T0202D T0 WHERE ISNULL(T0.DocEntry,0) = 0 
								
			END
		END			
	END -- Dispose (Sales)

	IF @object_type = ''FTS11T0203'' -- Correction Out
	BEGIN
		IF @transaction_type IN (''A'', ''U'', ''C'')
		BEGIN					
			IF @transaction_type = ''C''
			BEGIN
				IF EXISTS(SELECT 1 FROM FTS11T0203 T0 INNER JOIN FTS11A0203 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''O'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0203 SET U_CnclBy = U_UpdateBy, U_CnclDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
				IF EXISTS(SELECT 1 FROM FTS11T0203 T0 INNER JOIN FTS11A0203 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''P'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0203 SET U_RvrsBy = U_UpdateBy, U_RvrsDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
			END
			
			IF @transaction_type = ''C'' 
				OR (@transaction_type = ''U'' )--AND dbo.fn_FTS11_IsChanged_Doc(@object_type, @list_of_cols_val_tab_del) = 1)
				OR (@transaction_type = ''A'' AND EXISTS(SELECT 1 FROM FTS11T0203 WHERE Status = ''O'' AND DocEntry = @list_of_cols_val_tab_del))
			BEGIN
				-- Loop through each row in the document
				SELECT @LineId = MIN(T0.ResultLineID)
				FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0

				WHILE ISNULL(@LineId,0) <> 0
				BEGIN

					SELECT @FAKey = T0.ResultFAKey 
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID = @LineId

					SELECT @DocDate = T0.U_DocDate, @FAKey = T1.U_FAKey 
					FROM FTS11T0203 T0
					INNER JOIN FTS11T0203D T1 ON T0.DocEntry = T1.DocEntry 
					WHERE Object = @object_type AND T0.DocEntry = @list_of_cols_val_tab_del
					AND T1.Oid = @LineId AND T1.U_FAKey = @FAKey
				
					-- Recalculate Depreciation for period before to ensure correct ACCU				
					SELECT @PrdBegin = U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey
					SELECT @PrdBegin = ISNULL(MAX(T0.DocDate), @PrdBegin )
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus = ''P'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @PrdBegin

					EXEC sp_FTS11_GenerateDeprn @PrdBegin, @DocDate, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT	
							
					EXEC sp_FTS11_GenerateJDT1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey
					EXEC sp_FTS11_GenerateFAM1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey					
				
					-- Recalculate Depreciation for period after					
					SET @PrdEnd = @DocDate				
					SELECT @PrdEnd = ISNULL(MAX(T0.DocDate), @PrdEnd)
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus <> ''C'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @DocDate

					EXEC sp_FTS11_GenerateDeprn @DocDate, @PrdEnd, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT
					EXEC sp_FTS11_ResetOFAM @FAKey, ''Status''
					EXEC sp_FTS11_SetIsFieldEditable @FAKey	
					
					-- Next row
					SELECT @LineId = MIN(T0.ResultLineID)
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID > @LineId
				END
				
				DELETE T0 FROM FTS11T0203D T0 WHERE ISNULL(T0.DocEntry,0) = 0 
									
			END
		END			
	END -- Correction Out

	IF @object_type = ''FTS11T0204'' -- Transfer Out
	BEGIN
		IF @transaction_type IN (''A'', ''U'', ''C'')
		BEGIN					
			IF @transaction_type = ''C''
			BEGIN
				IF EXISTS(SELECT 1 FROM FTS11T0204 T0 INNER JOIN FTS11A0204 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''O'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0204 SET U_CnclBy = U_UpdateBy, U_CnclDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
				IF EXISTS(SELECT 1 FROM FTS11T0204 T0 INNER JOIN FTS11A0204 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''P'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0204 SET U_RvrsBy = U_UpdateBy, U_RvrsDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
			END
			
			IF @transaction_type = ''C'' 
				OR (@transaction_type = ''U'' )--AND dbo.fn_FTS11_IsChanged_Doc(@object_type, @list_of_cols_val_tab_del) = 1)
				OR (@transaction_type = ''A'' AND EXISTS(SELECT 1 FROM FTS11T0204 WHERE Status = ''O'' AND DocEntry = @list_of_cols_val_tab_del))
			BEGIN
				-- Loop through each row in the document
				SELECT @LineId = MIN(T0.ResultLineID)
				FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0

				WHILE ISNULL(@LineId,0) <> 0
				BEGIN

					SELECT @FAKey = T0.ResultFAKey 
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID = @LineId

					SELECT @DocDate = T0.U_DocDate, @FAKey = T1.U_FAKey 
					FROM FTS11T0204 T0
					INNER JOIN FTS11T0204D T1 ON T0.DocEntry = T1.DocEntry 
					WHERE Object = @object_type AND T0.DocEntry = @list_of_cols_val_tab_del
					AND T1.Oid = @LineId AND T1.U_FAKey = @FAKey
				
					-- Recalculate Depreciation for period before to ensure correct ACCU				
					SELECT @PrdBegin = U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey
					SELECT @PrdBegin = ISNULL(MAX(T0.DocDate), @PrdBegin )
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus = ''P'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @PrdBegin

					EXEC sp_FTS11_GenerateDeprn @PrdBegin, @DocDate, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT	
							
					EXEC sp_FTS11_GenerateJDT1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey
					EXEC sp_FTS11_GenerateFAM1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey				
				
					-- Recalculate Depreciation for period after					
					SET @PrdEnd = @DocDate				
					SELECT @PrdEnd = ISNULL(MAX(T0.DocDate), @PrdEnd)
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus <> ''C'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @DocDate

					EXEC sp_FTS11_GenerateDeprn @DocDate, @PrdEnd, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT
					EXEC sp_FTS11_ResetOFAM @FAKey, ''Status''
					EXEC sp_FTS11_SetIsFieldEditable @FAKey
							
					-- Next row
					SELECT @LineId = MIN(T0.ResultLineID)
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID > @LineId 
				END	
				
				DELETE T0 FROM FTS11T0204D T0 WHERE ISNULL(T0.DocEntry,0) = 0 
												
			END
		END			
	END -- Transfer Out

	IF @object_type = ''FTS11T0205'' -- Reclass Out
	BEGIN
		IF @transaction_type IN (''A'', ''U'', ''C'')
		BEGIN					
			IF @transaction_type = ''C''
			BEGIN
				IF EXISTS(SELECT 1 FROM FTS11T0205 T0 INNER JOIN FTS11A0205 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''O'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0205 SET U_CnclBy = U_UpdateBy, U_CnclDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
				IF EXISTS(SELECT 1 FROM FTS11T0205 T0 INNER JOIN FTS11A0205 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''P'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0205 SET U_RvrsBy = U_UpdateBy, U_RvrsDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
			END
			
			IF @transaction_type = ''C'' 
				OR (@transaction_type = ''U'' )--AND dbo.fn_FTS11_IsChanged_Doc(@object_type, @list_of_cols_val_tab_del) = 1)
				OR (@transaction_type = ''A'' AND EXISTS(SELECT 1 FROM FTS11T0205 WHERE Status = ''O'' AND DocEntry = @list_of_cols_val_tab_del))
			BEGIN
				-- Loop through each row in the document
				SELECT @LineId = MIN(T0.ResultLineID)
				FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0

				WHILE ISNULL(@LineId,0) <> 0
				BEGIN

					SELECT @FAKey = T0.ResultFAKey 
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID = @LineId

					SELECT @DocDate = T0.U_DocDate, @FAKey = T1.U_FAKey 
					FROM FTS11T0205 T0
					INNER JOIN FTS11T0205D T1 ON T0.DocEntry = T1.DocEntry 
					WHERE Object = @object_type AND T0.DocEntry = @list_of_cols_val_tab_del
					AND T1.Oid = @LineId AND T1.U_FAKey = @FAKey
				
					-- Recalculate Depreciation for period before to ensure correct ACCU				
					SELECT @PrdBegin = U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey
					SELECT @PrdBegin = ISNULL(MAX(T0.DocDate), @PrdBegin )
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus = ''P'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @PrdBegin

					EXEC sp_FTS11_GenerateDeprn @PrdBegin, @DocDate, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT	
							
					EXEC sp_FTS11_GenerateJDT1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey
					EXEC sp_FTS11_GenerateFAM1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey				
				
					-- Recalculate Depreciation for period after					
					SET @PrdEnd = @DocDate				
					SELECT @PrdEnd = ISNULL(MAX(T0.DocDate), @PrdEnd)
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus <> ''C'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @DocDate

					EXEC sp_FTS11_GenerateDeprn @DocDate, @PrdEnd, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT
					EXEC sp_FTS11_ResetOFAM @FAKey, ''Status''
					EXEC sp_FTS11_SetIsFieldEditable @FAKey

					-- Next row
					SELECT @LineId = MIN(T0.ResultLineID)
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID > @LineId 
				END
				
				DELETE T0 FROM FTS11T0205D T0 WHERE ISNULL(T0.DocEntry,0) = 0 
															
			END
		END			
	END -- Reclass Out

	IF @object_type = ''FTS11T0301'' -- Review Useful Life
	BEGIN
		IF @transaction_type IN (''A'', ''U'', ''C'')
		BEGIN					
			IF @transaction_type = ''C''
			BEGIN
				IF EXISTS(SELECT 1 FROM FTS11T0301 T0 INNER JOIN FTS11A0301 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''O'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0301 SET U_CnclBy = U_UpdateBy, U_CnclDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
				IF EXISTS(SELECT 1 FROM FTS11T0301 T0 INNER JOIN FTS11A0301 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''P'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0301 SET U_RvrsBy = U_UpdateBy, U_RvrsDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
			END
			
			IF @transaction_type = ''C'' 
				OR (@transaction_type = ''U'' )--AND dbo.fn_FTS11_IsChanged_Doc(@object_type, @list_of_cols_val_tab_del) = 1)
				OR (@transaction_type = ''A'' AND EXISTS(SELECT 1 FROM FTS11T0301 WHERE Status = ''O'' AND DocEntry = @list_of_cols_val_tab_del))
			BEGIN
				-- Loop through each row in the document
				SELECT @LineId = MIN(T0.ResultLineID)
				FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0

				WHILE ISNULL(@LineId,0) <> 0
				BEGIN

					SELECT @FAKey = T0.ResultFAKey 
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID = @LineId

					SELECT @DocDate = T0.U_DocDate, @FAKey = T1.U_FAKey 
					FROM FTS11T0301 T0
					INNER JOIN FTS11T0301D T1 ON T0.DocEntry = T1.DocEntry 
					WHERE Object = @object_type AND T0.DocEntry = @list_of_cols_val_tab_del
					AND T1.Oid = @LineId AND T1.U_FAKey = @FAKey
				
					-- Recalculate Depreciation for period before to ensure correct ACCU				
					SELECT @PrdBegin = U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey
					SELECT @PrdBegin = ISNULL(MAX(T0.DocDate), @PrdBegin )
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus = ''P'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @PrdBegin

					EXEC sp_FTS11_GenerateDeprn @PrdBegin, @DocDate, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT	
							
					EXEC sp_FTS11_GenerateJDT1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey
					EXEC sp_FTS11_GenerateFAM1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey	
					--EXEC sp_FTS11_ResetOFAM @FAKey, ''USELIFE''				
				
					-- Recalculate Depreciation for period after					
					SET @PrdEnd = @DocDate				
					SELECT @PrdEnd = ISNULL(MAX(T0.DocDate), @PrdEnd)
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus <> ''C'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @DocDate

					EXEC sp_FTS11_GenerateDeprn @DocDate, @PrdEnd, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT
					EXEC sp_FTS11_SetIsFieldEditable @FAKey
								
					-- Next row
					SELECT @LineId = MIN(T0.ResultLineID)
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID > @LineId 
				END
				
				DELETE T0 FROM FTS11T0301D T0 WHERE ISNULL(T0.DocEntry,0) = 0 

			END
		END			
	END -- Review Useful Life

	IF @object_type = ''FTS11T0302'' -- Revalue / Impairment
	BEGIN
		IF @transaction_type IN (''A'', ''U'', ''C'')
		BEGIN					
			IF @transaction_type = ''C''
			BEGIN
				IF EXISTS(SELECT 1 FROM FTS11T0302 T0 INNER JOIN FTS11A0302 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''O'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0302 SET U_CnclBy = U_UpdateBy, U_CnclDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
				IF EXISTS(SELECT 1 FROM FTS11T0302 T0 INNER JOIN FTS11A0302 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''P'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0302 SET U_RvrsBy = U_UpdateBy, U_RvrsDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
			END
			
			IF @transaction_type = ''C'' 
				OR (@transaction_type = ''U'' )--AND dbo.fn_FTS11_IsChanged_Doc(@object_type, @list_of_cols_val_tab_del) = 1)
				OR (@transaction_type = ''A'' AND EXISTS(SELECT 1 FROM FTS11T0302 WHERE Status = ''O'' AND DocEntry = @list_of_cols_val_tab_del))
			BEGIN
				-- Loop through each row in the document
				SELECT @LineId = MIN(T0.ResultLineID)
				FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0

				WHILE ISNULL(@LineId,0) <> 0
				BEGIN

					SELECT @FAKey = T0.ResultFAKey 
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID = @LineId

					SELECT @DocDate = T0.U_DocDate, @FAKey = T1.U_FAKey 
					FROM FTS11T0302 T0
					INNER JOIN FTS11T0302D T1 ON T0.DocEntry = T1.DocEntry 
					WHERE Object = @object_type AND T0.DocEntry = @list_of_cols_val_tab_del
					AND T1.Oid = @LineId AND T1.U_FAKey = @FAKey
				
					-- Recalculate Depreciation for period before to ensure correct ACCU				
					SELECT @PrdBegin = U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey
					SELECT @PrdBegin = ISNULL(MAX(T0.DocDate), @PrdBegin )
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus = ''P'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @PrdBegin

					EXEC sp_FTS11_GenerateDeprn @PrdBegin, @DocDate, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT	
							
					EXEC sp_FTS11_GenerateJDT1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey
					EXEC sp_FTS11_GenerateFAM1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey	
					--EXEC sp_FTS11_ResetOFAM @FAKey, ''COST''				
				
					-- Recalculate Depreciation for period after					
					SET @PrdEnd = @DocDate				
					SELECT @PrdEnd = ISNULL(MAX(T0.DocDate), @PrdEnd)
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus <> ''C'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @DocDate

					EXEC sp_FTS11_GenerateDeprn @DocDate, @PrdEnd, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT
					EXEC sp_FTS11_SetIsFieldEditable @FAKey
				
					-- Next row
					SELECT @LineId = MIN(T0.ResultLineID)
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID > @LineId 
				END
				
				DELETE T0 FROM FTS11T0302D T0 WHERE ISNULL(T0.DocEntry,0) = 0 
												
			END
		END			
	END -- Revalue / Impairment

	IF @object_type = ''FTS11T0303'' -- Accu Deprn Adjustment
	BEGIN
		IF @transaction_type IN (''A'', ''U'', ''C'')
		BEGIN					
			IF @transaction_type = ''C''
			BEGIN
				IF EXISTS(SELECT 1 FROM FTS11T0303 T0 INNER JOIN FTS11A0303 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''O'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0303 SET U_CnclBy = U_UpdateBy, U_CnclDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
				IF EXISTS(SELECT 1 FROM FTS11T0303 T0 INNER JOIN FTS11A0303 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''P'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0303 SET U_RvrsBy = U_UpdateBy, U_RvrsDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
			END
			
			IF @transaction_type = ''C'' 
				OR (@transaction_type = ''U'' )--AND dbo.fn_FTS11_IsChanged_Doc(@object_type, @list_of_cols_val_tab_del) = 1)
				OR (@transaction_type = ''A'' AND EXISTS(SELECT 1 FROM FTS11T0303 WHERE Status = ''O'' AND DocEntry = @list_of_cols_val_tab_del))
			BEGIN
				-- Loop through each row in the document
				SELECT @LineId = MIN(T0.ResultLineID)
				FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0

				WHILE ISNULL(@LineId,0) <> 0
				BEGIN

					SELECT @FAKey = T0.ResultFAKey 
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID = @LineId

					SELECT @DocDate = T0.U_DocDate, @FAKey = T1.U_FAKey 
					FROM FTS11T0303 T0
					INNER JOIN FTS11T0303D T1 ON T0.DocEntry = T1.DocEntry 
					WHERE Object = @object_type AND T0.DocEntry = @list_of_cols_val_tab_del
					AND T1.Oid = @LineId AND T1.U_FAKey = @FAKey
				
					-- Recalculate Depreciation for period before to ensure correct ACCU				
					SELECT @PrdBegin = U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey
					SELECT @PrdBegin = ISNULL(MAX(T0.DocDate), @PrdBegin )
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus = ''P'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @PrdBegin

					EXEC sp_FTS11_GenerateDeprn @PrdBegin, @DocDate, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT	
							
					EXEC sp_FTS11_GenerateJDT1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey
					EXEC sp_FTS11_GenerateFAM1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey	
					--EXEC sp_FTS11_ResetOFAM @FAKey, ''TRANSCNT''				
				
					-- Recalculate Depreciation for period after					
					SET @PrdEnd = @DocDate				
					SELECT @PrdEnd = ISNULL(MAX(T0.DocDate), @PrdEnd)
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus <> ''C'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @DocDate

					EXEC sp_FTS11_GenerateDeprn @DocDate, @PrdEnd, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT
					EXEC sp_FTS11_SetIsFieldEditable @FAKey
				
					-- Next row
					SELECT @LineId = MIN(T0.ResultLineID)
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID > @LineId 
				END
				
				DELETE T0 FROM FTS11T0303D T0 WHERE ISNULL(T0.DocEntry,0) = 0 
												
			END
		END			
	END -- Accu Deprn Adjustment

	IF @object_type = ''FTS11T0304'' -- Transfer Location
	BEGIN
		/* 
			Does not support partial move at this moment
		*/
		IF @transaction_type IN (''A'', ''U'', ''C'')
		BEGIN					
			IF @transaction_type = ''C''
			BEGIN
				IF EXISTS(SELECT 1 FROM FTS11T0304 T0 INNER JOIN FTS11A0304 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''O'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0304 SET U_CnclBy = U_UpdateBy, U_CnclDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
				
				IF EXISTS(SELECT 1 FROM FTS11T0304 T0 INNER JOIN FTS11A0304 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''P'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0304 SET U_RvrsBy = U_UpdateBy, U_RvrsDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
			END
			
			IF @transaction_type = ''C'' 
				OR (@transaction_type = ''U'' )--AND dbo.fn_FTS11_IsChanged_Doc(@object_type, @list_of_cols_val_tab_del) = 1)
				OR (@transaction_type = ''A'' AND EXISTS(SELECT 1 FROM FTS11T0304 WHERE Status = ''O'' AND DocEntry = @list_of_cols_val_tab_del))
			BEGIN
				-- Loop through each row in the document
				SELECT @LineId = MIN(T0.ResultLineID)
				FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0

				WHILE ISNULL(@LineId,0) <> 0
				BEGIN

					SELECT @FAKey = T0.ResultFAKey 
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID = @LineId

					SELECT @DocDate = T0.U_DocDate, @FAKey = T1.U_FAKey 
					FROM FTS11T0304 T0
					INNER JOIN FTS11T0304D T1 ON T0.DocEntry = T1.DocEntry 
					WHERE Object = @object_type AND T0.DocEntry = @list_of_cols_val_tab_del
					AND T1.Oid = @LineId AND T1.U_FAKey = @FAKey
				
					-- Recalculate Depreciation for period before to ensure correct ACCU				
					SELECT @PrdBegin = U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey
				
					IF EXISTS(SELECT 1 FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus = ''P'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @PrdBegin)
					SELECT @PrdBegin = ISNULL(MAX(T0.DocDate), @PrdBegin )
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus = ''P'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @PrdBegin

				
					EXEC sp_FTS11_GenerateDeprn @PrdBegin, @DocDate, @FAKey, @UserCode, @ErrorMsg OUTPUT	
				
					EXEC sp_FTS11_GenerateJDT1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey					
					EXEC sp_FTS11_GenerateFAM1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey	
					EXEC sp_FTS11_ResetOFAM @FAKey, ''Location''		
				
					-- Recalculate Depreciation for period after					
					SET @PrdEnd = @DocDate				
					SELECT @PrdEnd = ISNULL(MAX(T0.DocDate), @PrdEnd)
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus <> ''C'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @DocDate

					EXEC sp_FTS11_GenerateDeprn @DocDate, @PrdEnd, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT
					EXEC sp_FTS11_SetIsFieldEditable @FAKey
				
					-- Next row
					SELECT @LineId = MIN(T0.ResultLineID)
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID > @LineId 
				END
				
				DELETE T0 FROM FTS11T0304D T0 WHERE ISNULL(T0.DocEntry,0) = 0 
														
			END
		END			
	END -- Transfer Location	
	
	IF @object_type = ''FTS11T0305'' -- Revalue Residual
	BEGIN
		IF @transaction_type IN (''A'', ''U'', ''C'')
		BEGIN					
			IF @transaction_type = ''C''
			BEGIN
				IF EXISTS(SELECT 1 FROM FTS11T0305 T0 INNER JOIN FTS11A0305 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''O'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0305 SET U_CnclBy = U_UpdateBy, U_CnclDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
				IF EXISTS(SELECT 1 FROM FTS11T0305 T0 INNER JOIN FTS11A0305 T1 ON T0.Object = T1.Object AND T0.DocEntry = T1.DocEntry AND T0.LogInst = T1.LogInst + 1
				WHERE T0.Status = ''C'' AND T1.Status = ''P'' AND T0.DocEntry = @list_of_cols_val_tab_del)
				BEGIN
					UPDATE FTS11T0305 SET U_RvrsBy = U_UpdateBy, U_RvrsDate = GETDATE() WHERE DocEntry = @list_of_cols_val_tab_del
				END		
			END
			
			IF @transaction_type = ''C'' 
				OR (@transaction_type = ''U'' )--AND dbo.fn_FTS11_IsChanged_Doc(@object_type, @list_of_cols_val_tab_del) = 1)
				OR (@transaction_type = ''A'' AND EXISTS(SELECT 1 FROM FTS11T0305 WHERE Status = ''O'' AND DocEntry = @list_of_cols_val_tab_del))
			BEGIN
				-- Loop through each row in the document
				SELECT @LineId = MIN(T0.ResultLineID)
				FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0

				WHILE ISNULL(@LineId,0) <> 0
				BEGIN

					SELECT @FAKey = T0.ResultFAKey 
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID = @LineId

					SELECT @DocDate = T0.U_DocDate, @FAKey = T1.U_FAKey 
					FROM FTS11T0305 T0
					INNER JOIN FTS11T0305D T1 ON T0.DocEntry = T1.DocEntry 
					WHERE Object = @object_type AND T0.DocEntry = @list_of_cols_val_tab_del
					AND T1.Oid = @LineId AND T1.U_FAKey = @FAKey
				
					-- Recalculate Depreciation for period before to ensure correct ACCU				
					SELECT @PrdBegin = U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey
					SELECT @PrdBegin = ISNULL(MAX(T0.DocDate), @PrdBegin )
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus = ''P'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @PrdBegin

					EXEC sp_FTS11_GenerateDeprn @PrdBegin, @DocDate, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT	
							
					EXEC sp_FTS11_GenerateJDT1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey
					EXEC sp_FTS11_GenerateFAM1 @object_type, @transaction_type, @num_of_cols_in_key, @list_of_key_cols_tab_del, @list_of_cols_val_tab_del, @FAKey
					--EXEC sp_FTS11_ResetOFAM @FAKey, ''RESIVALUE''				
				
					-- Recalculate Depreciation for period after					
					SET @PrdEnd = @DocDate				
					SELECT @PrdEnd = ISNULL(MAX(T0.DocDate), @PrdEnd)
					FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus <> ''C'' AND T0.AcctCat = ''ACCU'' AND T1.U_TrnsGrp = ''DEP'' AND T0.FAKey = @FAKey
					AND T0.DocDate >= @DocDate

					EXEC sp_FTS11_GenerateDeprn @DocDate, @PrdEnd, @FAKey, @UserCode, @ErrorMsg = @ErrorMsg OUTPUT
					EXEC sp_FTS11_SetIsFieldEditable @FAKey
				
					-- Next row
					SELECT @LineId = MIN(T0.ResultLineID)
					FROM fn_FTS11_GetDocItemChanged (@object_type, @list_of_cols_val_tab_del) T0
					WHERE T0.ResultLineID > @LineId 
				END
				
				DELETE T0 FROM FTS11T0305D T0 WHERE ISNULL(T0.DocEntry,0) = 0
												
			END
		END			
	END -- Revalue Residual
	--------------------------------------------------------------------------------------------------------------------------------

	-- Select the return values
	select @error, @error_message
	
	SELECT @LogNote = ''End Procedure. '' + @error_message
	EXEC sp_FTS11_DebugLog ''SBO_SP_PostTransactionNotice'', @object_type, @list_of_cols_val_tab_del, @LogNote
end

');
    PRINT DB_NAME() + ': SBO_SP_PostTransactionNotice replaced with the standard version (old FTS11FAM2 insert).';
END
GO

-- 0e. sp_FTS11_RegenerateView - remove the old "Level 5 only" filter on the G/L account views
DECLARE @def NVARCHAR(MAX) = OBJECT_DEFINITION(OBJECT_ID('dbo.sp_FTS11_RegenerateView'));
IF @def LIKE '%Levels = ''''5'''' AND %' AND PATINDEX('%CREATE PROC%', @def) > 0
BEGIN
    SET @def = REPLACE(@def, 'Levels = ''''5'''' AND ', '');
    SET @def = STUFF(@def, PATINDEX('%CREATE PROC%', @def), LEN('CREATE'), 'ALTER');
    EXEC (@def);
    PRINT DB_NAME() + ': sp_FTS11_RegenerateView updated (all postable G/L accounts). Views refresh the next time Company Setup is saved.';
END
GO

-- 0f. SE_FASvrTyp - add MSSQL 2022
IF OBJECT_DEFINITION(OBJECT_ID('dbo.SE_FASvrTyp')) NOT LIKE '%MSSQL2022%'
    EXEC (N'ALTER VIEW [dbo].[SE_FASvrTyp] AS
SELECT ''MSSQL2005'' AS [Code], ''MSSQL 2005'' AS [Name]
UNION ALL SELECT ''MSSQL2008'', ''MSSQL 2008''
UNION ALL SELECT ''MSSQL2012'', ''MSSQL 2012''
UNION ALL SELECT ''MSSQL2014'', ''MSSQL 2014''
UNION ALL SELECT ''MSSQL2016'', ''MSSQL 2016''
UNION ALL SELECT ''MSSQL2017'', ''MSSQL 2017''
UNION ALL SELECT ''MSSQL2019'', ''MSSQL 2019''
UNION ALL SELECT ''MSSQL2022'', ''MSSQL 2022''');
PRINT DB_NAME() + ': earlier-version items checked.';
GO


/* #####################################################################################
   1. Password Administration
   ##################################################################################### */
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
GO

/* #####################################################################################
   2. Depreciation speed-up indexes
   ##################################################################################### */
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
/* Undo (only if ever needed):
   DROP INDEX IX_FTS11JDT1_FAKey    ON dbo.FTS11JDT1;
   DROP INDEX IX_FTS11JDT1_DocEntry ON dbo.FTS11JDT1;  */
GO

/* #####################################################################################
   3. Depreciation run log (progress window, block posting during a run, error list)
   ##################################################################################### */
-- The program also creates these on the first Run Depreciation; they are created here in case
-- the program's SQL login has no CREATE TABLE right.
-- Status: R Running, F Finished, E Error/stopped, X Interrupted (no progress for 10 minutes)
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
END
IF COL_LENGTH('dbo.FTS11ODRL', 'StopReq') IS NULL
    ALTER TABLE dbo.FTS11ODRL ADD StopReq BIT NOT NULL CONSTRAINT DF_FTS11ODRL_StopReq DEFAULT 0;
IF OBJECT_ID('dbo.FTS11DRL1') IS NULL
    CREATE TABLE dbo.FTS11DRL1 (
        RunEntry  INT NOT NULL,
        LineNum   INT NOT NULL,
        FAKey     INT NULL,
        FACode    NVARCHAR(200) NULL,
        Message   NVARCHAR(1000) NULL,
        CONSTRAINT PK_FTS11DRL1 PRIMARY KEY (RunEntry, LineNum)
    );
PRINT 'Depreciation run log OK on ' + DB_NAME();
/* Release a stuck run manually (normally not needed, it expires after 10 minutes without progress):
   UPDATE dbo.FTS11ODRL SET Status = 'X', EndDate = GETDATE(), Note = 'Released manually' WHERE Status = 'R';  */
GO

/* #####################################################################################
   4. FTS_fn_FADeprList - Run Depreciation asset list (31 parameters)
   ##################################################################################### */
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

/* #####################################################################################
   5. sp_FTS11_PrepareJEPreview - Posting Widget Project / Cost Centre D1-D5 filter
   ##################################################################################### */
-- =============================================================================
-- sp_FTS11_PrepareJEPreview - add Project and Cost Centre D1-D5 filters (2026-11-22)
-- Posting Widget: Preview / Post now respect Project From/To and Cost Centre D1-D5 From/To.
-- * 12 new parameters at the END, all default '' -> old calls (17 parameters) still work.
-- * Filter on the journal line (FTS11JDT1) values, the same values that are posted to SAP.
-- * Built from the FA_DEVELOPMENT version. Compare with each company DB's own version
--   before running it there.
-- =============================================================================
ALTER PROCEDURE [dbo].[sp_FTS11_PrepareJEPreview]
	@DateFr_ DateTime, @DateTo_ DateTime, @FAClsCodFr_ NVARCHAR(50), @FAClsCodTo_ NVARCHAR(50), @FAGrpCodFr_ NVARCHAR(50), @FAGrpCodTo_ NVARCHAR(50), @FACodeFr_ NVARCHAR(50), @FACodeTo_ NVARCHAR(50), 
	@JESplit_ NVARCHAR(50), @RefDate_ DATETIME, @UserID_ NVARCHAR(50), @Series_ INT, @Ref1_ NVARCHAR(50), @Ref2_ NVARCHAR(50), @Memo_ NVARCHAR(50), @PostTo_ NVARCHAR(10), @FileType_ NVARCHAR(10),
	-- 2026-11-22: Project / Cost Centre D1-D5 from-to filters (optional, blank = all). Matched on the journal line values that get posted.
	@ProjectFr_ NVARCHAR(100) = '', @ProjectTo_ NVARCHAR(100) = '', @ProfitCoFr_ NVARCHAR(100) = '', @ProfitCoTo_ NVARCHAR(100) = '', @OcrCode2Fr_ NVARCHAR(100) = '', @OcrCode2To_ NVARCHAR(100) = '', @OcrCode3Fr_ NVARCHAR(100) = '', @OcrCode3To_ NVARCHAR(100) = '', @OcrCode4Fr_ NVARCHAR(100) = '', @OcrCode4To_ NVARCHAR(100) = '', @OcrCode5Fr_ NVARCHAR(100) = '', @OcrCode5To_ NVARCHAR(100) = ''
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @DateFr DateTime, @DateTo DateTime, @FAClsCodFr NVARCHAR(50), @FAClsCodTo NVARCHAR(50), @FAGrpCodFr NVARCHAR(50), @FAGrpCodTo NVARCHAR(50), @FACodeFr NVARCHAR(50), @FACodeTo NVARCHAR(50), 
	@JESplit NVARCHAR(50), @RefDate DATETIME, @UserID NVARCHAR(50), @Series INT, @Ref1 NVARCHAR(50), @Ref2 NVARCHAR(50), @Memo NVARCHAR(50), @PostTo NVARCHAR(10), @FileType NVARCHAR(10)
	SELECT @DateFr = @DateFr_, @DateTo = @DateTo_, @FAClsCodFr = @FAClsCodFr_, @FAClsCodTo = @FAClsCodTo_, @FAGrpCodFr = @FAGrpCodFr_, @FAGrpCodTo = @FAGrpCodTo_, @FACodeFr = @FACodeFr_, @FACodeTo = @FACodeTo_, 
	@JESplit = @JESplit_, @RefDate = @RefDate_, @UserID = @UserID_, @Series = @Series_, @Ref1 = @Ref1_, @Ref2 = @Ref2_, @Memo = @Memo_, @PostTo = @PostTo_, @FileType = @FileType_
	--SELECT @DateFr = '20150701', @DatetO = '20150731', @FAClsCodFr = '', @FAClsCodTo = '', @FAGrpCodFr = '', @FAGrpCodTo = '', @FACodeFr = '', @FACodeTo = '', 
	--@JESplit = 'faclsnam_ocrcode3', @RefDate = '20150731', @UserID = '', @Series = 14, @Ref1 = '', @Ref2 = '', @Memo = 'Posting from Asset System', @PostTo = 'SBO', @FileType = 'JV'
	
	DECLARE @DocEntry INT
	DECLARE @IndexTable TABLE (IndexKey NVARCHAR(100), IndexNum INT)

	IF @JESplit = 'fagrpnam'
	BEGIN
		INSERT INTO @IndexTable
		SELECT T2.U_FAGrpNam, ROW_NUMBER() OVER (ORDER BY T2.U_FAGrpNam) - 1
		FROM FTS11JDT1 T0 INNER JOIN FTS11OFAM T1 ON T0.FaKey = T1.DocEntry
		INNER JOIN FTS11OFAG T2 ON T1.U_FAGrpKey = T2.DocEntry 
		INNER JOIN FTS11OFAC T3 ON T2.U_FAClsCod = T3.Code COLLATE DATABASE_DEFAULT
		WHERE T0.TransStatus = 'O'
		AND T0.DocDate BETWEEN @DateFr AND @DateTo
		AND (T3.Code >= @FAClsCodFr OR @FAClsCodFr = '') AND (T3.Code <= @FAClsCodTo OR @FAClsCodTo = '')
		AND (T2.U_FAGrpCod >= @FAGrpCodFr OR @FAGrpCodFr = '') AND (T2.U_FAGrpCod <= @FAGrpCodTo OR @FAGrpCodTo = '')
		AND (T1.U_FACode >= @FACodeFr OR @FACodeFr = '') AND (T1.U_FACode <= @FACodeTo OR @FACodeTo = '')
 AND (ISNULL(T0.Project,'') >= @ProjectFr_ OR @ProjectFr_ = '') AND (ISNULL(T0.Project,'') <= @ProjectTo_ OR @ProjectTo_ = '')
 AND (ISNULL(T0.ProfitCo,'') >= @ProfitCoFr_ OR @ProfitCoFr_ = '') AND (ISNULL(T0.ProfitCo,'') <= @ProfitCoTo_ OR @ProfitCoTo_ = '')
 AND (ISNULL(T0.OcrCode2,'') >= @OcrCode2Fr_ OR @OcrCode2Fr_ = '') AND (ISNULL(T0.OcrCode2,'') <= @OcrCode2To_ OR @OcrCode2To_ = '')
 AND (ISNULL(T0.OcrCode3,'') >= @OcrCode3Fr_ OR @OcrCode3Fr_ = '') AND (ISNULL(T0.OcrCode3,'') <= @OcrCode3To_ OR @OcrCode3To_ = '')
 AND (ISNULL(T0.OcrCode4,'') >= @OcrCode4Fr_ OR @OcrCode4Fr_ = '') AND (ISNULL(T0.OcrCode4,'') <= @OcrCode4To_ OR @OcrCode4To_ = '')
 AND (ISNULL(T0.OcrCode5,'') >= @OcrCode5Fr_ OR @OcrCode5Fr_ = '') AND (ISNULL(T0.OcrCode5,'') <= @OcrCode5To_ OR @OcrCode5To_ = '')
		GROUP BY T2.U_FAGrpNam
	END
	ELSE IF @JESplit = 'faclsnam'
	BEGIN
		INSERT INTO @IndexTable
		SELECT T3.Name, ROW_NUMBER() OVER (ORDER BY T3.Name) - 1
		FROM FTS11JDT1 T0 INNER JOIN FTS11OFAM T1 ON T0.FaKey = T1.DocEntry
		INNER JOIN FTS11OFAG T2 ON T1.U_FAGrpKey = T2.DocEntry 
		INNER JOIN FTS11OFAC T3 ON T2.U_FAClsCod = T3.Code COLLATE DATABASE_DEFAULT
		WHERE T0.TransStatus = 'O'
		AND T0.DocDate BETWEEN @DateFr AND @DateTo
		AND (T3.Code >= @FAClsCodFr OR @FAClsCodFr = '') AND (T3.Code <= @FAClsCodTo OR @FAClsCodTo = '')
		AND (T2.U_FAGrpCod >= @FAGrpCodFr OR @FAGrpCodFr = '') AND (T2.U_FAGrpCod <= @FAGrpCodTo OR @FAGrpCodTo = '')
		AND (T1.U_FACode >= @FACodeFr OR @FACodeFr = '') AND (T1.U_FACode <= @FACodeTo OR @FACodeTo = '')
 AND (ISNULL(T0.Project,'') >= @ProjectFr_ OR @ProjectFr_ = '') AND (ISNULL(T0.Project,'') <= @ProjectTo_ OR @ProjectTo_ = '')
 AND (ISNULL(T0.ProfitCo,'') >= @ProfitCoFr_ OR @ProfitCoFr_ = '') AND (ISNULL(T0.ProfitCo,'') <= @ProfitCoTo_ OR @ProfitCoTo_ = '')
 AND (ISNULL(T0.OcrCode2,'') >= @OcrCode2Fr_ OR @OcrCode2Fr_ = '') AND (ISNULL(T0.OcrCode2,'') <= @OcrCode2To_ OR @OcrCode2To_ = '')
 AND (ISNULL(T0.OcrCode3,'') >= @OcrCode3Fr_ OR @OcrCode3Fr_ = '') AND (ISNULL(T0.OcrCode3,'') <= @OcrCode3To_ OR @OcrCode3To_ = '')
 AND (ISNULL(T0.OcrCode4,'') >= @OcrCode4Fr_ OR @OcrCode4Fr_ = '') AND (ISNULL(T0.OcrCode4,'') <= @OcrCode4To_ OR @OcrCode4To_ = '')
 AND (ISNULL(T0.OcrCode5,'') >= @OcrCode5Fr_ OR @OcrCode5Fr_ = '') AND (ISNULL(T0.OcrCode5,'') <= @OcrCode5To_ OR @OcrCode5To_ = '')
		GROUP BY T3.Name
	END
	ELSE IF @JESplit = 'faclsnam_profitco'
	BEGIN
		INSERT INTO @IndexTable
		SELECT T3.Name + '_' + ISNULL(T0.ProfitCo,''), ROW_NUMBER() OVER (ORDER BY T3.Name, ISNULL(T0.ProfitCo,'')) - 1
		FROM FTS11JDT1 T0 INNER JOIN FTS11OFAM T1 ON T0.FaKey = T1.DocEntry
		INNER JOIN FTS11OFAG T2 ON T1.U_FAGrpKey = T2.DocEntry 
		INNER JOIN FTS11OFAC T3 ON T2.U_FAClsCod = T3.Code COLLATE DATABASE_DEFAULT
		WHERE T0.TransStatus = 'O'
		AND T0.DocDate BETWEEN @DateFr AND @DateTo
		AND (T3.Code >= @FAClsCodFr OR @FAClsCodFr = '') AND (T3.Code <= @FAClsCodTo OR @FAClsCodTo = '')
		AND (T2.U_FAGrpCod >= @FAGrpCodFr OR @FAGrpCodFr = '') AND (T2.U_FAGrpCod <= @FAGrpCodTo OR @FAGrpCodTo = '')
		AND (T1.U_FACode >= @FACodeFr OR @FACodeFr = '') AND (T1.U_FACode <= @FACodeTo OR @FACodeTo = '')
 AND (ISNULL(T0.Project,'') >= @ProjectFr_ OR @ProjectFr_ = '') AND (ISNULL(T0.Project,'') <= @ProjectTo_ OR @ProjectTo_ = '')
 AND (ISNULL(T0.ProfitCo,'') >= @ProfitCoFr_ OR @ProfitCoFr_ = '') AND (ISNULL(T0.ProfitCo,'') <= @ProfitCoTo_ OR @ProfitCoTo_ = '')
 AND (ISNULL(T0.OcrCode2,'') >= @OcrCode2Fr_ OR @OcrCode2Fr_ = '') AND (ISNULL(T0.OcrCode2,'') <= @OcrCode2To_ OR @OcrCode2To_ = '')
 AND (ISNULL(T0.OcrCode3,'') >= @OcrCode3Fr_ OR @OcrCode3Fr_ = '') AND (ISNULL(T0.OcrCode3,'') <= @OcrCode3To_ OR @OcrCode3To_ = '')
 AND (ISNULL(T0.OcrCode4,'') >= @OcrCode4Fr_ OR @OcrCode4Fr_ = '') AND (ISNULL(T0.OcrCode4,'') <= @OcrCode4To_ OR @OcrCode4To_ = '')
 AND (ISNULL(T0.OcrCode5,'') >= @OcrCode5Fr_ OR @OcrCode5Fr_ = '') AND (ISNULL(T0.OcrCode5,'') <= @OcrCode5To_ OR @OcrCode5To_ = '')
		GROUP BY T3.Name, ISNULL(T0.ProfitCo,'')
	END
	ELSE IF @JESplit = 'faclsnam_ocrcode2'
	BEGIN
		INSERT INTO @IndexTable
		SELECT T3.Name + '_' + ISNULL(T0.OcrCode2,''), ROW_NUMBER() OVER (ORDER BY T3.Name, ISNULL(T0.OcrCode2,'')) - 1
		FROM FTS11JDT1 T0 INNER JOIN FTS11OFAM T1 ON T0.FaKey = T1.DocEntry
		INNER JOIN FTS11OFAG T2 ON T1.U_FAGrpKey = T2.DocEntry 
		INNER JOIN FTS11OFAC T3 ON T2.U_FAClsCod = T3.Code COLLATE DATABASE_DEFAULT
		WHERE T0.TransStatus = 'O'
		AND T0.DocDate BETWEEN @DateFr AND @DateTo
		AND (T3.Code >= @FAClsCodFr OR @FAClsCodFr = '') AND (T3.Code <= @FAClsCodTo OR @FAClsCodTo = '')
		AND (T2.U_FAGrpCod >= @FAGrpCodFr OR @FAGrpCodFr = '') AND (T2.U_FAGrpCod <= @FAGrpCodTo OR @FAGrpCodTo = '')
		AND (T1.U_FACode >= @FACodeFr OR @FACodeFr = '') AND (T1.U_FACode <= @FACodeTo OR @FACodeTo = '')
 AND (ISNULL(T0.Project,'') >= @ProjectFr_ OR @ProjectFr_ = '') AND (ISNULL(T0.Project,'') <= @ProjectTo_ OR @ProjectTo_ = '')
 AND (ISNULL(T0.ProfitCo,'') >= @ProfitCoFr_ OR @ProfitCoFr_ = '') AND (ISNULL(T0.ProfitCo,'') <= @ProfitCoTo_ OR @ProfitCoTo_ = '')
 AND (ISNULL(T0.OcrCode2,'') >= @OcrCode2Fr_ OR @OcrCode2Fr_ = '') AND (ISNULL(T0.OcrCode2,'') <= @OcrCode2To_ OR @OcrCode2To_ = '')
 AND (ISNULL(T0.OcrCode3,'') >= @OcrCode3Fr_ OR @OcrCode3Fr_ = '') AND (ISNULL(T0.OcrCode3,'') <= @OcrCode3To_ OR @OcrCode3To_ = '')
 AND (ISNULL(T0.OcrCode4,'') >= @OcrCode4Fr_ OR @OcrCode4Fr_ = '') AND (ISNULL(T0.OcrCode4,'') <= @OcrCode4To_ OR @OcrCode4To_ = '')
 AND (ISNULL(T0.OcrCode5,'') >= @OcrCode5Fr_ OR @OcrCode5Fr_ = '') AND (ISNULL(T0.OcrCode5,'') <= @OcrCode5To_ OR @OcrCode5To_ = '')
		GROUP BY T3.Name, ISNULL(T0.OcrCode2,'')
	END
	ELSE IF @JESplit = 'faclsnam_ocrcode3'
	BEGIN
		INSERT INTO @IndexTable
		SELECT T3.Name + '_' + ISNULL(T0.OcrCode3,''), ROW_NUMBER() OVER (ORDER BY T3.Name, ISNULL(T0.OcrCode3,'')) - 1
		FROM FTS11JDT1 T0 INNER JOIN FTS11OFAM T1 ON T0.FaKey = T1.DocEntry
		INNER JOIN FTS11OFAG T2 ON T1.U_FAGrpKey = T2.DocEntry 
		INNER JOIN FTS11OFAC T3 ON T2.U_FAClsCod = T3.Code COLLATE DATABASE_DEFAULT
		WHERE T0.TransStatus = 'O'
		AND T0.DocDate BETWEEN @DateFr AND @DateTo
		AND (T3.Code >= @FAClsCodFr OR @FAClsCodFr = '') AND (T3.Code <= @FAClsCodTo OR @FAClsCodTo = '')
		AND (T2.U_FAGrpCod >= @FAGrpCodFr OR @FAGrpCodFr = '') AND (T2.U_FAGrpCod <= @FAGrpCodTo OR @FAGrpCodTo = '')
		AND (T1.U_FACode >= @FACodeFr OR @FACodeFr = '') AND (T1.U_FACode <= @FACodeTo OR @FACodeTo = '')
 AND (ISNULL(T0.Project,'') >= @ProjectFr_ OR @ProjectFr_ = '') AND (ISNULL(T0.Project,'') <= @ProjectTo_ OR @ProjectTo_ = '')
 AND (ISNULL(T0.ProfitCo,'') >= @ProfitCoFr_ OR @ProfitCoFr_ = '') AND (ISNULL(T0.ProfitCo,'') <= @ProfitCoTo_ OR @ProfitCoTo_ = '')
 AND (ISNULL(T0.OcrCode2,'') >= @OcrCode2Fr_ OR @OcrCode2Fr_ = '') AND (ISNULL(T0.OcrCode2,'') <= @OcrCode2To_ OR @OcrCode2To_ = '')
 AND (ISNULL(T0.OcrCode3,'') >= @OcrCode3Fr_ OR @OcrCode3Fr_ = '') AND (ISNULL(T0.OcrCode3,'') <= @OcrCode3To_ OR @OcrCode3To_ = '')
 AND (ISNULL(T0.OcrCode4,'') >= @OcrCode4Fr_ OR @OcrCode4Fr_ = '') AND (ISNULL(T0.OcrCode4,'') <= @OcrCode4To_ OR @OcrCode4To_ = '')
 AND (ISNULL(T0.OcrCode5,'') >= @OcrCode5Fr_ OR @OcrCode5Fr_ = '') AND (ISNULL(T0.OcrCode5,'') <= @OcrCode5To_ OR @OcrCode5To_ = '')
		GROUP BY T3.Name, ISNULL(T0.OcrCode3,'')
	END
	ELSE IF @JESplit = 'faclsnam_ocrcode4'
	BEGIN
		INSERT INTO @IndexTable
		SELECT T3.Name + '_' + ISNULL(T0.OcrCode4,''), ROW_NUMBER() OVER (ORDER BY T3.Name, ISNULL(T0.OcrCode4,'')) - 1
		FROM FTS11JDT1 T0 INNER JOIN FTS11OFAM T1 ON T0.FaKey = T1.DocEntry
		INNER JOIN FTS11OFAG T2 ON T1.U_FAGrpKey = T2.DocEntry 
		INNER JOIN FTS11OFAC T3 ON T2.U_FAClsCod = T3.Code COLLATE DATABASE_DEFAULT
		WHERE T0.TransStatus = 'O'
		AND T0.DocDate BETWEEN @DateFr AND @DateTo
		AND (T3.Code >= @FAClsCodFr OR @FAClsCodFr = '') AND (T3.Code <= @FAClsCodTo OR @FAClsCodTo = '')
		AND (T2.U_FAGrpCod >= @FAGrpCodFr OR @FAGrpCodFr = '') AND (T2.U_FAGrpCod <= @FAGrpCodTo OR @FAGrpCodTo = '')
		AND (T1.U_FACode >= @FACodeFr OR @FACodeFr = '') AND (T1.U_FACode <= @FACodeTo OR @FACodeTo = '')
 AND (ISNULL(T0.Project,'') >= @ProjectFr_ OR @ProjectFr_ = '') AND (ISNULL(T0.Project,'') <= @ProjectTo_ OR @ProjectTo_ = '')
 AND (ISNULL(T0.ProfitCo,'') >= @ProfitCoFr_ OR @ProfitCoFr_ = '') AND (ISNULL(T0.ProfitCo,'') <= @ProfitCoTo_ OR @ProfitCoTo_ = '')
 AND (ISNULL(T0.OcrCode2,'') >= @OcrCode2Fr_ OR @OcrCode2Fr_ = '') AND (ISNULL(T0.OcrCode2,'') <= @OcrCode2To_ OR @OcrCode2To_ = '')
 AND (ISNULL(T0.OcrCode3,'') >= @OcrCode3Fr_ OR @OcrCode3Fr_ = '') AND (ISNULL(T0.OcrCode3,'') <= @OcrCode3To_ OR @OcrCode3To_ = '')
 AND (ISNULL(T0.OcrCode4,'') >= @OcrCode4Fr_ OR @OcrCode4Fr_ = '') AND (ISNULL(T0.OcrCode4,'') <= @OcrCode4To_ OR @OcrCode4To_ = '')
 AND (ISNULL(T0.OcrCode5,'') >= @OcrCode5Fr_ OR @OcrCode5Fr_ = '') AND (ISNULL(T0.OcrCode5,'') <= @OcrCode5To_ OR @OcrCode5To_ = '')
		GROUP BY T3.Name, ISNULL(T0.OcrCode4,'')
	END
	ELSE IF @JESplit = 'faclsnam_ocrcode5'
	BEGIN
		INSERT INTO @IndexTable
		SELECT T3.Name + '_' + ISNULL(T0.OcrCode5,''), ROW_NUMBER() OVER (ORDER BY T3.Name, ISNULL(T0.OcrCode5,'')) - 1
		FROM FTS11JDT1 T0 INNER JOIN FTS11OFAM T1 ON T0.FaKey = T1.DocEntry
		INNER JOIN FTS11OFAG T2 ON T1.U_FAGrpKey = T2.DocEntry 
		INNER JOIN FTS11OFAC T3 ON T2.U_FAClsCod = T3.Code COLLATE DATABASE_DEFAULT
		WHERE T0.TransStatus = 'O'
		AND T0.DocDate BETWEEN @DateFr AND @DateTo
		AND (T3.Code >= @FAClsCodFr OR @FAClsCodFr = '') AND (T3.Code <= @FAClsCodTo OR @FAClsCodTo = '')
		AND (T2.U_FAGrpCod >= @FAGrpCodFr OR @FAGrpCodFr = '') AND (T2.U_FAGrpCod <= @FAGrpCodTo OR @FAGrpCodTo = '')
		AND (T1.U_FACode >= @FACodeFr OR @FACodeFr = '') AND (T1.U_FACode <= @FACodeTo OR @FACodeTo = '')
 AND (ISNULL(T0.Project,'') >= @ProjectFr_ OR @ProjectFr_ = '') AND (ISNULL(T0.Project,'') <= @ProjectTo_ OR @ProjectTo_ = '')
 AND (ISNULL(T0.ProfitCo,'') >= @ProfitCoFr_ OR @ProfitCoFr_ = '') AND (ISNULL(T0.ProfitCo,'') <= @ProfitCoTo_ OR @ProfitCoTo_ = '')
 AND (ISNULL(T0.OcrCode2,'') >= @OcrCode2Fr_ OR @OcrCode2Fr_ = '') AND (ISNULL(T0.OcrCode2,'') <= @OcrCode2To_ OR @OcrCode2To_ = '')
 AND (ISNULL(T0.OcrCode3,'') >= @OcrCode3Fr_ OR @OcrCode3Fr_ = '') AND (ISNULL(T0.OcrCode3,'') <= @OcrCode3To_ OR @OcrCode3To_ = '')
 AND (ISNULL(T0.OcrCode4,'') >= @OcrCode4Fr_ OR @OcrCode4Fr_ = '') AND (ISNULL(T0.OcrCode4,'') <= @OcrCode4To_ OR @OcrCode4To_ = '')
 AND (ISNULL(T0.OcrCode5,'') >= @OcrCode5Fr_ OR @OcrCode5Fr_ = '') AND (ISNULL(T0.OcrCode5,'') <= @OcrCode5To_ OR @OcrCode5To_ = '')
		GROUP BY T3.Name, ISNULL(T0.OcrCode5,'')
	END
	ELSE IF @JESplit = 'faclsnam_project'
	BEGIN
		INSERT INTO @IndexTable
		SELECT T3.Name + '_' + ISNULL(T0.Project,''), ROW_NUMBER() OVER (ORDER BY T3.Name, ISNULL(T0.Project,'')) - 1
		FROM FTS11JDT1 T0 INNER JOIN FTS11OFAM T1 ON T0.FaKey = T1.DocEntry
		INNER JOIN FTS11OFAG T2 ON T1.U_FAGrpKey = T2.DocEntry 
		INNER JOIN FTS11OFAC T3 ON T2.U_FAClsCod = T3.Code COLLATE DATABASE_DEFAULT
		WHERE T0.TransStatus = 'O'
		AND T0.DocDate BETWEEN @DateFr AND @DateTo
		AND (T3.Code >= @FAClsCodFr OR @FAClsCodFr = '') AND (T3.Code <= @FAClsCodTo OR @FAClsCodTo = '')
		AND (T2.U_FAGrpCod >= @FAGrpCodFr OR @FAGrpCodFr = '') AND (T2.U_FAGrpCod <= @FAGrpCodTo OR @FAGrpCodTo = '')
		AND (T1.U_FACode >= @FACodeFr OR @FACodeFr = '') AND (T1.U_FACode <= @FACodeTo OR @FACodeTo = '')
 AND (ISNULL(T0.Project,'') >= @ProjectFr_ OR @ProjectFr_ = '') AND (ISNULL(T0.Project,'') <= @ProjectTo_ OR @ProjectTo_ = '')
 AND (ISNULL(T0.ProfitCo,'') >= @ProfitCoFr_ OR @ProfitCoFr_ = '') AND (ISNULL(T0.ProfitCo,'') <= @ProfitCoTo_ OR @ProfitCoTo_ = '')
 AND (ISNULL(T0.OcrCode2,'') >= @OcrCode2Fr_ OR @OcrCode2Fr_ = '') AND (ISNULL(T0.OcrCode2,'') <= @OcrCode2To_ OR @OcrCode2To_ = '')
 AND (ISNULL(T0.OcrCode3,'') >= @OcrCode3Fr_ OR @OcrCode3Fr_ = '') AND (ISNULL(T0.OcrCode3,'') <= @OcrCode3To_ OR @OcrCode3To_ = '')
 AND (ISNULL(T0.OcrCode4,'') >= @OcrCode4Fr_ OR @OcrCode4Fr_ = '') AND (ISNULL(T0.OcrCode4,'') <= @OcrCode4To_ OR @OcrCode4To_ = '')
 AND (ISNULL(T0.OcrCode5,'') >= @OcrCode5Fr_ OR @OcrCode5Fr_ = '') AND (ISNULL(T0.OcrCode5,'') <= @OcrCode5To_ OR @OcrCode5To_ = '')
		GROUP BY T3.Name, ISNULL(T0.Project,'')
	END
	ELSE
	BEGIN	
		INSERT INTO @IndexTable
		SELECT '', 0
	END

	-- Get Latest OJDT Key and reset OJDT Key in none posted journals	
	SELECT @DocEntry = ISNULL(MAX(DocEntry),0) + 1 FROM FTS11OJDT
	--SELECT @DocEntry = nxt_sk_no FROM sysprm WHERE sk_code = 'FTS11OJDT'
	UPDATE FTS11JDT1 SET OJDTKey = 0 WHERE TransStatus = 'O'

	-- Assign OJDT Key to none posted journals
	UPDATE T0 SET T0.OJDTKey = @DocEntry + T4.IndexNum
	FROM FTS11JDT1 T0
	INNER JOIN FTS11STRN T0A ON T0.TransType = T0A.Code AND T0A.U_Post = 'True' COLLATE DATABASE_DEFAULT
	INNER JOIN FTS11OFAM T1 ON T0.FaKey = T1.DocEntry
	INNER JOIN FTS11OFAG T2 ON T1.U_FAGrpKey = T2.DocEntry
	INNER JOIN FTS11OFAC T3 ON T2.U_FAClsCod = T3.Code COLLATE DATABASE_DEFAULT
	INNER JOIN @IndexTable T4 ON T4.IndexKey =
		CASE @JESplit WHEN 'fagrpnam' THEN T2.U_FAGrpNam 
		WHEN 'faclsnam' THEN T3.Name 
		WHEN 'faclsnam_profitco' THEN T3.Name + '_' + ISNULL(T1.U_ProfitCode,'') 
		WHEN 'faclsnam_ocrcode2' THEN T3.Name + '_' + ISNULL(T1.U_OcrCode2,'') 
		WHEN 'faclsnam_ocrcode3' THEN T3.Name + '_' + ISNULL(T1.U_OcrCode3,'') 
		WHEN 'faclsnam_ocrcode4' THEN T3.Name + '_' + ISNULL(T1.U_OcrCode4,'') 
		WHEN 'faclsnam_ocrcode5' THEN T3.Name + '_' + ISNULL(T1.U_OcrCode5,'') 
		WHEN 'faclsnam_project' THEN T3.Name + '_' + ISNULL(T1.U_Project,'') 
		ELSE '' END
	WHERE T0.TransStatus = 'O' 
	AND T0.DocDate BETWEEN @DateFr AND @DateTo
	AND (T3.Code >= @FAClsCodFr OR @FAClsCodFr = '') AND (T3.Code <= @FAClsCodTo OR @FAClsCodTo = '')
	AND (T2.U_FAGrpCod >= @FAGrpCodFr OR @FAGrpCodFr = '') AND (T2.U_FAGrpCod <= @FAGrpCodTo OR @FAGrpCodTo = '')
	AND (T1.U_FACode >= @FACodeFr OR @FACodeFr = '') AND (T1.U_FACode <= @FACodeTo OR @FACodeTo = '')
 AND (ISNULL(T0.Project,'') >= @ProjectFr_ OR @ProjectFr_ = '') AND (ISNULL(T0.Project,'') <= @ProjectTo_ OR @ProjectTo_ = '')
 AND (ISNULL(T0.ProfitCo,'') >= @ProfitCoFr_ OR @ProfitCoFr_ = '') AND (ISNULL(T0.ProfitCo,'') <= @ProfitCoTo_ OR @ProfitCoTo_ = '')
 AND (ISNULL(T0.OcrCode2,'') >= @OcrCode2Fr_ OR @OcrCode2Fr_ = '') AND (ISNULL(T0.OcrCode2,'') <= @OcrCode2To_ OR @OcrCode2To_ = '')
 AND (ISNULL(T0.OcrCode3,'') >= @OcrCode3Fr_ OR @OcrCode3Fr_ = '') AND (ISNULL(T0.OcrCode3,'') <= @OcrCode3To_ OR @OcrCode3To_ = '')
 AND (ISNULL(T0.OcrCode4,'') >= @OcrCode4Fr_ OR @OcrCode4Fr_ = '') AND (ISNULL(T0.OcrCode4,'') <= @OcrCode4To_ OR @OcrCode4To_ = '')
 AND (ISNULL(T0.OcrCode5,'') >= @OcrCode5Fr_ OR @OcrCode5Fr_ = '') AND (ISNULL(T0.OcrCode5,'') <= @OcrCode5To_ OR @OcrCode5To_ = '')

	INSERT INTO FTS11OJDT 
	SELECT @DocEntry + T0.IndexNum, @RefDate, GETDATE(), @UserID, NULL, NULL, 'D', -1, @Series, @Ref1, @Ref2, @Memo, @PostTo, @FileType FROM @IndexTable T0

END
GO

/* #####################################################################################
   6. sp_FTS11_GenerateDeprn - corrected "Missing depreciation" check
   ##################################################################################### */
/* =====================================================================================
   sp_FTS11_GenerateDeprn - corrected "Missing depreciation" check
   -------------------------------------------------------------------------------------
   Built from the FA_DEVELOPMENT version of the procedure. ONLY the block
   "-- If Effective Date before Period Range" is changed (see 2026-11-22 note inside);
   the depreciation calculation is unchanged.

   !! Your live databases may have a newer / different version of this procedure.
      Before running this on a live DB, compare it with that DB's current procedure
      (SSMS: right-click sp_FTS11_GenerateDeprn > Modify) and copy only the changed block.

   Recommended order:
     1. Run Depreciation_GapCheck.sql (read-only) and fill any real gaps
        (Run Depreciation from the asset's first missing period).
     2. Then apply this change - otherwise those assets will stop with
        "Missing depreciation on ..." until their gaps are filled.
   ===================================================================================== */
ALTER PROC [dbo].[sp_FTS11_GenerateDeprn] 
@DateFrom DATETIME,
@DateTo DATETIME,
@FAKey INT,
@UserCode NVARCHAR(10),
@ErrorMsg NVARCHAR(255) OUTPUT
AS BEGIN
/*
	-- Generate Depreciation
	-- Patch:
		2012-10-01	Correction on Accumulated Depreciation Account
		2012-10-07	Avoid -ve Depreciation
					Correction to Min NBV Adjustment
		2012-10-11	Option to include Salvage value into Depreciation calculation
		2012-10-30	Correction to Deprn Cnt checking, should validate with Actual Useful Life, Actual Total Cost
					* UsefulLife is used in Depreciation Calculation
					* ActualUsefulLife is used in determine total useful life of the asset
					* TotalCost is used in Depreciation Calculation
					* ActualTotalCost is used in determine total cost of the asset
		2012-11-07	Correction to avoid cancel Reversal entry when generate depreciation
		2013-02-18	Correction to Posted Depr and Zero Qty checking (@PrdEnd changed to @DocDate)
		2013-05-03	Correction to @FinYear datatype to NVARCHAR(10)
		2013-08-19	Correction to skip Depreciation generation if current period earlier than Asset Effective Period
		2013-08-24	Correction to final depreciation calculation
		2013-09-17	Depreciation according to Location ratio and posting according to FTS11FAM2 settings
					Correction to Skip Depreciation if Zero Quantity checking
					Added Skip Depreciation if Current Period earlier than F/A Effective Period checking
					Correction to SMALLDATETIME data type to avoid unpredicted conversion error
					Correction to Annual Rounding calculation
		2013-11-06	Correction to Depreciation Location
					Correction to use Cost Centre in FTS11OLOC if FTS11FAM2 not available
					Correction to include Move at month end
		2013-11-25	Temporarily solved the depreciation halt error by adding a select statement.
		2013-11-27	Correction to CAST( TO NVARCHAR) length
		2014-01-07	Replace Cursor with WHILE for better performance.
					
	DECLARE @ErrorMsg NVARCHAR(255)
	EXEC sp_FTS11_GenerateDeprn '20120101', '20121231', 5, 'FA', @ErrorMsg = @ErrorMsg OUTPUT
	SELECT @ErrorMsg
	SELECT * FROM FTS11JDT1

*/
	--DECLARE @DateFrom SMALLDATETIME
	--DECLARE @DateTo SMALLDATETIME
	--DECLARE @FAKey INT
	--DECLARE @UserCode NVARCHAR(10)
	--DECLARE @ErrorMsg NVARCHAR(255)
	--SELECT @DateFrom = '20140301', @DateTo = '20140331', @UserCode = 'FA', @FAKey = 2054


	DECLARE @DocEntry INT
	DECLARE @ActualTCostLC NUMERIC(19,6)
	DECLARE @ActualTCostSC NUMERIC(19,6)
	DECLARE @TCostLC NUMERIC(19,6)
	DECLARE @TCostSC NUMERIC(19,6)
	DECLARE @MBaseLC NUMERIC(19,6)
	DECLARE @MBaseSC NUMERIC(19,6)
	DECLARE @SalvageLC NUMERIC(19,6)
	DECLARE @SalvageSC NUMERIC(19,6)
	DECLARE @AmountLC NUMERIC(19,6)
	DECLARE @AmountSC NUMERIC(19,6)
	DECLARE @AmountLCYr NUMERIC(19,6)
	DECLARE @AmountSCYr NUMERIC(19,6)
	DECLARE @AccuLC NUMERIC(19,6)
	DECLARE @AccuSC NUMERIC(19,6)
	DECLARE @AccuLCYr NUMERIC(19,6)
	DECLARE @AccuSCYr NUMERIC(19,6)
	DECLARE @PrdBegin DATETIME
	DECLARE @PrdEnd DATETIME
	DECLARE @DocDate DATETIME
	DECLARE @ActualUseLife NUMERIC(19,6) -- Changed from INT to NUMERIC(19,6) to cater percentage depreciation
	DECLARE @UseLife NUMERIC(19,6) -- Changed from INT to NUMERIC(19,6) to cater percentage depreciation
	DECLARE @DeprnCnt NUMERIC(19,6) -- Changed from INT to NUMERIC(19,6) to cater percentage depreciation
	DECLARE @DeprnTyp CHAR(1) -- S: Straight Line, R: Reducing Balance
	DECLARE @FinYear NVARCHAR(10)
	DECLARE @FinYearStart DATETIME -- First day of the financial year or effective date which ever later
	DECLARE @PrdPerYear INT -- How many period for the financial year
	DECLARE @PrdEff INT -- How many period for the financial year after effective date
	DECLARE @AnnualPcnt NUMERIC(19,6)	-- Annual Depreciation %
	DECLARE @PeriodPcnt NUMERIC(19,6)	-- Period Depreciation %
	DECLARE @RevalueYN CHAR(1)			-- Y: Reviewed Useful Life / Revalued Cost, N: Not revalued before
										-- Revalued asset will always use period rate as revaluation can happen any month

	DECLARE @DeprBy NVARCHAR(10)	-- USELIFE: Depr Amount = Total Cost / Period, RATE: Depr Amount = Total Cost x %
	DECLARE @DeprPriority CHAR(1)	-- Calculation Priority Y: Monthly Depr = Annual Depr / periods, P: Annual Depr = Monthly Depr x periods
	DECLARE @AnnualRnd CHAR(1)		-- Y: Accumulate annual depreciation differences to last period of the year
	DECLARE @SumDec INT				-- Amount Decimal Points
	DECLARE @DeprSalv CHAR(1)		-- Include Salvage Value (Min NBV) into Depreciation Calculation Y: Yes, N: No
	DECLARE @Location NVARCHAR(8)
	DECLARE @LineNum INT
	DECLARE @Quantity NUMERIC(19,6)
	DECLARE @TotalQuantity NUMERIC(19,6)
	DECLARE @Ditobs CHAR(1)
	
	DECLARE @revaluelc numeric(19,6)
	DECLARE @revaluesc numeric(19,6)
	DECLARE @SalvageLCM numeric(19,6)
	DECLARE @SalvageSCM numeric(19,6)
	DECLARE @AccuLC1 numeric(19,6)
	DECLARE @AccuSC1 numeric(19,6)
	DECLARE @DeprnCnt1 int
	
	SELECT @DeprPriority = U_RateYr, @AnnualRnd = CASE WHEN U_RndYr = 'True' THEN 'Y' ELSE 'N' END, 
	@SumDec = U_SumDec, @DeprSalv = CASE WHEN U_DeprSalv = 'True' THEN 'Y' ELSE 'N' END, @Ditobs = CASE WHEN U_ditobs = 'True' THEN 'Y' ELSE 'N' END FROM FTS11OADM

	SELECT @DeprBy = U_DeprnBy FROM FTS11OFAM WHERE DocEntry = @FAKey-- Change from FTS11OADM to FTS11OFAM
	
	SET @ErrorMsg = ''

	DECLARE @LogNote NVARCHAR(254)
	SELECT @LogNote = 'Begin Procedure. ' + CAST(@DateFrom AS NVARCHAR(20)) + ' - ' + CAST(@DateTo AS NVARCHAR(20))
	EXEC sp_FTS11_DebugLog 'sp_FTS11_GenerateDeprn', @FAKey, 1, @LogNote
-----------------------------------------------------------------------------------------------------

	Begin Try 

	-- Validation
	IF NOT EXISTS (SELECT 1 FROM FTS11OFAM WHERE DocEntry = @FAKey) -- Invalid FA
	BEGIN 
		SET @ErrorMsg = 'Invalid F/A Key (' + CAST(@FAKey AS NVARCHAR(20)) + ')'
		GOTO ReturnStep 
	END
	IF EXISTS (SELECT 1 FROM FTS11OFAM WHERE DocEntry = @FAKey AND U_Status <> 'O') GOTO ReturnStep -- Invalid FA Status
	IF EXISTS (SELECT 1 FROM FTS11OFAM WHERE DocEntry = @FAKey AND U_DeprnYN <> 'True') GOTO ReturnStep -- Not Depreciating FA

	SET @RevalueYN = 'N'
	SELECT @RevalueYN = 'Y' FROM FTS11JDT1 T0 --INNER JOIN FTS11_vw_OFPR T1 ON T0.DocDate >= T1.F_RefDate AND T0.DocDate <= T1.T_RefDate
	WHERE T0.FaKey = @FAKey AND T0.TransType IN ('T0301', 'T0302', 'T0303') 
	AND T0.TransStatus <> 'C'-- AND T1.Category = @FinYear
	
	-- Get Minimum NBV
	SELECT @DeprnTyp = U_DeprnTyp, @MBaseLC = ISNULL(U_MBaseLC,0), @MBaseSC = ISNULL(U_MBaseSC,0), @revaluelc = ISNULL(U_Revalue,0), @revaluesc = ISNULL(U_RevalueSC,0)
	FROM FTS11OFAM WHERE DocEntry = @FAKey

	IF @DeprSalv = 'Y'  -- Deprecation consider salvage value
		SELECT @SalvageLCM = @revaluelc, @SalvageSCM = @revaluesc
	ELSE
		SELECT @SalvageLCM = 0, @SalvageSCM = 0
	
	SELECT @PrdBegin = F_RefDate FROM FTS11_vw_OFPR WHERE @DateFrom >= F_RefDate AND @DateFrom <= T_RefDate
	SELECT @PrdEnd = T_RefDate FROM FTS11_vw_OFPR WHERE @DateTo >= F_RefDate AND @DateTo <= T_RefDate
	
	-- If Effective Date before Period Range
	-- 2026-11-22  Correction: missing depreciation check never fired (the INNER JOIN on the LEFT JOINed journal removed
	--             exactly the periods without depreciation). Now reports the first earlier period with no depreciation
	--             in which the asset was still depreciable (same skip rules as below: zero quantity, depreciation count
	--             reached useful life, NBV at minimum NBV).
	IF EXISTS (SELECT 1 FROM FTS11OFAM WHERE DocEntry = @FAKey AND U_EffDate < @PrdBegin)
	BEGIN
		DECLARE @MissingDate DATETIME
		SET @MissingDate = NULL

		SELECT @MissingDate = MIN(T1.T_RefDate)
		FROM FTS11_vw_OFPR T1
		WHERE T1.F_RefDate >= (SELECT U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey) AND T1.T_RefDate < @PrdBegin
		-- no depreciation in that period
		AND NOT EXISTS (SELECT 1 FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T2 ON T0.TransType = T2.Code AND T2.U_TrnsGrp = 'DEP' -- Depreciation
						WHERE T0.FAKey = @FAKey AND T0.DocDate = T1.T_RefDate AND T0.TransStatus <> 'C')
		-- but the asset was still depreciable in that period
		AND EXISTS (SELECT 1 FROM (
						SELECT CASE WHEN V0.AcctCat = 'COST' THEN ISNULL(V0.Quantity,0) ELSE 0 END AS Qty,
							   CASE WHEN V0.AcctCat = 'COST' THEN ISNULL(V0.UseLife,0) ELSE 0 END AS UseLife,
							   CASE WHEN V0.AcctCat = 'ACCU' AND V0.DocDate < T1.T_RefDate THEN ISNULL(V0.DeprnCnt,0) ELSE 0 END AS DeprnCnt,
							   CASE WHEN V0.AcctCat = 'COST' THEN ISNULL(V0.AmtLC,0)
									WHEN V0.AcctCat = 'ACCU' AND V0.DocDate < T1.T_RefDate THEN ISNULL(V0.AmtLC,0) ELSE 0 END AS NBV
						FROM FTS11JDT1 V0
						WHERE V0.FAKey = @FAKey AND V0.TransStatus <> 'C' AND V0.AcctCat IN ('COST','ACCU') AND V0.DocDate <= T1.T_RefDate
					) V
					HAVING SUM(V.Qty) > 0 AND SUM(V.DeprnCnt) < SUM(V.UseLife) AND SUM(V.NBV) > @MBaseLC)

		IF @MissingDate IS NOT NULL
		BEGIN
			SELECT @ErrorMsg = 'Missing depreciation on ' + CONVERT(VARCHAR(10), @MissingDate, 102)
			GOTO ReturnStep
		END
	END
	
	-- For each period in Calculation Range
	SELECT @DocDate = MIN(T_RefDate) FROM FTS11_vw_OFPR WHERE F_RefDate BETWEEN @PrdBegin AND @PrdEnd
	WHILE @DocDate IS NOT NULL
	BEGIN
		SELECT @AmountLC = 0, @AmountSC = 0
				
		-- Skip if current month depreciation posted
		IF EXISTS(SELECT 1 FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
					WHERE T0.TransStatus = 'P' AND T1.U_TrnsGrp = 'DEP' AND T0.AcctCat = 'ACCU' AND T0.FAKey = @FAKey AND T0.DocDate = @DocDate)--@PrdEnd)
		GOTO NextPeriod
		
		-- Get Financial Year
		SELECT @FinYear = Category FROM FTS11_vw_OFPR WHERE T_RefDate = @DocDate 
		SELECT @PrdPerYear = MAX(SubNum) FROM FTS11_vw_OFPR WHERE Category = @FinYear
				
		-- Cancel current month depreciation
		UPDATE T0 SET T0.TransStatus = 'C', T0.CancelDate = GetDate(), T0.CancelBy = @UserCode
		FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
		WHERE T0.TransStatus = 'O' AND T1.U_TrnsGrp = 'DEP' AND T0.FAKey = @FAKey AND T0.DocDate = @DocDate		
		AND T0.DocEntry NOT IN (SELECT ReconKey FROM FTS11JDT1 WHERE ReconKey <> 0 AND FAKey = @FAKey)
				
		-- Skip if current period earlier than F/A Effective Period
		IF EXISTS(SELECT 1 FROM FTS11OFAM T0 INNER JOIN FTS11_vw_OFPR T1 ON T0.U_EffDate BETWEEN T1.F_RefDate AND T1.T_RefDate
		WHERE T0.DocEntry = @FAKey AND @DocDate < T1.F_RefDate)
		GOTO NextPeriod
		
		-- Skip if zero quantity
		IF EXISTS(SELECT 1 FROM FTS11JDT1 T0 
		WHERE T0.AcctCat = 'COST' AND T0.TransStatus <> 'C' AND T0.FAKey = @FAKey AND T0.DocDate <= @DocDate HAVING ISNULL(SUM(Quantity),0) <= 0)
		GOTO NextPeriod
		
		-- Get Accumulated Useful Life, Actual Useful life, Cost, Deprn Count and Accu Deprn
		-- ** RUseLife: after deducted accumulated deprn count ** only for depreciation calculatuon
		SELECT @UseLife = 0, @ActualUseLife = 0, @TCostLC = 0, @TCostSC = 0, @ActualTCostLC = 0, @ActualTCostSC = 0, 
		@AccuLC = 0, @AccuSC = 0, @DeprnCnt = 0
		
		SELECT @UseLife = ISNULL(SUM(ISNULL(UseLife,0)),0), --ISNULL(SUM(ISNULL(CASE WHEN TransType = 'T0301' THEN RUseLife ELSE UseLife END, 0)),0),
		@ActualUseLife = ISNULL(SUM(ISNULL(UseLife,0)),0)
		FROM FTS11JDT1 WHERE TransStatus <> 'C' AND AcctCat = 'COST' AND DocDate <= @DocDate AND FaKey = @FAKey
				
		-- ** RAmtLC: deduction of accumulated deprn amout ** only for depreciation calculatuon
		SELECT @TCostLC = ISNULL(SUM(ISNULL(AmtLC, 0)),0), --ISNULL(SUM(ISNULL(CASE WHEN TransType = 'T0301' THEN RAmtLC ELSE AmtLC END, 0)),0), 
			   @TCostSC = ISNULL(SUM(ISNULL(AmtSC, 0)),0), --ISNULL(SUM(ISNULL(CASE WHEN TransType = 'T0301' THEN RAmtSC ELSE AmtSC END, 0)),0),
			   @ActualTCostLC = ISNULL(SUM(ISNULL(AmtLC, 0)),0),
			   @ActualTCostSC = ISNULL(SUM(ISNULL(AmtSC, 0)),0) 
		FROM FTS11JDT1 
		WHERE TransStatus <> 'C' AND AcctCat = 'COST' AND DocDate <= @DocDate AND FaKey = @FAKey	
		
		SELECT @AccuLC = ISNULL(SUM(ISNULL(AmtLC, 0)),0), @AccuSC = ISNULL(SUM(ISNULL(AmtSC, 0)),0), @DeprnCnt = ISNULL(SUM(ISNULL(DeprnCnt, 0)),0)
		FROM FTS11JDT1 
		WHERE FaKey = @FAKey  AND DocDate <= @DocDate AND  TransStatus <> 'C' AND AcctCat = 'ACCU'

		-- Get Adjust. Accu Depr
		--IF EXISTS(SELECT 1 FROM FTS11JDT1 T0 WHERE T0.FAKey = @FAKey AND T0.TransStatus <> 'C' AND T0.TransType = 'T0303')
		--BEGIN
		--	SELECT @TCostLC = @TCostLC + ISNULL(SUM(ISNULL(AmtLC, 0)),0), @TCostSC = @TCostSC + ISNULL(SUM(ISNULL(AmtSC, 0)),0), 
		--	@UseLife = @UseLife - ISNULL(SUM(ISNULL(DeprnCnt, 0)),0)
		--	FROM FTS11JDT1
		--	WHERE FaKey = @FAKey  AND DocDate <= @DocDate AND  TransStatus <> 'C' AND AcctCat = 'ACCU'
		--	AND TransType = 'T0303'
		--END
		
		-- Get Salvage from Revalue Residual
		SELECT @SalvageLC = @SalvageLCM, @SalvageSC = @SalvageSCM
		SELECT @AccuLC1 = 0, @AccuSC1 = 0, @DeprnCnt1 = 0
		IF @DeprSalv = 'Y'  -- Deprecation consider salvage value
		BEGIN
			IF EXISTS (SELECT 1 from FTS11T0305 T1 
			INNER JOIN FTS11T0305D T2 ON T1.DocEntry = T2.DocEntry
			INNER JOIN FTS11OFAM T0 on T2.U_FAKey = T0.DocEntry and T0.DocEntry = @FAKey and T1.U_DocDate <= @DocDate and T1.Status <> 'C')
			BEGIN
				SELECT @SalvageLC = @SalvageLC + SUM(T2.U_MBaseLCD), @SalvageSC = @SalvageSC + SUM(T2.U_MBaseSCD) FROM FTS11T0305 T1 
				INNER JOIN FTS11T0305D T2 ON T1.DocEntry = T2.DocEntry
				INNER JOIN FTS11OFAM T0 ON T2.U_FAKey = T0.DocEntry AND T0.DocEntry = @FAKey AND T1.U_DocDate <= @DocDate and T1.Status <> 'C'				
				
				--SELECT @AccuLC1 = ISNULL(SUM(ISNULL(AmtLC, 0)),0), @AccuSC1 = ISNULL(SUM(ISNULL(AmtSC, 0)),0), @DeprnCnt1 = ISNULL(SUM(ISNULL(DeprnCnt, 0)),0)
				--FROM FTS11JDT1 
				--WHERE FaKey = @FAKey  AND DocDate <= @DocDate AND  TransStatus <> 'C' AND AcctCat = 'ACCU'
				
				--SET @TCostLC = @TCostLC + @AccuLC1
				--SET @TCostSC = @TCostSC + @AccuSC1
				--SET @UseLife = @UseLife - @DeprnCnt1
			END
		END
		
		-- Remaining NBV / Remaining UseLife after Adjustment
		IF EXISTS(SELECT 1 FROM FTS11JDT1 T0 WHERE T0.FAKey = @FAKey AND DocDate <= @DocDate  AND T0.TransStatus <> 'C' AND T0.TransType IN ('T0301','T0302','T0303','T0305'))
		BEGIN
			SET @TCostLC = @TCostLC + @AccuLC
			SET @TCostSC = @TCostSC + @AccuSC
			SET @UseLife = @UseLife - @DeprnCnt
		END

		-- Skip if met maximum Depreciation Count
		IF @DeprnCnt >= @ActualUseLife GOTO NextPeriod 

		-- If final depreciation
		IF @DeprnCnt + 1 >= @ActualUseLife
		BEGIN
			-- Get Remaining Depreciation
			SELECT @AmountLC = @ActualTCostLC - @MBaseLC + @AccuLC, @AmountSC = @ActualTCostSC - @MBaseSC + @AccuSC 
		END
		ELSE
		BEGIN
			IF @DeprnTyp = 'R' -- Reducing Balance
			BEGIN
				-- Calculate Depreciation
				SELECT @AmountLC = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@ActualTCostLC + @AccuLC - @SalvageLC) / @UseLife, 2) END, 
				@AmountSC = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@ActualTCostSC + @AccuSC - @SalvageSC) / @UseLife, 2) END			
			END
			ELSE
			BEGIN			
				IF @DeprBy = 'P' -- Calculate by Total Cost * %
				BEGIN
					IF @DeprPriority = 'Y' AND @RevalueYN <> 'Y' -- Annual Depr Priority AND NOT Revalued (Revalued asset always use monthly priority)
					BEGIN
						SELECT @AnnualPcnt = CASE WHEN ROUND(@UseLife / 12,0) = 0 THEN 0 ELSE ROUND(1.000000 / ROUND(@UseLife / 12,0) * 100,6) END

						-- Calculate Annual Depreciation				
						SELECT @AmountLCYr = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@TCostLC - @SalvageLC) * @AnnualPcnt / 100, @SumDec) END, 
						@AmountSCYr = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@TCostSC - @SalvageSC) * @AnnualPcnt / 100, @SumDec) END
						
						-- Divide Annual Depreciation to months
						SELECT @AmountLC = CASE WHEN @PrdPerYear = 0 THEN 0 ELSE ROUND((@TCostLC - @SalvageLC) * @AnnualPcnt / 100 / @PrdPerYear, @SumDec) END, 
						@AmountSC = CASE WHEN @PrdPerYear = 0 THEN 0 ELSE ROUND((@TCostSC - @SalvageSC) * @AnnualPcnt / 100 / @PrdPerYear, @SumDec) END	
											
					END
					ELSE -- Period Depr Priority
					BEGIN	
						SELECT @PeriodPcnt = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND(1.000000 / @UseLife * 100,6) END

						-- Calculate Annual Depreciation				
						SELECT @AmountLCYr = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@TCostLC - @SalvageLC) * @PeriodPcnt / 100, @SumDec) * @PrdPerYear END, 
						@AmountSCYr = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@TCostSC - @SalvageSC) * @PeriodPcnt / 100, @SumDec) * @PrdPerYear END
						
						-- Calculate Monthly Depreciation				
						SELECT @AmountLC = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@TCostLC - @SalvageLC) * @PeriodPcnt / 100, @SumDec) END, 
						@AmountSC = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@TCostSC - @SalvageSC) * @PeriodPcnt / 100, @SumDec) END

					END
				END
				ELSE -- Calculate by Total Cost / Useful Life
				BEGIN	
					IF @DeprPriority = 'Y' AND @RevalueYN <> 'Y' -- Annual Depr Priority AND NOT Revalued (Revalued asset always use monthly priority)
					BEGIN
						-- Calculate Annual Depreciation				
						SELECT @AmountLCYr = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@TCostLC - @SalvageLC) / ROUND(@UseLife / 12, 0), @SumDec) END, 
						@AmountSCYr = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@TCostSC - @SalvageSC) / ROUND(@UseLife / 12, 0), @SumDec) END
						
						-- Divide Annual Depreciation to months
						SELECT @AmountLC = CASE WHEN @PrdPerYear = 0 THEN 0 ELSE ROUND(@AmountLCYr / @PrdPerYear, @SumDec) END, 
						@AmountSC = CASE WHEN @PrdPerYear = 0 THEN 0 ELSE ROUND(@AmountSCYr / @PrdPerYear, @SumDec) END	
						
					END
					ELSE -- Period Depr Priority
					BEGIN
						-- Calculate Annual Depreciation	
						SELECT @AmountLCYr = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@TCostLC - @SalvageLC) / (@UseLife), @SumDec) * @PrdPerYear END, 
						@AmountSCYr = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@TCostSC - @SalvageSC) / (@UseLife), @SumDec) * @PrdPerYear END
						
						-- Calculate Monthly Depreciation				
						SELECT @AmountLC = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@TCostLC - @SalvageLC) / (@UseLife), @SumDec) END, 
						@AmountSC = CASE WHEN @UseLife = 0 THEN 0 ELSE ROUND((@TCostSC - @SalvageSC) / (@UseLife), @SumDec) END
						
					END
				END
				
				-- 1st Month Prorate
				IF EXISTS(SELECT 1 FROM FTS11OFAM WHERE DocEntry = @FAKey AND U_Prorate = 'Y')
				BEGIN
					-- Current Month is First Effective Month
					IF EXISTS (SELECT 1 FROM FTS11OFAM T0 INNER JOIN FTS11_vw_OFPR T1 ON T0.U_EffDate >= T1.F_RefDate AND T0.U_EffDate <= T1.T_RefDate 
								WHERE T0.DocEntry = @FAKey AND T1.T_RefDate  = @DocDate)
					BEGIN
						-- Get balance effective periods						
						SELECT @PrdEff = @PrdPerYear - T1.SubNum FROM FTS11_vw_OFPR T1 WHERE T1.T_RefDate = @DocDate
						
						SELECT @AmountLC = @AmountLCYr - (@AmountLC * @PrdEff), @AmountSC = @AmountSCYr - (@AmountSC * @PrdEff)						
					END				
				END
				
				-- Average Prorate
				IF EXISTS(SELECT 1 FROM FTS11OFAM WHERE DocEntry = @FAKey AND U_Prorate = 'A')
				BEGIN
					-- Current Month is First Effective Year
					IF EXISTS(SELECT 1 FROM FTS11OFAM T0 INNER JOIN FTS11_vw_OFPR T1 ON T0.U_EffDate >= T1.F_RefDate AND T0.U_EffDate <= T1.T_RefDate
					WHERE T0.DocEntry = @FAKey AND T1.Category = @FinYear)
					BEGIN
						-- Get balance effective periods
						SELECT @PrdEff = @PrdPerYear - T1.SubNum + 1 FROM FTS11_vw_OFPR T1 WHERE T1.T_RefDate = @DocDate
									
						SELECT @AmountLC = CASE WHEN @PrdEff = 0 THEN 0 ELSE  ROUND(@AmountLCYr / @PrdEff, 2) END, 
						@AmountSC = CASE WHEN @PrdEff = 0 THEN 0 ELSE  ROUND(@AmountSCYr / @PrdEff, 2) END				
					END
				END				
				
				-- Annual Rounding
				IF @AnnualRnd = 'Y'
				BEGIN
					-- Do Annual Rounding if has NOT done Uselife Review / Revaluation
					-- AND Current Period is Year End Period
					-- AND F/A Effective Date is equal or earlier than Current Year Start
					IF @RevalueYN <> 'Y' AND EXISTS (SELECT 1 FROM FTS11_vw_OFPR WHERE Category = @FinYear HAVING @DocDate = MAX(T_RefDate))
					BEGIN				
						SELECT @FinYearStart = MIN(F_RefDate) FROM FTS11_vw_OFPR WHERE Category = @FinYear
						IF EXISTS(SELECT 1 FROM FTS11OFAM T0 WHERE T0.DocEntry = @FAKey AND T0.U_EffDate <= @FinYearStart)					
						BEGIN
							-- Get current year accumulated depreciation
							SELECT @AccuLCYr = SUM(ISNULL(AmtLC, 0)), @AccuSCYr = SUM(ISNULL(AmtSC, 0))
							FROM FTS11JDT1 
							WHERE FaKey = @FAKey AND TransStatus <> 'C' AND AcctCat = 'DEPR' AND DocDate BETWEEN @FinYearStart AND @DocDate
							SELECT @AccuLCYr = ISNULL(@AccuLCYr,0), @AccuSCYr = ISNULL(@AccuSCYr,0)
							
							SELECT @AmountLC = @AmountLCYr - @AccuLCYr, @AmountSC = @AmountSCYr - @AccuSCYr		
						END			
					END		
				END				
			END
				
			-- Minimum NBV Adjustment
			SELECT @AmountLC = CASE WHEN @ActualTCostLC + @AccuLC - @AmountLC < @MBaseLC THEN @ActualTCostLC + @AccuLC - @MBaseLC ELSE @AmountLC END,
			@AmountSC = CASE WHEN @ActualTCostSC + @AccuSC - @AmountSC  < @MBaseSC THEN @ActualTCostSC + @AccuSC - @MBaseSC ELSE @AmountSC END		
		END
		
		-- Skip if <= 0 depreciation
		if ISNULL(@AmountLC,0) <= 0 SET @AmountLC = 0
		if ISNULL(@AmountSC,0) <= 0 SET @AmountSC = 0
		IF ISNULL(@AmountLC,0) = 0 AND ISNULL(@AmountSC,0) = 0 GOTO NextPeriod

		-- Get Next DocEntry
		SELECT @DocEntry = MAX(DocEntry) FROM FTS11JDT1 
		SELECT @DocEntry = ISNULL(@DocEntry, 0) + 1	

		SELECT @TotalQuantity = SUM(Quantity) FROM FTS11JDT1 T0 
		WHERE T0.AcctCat = 'COST' AND T0.TransStatus <> 'C' AND T0.FAKey = @FAKey AND T0.DocDate <= @DocDate 
		
		SET @LineNum = 1
		SELECT @Location = MIN(Location)
		FROM FTS11JDT1 WHERE AcctCat = 'COST' AND TransStatus <> 'C' AND FAKey = @FAKey AND DocDate <= @DocDate
		GROUP BY FaKey, Location HAVING SUM(Quantity) > 0
		
		WHILE @Location IS NOT NULL
		BEGIN
			SELECT @Quantity = SUM(Quantity) 
			FROM FTS11JDT1 WHERE AcctCat = 'COST' AND TransStatus <> 'C' AND FAKey = @FAKey AND Location = @Location AND DocDate <= @DocDate
				
			INSERT INTO FTS11JDT1
			SELECT @DocEntry, @LineNum, T0.DocEntry, @DocDate, GETDATE(), @UserCode, 
			NULL, NULL, NULL, NULL, NULL, NULL, '', -1, 0, CASE WHEN @DeprnTyp = 'R' THEN 'T9902' ELSE 'T9901' END, 'O', '', -1, -1, 
			'DEPR', 
			CASE WHEN ISNULL(T1.U_AccFrmD,'') <> '' THEN T1.U_AccFrmD ELSE T0.U_AccFrmD END, 
			CASE WHEN ISNULL(T1.U_AccFrmD,'') <> '' THEN T1.U_AccCodD ELSE T0.U_AccCodD END, 
			CASE WHEN ISNULL(T1.U_Project,'') <> '' THEN T1.U_Project ELSE (CASE WHEN ISNULL(T2.U_Project,'') <> '' THEN T2.U_Project ELSE T0.U_Project END) END, 
			CASE WHEN ISNULL(T1.U_ProfitCode,'') <> '' THEN T1.U_ProfitCode ELSE (CASE WHEN ISNULL(T2.U_ProfitCode,'') <> '' THEN T2.U_ProfitCode ELSE T0.U_ProfitCode END) END, 
			CASE WHEN ISNULL(T1.U_OcrCode2,'') <> '' THEN T1.U_OcrCode2 ELSE (CASE WHEN ISNULL(T2.U_OcrCode2,'') <> '' THEN T2.U_OcrCode2 ELSE T0.U_OcrCode2 END) END, 
			CASE WHEN ISNULL(T1.U_OcrCode3,'') <> '' THEN T1.U_OcrCode3 ELSE (CASE WHEN ISNULL(T2.U_OcrCode3,'') <> '' THEN T2.U_OcrCode3 ELSE T0.U_OcrCode3 END) END, 
			CASE WHEN ISNULL(T1.U_OcrCode4,'') <> '' THEN T1.U_OcrCode4 ELSE (CASE WHEN ISNULL(T2.U_OcrCode4,'') <> '' THEN T2.U_OcrCode4 ELSE T0.U_OcrCode4 END) END, 
			CASE WHEN ISNULL(T1.U_OcrCode5,'') <> '' THEN T1.U_OcrCode5 ELSE (CASE WHEN ISNULL(T2.U_OcrCode5,'') <> '' THEN T2.U_OcrCode5 ELSE T0.U_OcrCode5 END) END, 
			0, 
			CASE WHEN @TotalQuantity = @Quantity THEN @AmountLC WHEN @TotalQuantity = 0 THEN 0 ELSE ROUND(@AmountLC * @Quantity / @TotalQuantity, 2) END,
			CASE WHEN @TotalQuantity = @Quantity THEN @AmountSC WHEN @TotalQuantity = 0 THEN 0 ELSE ROUND(@AmountSC * @Quantity / @TotalQuantity, 2) END,	
			1, 0, 0, 
			0, 0, 0,
			0, 0, 0,
			@Location, 0, 0, 0
			FROM FTS11OFAM T0 
			LEFT OUTER JOIN FTS11FAM2 T1 ON T0.DocEntry = T1.U_FAKey AND T1.U_Location = @Location
			LEFT OUTER JOIN FTS11OLOC T2 ON T2.Code = @Location
			WHERE T0.DocEntry = @FAKey			

			SET @LineNum = @LineNum + 1
			
			INSERT INTO FTS11JDT1
			SELECT @DocEntry, @LineNum, T0.DocEntry, @DocDate, GETDATE(), @UserCode, 
			NULL, NULL, NULL, NULL, NULL, NULL, '', -1, 0, CASE WHEN @DeprnTyp = 'R' THEN 'T9902' ELSE 'T9901' END, 'O', '', -1, -1, 
			'ACCU', 
			CASE WHEN ISNULL(T1.U_AccFrmC,'') <> '' THEN T1.U_AccFrmC ELSE T0.U_AccFrmC END, 
			CASE WHEN ISNULL(T1.U_AccFrmC,'') <> '' THEN T1.U_AccCodC ELSE T0.U_AccCodC END, 
			CASE WHEN @Ditobs = 'Y' THEN (CASE WHEN ISNULL(T1.U_Project,'') <> '' THEN T1.U_Project ELSE (CASE WHEN ISNULL(T2.U_Project,'') <> '' THEN T2.U_Project ELSE T0.U_Project END) END) ELSE '' END, 
			CASE WHEN @Ditobs = 'Y' THEN (CASE WHEN ISNULL(T1.U_ProfitCode,'') <> '' THEN T1.U_ProfitCode ELSE (CASE WHEN ISNULL(T2.U_ProfitCode,'') <> '' THEN T2.U_ProfitCode ELSE T0.U_ProfitCode END) END) ELSE '' END, 
			CASE WHEN @Ditobs = 'Y' THEN (CASE WHEN ISNULL(T1.U_OcrCode2,'') <> '' THEN T1.U_OcrCode2 ELSE (CASE WHEN ISNULL(T2.U_OcrCode2,'') <> '' THEN T2.U_OcrCode2 ELSE T0.U_OcrCode2 END) END) ELSE '' END, 
			CASE WHEN @Ditobs = 'Y' THEN (CASE WHEN ISNULL(T1.U_OcrCode3,'') <> '' THEN T1.U_OcrCode3 ELSE (CASE WHEN ISNULL(T2.U_OcrCode3,'') <> '' THEN T2.U_OcrCode3 ELSE T0.U_OcrCode3 END) END) ELSE '' END, 
			CASE WHEN @Ditobs = 'Y' THEN (CASE WHEN ISNULL(T1.U_OcrCode4,'') <> '' THEN T1.U_OcrCode4 ELSE (CASE WHEN ISNULL(T2.U_OcrCode4,'') <> '' THEN T2.U_OcrCode4 ELSE T0.U_OcrCode4 END) END) ELSE '' END, 
			CASE WHEN @Ditobs = 'Y' THEN (CASE WHEN ISNULL(T1.U_OcrCode5,'') <> '' THEN T1.U_OcrCode5 ELSE (CASE WHEN ISNULL(T2.U_OcrCode5,'') <> '' THEN T2.U_OcrCode5 ELSE T0.U_OcrCode5 END) END) ELSE '' END, 
			0, 
			-CASE WHEN @TotalQuantity = @Quantity THEN @AmountLC WHEN @TotalQuantity = 0 THEN 0 ELSE ROUND(@AmountLC * @Quantity / @TotalQuantity, 2) END,
			-CASE WHEN @TotalQuantity = @Quantity THEN @AmountSC WHEN @TotalQuantity = 0 THEN 0 ELSE ROUND(@AmountSC * @Quantity / @TotalQuantity, 2) END,			
			1, 0, 0, 
			0, 0, 0,
			0, 0, 0,
			@Location, 0, 0, 0
			FROM FTS11OFAM T0 
			LEFT OUTER JOIN FTS11FAM2 T1 ON T0.DocEntry = T1.U_FAKey AND T1.U_Location = @Location
			LEFT OUTER JOIN FTS11OLOC T2 ON T2.Code = @Location
			WHERE T0.DocEntry = @FAKey

			SET @LineNum = @LineNum + 1
			
			-- Next Location
			SET @Location = NULL
			SELECT @Location = MIN(Location)
			FROM FTS11JDT1 WHERE AcctCat = 'COST' AND TransStatus <> 'C' AND FAKey = @FAKey AND DocDate <= @DocDate AND Location > @Location 
			HAVING SUM(Quantity) > 0
		END
		
		SET @DocEntry = @DocEntry + 1

		NextPeriod:
		SELECT @PrdBegin = @DocDate, @DocDate = NULL
		SELECT @DocDate = MIN(T_RefDate) FROM FTS11_vw_OFPR WHERE F_RefDate BETWEEN @PrdBegin AND @PrdEnd
	END
	begin
		-- Update Dep Count
		UPDATE T0 SET U_deprncnt = ISNULL(T1.TransCnt ,0)
		FROM FTS11OFAM T0 LEFT OUTER JOIN
		(
			SELECT FAKey, COUNT(*) AS [TransCnt] 
			FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code 
			WHERE TransStatus <> 'C' AND T1.U_TrnsGrp = 'DEP' and T0.AcctCat = 'ACCU'
			GROUP BY FAKey
		) T1 ON T0.DocEntry = T1.FAKey
		WHERE T0.DocEntry = @FAKey

		-- Update Latest Cost, Depr,NBV
		EXEC sp_FTS11_SetIsFieldEditable @FAKey
	end
	End Try 
	Begin Catch 	
		SELECT @LogNote = 'End with Exception: ' + ERROR_MESSAGE()
		EXEC sp_FTS11_DebugLog 'sp_FTS11_GenerateDeprn', @FAKey, 1, @LogNote
	End catch 	
	
	ReturnStep:
	SELECT CASE WHEN @ErrorMsg = '' THEN 0 ELSE -1 END, @ErrorMsg
	
	SELECT @LogNote = 'End Procedure. ' + @ErrorMsg
	EXEC sp_FTS11_DebugLog 'sp_FTS11_GenerateDeprn', @FAKey, 1, @LogNote
END
GO


/* #####################################################################################
   7. sp_FTS11_ResetDatabase - also clear the depreciation run log (FTS11ODRL, FTS11DRL1)
   ##################################################################################### */
/* Adds 2 lines before the procedure's last END. The rest of the procedure (including any
   client-specific lines) is kept. Password Administration settings / history are NOT
   cleared, because Reset Database keeps the users and Company Setup. */
DECLARE @def NVARCHAR(MAX) = OBJECT_DEFINITION(OBJECT_ID('dbo.sp_FTS11_ResetDatabase'));
IF @def IS NOT NULL AND @def NOT LIKE '%FTS11DRL1%' AND PATINDEX('%CREATE PROC%', @def) > 0
BEGIN
    DECLARE @lastEnd INT = LEN(@def + 'x') - CHARINDEX('DNE', REVERSE(@def)) - 2;   -- position of the last END
    SET @def = STUFF(@def, @lastEnd, 0,
          CHAR(13) + CHAR(10) + CHAR(9) + '-- Depreciation run log (V6)'
        + CHAR(13) + CHAR(10) + CHAR(9) + 'DELETE FROM FTS11DRL1'
        + CHAR(13) + CHAR(10) + CHAR(9) + 'DELETE FROM FTS11ODRL'
        + CHAR(13) + CHAR(10) + CHAR(9) + 'DBCC CHECKIDENT (''FTS11ODRL'', RESEED, 0)'
        + CHAR(13) + CHAR(10));
    SET @def = STUFF(@def, PATINDEX('%CREATE PROC%', @def), LEN('CREATE'), 'ALTER');
    BEGIN TRY
        EXEC (@def);
        PRINT DB_NAME() + ': sp_FTS11_ResetDatabase now also clears the depreciation run log.';
    END TRY
    BEGIN CATCH
        PRINT DB_NAME() + ': sp_FTS11_ResetDatabase NOT changed (' + ERROR_MESSAGE() + '). Add the 2 DELETE lines by hand.';
    END CATCH
END
GO


/* #####################################################################################
   8. Kossan captions
   ##################################################################################### */
/* The V6 program takes the Cost Centre / Indicator captions from Company Setup.
   Set Kossan's names, only where the caption is blank or still the standard default,
   so a caption the client already changed in Company Setup is kept. */
UPDATE dbo.FTS11OADM SET U_Ocr1Capt = 'Project Code' WHERE ISNULL(U_Ocr1Capt, '') IN ('', 'Cost Centre D1');
UPDATE dbo.FTS11OADM SET U_Ocr2Capt = 'Department'   WHERE ISNULL(U_Ocr2Capt, '') IN ('', 'Cost Centre D2');
UPDATE dbo.FTS11OADM SET U_Ind1Capt = 'Area Code'    WHERE ISNULL(U_Ind1Capt, '') IN ('', 'Indicator 1');
PRINT DB_NAME() + ': Kossan captions set (Project Code, Department, Area Code).';
GO


/* #####################################################################################
   Result - every column should say OK
   ##################################################################################### */
SELECT DB_NAME() AS [Database],
    CASE WHEN COL_LENGTH('dbo.FTS11OADM','U_Ref2') IS NOT NULL AND COL_LENGTH('dbo.FTS11FAM2','U_CnclDate') IS NOT NULL
              AND COL_LENGTH('dbo.FTS11FAG1','U_CnclDate') IS NOT NULL AND OBJECT_ID('dbo.FTS11AFAM2') IS NOT NULL
              AND REPLACE(REPLACE(REPLACE(REPLACE(OBJECT_DEFINITION(OBJECT_ID('dbo.SBO_SP_PostTransactionNotice')),' ',''),CHAR(9),''),CHAR(13),''),CHAR(10),'') NOT LIKE '%INSERTINTOFTS11FAM2SELECT%'
         THEN 'OK' ELSE 'MISSING' END AS [0 Earlier Versions],
    CASE WHEN OBJECT_ID('dbo.FTS11OPPA') IS NOT NULL AND OBJECT_ID('dbo.FTS11OUPH') IS NOT NULL
              AND COL_LENGTH('dbo.SystemUsers','FailedLogonCount') IS NOT NULL THEN 'OK' ELSE 'MISSING' END AS [1 Password Admin],
    CASE WHEN (SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.FTS11JDT1')
               AND name IN ('IX_FTS11JDT1_FAKey','IX_FTS11JDT1_DocEntry')) = 2 THEN 'OK' ELSE 'MISSING' END AS [2 Indexes],
    CASE WHEN COL_LENGTH('dbo.FTS11ODRL','StopReq') IS NOT NULL AND OBJECT_ID('dbo.FTS11DRL1') IS NOT NULL THEN 'OK' ELSE 'MISSING' END AS [3 Run Log],
    CASE WHEN (SELECT COUNT(*) FROM sys.parameters WHERE object_id = OBJECT_ID('dbo.FTS_fn_FADeprList')) = 31 THEN 'OK' ELSE 'MISSING' END AS [4 FADeprList],
    CASE WHEN (SELECT COUNT(*) FROM sys.parameters WHERE object_id = OBJECT_ID('dbo.sp_FTS11_PrepareJEPreview')) = 29 THEN 'OK' ELSE 'MISSING' END AS [5 JE Preview],
    CASE WHEN OBJECT_DEFINITION(OBJECT_ID('dbo.sp_FTS11_GenerateDeprn')) LIKE '%2026-11-22%' THEN 'OK' ELSE 'MISSING' END AS [6 Missing Check],
    CASE WHEN OBJECT_DEFINITION(OBJECT_ID('dbo.sp_FTS11_ResetDatabase')) LIKE '%FTS11DRL1%' THEN 'OK' ELSE 'MISSING' END AS [7 Reset DB],
    CASE WHEN (SELECT COUNT(*) FROM dbo.FTS11OADM WHERE ISNULL(U_Ocr1Capt,'') <> '' AND ISNULL(U_Ocr2Capt,'') <> '' AND ISNULL(U_Ind1Capt,'') <> '') > 0 THEN 'OK' ELSE 'MISSING' END AS [8 Kossan Captions];
PRINT 'Kossan Version Patch V6 finished on ' + DB_NAME();
GO
SET NOEXEC OFF;
GO
