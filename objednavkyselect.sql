﻿SELECT o.Id, a.Jmeno as 'Zákazník', p.Nazev as 'Název Produktu', k.Celkem_Kusu as 'Počet kusů', k.Celkem_Kusu * p.Cena as 'Cena'
from Objednavky o
inner join Produkt p on ProduktID=p.Id
inner join Kosik k on KosikID=k.Id
inner join Auth a on o.ZakaznikID=a.Id;

