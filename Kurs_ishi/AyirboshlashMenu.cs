using Npgsql;
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
using System.Net;
using System.Text.RegularExpressions;

namespace Kurs_ishi
{
    public partial class AyirboshlashMenu : Form
    {
        private string connString = String.Format("Server={0};Port={1};" + "User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "root", "kursishi");
        string yy = "";
        string line = "";
        double sum = 0, yfoiz=0;
        string com1 = "", com2 = "";
        int k = 0;
        double chiquvchiSumma = 0, olinadigan=0, oldingi=0, beriladigan=0;
        double[] bazaSumma=new double[2];
       

        public AyirboshlashMenu()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            comboBox2.Enabled = false;
            textBox1.Enabled = false;
        }
        double foiz = 0;
        private void AyirboshlashMenu_Load(object sender, EventArgs e)
        {
/*            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM malumotlar";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                guna2DataGridView1.DataSource = dt;
            }*/
            NpgsqlConnection connection = new NpgsqlConnection(connString);
            connection.Open();
            NpgsqlCommand cmd1 = new NpgsqlCommand();
            cmd1.Connection = connection;
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = $"select * from foiz";
            DataTable table = new DataTable();
            table.Load(cmd1.ExecuteReader());
            label4.Text = "Xizmat haqqi: " + table.Rows[0][0].ToString() + "%";
            foiz= double.Parse(table.Rows[0][0].ToString());
            using (WebClient wc = new WebClient())
            {

                line = wc.DownloadString("https://cbu.uz/uz/arkhiv-kursov-valyut/xml/");
                Regex re = new Regex("вЂ");
                line = re.Replace(line, "`");
/*                comboBox1.Items.Add("UZS");
                comboBox2.Items.Add("UZS");
                MatchCollection match = Regex.Matches(line, $"<Ccy>(.*?)<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s *)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>");

                foreach (Match item in match)
                {
                    comboBox1.Items.Add(item.Groups[1]);
                    comboBox2.Items.Add(item.Groups[1]);
                }*/

                cmd1.CommandText=$"select valyuta from miqdor";
                NpgsqlDataReader dataReader= cmd1.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader[0].ToString());
                    comboBox2.Items.Add(dataReader[0].ToString());

                }
                dataReader.Close();
            }
            connection.Close();

            /*            cmd.Dispose();
                        conn.Close();*/
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            BoshMenu b = new BoshMenu();
            b.Show();
            this.Hide();
        }
        public bool t = false;
        
        private void ayirboshlashBtn_Click(object sender, EventArgs e)
        {
            string a, b;
            a = Convert.ToString(comboBox1.Text);
            b = Convert.ToString(comboBox2.Text);

            if (a.Equals("") || b.Equals(""))
            {
                MessageBox.Show("Xatolik, iltimos valyutani tanlang!", "Xato", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("Xatolik, iltimos summani kiriting!", "Xato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //dollar olish
            else if (!textBox1.Text.Equals(""))
            {
                k = int.Parse(textBox1.Text);
                miqdorTekshir();
                
                
                if (a == "USD" && b == "UZS" && miqdorTekshir())
                {
                    if (k <= 1)
                    {
                        textBox1.Text = "";
                        MessageBox.Show("Biz bunday mablag'ni ayirboshlay olmaymiz!", "Diqqat!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else if (k > 100)
                    {
                        DialogResult dialogResult = MessageBox.Show("Bunday mablag' uchun pasport malumotlari kerak!", "Diqqat!!!", MessageBoxButtons.OKCancel);
                        if (dialogResult == DialogResult.OK)
                        {
                            RoyxatdanOtish royhat = new RoyxatdanOtish();
                            royhat.Show();
                            //suma_natija.Text = (k / valyuta).ToString() + " rubl";
                            yy = "AA";
                            ayirboshlashBtn.Enabled = false;

                        }
                        else if (dialogResult == DialogResult.Cancel)
                        {
                            FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                            StreamWriter yoz6 = obj6.CreateText();
                            yoz6.Write("false");
                            yoz6.Close();
                            this.Show();
                            ayirboshlashBtn.Enabled = true;

                        }

                    }
                    else
                    {
                        ayirboshlashBtn.Enabled = false;
                        FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                        StreamWriter yoz6 = obj6.CreateText();
                        yoz6.Write("true");
                        yoz6.Close();
                        yy = "";
                    }
                }
                //dollar sotish
                if (a == "UZS" && b == "USD" && miqdorTekshir())
                {

                    if (k <= 50000)
                    {
                        textBox1.Text = "";
                        MessageBox.Show("Biz bunday mablag'ni ayirboshlay olmaymiz!", "Diqqat!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else if (k > 1000000)
                    {
                        DialogResult dialogResult = MessageBox.Show("Bunday mablag' uchun pasport malumotlari kerak!", "Diqqat!!!", MessageBoxButtons.OKCancel);
                        if (dialogResult == DialogResult.OK)
                        {
                            RoyxatdanOtish royhat = new RoyxatdanOtish();
                            royhat.Show();
                            yy = "AA";
                            ayirboshlashBtn.Enabled = false;

                        }
                        else if (dialogResult == DialogResult.Cancel)
                        {
                            FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                            StreamWriter yoz6 = obj6.CreateText();
                            yoz6.Write("false");
                            yoz6.Close();
                            this.Show();
                            ayirboshlashBtn.Enabled = true;

                        }

                    }
                    else
                    {
                        FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                        StreamWriter yoz6 = obj6.CreateText();
                        yoz6.Write("true");
                        yoz6.Close();
                        yy = "";
                        ayirboshlashBtn.Enabled = false;

                    }
                }
                //Euro olish
                if (a == "EUR" && b == "UZS" && miqdorTekshir())
                {
                    if (k <= 1)
                    {
                        textBox1.Text = "";
                        MessageBox.Show("Bunday mablag'ni ayirboshlay olmaymiz olmaymiz!", "Diqqat!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    else if (k > 90)
                    {
                        DialogResult dialogResult = MessageBox.Show("Bunday mablag' uchun pasport malumotlari kerak!", "Diqqat!!!", MessageBoxButtons.OKCancel);
                        if (dialogResult == DialogResult.OK)
                        {
                            RoyxatdanOtish royhat = new RoyxatdanOtish();
                            royhat.Show();
                            yy = "AA";
                            ayirboshlashBtn.Enabled = false;


                        }
                        else if (dialogResult == DialogResult.Cancel)
                        {
                            FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                            StreamWriter yoz6 = obj6.CreateText();
                            yoz6.Write("false");
                            yoz6.Close();
                            this.Show();
                            ayirboshlashBtn.Enabled = true;

                        }

                    }
                    else
                    {
                        FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                        StreamWriter yoz6 = obj6.CreateText();
                        yoz6.Write("true");
                        yoz6.Close();
                        yy = "";
                        ayirboshlashBtn.Enabled = false;

                    }

                }
                //Euro sotish
                if (a == "UZS" && b == "EUR" && miqdorTekshir())
                {
                    if (k <= 50000)
                    {
                        textBox1.Text = "";
                        MessageBox.Show("Bunday mablag'ni ayirboshlay olmaymiz olmaymiz!", "Diqqat!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    else if (k > 1000000)
                    {
                        DialogResult dialogResult = MessageBox.Show("Bunday mablag' uchun pasport malumotlari kerak!", "Diqqat!!!", MessageBoxButtons.OKCancel);
                        if (dialogResult == DialogResult.OK)
                        {
                            RoyxatdanOtish royhat = new RoyxatdanOtish();
                            royhat.Show();
                            yy = "AA";
                            ayirboshlashBtn.Enabled = false;


                        }
                        else if (dialogResult == DialogResult.Cancel)
                        {
                            FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                            StreamWriter yoz6 = obj6.CreateText();
                            yoz6.Write("false");
                            yoz6.Close();
                            this.Show();
                            ayirboshlashBtn.Enabled = true;

                        }

                    }
                    else
                    {
                        FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                        StreamWriter yoz6 = obj6.CreateText();
                        yoz6.Write("true");
                        yoz6.Close();
                        yy = "";
                        ayirboshlashBtn.Enabled = false;

                    }

                }
                //rubl olish
                if (a == "RUB" && b == "UZS" && miqdorTekshir())
                {
                    if (k <= 1)
                    {
                        textBox1.Text = "";
                        MessageBox.Show("Bunday mablag'ni ayirboshlay olmaymiz olmaymiz!", "Diqqat!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    else if (k > 7000)
                    {
                        DialogResult dialogResult = MessageBox.Show("Bunday mablag' uchun pasport malumotlari kerak!", "Diqqat!!!", MessageBoxButtons.OKCancel);
                        if (dialogResult == DialogResult.OK)
                        {
                            RoyxatdanOtish royhat = new RoyxatdanOtish();
                            royhat.Show();
                            yy = "AA";
                        }
                        else if (dialogResult == DialogResult.Cancel)
                        {
                            FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                            StreamWriter yoz6 = obj6.CreateText();
                            yoz6.Write("false");
                            yoz6.Close();
                            this.Show();
                        }

                    }
                    else
                    {
                        FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                        StreamWriter yoz6 = obj6.CreateText();
                        yoz6.Write("true");
                        yoz6.Close();
                        yy = "";
                    }

                }
                //rubl sotish
                if (a == "UZS" && b == "RUB" && miqdorTekshir())
                {
                    if (k <= 10000)
                    {
                        textBox1.Text = "";
                        MessageBox.Show("Bunday mablag'ni ayirboshlay olmaymiz olmaymiz!", "Diqqat!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    else if (k >= 1000000)
                    {
                        DialogResult dialogResult = MessageBox.Show("Bunday mablag' uchun pasport malumotlari kerak!", "Diqqat!!!", MessageBoxButtons.OKCancel);
                        if (dialogResult == DialogResult.OK )
                        {
                            RoyxatdanOtish royhat = new RoyxatdanOtish();
                            royhat.Show();
                            yy = "AA";
                        }
                        else if (dialogResult == DialogResult.Cancel)
                        {
                            FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                            StreamWriter yoz6 = obj6.CreateText();
                            yoz6.Write("false");
                            yoz6.Close();
                            this.Show();
                        }

                    }
                    else
                    {
                        FileInfo obj6 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
                        StreamWriter yoz6 = obj6.CreateText();
                        yoz6.Write("true");
                        yoz6.Close();
                        yy = "";
                    }
                }
                if (!textBox1.Text.Equals(""))
                {
                    try
                    {
                        if ((comboBox1.Text.Equals("UZS") && comboBox2.Text.Equals("USD")) || (comboBox1.Text.Equals("UZS") && comboBox2.Text.Equals("EUR")) || comboBox1.Text.Equals("UZS"))
                        {
                            yfoiz = Double.Parse(textBox1.Text) * foiz / 100;
                            suma_natija.Text = ((Double.Parse(textBox1.Text) - (Double.Parse(textBox1.Text) * foiz / 100)) / sum).ToString();
                            chiquvchiSumma=Double.Parse(suma_natija.Text);
                            miqdorTekshir();
                        }
                        else
                        {
                            yfoiz = Double.Parse(textBox1.Text) * foiz / 100;
                            suma_natija.Text = ((Double.Parse(textBox1.Text) - (Double.Parse(textBox1.Text) * foiz / 100)) * sum).ToString();
                            chiquvchiSumma = Double.Parse(suma_natija.Text);
                            miqdorTekshir();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        suma_natija.Text = "";
                    }
                }
                else
                {
                    suma_natija.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Xatolik iltimos summani kiriting!", "Xato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saqlashBtn_Click(object sender, EventArgs e)
        {
            ayirboshlashBtn.Enabled = true;

            FileInfo obj = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\tekshirish.txt");
            StreamReader oqish1 = obj.OpenText();
            String str1 = oqish1.ReadLine();
            oqish1.Close();
            if (comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Valyutani tanlang", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("Summani kiriting", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox1.Text != "" && str1.Equals("true"))
            {
                FileInfo obj4 = new FileInfo(@"C:\Users\Sirli\source\repos\Kurs_ishi\Kurs_ishi\New folder\files\malumot.txt");
                StreamReader oqish = obj4.OpenText();
                String str = oqish.ReadLine();
                string[] a = str.Split(',');
                oqish.Close();

                string m1 = comboBox1.Text;
                string c, b;
                double m;
                string day =DateTime.Now.ToString().Substring(0,9);
                m = Double.Parse(textBox1.Text);
                c = Convert.ToString(comboBox1.Text);
                b = Convert.ToString(comboBox2.Text);


                NpgsqlConnection conn = new NpgsqlConnection(connString);
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                if (suma_natija.Text == "")
                {
                    MessageBox.Show("Summani kiriting", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(miqdorTekshir())
                {
                    double old = beriladigan;
                    double natija =olinadigan;
                    if (yy == "AA")
                    {
                        cmd.CommandText = "INSERT INTO malumotlar(ismi, familyasi, otasining_ismi, seriya_raqami, valyutadan, valyutaga, sana, vaqt,  kiritilgan_summa, chiqarilgan_summa, komissiya)VALUES('" + a[2] + "','" + a[1] + "','" + a[3] + "','" + a[4] + "','" + c + "','" + b + "','" + day + "','" + DateTime.Now.ToLongTimeString() + "','" + m + "','" + Double.Parse(suma_natija.Text) + "','"+yfoiz+"')";
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        yy = "NN";
                        cmd.CommandText = "INSERT INTO malumotlar(ismi, familyasi, otasining_ismi, seriya_raqami, valyutadan, valyutaga, sana, vaqt,  kiritilgan_summa, chiqarilgan_summa, komissiya)VALUES('" + yy + "','" + yy + "','" + yy + "','" + yy + "','" + c + "','" + b + "','" + day + "','" + DateTime.Now.ToLongTimeString() + "','" + m + "','" + Double.Parse(suma_natija.Text) + "','" + yfoiz + "')";
                        cmd.ExecuteNonQuery();
                    }
                    cmd.CommandText = $"update miqdor set miqdori={natija - chiquvchiSumma} where valyuta='{comboBox2.SelectedItem}'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"update miqdor set miqdori={Double.Parse(textBox1.Text) + old} where valyuta='{comboBox1.SelectedItem}'";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Malumotlar muvaffaqiyatli saqlandi!","Saqlandi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    suma_natija.Text = "";
                    textBox1.Text = "";
                    comboBox1.SelectedText = "";
                    comboBox2.SelectedText = "";
                }
                conn.Close();
            }
            else
            {
                MessageBox.Show("Xatolik, ayirboshlash tugmasini bosing va ma'lumotlar to'g'ri ekanligiga ishonch hosil qiling!", "Xatolik!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tozalashBtn_Click(object sender, EventArgs e)
        {
            suma_natija.Text = "";
            textBox1.Text = "";
            comboBox1.SelectedText = "UZS";
            comboBox2.SelectedText = "USD";
            ayirboshlashBtn.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                /*com1 = comboBox1.SelectedText();*/
                comboBox2.Enabled = true;
                com1 = comboBox1.Text;
                /*                if (!com1.Equals(""))
                                {
                                    comboBox2.Items.Remove(com1);
                                    comboBox2.Items.Add(com1);
                                }
                                comboBox2.Items.Remove(comboBox1.SelectedItem);
                                com1 = comboBox1.SelectedItem.ToString();*/
                /*                if (comboBox2.SelectedIndex != -1)
                                {

                                }*/

/*                label5.Text = comboBox2.Text;
                if (!textBox1.Text.Equals(""))
                    suma_natija.Text = (Double.Parse(textBox1.Text) * sum).ToString();

                if (comboBox1.Text.Equals("UZS") && !comboBox2.Text.Equals("UZS"))
                {
                    Match match = Regex.Match(line, $"<Ccy>{comboBox2.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                    sum = Double.Parse(match.Groups[27].Value) / Double.Parse(match.Groups[23].Value);
                    label3.Text = $"1 {comboBox1.SelectedItem}  {sum} {comboBox2.SelectedItem} ga teng";
                }
                else if (comboBox2.Text.Equals("UZS") && !comboBox1.Text.Equals("UZS"))
                {
                    Match match = Regex.Match(line, $"<Ccy>{comboBox1.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                    sum = Double.Parse(match.Groups[27].Value) / Double.Parse(match.Groups[23].Value);

                    label3.Text = $"1 {comboBox1.SelectedItem}  {sum} {comboBox2.SelectedItem} ga teng";
                }
                else if (comboBox1.Text.Equals("UZS") && comboBox2.Text.Equals("UZS"))
                {
                    label3.Text = $"1 {comboBox1.SelectedItem}  1 {comboBox2.SelectedItem} ga teng";
                }
                else
                {
                    Match match = Regex.Match(line, $"<Ccy>{comboBox1.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                    Match match1 = Regex.Match(line, $"<Ccy>{comboBox2.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                    double d = Double.Parse(match1.Groups[23].Value) / Double.Parse(match1.Groups[27].Value);
                    sum = Double.Parse(match.Groups[27].Value) * d;
                    label3.Text = $"1 {comboBox1.SelectedItem}  {sum} {comboBox2.SelectedItem} ga teng";
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yaxshi davom eting!, qaysi valyutaga ayirboshlamoqchisiz?","raz bir-bir");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            try
            {
                /*                if (!com2.Equals(""))
                                {
                                    comboBox1.Items.Remove(com2);
                                    comboBox1.Items.Add(com2);
                                }
                                comboBox1.Items.Remove(comboBox2.SelectedItem);
                                com2 = comboBox2.Text;*/

                com2 = comboBox2.Text;
                label5.Text = comboBox2.SelectedItem.ToString();
                if (comboBox1.Text.Equals("UZS") && !comboBox2.Text.Equals("UZS"))
                {
                    Match match = Regex.Match(line, $"<Ccy>{comboBox2.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                    sum = Double.Parse(match.Groups[27].Value) / Double.Parse(match.Groups[23].Value);
                    label3.Text = $"1 {comboBox2.SelectedItem}  {sum} {comboBox1.SelectedItem} ga teng";
                }
                else if (comboBox2.Text.Equals("UZS") && !comboBox1.Text.Equals("UZS"))
                {
                    Match match = Regex.Match(line, $"<Ccy>{comboBox1.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                    sum = Double.Parse(match.Groups[27].Value) / Double.Parse(match.Groups[23].Value);

                    label3.Text = $"1 {comboBox1.SelectedItem}  {sum} {comboBox2.SelectedItem} ga teng";
                }
                else if (comboBox1.Text.Equals("UZS") && comboBox2.Text.Equals("UZS"))
                {
                    label3.Text = $"1 {comboBox1.SelectedItem}  1 {comboBox2.SelectedItem} ga teng";
                }
                else
                {
                    Match match = Regex.Match(line, $"<Ccy>{comboBox1.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                    Match match1 = Regex.Match(line, $"<Ccy>{comboBox2.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                    double d = Double.Parse(match1.Groups[23].Value) / Double.Parse(match1.Groups[27].Value);
                    sum = Double.Parse(match.Groups[27].Value) * d;
                    label3.Text = $"1 {comboBox1.SelectedItem}  {sum} {comboBox2.SelectedItem} ga teng";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"raz bir-ikki");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
        private bool miqdorTekshir()
        {

            NpgsqlConnection connection = new NpgsqlConnection(connString);
            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"select miqdori from miqdor where valyuta='{com2}'";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                int i = 0;
                while (reader.Read())
                {
                    olinadigan = reader.GetDouble(0);
                    i++;
                }
            }

            else
            {
                suma_natija.Text = "";
                textBox1.Text = "";
                MessageBox.Show("Bunday valyutaga ayirboshlash xizmati mavjud emas.","Kechirasiz!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            reader.Close();
            connection.Close();

            NpgsqlConnection connection1 = new NpgsqlConnection(connString);
            connection1.Open();
            NpgsqlCommand cmd1 = new NpgsqlCommand();
            cmd1.Connection = connection1;
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = $"select miqdori from miqdor where valyuta='{com1}'";
            NpgsqlDataReader reader1 = cmd1.ExecuteReader();
            if (reader1.HasRows)
            {
                int i = 0;
                while (reader1.Read())
                {
                    beriladigan = reader1.GetDouble(0);
                    i++;
                }
            }

            else
            {
                suma_natija.Text = "";
                textBox1.Text = "";
                MessageBox.Show("Bunday valyutaga ayirboshlash xizmati mavjud emas.", "Kechirasiz!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            reader.Close();
            connection.Close();

            double natija = olinadigan;
            if (natija - chiquvchiSumma >= 0)
            {
                return true;
            }
            else
            {
                suma_natija.Text = "";
                textBox1.Text = "";
                MessageBox.Show("Bazada mablag' yetarli emas","Kechirasiz!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
        }
    }
}
