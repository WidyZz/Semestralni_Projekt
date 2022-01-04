CREATE TABLE [dbo].[Objednavky]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdZakaznika] INT NULL,
    [IdProduktu] INT NULL,
    [IdKosiku] INT NULL, 
    CONSTRAINT [FK_Objednavky_Zakaznik] FOREIGN KEY ([IdZakaznika]) REFERENCES [Zakaznik]([Id]), 
    CONSTRAINT [FK_Objednavky_Produkt] FOREIGN KEY ([IdProduktu]) REFERENCES [Produkt]([Id]),
)
