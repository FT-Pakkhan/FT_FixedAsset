
GO
/****** Object:  View [dbo].[SE_FAProrate]    Script Date: 9/5/2023 4:24:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[SE_FAProrate] AS

SELECT 'N' AS [Code], 'No Prorate' AS [Name]
UNION ALL
SELECT 'M' AS [Code], '1st Month Prorate (Eff Date)' AS [Name]
UNION ALL
SELECT 'Y' AS [Code], '1st Month Prorate (Annual Depr)' AS [Name]
UNION ALL
SELECT 'A' AS [Code], 'Average Month Prorate (Annual Depr)' AS [Name]
GO
/****** Object:  StoredProcedure [dbo].[sp_FTS11_GenerateDeprn]    Script Date: 9/5/2023 4:24:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
	INNER JOIN FTS11OFAM T1 ON T0.FAKey = T0.DocEntry
	WHERE T0.FaKey = @FAKey 
	AND T0.TransType IN ('T0301', 'T0302', 'T0303', 'T0305') 
	AND T0.TransStatus <> 'C'-- AND T1.Category = @FinYear
	OR T1.U_Prorate = 'M'
	
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
	IF EXISTS (SELECT 1 FROM FTS11OFAM WHERE DocEntry = @FAKey AND U_EffDate < @PrdBegin)
	BEGIN	
		-- Check missing depreciation prior Calculation Range
		IF EXISTS (SELECT 1 FROM FTS11_vw_OFPR T1 
			LEFT OUTER JOIN FTS11JDT1 T0 ON T0.DocDate = T1.T_RefDate AND T0.FAKey = @FAKey AND T0.TransStatus <> 'C'
			INNER JOIN FTS11STRN T2 ON T0.TransType = T2.Code AND T2.U_TrnsGrp = 'DEP' -- Depreciation
			WHERE T1.F_RefDate >= (SELECT U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey) AND T1.T_RefDate < @PrdBegin
			AND T0.DocDate IS NULL)
		BEGIN
			SELECT @ErrorMsg = 'Missing depreciation on ' + CONVERT(VARCHAR(10), MIN(T1.T_RefDate), 102)
			FROM FTS11_vw_OFPR T1 
			LEFT OUTER JOIN FTS11JDT1 T0 ON T0.DocDate = T1.T_RefDate AND T0.FAKey = @FAKey AND T0.TransStatus <> 'C'
			INNER JOIN FTS11STRN T2 ON T0.TransType = T2.Code AND T2.U_TrnsGrp = 'DEP' -- Depreciation
			WHERE T1.F_RefDate >= (SELECT U_EffDate FROM FTS11OFAM WHERE DocEntry = @FAKey) AND T1.T_RefDate < @PrdBegin
			AND T0.DocDate IS NULL
			
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
		IF EXISTS(SELECT 1 FROM FTS11JDT1 T0 INNER JOIN FTS11OFAM T1 ON T0.FAKey = T1.DocEntry
		WHERE T0.FAKey = @FAKey AND DocDate <= @DocDate  
		AND T0.TransStatus <> 'C' AND T0.TransType IN ('T0301','T0302','T0303','T0305') OR T1.U_Prorate = 'M')

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

				-- 1st Month Prorate (Eff Date)
				IF EXISTS(SELECT 1 FROM FTS11OFAM WHERE DocEntry = @FAKey AND U_Prorate = 'M')
				BEGIN
					-- Current Month is First Effective Month
					IF EXISTS (SELECT 1 FROM FTS11OFAM T0 INNER JOIN FTS11_vw_OFPR T1 ON T0.U_EffDate >= T1.F_RefDate AND T0.U_EffDate <= T1.T_RefDate 
								WHERE T0.DocEntry = @FAKey AND T1.T_RefDate  = @DocDate)
					BEGIN 
					
						SELECT @AmountLC = @AmountLC / (DATEDIFF(DAY, T1.F_RefDate, T1.T_RefDate) + 1) *  (DATEDIFF(DAY, T0.U_EffDate, T1.T_RefDate) + 1) 
						FROM FTS11OFAM T0 INNER JOIN FTS11_vw_OFPR T1 ON T0.U_EffDate >= T1.F_RefDate AND T0.U_EffDate <= T1.T_RefDate 
						WHERE T0.DocEntry = @FAKey AND T1.T_RefDate  = @DocDate
						
					END				
				END
				
				-- 1st Month Prorate (Annual Depr)
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
				
				-- Average Prorate (Annual Depr)
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
