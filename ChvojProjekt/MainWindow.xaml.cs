﻿using System;
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
        public DBDataGrid dBDataGrid = new DBDataGrid();
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
            GetProductID();
            dBDataGrid.SQLPridatDoKosiku(dtbl, UserID, ProductID);
            GridData.Items.Refresh();
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
            Prihlaseni prihlaseni = new Prihlaseni();
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
        //Metoda pro ziskani ID Produktu
        private void GetProductID()
        {
            //
            if (GridData.SelectedIndex != -1)
            {
                DataRowView productID = GridData.Items[GridData.SelectedIndex] as DataRowView;
                ProductID = (int)productID.Row.ItemArray[0];
                MessageBox.Show($"{productID.Row.ItemArray[0]}", "DEBUG", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show($"Vyberte produkt", "UPOZORNĚNÍ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }
    }
}
