using System;
using System.Windows;
using System.Linq;
using System.Data.SqlClient;
using ChvojProjekt.Auth;
using ChvojProjekt.Core;
using System.Data;

namespace ChvojProjekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public string IsAdmin { get; set; }
        public DBDataGrid dBDataGrid = new DBDataGrid();
        public MainWindow()
        { 
            InitializeComponent();
            // Nacteni produktu po spusteni
            ProduktUpdate();
        }
        // MENU Po stisknuti
        private void RBProdukt_Click(object sender, EventArgs e)
        {
            GridData.Columns.Clear();
            ProduktUpdate();
        }
        private void RBKosik_Click(object sender, RoutedEventArgs e)
        {
            GridData.Columns.Clear();
            KosikUpdate();
        }
        private void RBObjednavky_Click(object sender, EventArgs e)
        {
            GridData.Columns.Clear();
            ObjednavkyUpdate();
        }
        private void RBZakaznik_Click(object sender, RoutedEventArgs e)
        {
            GridData.Columns.Clear();
            ZakaznikUpdate();
        }
        private void PridatBtn_Click(object sender, EventArgs e)
        {
            DataTable dtbl = new DataTable();
            GetProductID();
            dBDataGrid.SQLPridatDoKosiku(dtbl, UserID, ProductID);
            //Refresh podle stranky
            if (RBKosik.IsChecked == true)
            { KosikUpdate(); }
            else if (RBObjednavky.IsChecked == true)
            { ObjednavkyUpdate(); }
            else if (RBProdukt.IsChecked == true)
            { ProduktUpdate(); }
            else if (RBZakaznik.IsChecked == true)
            { ZakaznikUpdate(); }
                
        }
        private void OdebratBtn_Click(object sender, EventArgs e)
        {
            DataTable dtbl = new DataTable();
            GetProductID();
            if (RBKosik.IsChecked == true)
            { 
              dBDataGrid.SQLOdebrat(dtbl, UserID, ProductID, true);
              KosikUpdate();
            }
            else if (RBObjednavky.IsChecked == true)
            { 
              dBDataGrid.SQLOdebrat(dtbl, UserID, ProductID, false);
              ObjednavkyUpdate();
            }

        }
        //Update metody
        private void ProduktUpdate()
        {
            // Viditelne je pouze -> tlacitko pro odebirani
            OdebratBtn.Visibility = Visibility.Collapsed;
            PridatBtn.Visibility = Visibility.Visible;
            ObjednatLbl.Visibility = Visibility.Collapsed;
            // Vytvoreni tabulky
            DataTable dtbl = new DataTable("Produkt");
            // Data IN
            dBDataGrid.SQLProdukt(dtbl);
            Refresh();
            // Data OUT
            GridData.ItemsSource = dtbl.DefaultView;
        }
        private void ObjednavkyUpdate()
        {
            // Viditelne je pouze -> tlacitko pro odebirani
            ObjednatLbl.Visibility = Visibility.Hidden;
            OdebratBtn.Visibility = Visibility.Visible;
            PridatBtn.Visibility = Visibility.Collapsed;
            // Vytvoreni tabulky
            DataTable dtbl = new DataTable("Objednavky");
            dBDataGrid.SQLObjednavky(dtbl, UserID, IsAdmin);
            Refresh();
            GridData.ItemsSource = dtbl.DefaultView;
        }
        private void KosikUpdate()
        {
            // Viditelne jsou pouze -> tlacitko pro odebirani a objednani
            ObjednatLbl.Visibility = Visibility.Visible;
            OdebratBtn.Visibility = Visibility.Visible;
            PridatBtn.Visibility = Visibility.Collapsed;
            // Vytvoreni tabulky
            DataTable dtbl = new DataTable("Kosik");
            // Data IN
            dBDataGrid.SQLKosik(dtbl, UserID, IsAdmin);
            Refresh();
            // Data OUT
            GridData.ItemsSource = dtbl.DefaultView;
        }
        private void ZakaznikUpdate()
        {
            // Neviditelna tlacitka
            ObjednatLbl.Visibility = Visibility.Collapsed;
            OdebratBtn.Visibility = Visibility.Collapsed;
            PridatBtn.Visibility = Visibility.Collapsed;
            // Vytvoreni tabulky
            DataTable dtbl = new DataTable("Auth");
            // Data IN
            dBDataGrid.SQLZakaznici(dtbl, IsAdmin);
            Refresh();
            // Data OUT
            GridData.ItemsSource = dtbl.DefaultView;
        }
        //Metoda pro ziskani ID Produktu i ID Objednavky
        private void GetProductID()
        {
            //Vyber hodnoty bunky z prvniho sloupce oznaceneho radku
            if (GridData.SelectedIndex != -1)
            {
                DataRowView productID = GridData.Items[GridData.SelectedIndex] as DataRowView;
                ProductID = (int)productID.Row.ItemArray[0];
            }
        }
        // Krizek -> Vypnuti aplikace
        private void TextBlock_MouseLeftButtonDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        // refresh
        private void Refresh()
        {
            GridData.Items.Refresh();
        }
        private void Odhlaseni(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Prihlaseni prihlaseni = new Prihlaseni();
            this.Hide();
            prihlaseni.Show();
        }
        // Objednavaci tlacitko
        private void ObjednatLbl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataTable dtbl = new DataTable("Kosik");
            dBDataGrid.SQLObjednat(dtbl, UserID);
            KosikUpdate();
        }
    }
}
