using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Npgsql;
namespace ValyutaAyirboshlash
{
    public partial class Test : Form
    {

        string line = "";
        double sum = 0;
        string com1 = "", com2 = "";
        private string connString = String.Format("Server={0};Port={1};" + "User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "root", "kursishi");


        public Test()
        {
            InitializeComponent();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 1)
            {
                double kiritlgan = Double.Parse(textBox1.Text);
                double qiymat = Double.Parse(textBox2.Text);
                if (qiymat > 100)
                {
                    NpgsqlConnection connection = new NpgsqlConnection(connString);
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $"select * from Miqdor where valyuta in ('{comboBox2.SelectedItem}','{comboBox1.SelectedItem}')";
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataSet dataset = new DataSet();
                    /*DataTable table = new DataTable();
                    table.Load(cmd.ExecuteReader());
                    dataGridView1.DataSource = table;*/
                    dataset.Reset();
                    adapter.Fill(dataset);
                    double old = Double.Parse(dataset.Tables[0].Rows[1][3].ToString());
                    double natija = Double.Parse(dataset.Tables[0].Rows[0][3].ToString());
                    if (natija - qiymat >= 0)
                    {
                        cmd.CommandText = $"update Miqdor set miqdori={natija - qiymat} where valyuta='{comboBox2.SelectedItem}'";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"update Miqdor set miqdori={kiritlgan + old} where valyuta='{comboBox1.SelectedItem}'";
                        cmd.ExecuteNonQuery();
                        String s = DateTime.Now.ToString("yyyy-MM-dd");
                        cmd.CommandText = $"insert into Amaliyot(kvaluta,ovaluta,kmiqdor,omiqdor,kurs,terminal,date) values('{comboBox1.SelectedItem}','{comboBox2.SelectedItem}',{kiritlgan},{qiymat},{sum},'{01711255}','{s}')";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Marhamat pullaringizni oling");

                        textBox2.Text = "";
                        textBox1.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Bazada mablag' yetarli emas");
                    }

                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Natog'ri qiymat kiritildi");
                }
            }
            else
            {
                MessageBox.Show("Pul miqdorini kiriting");
            }
        }

        private void Test_Load(object sender, EventArgs e)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connString);
            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"select * from foiz";
            DataTable table = new DataTable();
            table.Load(cmd.ExecuteReader());
            label4.Text = "Xizmat haqqi " + table.Rows[0][0].ToString() + "%";
            connection.Close();
            using (WebClient wc = new WebClient())
            {

                line = wc.DownloadString("https://cbu.uz/uz/arkhiv-kursov-valyut/xml/");
                Regex re = new Regex("вЂ");
                line = re.Replace(line, "`");
                comboBox1.Items.Add("UZS");
                comboBox2.Items.Add("UZS");
                MatchCollection match = Regex.Matches(line, $"<Ccy>(.*?)<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s *)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>");

                foreach (Match item in match)
                {
                    comboBox1.Items.Add(item.Groups[1]);
                    comboBox2.Items.Add(item.Groups[1]);
                }
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                try
                {
                    textBox2.Text = (Double.Parse(textBox1.Text) * sum).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    textBox2.Text = "";
                }
            }
            else
            {
                textBox2.Text = "";
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {

                comboBox2.Enabled = true;
                if (!com1.Equals(""))
                {
                    comboBox2.Items.Remove(com1);
                    comboBox2.Items.Add(com1);
                }
                comboBox2.Items.Remove(comboBox1.SelectedItem);
                com1 = comboBox1.SelectedItem.ToString();
                if (comboBox2.SelectedIndex != -1)
                {
                    label2.Text = "Pul miqdorini kiriting (" + comboBox1.SelectedItem + ")";
                    label5.Text = "Umumiy mablag' (" + comboBox2.SelectedItem + ")";
                    if (!textBox1.Text.Equals(""))
                        textBox2.Text = (Double.Parse(textBox1.Text) * sum).ToString();
                    if (comboBox1.SelectedItem.Equals("UZS") && !comboBox2.SelectedItem.Equals("UZS"))
                    {
                        Match match = Regex.Match(line, $"<Ccy>{comboBox2.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                        sum = Double.Parse(match.Groups[27].Value) / Double.Parse(match.Groups[23].Value);
                        label3.Text = $"1 {comboBox1.SelectedItem} 👉 {sum} {comboBox2.SelectedItem} ga teng";
                    }
                    else if (comboBox2.SelectedItem.Equals("UZS") && !comboBox1.SelectedItem.Equals("UZS"))
                    {
                        Match match = Regex.Match(line, $"<Ccy>{comboBox1.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                        sum = Double.Parse(match.Groups[27].Value) / Double.Parse(match.Groups[23].Value);

                        label3.Text = $"1 {comboBox1.SelectedItem} 👉 {sum} {comboBox2.SelectedItem} ga teng";
                    }
                    else if (comboBox1.SelectedItem.Equals("UZS") && comboBox2.SelectedItem.Equals("UZS"))
                    {
                        label3.Text = $"1 {comboBox1.SelectedItem} 👉 1 {comboBox2.SelectedItem} ga teng";
                    }
                    else
                    {
                        Match match = Regex.Match(line, $"<Ccy>{comboBox1.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                        Match match1 = Regex.Match(line, $"<Ccy>{comboBox2.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                        double d = Double.Parse(match1.Groups[23].Value) / Double.Parse(match1.Groups[27].Value);
                        sum = Double.Parse(match.Groups[27].Value) * d;
                        label3.Text = $"1 {comboBox1.SelectedItem} 👉 {sum} {comboBox2.SelectedItem} ga teng";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (!com2.Equals(""))
                {
                    comboBox1.Items.Remove(com2);
                    comboBox1.Items.Add(com2);
                }
                comboBox1.Items.Remove(comboBox2.SelectedItem);
                com2 = comboBox2.SelectedItem.ToString();
                //panel1.Visible = true;
                label2.Text = "Pul miqdorini kiriting (" + comboBox1.SelectedItem + ")";
                label5.Text = "Umumiy mablag' (" + comboBox2.SelectedItem + ")";
                if (!textBox1.Text.Equals(""))
                    textBox2.Text = (Double.Parse(textBox1.Text) * sum).ToString();
                if (comboBox1.SelectedItem.Equals("UZS") && !comboBox2.SelectedItem.Equals("UZS"))
                {
                    Match match = Regex.Match(line, $"<Ccy>{comboBox2.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                    sum = Double.Parse(match.Groups[27].Value) / Double.Parse(match.Groups[23].Value);
                    label3.Text = $"1 {comboBox1.SelectedItem} 👉 {sum} {comboBox2.SelectedItem} ga teng";
                }
                else if (comboBox2.SelectedItem.Equals("UZS") && !comboBox1.SelectedItem.Equals("UZS"))
                {
                    Match match = Regex.Match(line, $"<Ccy>{comboBox1.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                    sum = Double.Parse(match.Groups[27].Value) / Double.Parse(match.Groups[23].Value);

                    label3.Text = $"1 {comboBox1.SelectedItem} 👉 {sum} {comboBox2.SelectedItem} ga teng";
                }
                else if (comboBox1.SelectedItem.Equals("UZS") && comboBox2.SelectedItem.Equals("UZS"))
                {
                    label3.Text = $"1 {comboBox1.SelectedItem} 👉 1 {comboBox2.SelectedItem} ga teng";
                }
                else
                {
                    Match match = Regex.Match(line, $"<Ccy>{comboBox1.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                    Match match1 = Regex.Match(line, $"<Ccy>{comboBox2.SelectedItem}<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s*)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>(\n)(\\s*)<CcyNm_UZC>(.*?)<(.)CcyNm_UZC>(\n)(\\s*)<CcyNm_EN>(.*?)<(.)CcyNm_EN>(\n)(\\s*)<CcyMnrUnts>2<(.)CcyMnrUnts>(\n)(\\s*)<Nominal>(.*?)<(.)Nominal>(\n)(\\s*)<Rate>(.*?)<(.)Rate>");
                    double d = Double.Parse(match1.Groups[23].Value) / Double.Parse(match1.Groups[27].Value);
                    sum = Double.Parse(match.Groups[27].Value) * d;
                    label3.Text = $"1 {comboBox1.SelectedItem} 👉 {sum} {comboBox2.SelectedItem} ga teng";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Test().Show();
/*            NpgsqlConnection connection = new NpgsqlConnection(connString);
            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"select miqdori from miqdor where valyuta in ('{comboBox2.SelectedItem}','{comboBox1.SelectedItem}')";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                int i = 0;
                while (reader.Read())
                {
                    bazaSumma[i] = reader.GetDouble(0);
                    MessageBox.Show(bazaSumma[i].ToString(), "aaaaaaaaaaaaaaaaaaaa", MessageBoxButtons.OK, MessageBoxIcon.Stop);

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
            double natija = bazaSumma[0];
            if (natija - chiquvchiSumma >= 0)
            {
                return true;
            }
            else
            {
                suma_natija.Text = "";
                textBox1.Text = "";
                MessageBox.Show("Bazada mablag' yetarli emas", "Kechirasiz!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }*/
        }

    }
}