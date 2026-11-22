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
