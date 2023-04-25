using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kurs_ishi
{
    public partial class AdminMenu : Form
    {
        public AdminMenu()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

        }
        private string connString = String.Format("Server={0};Port={1};" + "User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "root", "kursishi");

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        string line = "";

        private void AdminMenu_Load(object sender, EventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT id, ismi, familyasi, otasining_ismi as otasi, seriya_raqami as passport, valyutadan, valyutaga, sana, vaqt, kiritilgan_summa as kiritilgan, chiqarilgan_summa as chiqarilgan, komissiya FROM malumotlar";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                guna2DataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Ma'lumotlar mavjud emas", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            NpgsqlConnection connection = new NpgsqlConnection(connString);
            connection.Open();
            NpgsqlCommand cmd1 = new NpgsqlCommand();
            cmd1.Connection = connection;
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = $"select * from foiz";
            DataTable table = new DataTable();
            table.Load(cmd1.ExecuteReader());
            joriy.Text = table.Rows[0][0].ToString() + "%";

            using (WebClient wc = new WebClient())
            {
                line = wc.DownloadString("https://cbu.uz/uz/arkhiv-kursov-valyut/xml/");
                Regex re = new Regex("вЂ");
                line = re.Replace(line, "`");
                /*                comboBox1.Items.Add("UZS");
                                MatchCollection match = Regex.Matches(line, $"<Ccy>(.*?)<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s *)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>");

                                foreach (Match item in match)
                                {
                                    comboBox1.Items.Add(item.Groups[1]);
                                }*/
                cmd1.CommandText = $"select valyuta from miqdor";
                NpgsqlDataReader dataReader = cmd1.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader[0].ToString());
                }
                dataReader.Close();
            }
            connection.Close();

            cmd.Dispose();

            NpgsqlConnection conn3 = new NpgsqlConnection(connString);
            conn3.Open();
            NpgsqlCommand cmd3 = new NpgsqlCommand();
            cmd3.Connection = conn3;
            cmd3.CommandType = CommandType.Text;
            cmd3.CommandText = "SELECT * FROM hodim";
            NpgsqlDataReader reader3 = cmd3.ExecuteReader();
            if (reader3.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(reader3);
                guna2DataGridView3.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Ma'lumotlar mavjud emas", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            NpgsqlConnection connection4 = new NpgsqlConnection(connString);
            connection4.Open();
            NpgsqlCommand cmd4 = new NpgsqlCommand();
            cmd4.Connection = connection4;
            cmd4.CommandType = CommandType.Text;
            cmd4.CommandText = $"select * from miqdor";
            /*            DataTable table4 = new DataTable();
                        table4.Load(cmd4.ExecuteReader());
                        joriy.Text = table4.Rows[0][0].ToString() + "%";*/
            NpgsqlDataReader reader4 = cmd4.ExecuteReader();
            if (reader4.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(reader4);
                guna2DataGridView4.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Ma'lumotlar mavjud emas", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            connection.Close();

            using (WebClient wc = new WebClient())
            {
                line = wc.DownloadString("https://cbu.uz/uz/arkhiv-kursov-valyut/xml/");
                Regex re = new Regex("вЂ");
                line = re.Replace(line, "`");
                MatchCollection match = Regex.Matches(line, $"<Ccy>(.*?)<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s *)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>");

                foreach (Match item in match)
                {
                    valyutaCombo.Items.Add(item.Groups[1]);
                }
            }
            cmd.Dispose();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            /*            int s = 0;
                        obj.Open();
                        OleDbCommand h = obj.CreateCommand();
                        h.CommandType = CommandType.Text;
                        h.CommandText = "select * from valuta2 where Sana='" + textBox1.Text + "'";
                        h.ExecuteNonQuery();
                        DataTable dt = new DataTable();
                        OleDbDataAdapter da = new OleDbDataAdapter(h);
                        da.Fill(dt);
                        s = Convert.ToInt32(dt.Rows.Count.ToString());
                        dataGridView1.DataSource = dt;
                        obj.Close();
                        if (s == 0)
                        {
                            MessageBox.Show("Bunday sanada operatsiya bajarilmagan", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }*/
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"select id, ismi, familyasi, otasining_ismi as otasi, seriya_raqami as passport, valyutadan, valyutaga, sana, vaqt, kiritilgan_summa as kiritilgan, chiqarilgan_summa as chiqarilgan, komissiya from malumotlar where sana='{textBox1.Text}'";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if(textBox1.Text.Equals(""))
                MessageBox.Show("Qidirish uchun sanani kiriting", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (reader.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                guna2DataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Bunday sanada operatsiya bajarilmagan", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (!foizQiymat.Text.Equals(""))
            {
                int f = int.Parse(foizQiymat.Text);
                NpgsqlConnection conn = new NpgsqlConnection(connString);
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"update foiz set foiz={f}";
                cmd.ExecuteReader();
                MessageBox.Show("Komissiya miqdori muvaffaqiyatli saqlandi", "Bajarildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                foizQiymat.Text = "";
                NpgsqlConnection connection = new NpgsqlConnection(connString);
                connection.Open();
                NpgsqlCommand cmd1 = new NpgsqlCommand();
                cmd1.Connection = connection;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = $"select * from foiz";
                DataTable table = new DataTable();
                table.Load(cmd1.ExecuteReader());
                joriy.Text = table.Rows[0][0].ToString() + "%";
                connection.Close();
            }
            else
            {
                MessageBox.Show("Komissiya miqdorini kiriting!", "Ogohlantirish", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }
/*
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connString);
            connection.Open();
            NpgsqlCommand cmd1 = new NpgsqlCommand();
            cmd1.Connection = connection;
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = $"select * from foiz";
            DataTable table = new DataTable();
            table.Load(cmd1.ExecuteReader());
            joriy.Text = table.Rows[0][0].ToString() + "%";
            connection.Close();
        }*/

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"select id, ismi, familyasi, otasining_ismi as otasi, seriya_raqami as passport, valyutadan, valyutaga, sana, vaqt, kiritilgan_summa as kiritilgan, chiqarilgan_summa as chiqarilgan, komissiya from malumotlar where valyutadan='{comboBox1.Text}'";
            NpgsqlDataReader reader = cmd.ExecuteReader();

            NpgsqlConnection conn1 = new NpgsqlConnection(connString);
            conn1.Open();
            NpgsqlCommand cmd1 = new NpgsqlCommand();
            cmd1.Connection = conn1;
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = $"select sum(kiritilgan_summa) from malumotlar where valyutadan='{comboBox1.Text}'";

            if (reader.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                guna2DataGridView2.DataSource = dt;
                DataTable table = new DataTable();
                table.Load(cmd1.ExecuteReader());
                uNatija.Text = comboBox1.Text + " valyutadan ja'mi kiritilgan mablag': " + table.Rows[0][0].ToString() + " "+comboBox1.Text;
            }
            else
            {
                guna2DataGridView2.ClearSelection();
                MessageBox.Show("Bunday valyutada operatsiya bajarilmagan", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"select id, ismi, familyasi, otasining_ismi as otasi, seriya_raqami as passport, valyutadan, valyutaga, sana, vaqt, kiritilgan_summa as kiritilgan, chiqarilgan_summa as chiqarilgan, komissiya from malumotlar where valyutadan='{comboBox1.Text}'";
            NpgsqlDataReader reader = cmd.ExecuteReader();

            NpgsqlConnection conn1 = new NpgsqlConnection(connString);
            conn1.Open();
            NpgsqlCommand cmd1 = new NpgsqlCommand();
            cmd1.Connection = conn1;
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = $"select sum(komissiya) from malumotlar where valyutadan='{comboBox1.Text}'";

            if (reader.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                guna2DataGridView2.DataSource = dt;
                DataTable table = new DataTable();
                table.Load(cmd1.ExecuteReader());
                uNatija.Text = comboBox1.Text + " valyutadan ja'mi ushlab qolingan mablag': " + table.Rows[0][0].ToString() + " " + comboBox1.Text;
            }
            else
            {
                guna2DataGridView2.ClearSelection();
                MessageBox.Show("Bunday valyutadan komissiya mablag'i ushlanmagan", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"select id, ismi, familyasi, otasining_ismi as otasi, seriya_raqami as passport, valyutadan, valyutaga, sana, vaqt, kiritilgan_summa as kiritilgan, chiqarilgan_summa as chiqarilgan, komissiya from malumotlar where valyutaga='{comboBox1.Text}'";
            NpgsqlDataReader reader = cmd.ExecuteReader();

            NpgsqlConnection conn1 = new NpgsqlConnection(connString);
            conn1.Open();
            NpgsqlCommand cmd1 = new NpgsqlCommand();
            cmd1.Connection = conn1;
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = $"select sum(chiqarilgan_summa) from malumotlar where valyutaga='{comboBox1.Text}'";

            if (reader.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                guna2DataGridView2.DataSource = dt;
                DataTable table = new DataTable();
                table.Load(cmd1.ExecuteReader());
                uNatija.Text = comboBox1.Text + " valyutadan ja'mi chiqarilgan mablag': " + table.Rows[0][0].ToString().Substring(0,10) + " " + comboBox1.Text;
            }
            else
            {
                MessageBox.Show("Bunday valyutada operatsiya bajarilmagan", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void hodimQoshish_Click(object sender, EventArgs e)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                int id1 = 0;
                string hodim = "";
                using (var command = new NpgsqlCommand("SELECT * FROM hodim", conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        hodim = reader.GetString(2);
                    }
                    reader.Close();
                }
                if ( hodimIsm.Text.Equals("") || hodimLogin.Text.Equals("") || hodimParol.Text.Equals(""))
                    MessageBox.Show("Maydonni to'ldiring!", "Diqqat!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (!hodimLogin.Text.Equals(hodim))
                {
                    NpgsqlConnection conn4 = new NpgsqlConnection(connString);
                    conn4.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn4;
                    cmd.CommandType = CommandType.Text;
                    if (!hodimLogin.Text.Equals(hodim))
                    {
                        cmd.CommandText = $"insert into hodim(hodim, login, parol) values('{hodimIsm.Text}', '{hodimLogin.Text}','{hodimParol.Text}')";
                        cmd.ExecuteReader();
                        MessageBox.Show("Hodim muvaffaqiyatli qo'shildi", "Yaxshi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        hodimId.Text = "";
                        hodimIsm.Text = "";
                        hodimLogin.Text = "";
                        hodimParol.Text = "";
                        NpgsqlConnection conn3 = new NpgsqlConnection(connString);
                        conn3.Open();
                        NpgsqlCommand cmd3 = new NpgsqlCommand();
                        cmd3.Connection = conn3;
                        cmd3.CommandType = CommandType.Text;
                        cmd3.CommandText = "SELECT * FROM hodim";
                        NpgsqlDataReader reader3 = cmd3.ExecuteReader();
                        if (reader3.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader3);
                            guna2DataGridView3.DataSource = dt;
                        }
                        else
                        {
                            MessageBox.Show("Ma'lumotlar mavjud emas", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Login bir xil bo'lishi mumkin emas", "Ogohlantirish", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        hodimLogin.Text = "";
                    }

                }
                else
                {
                    MessageBox.Show("Bunday login ga ega hodim mavjud", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void hodimYangilash_Click(object sender, EventArgs e)
        {
            try
            {
                if (!hodimId.Text.Equals(""))
                {
                    int id = int.Parse(hodimId.Text);
                    using (var conn = new NpgsqlConnection(connString))
                    {
                        conn.Open();
                        int id1 = 0;
                        string hodim = "";
                        using (var command = new NpgsqlCommand("SELECT * FROM hodim where id=" + id + "", conn))
                        {
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                id1 = reader.GetInt32(0);
                                hodim = reader.GetString(2);
                            }
                            reader.Close();
                        }
                        if (hodimId.Text.Equals("") || hodimIsm.Text.Equals("") || hodimLogin.Text.Equals("") || hodimParol.Text.Equals(""))
                            MessageBox.Show("Maydonni to'ldiring!", "Diqqat!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else if (id == id1)
                        {
                            NpgsqlConnection conn4 = new NpgsqlConnection(connString);
                            conn4.Open();
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            cmd.Connection = conn4;
                            cmd.CommandType = CommandType.Text;
                            if (!hodimLogin.Text.Equals(hodim))
                            {
                                cmd.CommandText = $"update hodim set hodim='{hodimIsm.Text}', login='{hodimLogin.Text}',parol='{hodimParol.Text}' where id={id}";
                                cmd.ExecuteReader();
                                MessageBox.Show("Hodim muvaffaqiyatli yangilandi", "Yaxshi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                hodimId.Text = "";
                                hodimIsm.Text = "";
                                hodimLogin.Text = "";
                                hodimParol.Text = "";
                                NpgsqlConnection conn3 = new NpgsqlConnection(connString);
                                conn3.Open();
                                NpgsqlCommand cmd3 = new NpgsqlCommand();
                                cmd3.Connection = conn3;
                                cmd3.CommandType = CommandType.Text;
                                cmd3.CommandText = "SELECT * FROM hodim";
                                NpgsqlDataReader reader3 = cmd3.ExecuteReader();
                                if (reader3.HasRows)
                                {
                                    DataTable dt = new DataTable();
                                    dt.Load(reader3);
                                    guna2DataGridView3.DataSource = dt;
                                }
                                else
                                {
                                    MessageBox.Show("Ma'lumotlar mavjud emas", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Login bir xil bo'lishi mumkin emas", "Ogohlantirish", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                hodimLogin.Text = "";
                            }

                        }
                        else
                        {
                            MessageBox.Show("Bunday id ga ega hodim mavjud emas", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            hodimId.Text = "";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Id bo'sh bo'lmasligi kerak!", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void jadvalniYangilash_Click(object sender, EventArgs e)
        {
/*            NpgsqlConnection conn3 = new NpgsqlConnection(connString);
            conn3.Open();
            NpgsqlCommand cmd3 = new NpgsqlCommand();
            cmd3.Connection = conn3;
            cmd3.CommandType = CommandType.Text;
            cmd3.CommandText = "SELECT * FROM hodim";
            NpgsqlDataReader reader3 = cmd3.ExecuteReader();
            if (reader3.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(reader3);
                guna2DataGridView3.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Ma'lumotlar mavjud emas", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }*/
        }

        private void hodimOchirish_Click(object sender, EventArgs e)
        {
            try
            {
                if (!hodimId.Text.Equals(""))
                {
                    int id = int.Parse(hodimId.Text);
                    using (var conn = new NpgsqlConnection(connString))
                    {
                        conn.Open();
                        int id1 = 0;
                        using (var command = new NpgsqlCommand("SELECT * FROM hodim", conn))
                        {
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                id1 = reader.GetInt32(0);
                            }
                            reader.Close();
                        }
                        if (id == id1)
                        {
                            NpgsqlConnection conn4 = new NpgsqlConnection(connString);
                            conn4.Open();
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            cmd.Connection = conn4;
                            cmd.CommandType = CommandType.Text;
                                cmd.CommandText = $"delete from hodim  where id={id}";
                                cmd.ExecuteReader();
                                MessageBox.Show("Hodim muvaffaqiyatli o'chirildi", "Yaxshi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                hodimId.Text = "";
                                hodimIsm.Text = "";
                                hodimLogin.Text = "";
                                hodimParol.Text = "";
                            NpgsqlConnection conn3 = new NpgsqlConnection(connString);
                            conn3.Open();
                            NpgsqlCommand cmd3 = new NpgsqlCommand();
                            cmd3.Connection = conn3;
                            cmd3.CommandType = CommandType.Text;
                            cmd3.CommandText = "SELECT * FROM hodim";
                            NpgsqlDataReader reader3 = cmd3.ExecuteReader();
                            if (reader3.HasRows)
                            {
                                DataTable dt = new DataTable();
                                dt.Load(reader3);
                                guna2DataGridView3.DataSource = dt;
                            }
                            else
                            {
                                MessageBox.Show("Ma'lumotlar mavjud emas", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Bunday id ga ega hodim mavjud emas", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            hodimId.Text = "";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Id bo'sh bo'lmasligi kerak!", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void guna2Button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            BoshMenu bosh = new BoshMenu();
            bosh.Show();
        }


        private void guna2Button8_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            BoshMenu bosh = new BoshMenu();
            bosh.Show();
        }

        private void ochirish_Click(object sender, EventArgs e)
        {
            if (!valyutaCombo.Text.Equals(""))
            {
                string valCom = valyutaCombo.Text;
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    double miqdor = 0;
                    string valyuta = "";
                    using (var command = new NpgsqlCommand("SELECT * FROM miqdor where valyuta='" + valCom + "'", conn))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            valyuta = reader.GetString(1);
                            miqdor = reader.GetDouble(2);

                        }
                        reader.Close();
                    }
                    NpgsqlConnection conn4 = new NpgsqlConnection(connString);
                    conn4.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn4;
                    cmd.CommandType = CommandType.Text;
                    if (valCom.Equals(valyuta))
                    {

                        cmd.CommandText = $"delete from miqdor where valyuta='{valCom}'";
                        cmd.ExecuteReader();
                        MessageBox.Show("Valyuta muvaffaqiyatli o'chirildi!", "Amal bajarildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        NpgsqlConnection connection4 = new NpgsqlConnection(connString);
                        connection4.Open();
                        NpgsqlCommand cmd4 = new NpgsqlCommand();
                        cmd4.Connection = connection4;
                        cmd4.CommandType = CommandType.Text;
                        cmd4.CommandText = $"select * from miqdor";
                        /*            DataTable table4 = new DataTable();
                                    table4.Load(cmd4.ExecuteReader());
                                    joriy.Text = table4.Rows[0][0].ToString() + "%";*/
                        NpgsqlDataReader reader4 = cmd4.ExecuteReader();
                        if (reader4.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader4);
                            guna2DataGridView4.DataSource = dt;
                        }
                        else
                        {
                            MessageBox.Show("Ma'lumotlar mavjud emas", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        connection4.Close();

                        using (WebClient wc = new WebClient())
                        {
                            line = wc.DownloadString("https://cbu.uz/uz/arkhiv-kursov-valyut/xml/");
                            Regex re = new Regex("вЂ");
                            line = re.Replace(line, "`");
                            MatchCollection match = Regex.Matches(line, $"<Ccy>(.*?)<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s *)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>");

                            foreach (Match item in match)
                            {
                                valyutaCombo.Items.Add(item.Groups[1]);
                            }
                        }
                        cmd4.Dispose();
                        valyutaMiqdor.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Bunday valyuta mavjud emas!", "Xato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        valyutaMiqdor.Text = "";
                    }
                    valyutaMiqdor.Text = "";

                }
            }
            else
            {
                MessageBox.Show("Maydonni to'ldiring!", "Diqqat!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ayirish_Click(object sender, EventArgs e)
        {
            if (!valyutaCombo.Text.Equals(""))
            {
                string valCom = valyutaCombo.Text;
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    double miqdor = 0;
                    string valyuta = "";
                    using (var command = new NpgsqlCommand("SELECT * FROM miqdor where valyuta='" + valCom + "'", conn))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            valyuta = reader.GetString(1);
                            miqdor = reader.GetDouble(2);

                        }
                        reader.Close();
                    }
                    NpgsqlConnection conn4 = new NpgsqlConnection(connString);
                    conn4.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn4;
                    cmd.CommandType = CommandType.Text;
                    if (valyutaMiqdor.Text.Equals(""))
                        MessageBox.Show("Maydonni to'ldiring!", "Diqqat!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (valCom.Equals(valyuta) && miqdor - Double.Parse(valyutaMiqdor.Text) > 0)
                    {

                        cmd.CommandText = $"update miqdor set miqdori={miqdor - Double.Parse(valyutaMiqdor.Text)} where valyuta='{valCom}'";
                        cmd.ExecuteReader();
                        MessageBox.Show("Kiritilgan miqdor valyutadan muvaffaqiyatli ayrildi", "Amal bajarildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        NpgsqlConnection connection4 = new NpgsqlConnection(connString);
                        connection4.Open();
                        NpgsqlCommand cmd4 = new NpgsqlCommand();
                        cmd4.Connection = connection4;
                        cmd4.CommandType = CommandType.Text;
                        cmd4.CommandText = $"select * from miqdor";
                        /*            DataTable table4 = new DataTable();
                                    table4.Load(cmd4.ExecuteReader());
                                    joriy.Text = table4.Rows[0][0].ToString() + "%";*/
                        NpgsqlDataReader reader4 = cmd4.ExecuteReader();
                        if (reader4.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader4);
                            guna2DataGridView4.DataSource = dt;
                        }
                        else
                        {
                            MessageBox.Show("Ma'lumotlar mavjud emas", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        connection4.Close();

                        using (WebClient wc = new WebClient())
                        {
                            line = wc.DownloadString("https://cbu.uz/uz/arkhiv-kursov-valyut/xml/");
                            Regex re = new Regex("вЂ");
                            line = re.Replace(line, "`");
                            MatchCollection match = Regex.Matches(line, $"<Ccy>(.*?)<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s *)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>");

                            foreach (Match item in match)
                            {
                                valyutaCombo.Items.Add(item.Groups[1]);
                            }
                        }
                        cmd4.Dispose();
                        valyutaMiqdor.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Bunday valyuta mavjud emas yoki mablag' yetarli emas!", "Ogohlantirish", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        hodimId.Text = "";
                    }
                    valyutaMiqdor.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Maydonni to'ldiring!", "Diqqat!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void valYangilashBtn_Click(object sender, EventArgs e)
        {
            /*NpgsqlConnection connection4 = new NpgsqlConnection(connString);
            connection4.Open();
            NpgsqlCommand cmd4 = new NpgsqlCommand();
            cmd4.Connection = connection4;
            cmd4.CommandType = CommandType.Text;
            cmd4.CommandText = $"select * from miqdor";
            *//*            DataTable table4 = new DataTable();
                        table4.Load(cmd4.ExecuteReader());
                        joriy.Text = table4.Rows[0][0].ToString() + "%";*//*
            NpgsqlDataReader reader4 = cmd4.ExecuteReader();
            if (reader4.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(reader4);
                guna2DataGridView4.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Ma'lumotlar mavjud emas", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            connection4.Close();

            using (WebClient wc = new WebClient())
            {
                line = wc.DownloadString("https://cbu.uz/uz/arkhiv-kursov-valyut/xml/");
                Regex re = new Regex("вЂ");
                line = re.Replace(line, "`");
                MatchCollection match = Regex.Matches(line, $"<Ccy>(.*?)<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s *)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>");

                foreach (Match item in match)
                {
                    valyutaCombo.Items.Add(item.Groups[1]);
                }
            }
            cmd4.Dispose();
            valyutaMiqdor.Text = "";*/
        }

        private void valQoshishBtn_Click(object sender, EventArgs e)
        {
            if (!valyutaCombo.Text.Equals(""))
            {
                string valCom = valyutaCombo.Text;
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    double miqdor = 0;
                    string valyuta = "";
                    using (var command = new NpgsqlCommand("SELECT * FROM miqdor where valyuta='" + valCom + "'", conn))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            valyuta = reader.GetString(1);
                            miqdor = reader.GetDouble(2);

                        }
                        reader.Close();
                    }
                    NpgsqlConnection conn4 = new NpgsqlConnection(connString);
                    conn4.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = conn4;
                    cmd.CommandType = CommandType.Text;
                    if (valyutaMiqdor.Text.Equals(""))
                        MessageBox.Show("Maydonni to'ldiring!", "Diqqat!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (valCom.Equals(valyuta))
                    {

                        cmd.CommandText = $"update miqdor set miqdori={miqdor + Double.Parse(valyutaMiqdor.Text)} where valyuta='{valCom}'";
                        cmd.ExecuteReader();
                        MessageBox.Show("Valyuta muvaffaqiyatli yangilandi", "Yaxshi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        NpgsqlConnection connection4 = new NpgsqlConnection(connString);
                        connection4.Open();
                        NpgsqlCommand cmd4 = new NpgsqlCommand();
                        cmd4.Connection = connection4;
                        cmd4.CommandType = CommandType.Text;
                        cmd4.CommandText = $"select * from miqdor";
                        /*            DataTable table4 = new DataTable();
                                    table4.Load(cmd4.ExecuteReader());
                                    joriy.Text = table4.Rows[0][0].ToString() + "%";*/
                        NpgsqlDataReader reader4 = cmd4.ExecuteReader();
                        if (reader4.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader4);
                            guna2DataGridView4.DataSource = dt;
                        }
                        else
                        {
                            MessageBox.Show("Ma'lumotlar mavjud emas", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        connection4.Close();

                        using (WebClient wc = new WebClient())
                        {
                            line = wc.DownloadString("https://cbu.uz/uz/arkhiv-kursov-valyut/xml/");
                            Regex re = new Regex("вЂ");
                            line = re.Replace(line, "`");
                            MatchCollection match = Regex.Matches(line, $"<Ccy>(.*?)<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s *)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>");

                            foreach (Match item in match)
                            {
                                valyutaCombo.Items.Add(item.Groups[1]);
                            }
                        }
                        cmd4.Dispose();
                        valyutaMiqdor.Text = "";
                    }
                    else
                    {
                        cmd.CommandText = $"insert into miqdor(valyuta, miqdori) values ('{valCom}',{Double.Parse(valyutaMiqdor.Text)})";
                        cmd.ExecuteReader();
                        MessageBox.Show("Yangi valyuta muvaffaqiyatli qo'shildi", "Bajarildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        valyutaMiqdor.Text = "";
                        NpgsqlConnection connection4 = new NpgsqlConnection(connString);
                        connection4.Open();
                        NpgsqlCommand cmd4 = new NpgsqlCommand();
                        cmd4.Connection = connection4;
                        cmd4.CommandType = CommandType.Text;
                        cmd4.CommandText = $"select * from miqdor";
                        /*            DataTable table4 = new DataTable();
                                    table4.Load(cmd4.ExecuteReader());
                                    joriy.Text = table4.Rows[0][0].ToString() + "%";*/
                        NpgsqlDataReader reader4 = cmd4.ExecuteReader();
                        if (reader4.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader4);
                            guna2DataGridView4.DataSource = dt;
                        }
                        else
                        {
                            MessageBox.Show("Ma'lumotlar mavjud emas", "Diqqat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        connection4.Close();

                        using (WebClient wc = new WebClient())
                        {
                            line = wc.DownloadString("https://cbu.uz/uz/arkhiv-kursov-valyut/xml/");
                            Regex re = new Regex("вЂ");
                            line = re.Replace(line, "`");
                            MatchCollection match = Regex.Matches(line, $"<Ccy>(.*?)<(.)Ccy>(\n)(\\s*)<CcyNm_RU>(.*?)<(.)CcyNm_RU>(\n)(\\s *)<CcyNm_UZ>(.*?)<(.)CcyNm_UZ>");

                            foreach (Match item in match)
                            {
                                valyutaCombo.Items.Add(item.Groups[1]);
                            }
                        }
                        cmd4.Dispose();
                        valyutaMiqdor.Text = "";
                    }
                    valyutaMiqdor.Text = "";

                }
            }
            else
            {
                MessageBox.Show("Maydonni to'ldiring!", "Diqqat!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (!adminLogin.Text.Equals(""))
                {
                    string login = (adminLogin.Text);
                    string parol = adminParol.Text;
                    using (var conn = new NpgsqlConnection(connString))
                    {
                        conn.Open();
                        string login1 = "";
                        string parol1 = "";
                        using (var command = new NpgsqlCommand("SELECT * FROM admin", conn))
                        {
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                login1 = reader.GetString(1);
                                parol1 = reader.GetString(2);
                            }
                            reader.Close();
                        }
                        if (adminParol.Text.Equals("") || adminLogin.Text.Equals(""))
                            MessageBox.Show("Maydonni to'ldiring!", "Diqqat!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                        {
                            NpgsqlConnection conn4 = new NpgsqlConnection(connString);
                            conn4.Open();
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            cmd.Connection = conn4;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = $"update admin set login='{adminLogin.Text}',parol='{adminParol.Text}'";
                            cmd.ExecuteReader();
                            MessageBox.Show("Admin muvaffaqiyatli yangilandi", "Yaxshi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            adminLogin.Text = "";
                            adminParol.Text = "";

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Bo'sh bo'lmasligi kerak!", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
