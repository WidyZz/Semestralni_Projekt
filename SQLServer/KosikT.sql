CREATE TABLE [dbo].[Kosik]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdZakaznika] INT NULL, 
    [IdProduktu] INT NULL, 
    [Nazev Produktu] nchar(20),
    [Kusu] INT NULL, 
    [Cena] DECIMAL(18, 2) NULL, 
    [Celkova Hmotnost] FLOAT NULL, 
    CONSTRAINT [FK_Kosik_IdZakaznika] FOREIGN KEY ([IdZakaznika]) REFERENCES [Zakaznik]([Id]),
    CONSTRAINT [FK_Kosik_IdProduktu] FOREIGN KEY ([IdProduktu]) REFERENCES [Produkt]([Id]),
)
