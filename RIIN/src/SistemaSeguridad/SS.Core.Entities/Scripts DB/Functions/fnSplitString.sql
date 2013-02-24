USE ALXout01DB
GO
-- CREA LA TABLA DE TIPO Numeros, y le inserta valores
IF EXISTS ( SELECT * FROM sysObjects where Name = 'Tally' AND XType ='U')
BEGIN
	DROP TABLE Tally
END
CREATE TABLE Tally (Number INT)
INSERT Tally
SELECT ROW_NUMBER() OVER(ORDER BY Id) 
FROM SysObjects SO

-- CREA LA FUNCION DE SPLIT
IF EXISTS (SELECT * FROM sysobjects Where Name = 'SplitString' AND XTYPE = 'TF')
BEGIN
	DROP FUNCTION dbo.SplitString
END
GO
CREATE FUNCTION dbo.SplitString(@arr AS VARCHAR(8000), @sep AS VARCHAR(10))
  RETURNS @tblReturn TABLE (
		Pos		INT,
		Valor VARCHAR(8000)
)
AS
BEGIN
  
 INSERT  @tblReturn
 SELECT	(x.number - 1) - LEN(REPLACE(LEFT(@arr, x.number - 1), @sep, '')) + 1 AS pos,
		SUBSTRING(@arr, x.number, CHARINDEX(@sep, @arr + @sep, x.number) - x.number) AS Valor
 FROM	Tally AS X
 WHERE	x.number <= LEN(@arr) + 1
		AND SUBSTRING(@sep + @arr, x.number, LEN(@sep)) = @sep;

 RETURN 
END
GO




