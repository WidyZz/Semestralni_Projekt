INSERT INTO Objednavky (ZakaznikID, KosikID, ProduktID, Cena, CasObjednani)
SELECT k.ZakaznikID, k.Id, k.ProduktID, k.Celkem_Kusu * p.Cena, GETDATE() from Kosik k inner join Produkt p
ON k.ProduktID = p.Id
WHERE k.ZakaznikID = 1;
DELETE FROM Kosik where ZakaznikID = 1;