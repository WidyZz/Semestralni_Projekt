using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChvojProjekt.Auth;

namespace ChvojProjekt.Core
{
    public class DBDataGrid
    {
        // SQL query -> Bez prizarovani C#ovych promennych
        private string queryObjednavkyA = File.ReadAllText(@"D:\Users\vitch\Desktop\ChvojProjekt\ObjednavkyQueryA.sql");
        private string queryUzivateleA = File.ReadAllText(@"D:\Users\vitch\Desktop\ChvojProjekt\AuthQuery.sql");
        private string queryProdukt = File.ReadAllText(@"D:\Users\vitch\Desktop\ChvojProjekt\ProduktyQuery.sql");
        private string queryKosikA = File.ReadAllText(@"D:\Users\vitch\Desktop\ChvojProjekt\KosikQuery.sql");
        // Pripojeni k SQL databazi
        private SqlConnection _connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Users\vitch\Desktop\ChvojProjekt\ChvojProjekt\DB\DB.mdf;Integrated Security=True");
        // Vypis objednavek z dbo.Objednavky
        public void SQLObjednavky(DataTable dtbl, int userID, string IsAdmin)
        {
            // Pokud je uzivatel admin, ukazeme mu vsechny objednane polozky
            if (IsAdmin == "A")
            {
                SqlDataAdapter sda = new SqlDataAdapter(queryObjednavkyA, _connection);
                sda.Fill(dtbl);
            }
            else
            {
                string queryObjednavkyU = "SELECT o.Id as 'ID', p.Nazev as 'Objednané zboží', o.Kusu as 'Počet kusů', o.Cena, CasObjednani as 'Datum objednání' from Objednavky o  inner join Produkt p on o.ProduktID=p.Id left join Kosik k on o.KosikID=k.Id inner join Auth a on o.ZakaznikID=a.Id WHERE a.Id = " + userID + ";";
                SqlDataAdapter sda = new SqlDataAdapter(queryObjednavkyU, _connection);
                sda.Fill(dtbl);
            }
        }
        // Vypis objednavek z dbo.Auth
        public void SQLZakaznici(DataTable dtbl, string IsAdmin)
        {
            // Kdyby nahodou uzivatel videl sekci s uzivateli, tak mu nic nezobrazime
            if (IsAdmin == "A")
            {
                SqlDataAdapter sda = new SqlDataAdapter(queryUzivateleA, _connection);
                sda.Fill(dtbl);
            }
        }
        // Vypis objednavek z dbo.Objednavky
        public void SQLProdukt(DataTable dtbl)
        {
            SqlDataAdapter sda = new SqlDataAdapter(queryProdukt, _connection);
            sda.Fill(dtbl);
        }
        // Vypis objednavek z dbo.Kosik
        public void SQLKosik(DataTable dtbl, int userID, string IsAdmin)
        {
            //Pokud je admin
            if (IsAdmin == "A")
            {
                //Zobrazime vsechny polozky vsech uzivatelu v kosiku
                SqlDataAdapter sda = new SqlDataAdapter(queryKosikA, _connection);
                sda.Fill(dtbl);
            }
            else
            {
                //Pokud ne, tak jen osobni kosik
                string queryKosikU = "DECLARE @id INT =" + userID + "  Select Id as 'Číslo košíku', ZakaznikID as 'ID Zákazníka', ProduktID as 'Číslo produktu', Celkem_Kusu as 'Počet kusů', Vytvoreno as 'Vytvořeno' From Kosik WHERE ZakaznikID = @id order by Vytvoreno DESC";
                SqlDataAdapter sda = new SqlDataAdapter(queryKosikU, _connection);
                sda.Fill(dtbl);
            }
        }
        // Presun polozek z kosiku do objednavky
        public void SQLObjednat(DataTable dtbl, int userID)
        {
            string queryObjednat = "DECLARE @id INT = " + userID + " INSERT INTO Objednavky (ZakaznikID, KosikID, ProduktID, Kusu, Cena, CasObjednani) SELECT k.ZakaznikID, k.Id, k.ProduktID, k.Celkem_Kusu, k.Celkem_Kusu * p.Cena, GETDATE() from Kosik k inner join Produkt p ON k.ProduktID = p.Id WHERE k.ZakaznikID = @id; DELETE FROM Kosik where ZakaznikID = @id;";
            SqlDataAdapter sda = new SqlDataAdapter(queryObjednat, _connection);
            sda.Fill(dtbl);
        }
        //Metoda pro pridavani polozek do kosiku
        public void SQLPridatDoKosiku(DataTable dtbl, int userID, int produktID)
        {
            try
            {
                //Pokud polozka existuje, updatneme pocet kusu, pokud ne, vytvorime ji
                string queryPridatDoKosiku = "DECLARE @id INT = " + userID + ", @produktID INT = " + produktID + " IF EXISTS(SELECT ZakaznikID, ProduktID FROM Kosik where ZakaznikID = @id AND ProduktID = @produktID) BEGIN UPDATE Kosik set Celkem_Kusu = Celkem_Kusu + 1, Vytvoreno = GETDATE() where ZakaznikID = @id AND ProduktID = @produktID END ELSE BEGIN INSERT INTO Kosik (ZakaznikID, ProduktID, Celkem_Kusu, Vytvoreno) Values (@id, @produktID, 1, GETDATE()) END UPDATE Produkt SET Kusu = Kusu -1 WHERE Id = @produktID;";
                // A nasypeme
                SqlDataAdapter sda = new SqlDataAdapter(queryPridatDoKosiku, _connection);
                sda.Fill(dtbl);
            }
            catch { }

        }
        //Metoda pro odebirani polozek
        public void SQLOdebrat(DataTable dtbl, int userID, int rowID, bool kosikIsChecked)
        {
            try {
                if (kosikIsChecked == true)
                {
                    //Vysypani kosiku - Pricteni zbozi do dbo.Produkt, odstraneni radku v dbo.Kosik s id = produktID
                    string queryOdebratZKosiku = "DECLARE @id INT = "+userID+", @kosikID INT = " + rowID + " UPDATE Produkt SET Kusu = Kusu + (Select Celkem_Kusu from Kosik where ZakaznikID = @id AND Id = @kosikID) where Produkt.Id = (Select ProduktID from Kosik where Id = @kosikID and ZakaznikID = @id); DELETE FROM KOSIK WHERE ZakaznikID = @id AND Id = @kosikID";
                    SqlDataAdapter sda = new SqlDataAdapter(queryOdebratZKosiku, _connection);
                    sda.Fill(dtbl);
                }
                else
                {
                    //Zruseni objednavky - Pricteni zbozi do dbo.Produkt, odstraneni radku v dbo.Objednavky s id = produktID
                    string queryOdebratZObjednavek = "DECLARE @id INT = " + userID + ", @objednavkaID INT = " + rowID + " UPDATE Produkt SET Kusu = Kusu + (Select Kusu from Objednavky where ZakaznikID = @id AND Id = @objednavkaID) where Produkt.Id = (SELECT ProduktID from Objednavky where Id = @objednavkaID); DELETE FROM Objednavky WHERE ZakaznikID=@id and Id=@objednavkaID;";
                    SqlDataAdapter sda = new SqlDataAdapter(queryOdebratZObjednavek, _connection);
                    sda.Fill(dtbl);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "CHYBA", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}