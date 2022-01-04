using ChvojProjekt.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChvojProjekt.Auth
{
    /// <summary>
    /// Interakční logika pro Prihlaseni.xaml
    /// </summary>
    public partial class Prihlaseni : Window
    {
        //Props
        public string Jmeno { get; set; }
        private string Heslo { get; set; }
        private string IsAdmin { get; set; }
        //Init
        private DBDataGrid dB;
        public Prihlaseni()
        {
            InitializeComponent();
            this.dB = new DBDataGrid();
        }
        private void Init()
        {
            Jmeno = PrJmeno.Text.Trim();
            Heslo = PrHeslo.Password.ToString();
        }
        /// <summary>
        /// Tato metoda se vyvolá po úspěšném přihlášení
        /// Tato metoda určuje podle oprávnění, zda se má v menu objevit Zakaznici
        /// Admin zákazníky vidí, běžný uživatel nikoliv
        /// </summary>
        public void PrihlaseniOK()
        {
            MainWindow mainWindow = new MainWindow();
            if (IsAdmin != "A")
                mainWindow.RBZakaznik.Visibility = Visibility.Collapsed;
            this.Hide();
            mainWindow.Show();
            
        }
        //Prihlasovaci tlacitko
        private void PrihlaseniBtn(object sender, EventArgs e)
        {
            Init();
            var query = "Select * from Auth Where Jmeno = '" + Jmeno + "' AND Heslo = '" + Heslo + "'";
            Autentifikace autentifikace = new Autentifikace();
            DataTable dtbl = new DataTable("Auth");
            autentifikace.SQLAutentifikace(query, dtbl);
            if (dtbl.Rows.Count > 0)
            {
                this.dB.UserID = (int)dtbl.Rows[0][0];
                //data z IsAdmin sloupce
                IsAdmin = dtbl.Rows[0][5].ToString();
                //vyvolání aplikace
                PrihlaseniOK();
                //Pozdrav podle oprávnění
                if (IsAdmin == "A") 
                    MessageBox.Show($"Vítejte Admine: \n\r{Jmeno.ToUpper()}", "Úspěšné přihlášení", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show($"Vítejte {Jmeno}!", "Úspěšné přihlášení", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }
            else
            {
                //Špatné přihlašovací údaje
                MessageBox.Show($"Zadane udaje jsou neplatne","CHYBA", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Přesměrování k registraci
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Registrace registrace = new Registrace();
            registrace.Show();
        }
        //Křížek - vypnutí aplikace
        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
