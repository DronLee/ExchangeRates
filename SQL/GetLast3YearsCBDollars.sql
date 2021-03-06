USE [ExchangeRates]
GO
/****** Object:  StoredProcedure [dbo].[GetLastYearCBDollars]    Script Date: 02.09.2018 14:33:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: 09.09.2017
-- Description:	Получение курса долара за последние 3 года.
-- =============================================
CREATE PROCEDURE [dbo].[GetLast3YearsCBDollars]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT C.[Date],C.Course FROM dbo.CBCourse C
		WHERE C.CurrencyId = 1 AND C.[Date] > DATEADD(YEAR, -3, GETDATE())  
END
