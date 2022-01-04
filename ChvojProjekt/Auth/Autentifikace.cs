using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChvojProjekt.Auth
{
    public class Autentifikace
    {
        // Data source
        private SqlConnection _connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Users\vitch\Desktop\ChvojProjekt\ChvojProjekt\DB\DB.mdf;Integrated Security=True");
        // Metoda pro zpracovani sql prikazu
        public void SQLAutentifikace(string query, DataTable dtbl)
        {
            SqlDataAdapter sda = new SqlDataAdapter(query, _connection);
            sda.Fill(dtbl);
            sda.Update(dtbl);
        }
    }
}
