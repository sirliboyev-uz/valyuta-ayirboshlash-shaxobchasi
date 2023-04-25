using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using ValyutaAyirboshlash;

namespace Kurs_ishi
{
    public partial class BoshMenu : Form
    {
        private string connString = String.Format("Server={0};Port={1};" + "User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "root", "kursishi");
        public BoshMenu()
        {
            InitializeComponent();
            parolText.PasswordChar = '●';
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
/*            Test a = new Test();
            a.Show();*/
            if (tanlov.SelectedIndex == 0)
            {
                hodimKirish();
            }
            if (tanlov.SelectedIndex == 1)
            {
                adminKirish();
            }
        }
        private void hodimKirish()
        {
            string login = loginText.Text;
            string parol = parolText.Text;
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string login1 = "", parol1 = "";
                using (var command = new NpgsqlCommand("SELECT * FROM hodim where login='" + login + "' and parol='" + parol + "'", conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        login1 = reader.GetString(2);
                        parol1 = reader.GetString(3);
                    }
                    reader.Close();
                }
                if (login.Equals("") || parol.Equals(""))
                    MessageBox.Show("Maydonni to'ldiring!", "Diqqat!");
                else if (login.Equals(login1) && parol.Equals(parol1))
                {

                    AyirboshlashMenu ayirboshlash = new AyirboshlashMenu();
                    ayirboshlash.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Parol yoki login xato, iltimos tekshirib qaytadan kiriting", "Xatolik");
                }
            }
        }
        private void adminKirish()
        {
            string login = loginText.Text;
            string parol = parolText.Text;
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string login1 = "", parol1 = "";
                using (var command = new NpgsqlCommand("SELECT * FROM admin where login='" + login + "' and parol='" + parol + "'", conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        login1 = reader.GetString(1);
                        parol1 = reader.GetString(2);
                    }
                    reader.Close();
                }
                if (login.Equals("") || parol.Equals(""))
                    MessageBox.Show("Maydonni to'ldiring!", "Diqqat!");
                else if (login.Equals(login1) && parol.Equals(parol1))
                {

                    AdminMenu ayirboshlash = new AdminMenu();
                    ayirboshlash.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Parol yoki login xato, iltimos tekshirib qaytadan kiriting", "Xatolik");
                }
            }
        }

        private void BoshMenu_Load(object sender, EventArgs e)
        {
        }
    }
}
