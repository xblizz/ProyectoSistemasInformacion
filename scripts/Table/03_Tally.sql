USE [ALXout01DB]
GO
/****** Object:  Table [dbo].[Tally]    Script Date: 02/04/2012 12:20:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS ( SELECT * FROM SysObjects WHERE Name = 'Tally' AND XType='U' )
BEGIN
	DROP TABLE Tally
	PRINT 'Delete Table Tally'
END
GO
CREATE TABLE [dbo].[Tally](
	[Number] [int] NULL
) ON [PRIMARY]
GO
PRINT 'Create Table Tally'
GO
-- INSERTA INFORMACION EN TABLA 
INSERT Tally
SELECT  ROW_NUMBER() OVER( ORDER BY SO.Id )
FROM	SysObjects SO
		
