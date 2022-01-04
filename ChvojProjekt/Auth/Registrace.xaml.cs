using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Interakční logika pro Registrace.xaml
    /// </summary>
    public partial class Registrace : Window
    {
        //Propy
        public string Jmeno { get; set; }
        public string Heslo { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        
        public Registrace()
        {
            InitializeComponent();
        }
        //Nahravani dat z textboxu
        private void Init()
        {
            Jmeno = RegJmeno.Text.Trim();
            Heslo = RegHeslo.Password.Trim();
            Email = RegEmail.Text.Trim();
            Telefon = RegTelefon.Text.Trim();
        }
        //Presmerovani na login
        private void Redirect()
        {
            Prihlaseni login = new Prihlaseni();
            this.Close();
            login.Show();
        }
        private void MamUcetBtn_OnClick(object sender, EventArgs e)
        {
            //Presmerovani na prihlaseni
            Redirect();
        }
        private void RegistraceBtn(object sender, EventArgs e)
        {
            //Nahrajeme data z textboxu
            Init();
            Autentifikace autentifikace = new Autentifikace();
            DataTable dtbl = new DataTable();
            //Tridy pro overeni platnosti zadanych dat
            var checkMail = new EmailAddressAttribute();
            var checkPhone = new PhoneAttribute();
            //sql prikaz
            string query = "INSERT INTO Auth (Jmeno, Heslo, Email, Telefon) Values ('" + Jmeno + "', '" + Heslo + "', '" + Email + "', '" + Telefon + "');";
            // Registrace, kontrola platných údajů
            if (Jmeno == "" || Heslo == "" || Email == "" || Telefon == "")
                MessageBox.Show("Všechna pole musí být vyplněna!", "CHYBA", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (!checkMail.IsValid(Email))
                MessageBox.Show("Email není platný!", "CHYBA", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (!checkPhone.IsValid(Telefon))
                MessageBox.Show("Telefon není platný!", "CHYBA", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                //Try insertnout data zadana uzivatelem
                try
                {
                    //sql
                    autentifikace.SQLAutentifikace(query, dtbl);
                    //Presmerovani na prihlaseni
                    Redirect();
                    MessageBox.Show("Úspěšná registrace!", "Registrace", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                //pokud se najde stejny jmeno, email nebo tel.c.
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show("Účet již existuje!", $"{ex}", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
