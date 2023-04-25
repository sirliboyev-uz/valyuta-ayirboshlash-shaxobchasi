using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Kurs_ishi
{
    public partial class RoyxatdanOtish : Form
    {
        public RoyxatdanOtish()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
            StreamWriter yoz6 = obj6.CreateText();
            yoz6.Write("false");
            MessageBox.Show("Ro'yxatdan o'tish bekor qilindi", "Diqqat!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            yoz6.Close();
            //AyirboshlashMenu a = new AyirboshlashMenu();
            //a.Show();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="" && textBox2.Text!="" && textBox3.Text!="" && textBox4.Text != "")
            {
                if (textBox4.Text.Length == 9)
                {
                    int k;
                    FileInfo obj4 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\v1.txt");
                    StreamReader oqish = obj4.OpenText();
                    String str = oqish.ReadLine();
                    oqish.Close();
                    FileInfo obj3 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\malumot.txt");
                    StreamWriter yoz = obj3.CreateText();
                    yoz.Write(str + "," + textBox1.Text + "," + textBox2.Text + "," + textBox3.Text + "," + textBox4.Text + ",");
                    yoz.Close();
                    FileInfo obj5 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\v1.txt");
                    StreamWriter yoz5 = obj5.CreateText();
                    k = Convert.ToInt32(str);
                    k++;
                    str = k.ToString();
                    yoz5.Write(str);
                    MessageBox.Show("Ma'lumot muvaffaqiyatli saqlandi","Tasdiqlandi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    yoz5.Close();

                    FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                    StreamWriter yoz6 = obj6.CreateText();
                    yoz6.Write("true");
                    yoz6.Close();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Pasport raqami xato", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Barcha maydon to'ldirilishi shart!", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
    }
}
