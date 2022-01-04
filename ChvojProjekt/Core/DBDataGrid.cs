using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChvojProjekt.Auth;

namespace ChvojProjekt.Core
{
    public class DBDataGrid
    {
        static MainWindow mainW = new MainWindow();
        public int UserID { get; set; }
        private string queryObjednavky = "SELECT o.Id as 'Číslo objednávky', a.Jmeno as 'Zákazník', p.Nazev as 'Název Produktu', k.Celkem_Kusu as 'Počet kusů', k.Celkem_Kusu * p.Cena as 'Cena' from Objednavky o inner join Produkt p on ProduktID=p.Id inner join Kosik k on KosikID=k.Id inner join Auth a on o.ZakaznikID=a.Id;";
        private string queryUzivatele = "select ID, Jmeno, email as 'E-mail', Telefon, isadmin as 'Admin?' from Auth order by IsAdmin";
        private string queryProdukt = "select * from Produkt order by Kusu DESC";
        
        private SqlConnection _connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Users\vitch\Desktop\ChvojProjekt\ChvojProjekt\DB\DB.mdf;Integrated Security=True");
        // Metoda pro zpracovani sql prikazu
        public void SQLObjednavky(DataTable dtbl)
        {
            SqlDataAdapter sda = new SqlDataAdapter(queryObjednavky, _connection);
            sda.Fill(dtbl);
        }
        public void SQLZakaznici(DataTable dtbl)
        {
            SqlDataAdapter sda = new SqlDataAdapter(queryUzivatele, _connection);
            sda.Fill(dtbl);
        }
        public void SQLProdukt(DataTable dtbl)
        {
            SqlDataAdapter sda = new SqlDataAdapter(queryProdukt, _connection);
            sda.Fill(dtbl);
        }
        //public void SQLObjednavky(DataTable dtbl)
        //{
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    SqlDataAdapter sda = new SqlDataAdapter(queryObjednavky, _connection);
        //    sda.Fill(dtbl);
        //}
        public void SQLPridatDoKosiku(DataTable dtbl)
        {
            try
            {
                string queryPridatDoKosiku = "update KOSIK set Celkem_Kusu = Celkem_Kusu + 1 where ZakaznikID = " + UserID + ";"
                                           + "update PRODUKT set Kusu = Kusu - 1;";
                SqlDataAdapter sda = new SqlDataAdapter(queryPridatDoKosiku, _connection);
                sda.Fill(dtbl);
            }
            catch { }
            
        }
    }
}
