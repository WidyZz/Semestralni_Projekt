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
        Prihlaseni prihlaseni = new Prihlaseni();
        DBDataGrid dBDataGrid = new DBDataGrid();
        public MainWindow()
        { 
            InitializeComponent();
        }
        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            Registrace registrace = new Registrace();
            registrace.Show();
        }
        private void PridatBtn_Click(object sender, EventArgs e)
        {
            DataTable dtbl = new DataTable();
            GridData.Items.Refresh();
            dBDataGrid.SQLPridatDoKosiku(dtbl);
        }
        private void OdebratBtn_Click(object sender, EventArgs e)
        {

        }
        private void RBProdukt_Click(object sender, EventArgs e)
        {
            DataTable dtbl = new DataTable("Produkt");
            dBDataGrid.SQLProdukt(dtbl);
            Refresh();
            GridData.ItemsSource = dtbl.DefaultView;
        }
        private void RBObjednavky_Click(object sender, EventArgs e)
        {

            DataTable dtbl = new DataTable("Objednavky");
            dBDataGrid.SQLObjednavky(dtbl);
            Refresh();
            GridData.ItemsSource = dtbl.DefaultView;
        }
        private void Refresh()
        {
            GridData.Columns.Clear();
            GridData.ItemsSource = null;
            GridData.Items.Refresh();
        }

        private void Odhlaseni(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Hide();
            prihlaseni.Show();
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void RBZakaznik_Click(object sender, RoutedEventArgs e)
        {
            DataTable dtbl = new DataTable("Auth");
            dBDataGrid.SQLZakaznici(dtbl);
            Refresh();
            GridData.ItemsSource = dtbl.DefaultView;

        }
    }
}
