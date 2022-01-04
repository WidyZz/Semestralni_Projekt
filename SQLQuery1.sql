drop table Kosik;


CREATE TABLE [dbo].[Kosik] (
    [Id]         INT        IDENTITY (1, 1) NOT NULL,
    [ZakaznikID] INT        NULL,
    [ProduktID]  INT        NULL,
    [Celkem_Kusu]       INT        NULL,
    [Vytvoreno]  TIMESTAMP NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Kosik_Uzivatel] FOREIGN KEY ([ZakaznikID]) REFERENCES [dbo].[Auth] ([Id]),
    CONSTRAINT [FK_Kosik_Produkt] FOREIGN KEY ([ProduktID]) REFERENCES [dbo].[Produkt] ([Id])
);
select Cena from Objednavky where Cena = (SELECT Cena * Celkem_Kusu From Produkt, Kosik Where ProduktID = Produkt.Id and KosikID = Kosik.Id);