CREATE TABLE [dbo].[Zakaznik]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Jmeno] NCHAR(10) NULL, 
    [Prijmeni] NCHAR(10) NULL, 
    [Email] NCHAR(40) NULL, 
    [Datum Narozeni] DATE NULL
)
