﻿select Id as 'Číslo produktu', Nazev as 'Název produktu', Hmotnost, Kusu as 'Kusů skladem', Cena from Produkt where Kusu > 0 order by Kusu DESC;