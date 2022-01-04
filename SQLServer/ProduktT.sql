CREATE TABLE [dbo].[Produkt]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Nazev] NCHAR(20) NOT NULL, 
    [Hmotnost] INT NOT NULL, 
    [Kusu] INT NULL
)
