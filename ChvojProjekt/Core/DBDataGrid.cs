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
        //Prop
        private string queryObjednavky = File.ReadAllText(@"D:\Users\vitch\Desktop\ChvojProjekt\ObjednavkyQuery.sql");
        private string queryUzivatele = File.ReadAllText(@"D:\Users\vitch\Desktop\ChvojProjekt\AuthQuery.sql");
        private string queryProdukt = File.ReadAllText(@"D:\Users\vitch\Desktop\ChvojProjekt\ProduktyQuery.sql");
        private string queryKosik = File.ReadAllText(@"D:\Users\vitch\Desktop\ChvojProjekt\KosikQuery.sql");
        private string queryObjednat = File.ReadAllText(@"D:\Users\vitch\Desktop\ChvojProjekt\Objednat.sql");

        private SqlConnection _connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Users\vitch\Desktop\ChvojProjekt\ChvojProjekt\DB\DB.mdf;Integrated Security=True");
        // Vypis objednavek z dbo.Objednavky
        public void SQLObjednavky(DataTable dtbl)
        {
            SqlDataAdapter sda = new SqlDataAdapter(queryObjednavky, _connection);
            sda.Fill(dtbl);
        }
        // Vypis objednavek z dbo.Auth
        public void SQLZakaznici(DataTable dtbl)
        {
            SqlDataAdapter sda = new SqlDataAdapter(queryUzivatele, _connection);
            sda.Fill(dtbl);
        }
        // Vypis objednavek z dbo.Objednavky
        public void SQLProdukt(DataTable dtbl)
        {
            SqlDataAdapter sda = new SqlDataAdapter(queryProdukt, _connection);
            sda.Fill(dtbl);
        }
        // Vypis objednavek z dbo.Kosik
        public void SQLKosik(DataTable dtbl)
        {
            SqlDataAdapter sda = new SqlDataAdapter(queryKosik, _connection);
            sda.Fill(dtbl);
        }

        public void SQLPridatDoKosiku(DataTable dtbl, int userID, int productID)
        {
            MessageBox.Show($"{userID}, {productID}", "DEBUG", MessageBoxButton.OK);
            try
            {
                string queryPridatDoKosiku = "update KOSIK set Celkem_Kusu = Celkem_Kusu + 1 where ZakaznikID = " +  userID  + ";"
                                           + "update PRODUKT set Kusu = Kusu - 1 where Id=" + productID + ";";
                SqlDataAdapter sda = new SqlDataAdapter(queryPridatDoKosiku, _connection);
                sda.Fill(dtbl);
            }
            catch { }
            
        }
    }
}
