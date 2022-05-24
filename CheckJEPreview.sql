		DECLARE @DateFr DATETIME, @DateTo DATETIME, 
		@FAClsCodFr NVARCHAR(100), @FAClsCodTo NVARCHAR(100), 
		@FAGrpCodFr NVARCHAR(100), @FAGrpCodTo NVARCHAR(100),
		@FACodeFr NVARCHAR(100), @FACodeTo NVARCHAR(100)

		SET  @DateFr = ''
		SET @DateTo = '20210531'
		SET @FAClsCodFr = '' 
		SET @FAClsCodTo = '' 
		SET @FAGrpCodFr = ''
		SET @FAGrpCodTo = ''
		SET @FACodeFr = ''
		SET @FACodeTo = ''
		
		SELECT T3.Name + '_' + ISNULL(T0.OcrCode2,''), ROW_NUMBER() OVER (ORDER BY T3.Name, ISNULL(T0.OcrCode2,'')) - 1
		FROM FTS11JDT1 T0 INNER JOIN FTS11OFAM T1 ON T0.FaKey = T1.DocEntry
		INNER JOIN FTS11OFAG T2 ON T1.U_FAGrpKey = T2.DocEntry 
		INNER JOIN FTS11OFAC T3 ON T2.U_FAClsCod = T3.Code COLLATE DATABASE_DEFAULT
		WHERE T0.TransStatus = 'O'
		AND T0.DocDate BETWEEN @DateFr AND @DateTo
		AND (T3.Code >= @FAClsCodFr OR @FAClsCodFr = '') AND (T3.Code <= @FAClsCodTo OR @FAClsCodTo = '')
		AND (T2.U_FAGrpCod >= @FAGrpCodFr OR @FAGrpCodFr = '') AND (T2.U_FAGrpCod <= @FAGrpCodTo OR @FAGrpCodTo = '')
		AND (T1.U_FACode >= @FACodeFr OR @FACodeFr = '') AND (T1.U_FACode <= @FACodeTo OR @FACodeTo = '')
		GROUP BY T3.Name, ISNULL(T0.OcrCode2,'')