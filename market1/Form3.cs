using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;

namespace market1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        byte umumi_no = 0;
        byte anliq_no = 0;
        double iscinin_umumi_meblegi = 0;
        public string ad;

        OleDbConnection access = new OleDbConnection("Provider= Microsoft.JET.OleDb.4.0;Data Source=" +
            Application.StartupPath + "\\market.mdb");

        private void cedvele_yukle()
        {
            try
            {
                access.Open();
                OleDbDataAdapter yukle = new OleDbDataAdapter("select * from anliq_satis", access);
                DataSet yaddas = new DataSet();
                yukle.Fill(yaddas);
                dataGridView1.DataSource = yaddas.Tables[0];
                access.Close();
            }
            catch (Exception xatamsj)
            {
                MessageBox.Show(xatamsj.Message);
                access.Close();
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            button7.Enabled = false;
            textBox1.Text = ad;
            access.Open();
            OleDbDataAdapter listele = new OleDbDataAdapter("select * from anliq_satis", access);
            DataSet siyahi = new DataSet();
            listele.Fill(siyahi);
            dataGridView1.DataSource = siyahi.Tables[0];
            OleDbCommand temizle = new OleDbCommand("delete * from anliq_satis", access);
            temizle.ExecuteReader();
            access.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            access.Open();
            string button = button1.Text;
            OleDbCommand mehsul_var = new OleDbCommand("select Adi,Qiymet,Say from mehsullar where " +
                "Adi = '" + button + "' and Say>0", access);
            OleDbDataReader mehsul_yoxlama = mehsul_var.ExecuteReader();
            if (mehsul_yoxlama.Read())
            {
                string s_h_mehsul_sayi = mehsul_yoxlama["Say"].ToString();
                int h_mehsul_sayi = Convert.ToInt32(s_h_mehsul_sayi);
                OleDbCommand umumi_var = new OleDbCommand("select * from umumi_satis where " +
                "Adi = '" + button + "'", access);
                OleDbDataReader umumi_yoxlama = umumi_var.ExecuteReader();
                if (umumi_yoxlama.Read())
                {
                    OleDbCommand anliq_var = new OleDbCommand("select * from anliq_satis where " +
                        "Adi = '" + button + "'", access);
                    OleDbDataReader anliq_yoxlama = anliq_var.ExecuteReader();
                    if (anliq_yoxlama.Read())
                    {
                        h_mehsul_sayi = Convert.ToInt32(s_h_mehsul_sayi) - 1;
                        string umumi_mehsul_sayi = umumi_yoxlama["Sayi"].ToString();
                        int i_umumi_mehsul_sayi = Convert.ToInt32(umumi_mehsul_sayi) + 1;
                        string s_umumi_sayi = Convert.ToString(i_umumi_mehsul_sayi);
                        string str_h_mehsul_sayi = Convert.ToString(h_mehsul_sayi);
                        string s_anliq_mehsul_sayi = anliq_yoxlama["Sayi"].ToString();
                        int i_anliq_mehsul_sayi = Convert.ToInt32(s_anliq_mehsul_sayi) + 1;
                        string anliq_mehsul_sayi = Convert.ToString(i_anliq_mehsul_sayi);
                        string mehsul_qiymeti = mehsul_yoxlama["Qiymet"].ToString();
                        double d_mehsul_qiymeti = Convert.ToDouble(mehsul_qiymeti);
                        double d_mehsul_meblegi = d_mehsul_qiymeti * i_anliq_mehsul_sayi;
                        string mehsul_meblegi = Convert.ToString(d_mehsul_meblegi);
                        double i_umumi_mebleg = i_umumi_mehsul_sayi * d_mehsul_qiymeti;
                        string str_umumi_mehsul_meblegi = Convert.ToString(i_umumi_mebleg);
                        OleDbCommand anliq_yenile = access.CreateCommand();
                        anliq_yenile.CommandType = CommandType.Text;
                        anliq_yenile.CommandText = "update anliq_satis set Sayi='" + anliq_mehsul_sayi + "' " +
                            ",Meblegi='" + mehsul_meblegi + "' where Adi='" + button + "'";
                        anliq_yenile.ExecuteNonQuery();
                        OleDbCommand umumi_yenile = access.CreateCommand();
                        umumi_yenile.CommandType = CommandType.Text;
                        umumi_yenile.CommandText = "update umumi_satis set Sayi='" + s_umumi_sayi + "' " +
                            ",Meblegi='" + str_umumi_mehsul_meblegi + "' where Adi='" + button + "'";
                        umumi_yenile.ExecuteNonQuery();
                        OleDbCommand mehsul_yenile = access.CreateCommand();
                        mehsul_yenile.CommandType = CommandType.Text;
                        mehsul_yenile.CommandText = "update mehsullar set Say='" + str_h_mehsul_sayi + "' " +
                            "where Adi='" + button + "'";
                        mehsul_yenile.ExecuteNonQuery();
                        access.Close();
                        cedvele_yukle();
                    }
                    else
                    {
                        anliq_no++;
                        h_mehsul_sayi = Convert.ToInt32(s_h_mehsul_sayi) - 1;
                        string umumi_mehsul_sayi = umumi_yoxlama["Sayi"].ToString();
                        int i_umumi_mehsul_sayi = Convert.ToInt32(umumi_mehsul_sayi) + 1;
                        string s_umumi_sayi = Convert.ToString(i_umumi_mehsul_sayi);
                        string str_h_mehsul_sayi = Convert.ToString(h_mehsul_sayi);
                        string str_anliq_no = Convert.ToString(anliq_no);
                        string anliq_mehsul_sayi = "1";
                        string mehsul_qiymeti = mehsul_yoxlama["Qiymet"].ToString();
                        double d_mehsul_qiymeti = Convert.ToDouble(mehsul_qiymeti);
                        string mehsul_meblegi = "0.5";
                        double i_umumi_mebleg = i_umumi_mehsul_sayi * d_mehsul_qiymeti;
                        string str_umumi_mehsul_meblegi = Convert.ToString(i_umumi_mebleg);
                        OleDbCommand anliq_artir = access.CreateCommand();
                        anliq_artir.CommandType = CommandType.Text;
                        anliq_artir.CommandText = "insert into anliq_satis values('" + str_anliq_no + "'," +
                            "'" + button + "','" + mehsul_qiymeti + "','" + anliq_mehsul_sayi + "','" + mehsul_meblegi + "')";
                        anliq_artir.ExecuteNonQuery();
                        OleDbCommand umumi_yenile = access.CreateCommand();
                        umumi_yenile.CommandType = CommandType.Text;
                        umumi_yenile.CommandText = "update umumi_satis set Sayi='" + s_umumi_sayi + "' " +
                            ",Meblegi='" + str_umumi_mehsul_meblegi + "' where Adi='" + button + "'";
                        umumi_yenile.ExecuteNonQuery();
                        OleDbCommand mehsul_yenile = access.CreateCommand();
                        mehsul_yenile.CommandType = CommandType.Text;
                        mehsul_yenile.CommandText = "update mehsullar set Say='" + str_h_mehsul_sayi + "' " +
                            "where Adi='" + button + "'";
                        mehsul_yenile.ExecuteNonQuery();
                        access.Close();
                        cedvele_yukle();
                    }
                }
                else
                {
                    umumi_no++;
                    anliq_no++;
                    h_mehsul_sayi--;
                    string mehsul_sayi = "1";
                    string mehsul_qiymeti = mehsul_yoxlama["Qiymet"].ToString();
                    string mehsul_meblegi = mehsul_qiymeti;
                    double d_mehsul_meblegi = Convert.ToDouble(mehsul_meblegi);
                    string str_umumi_no = Convert.ToString(umumi_no);
                    string str_anliq_no = Convert.ToString(anliq_no);
                    string str_h_mehsul_sayi = Convert.ToString(h_mehsul_sayi);
                    OleDbCommand anliq_artir = new OleDbCommand("insert into anliq_satis " +
                        "values ('" + str_anliq_no + "', '" + button + "', '" + mehsul_qiymeti + "'," +
                        "'" + mehsul_sayi + "', '" + mehsul_meblegi + "')", access);
                    anliq_artir.ExecuteNonQuery();
                    OleDbCommand umumi_artir = new OleDbCommand("insert into umumi_satis " +
                        "values ('" + str_umumi_no + "', '" + button + "', '" + mehsul_qiymeti + "'," +
                        "'" + mehsul_sayi + "', '" + mehsul_meblegi + "')", access);
                    umumi_artir.ExecuteNonQuery();
                    OleDbCommand mehsul_yenile = access.CreateCommand();
                    mehsul_yenile.CommandType = CommandType.Text;
                    mehsul_yenile.CommandText = "update mehsullar set Say='" + str_h_mehsul_sayi + "'where Adi='" + button + "'";
                    mehsul_yenile.ExecuteNonQuery();
                    access.Close();
                    cedvele_yukle();
                }
            }
            else
            {
                MessageBox.Show("Mehsul bitib", "Anbar melumati");
                access.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            access.Open();
            string button = button2.Text;
            OleDbCommand mehsul_var = new OleDbCommand("select Adi,Qiymet,Say from mehsullar where " +
                "Adi = '" + button + "' and Say>0", access);
            OleDbDataReader mehsul_yoxlama = mehsul_var.ExecuteReader();
            if (mehsul_yoxlama.Read())
            {
                string s_h_mehsul_sayi = mehsul_yoxlama["Say"].ToString();
                int h_mehsul_sayi = Convert.ToInt32(s_h_mehsul_sayi);
                OleDbCommand umumi_var = new OleDbCommand("select * from umumi_satis where " +
                "Adi = '" + button + "'", access);
                OleDbDataReader umumi_yoxlama = umumi_var.ExecuteReader();
                if (umumi_yoxlama.Read())
                {
                    OleDbCommand anliq_var = new OleDbCommand("select * from anliq_satis where " +
                        "Adi = '" + button + "'", access);
                    OleDbDataReader anliq_yoxlama = anliq_var.ExecuteReader();
                    if (anliq_yoxlama.Read())
                    {
                        h_mehsul_sayi = Convert.ToInt32(s_h_mehsul_sayi) - 1;
                        string umumi_mehsul_sayi = umumi_yoxlama["Sayi"].ToString();
                        int i_umumi_mehsul_sayi = Convert.ToInt32(umumi_mehsul_sayi) + 1;
                        string s_umumi_sayi = Convert.ToString(i_umumi_mehsul_sayi);
                        string str_h_mehsul_sayi = Convert.ToString(h_mehsul_sayi);
                        string s_anliq_mehsul_sayi = anliq_yoxlama["Sayi"].ToString();
                        int i_anliq_mehsul_sayi = Convert.ToInt32(s_anliq_mehsul_sayi) + 1;
                        string anliq_mehsul_sayi = Convert.ToString(i_anliq_mehsul_sayi);
                        string mehsul_qiymeti = mehsul_yoxlama["Qiymet"].ToString();
                        double d_mehsul_qiymeti = Convert.ToDouble(mehsul_qiymeti);
                        double d_mehsul_meblegi = d_mehsul_qiymeti * i_anliq_mehsul_sayi;
                        string mehsul_meblegi = Convert.ToString(d_mehsul_meblegi);
                        double i_umumi_mebleg = i_umumi_mehsul_sayi * d_mehsul_qiymeti;
                        string str_umumi_mehsul_meblegi = Convert.ToString(i_umumi_mebleg);
                        OleDbCommand anliq_yenile = access.CreateCommand();
                        anliq_yenile.CommandType = CommandType.Text;
                        anliq_yenile.CommandText = "update anliq_satis set Sayi='" + anliq_mehsul_sayi + "' " +
                            ",Meblegi='" + mehsul_meblegi + "' where Adi='" + button + "'";
                        anliq_yenile.ExecuteNonQuery();
                        OleDbCommand umumi_yenile = access.CreateCommand();
                        umumi_yenile.CommandType = CommandType.Text;
                        umumi_yenile.CommandText = "update umumi_satis set Sayi='" + s_umumi_sayi + "' " +
                            ",Meblegi='" + str_umumi_mehsul_meblegi + "' where Adi='" + button + "'";
                        umumi_yenile.ExecuteNonQuery();
                        OleDbCommand mehsul_yenile = access.CreateCommand();
                        mehsul_yenile.CommandType = CommandType.Text;
                        mehsul_yenile.CommandText = "update mehsullar set Say='" + str_h_mehsul_sayi + "' " +
                            "where Adi='" + button + "'";
                        mehsul_yenile.ExecuteNonQuery();
                        access.Close();
                        cedvele_yukle();
                    }
                    else
                    {
                        anliq_no++;
                        h_mehsul_sayi = Convert.ToInt32(s_h_mehsul_sayi) - 1;
                        string umumi_mehsul_sayi = umumi_yoxlama["Sayi"].ToString();
                        int i_umumi_mehsul_sayi = Convert.ToInt32(umumi_mehsul_sayi) + 1;
                        string s_umumi_sayi = Convert.ToString(i_umumi_mehsul_sayi);
                        string str_h_mehsul_sayi = Convert.ToString(h_mehsul_sayi);
                        string str_anliq_no = Convert.ToString(anliq_no);
                        string anliq_mehsul_sayi = "1";
                        string mehsul_qiymeti = mehsul_yoxlama["Qiymet"].ToString();
                        double d_mehsul_qiymeti = Convert.ToDouble(mehsul_qiymeti);
                        string mehsul_meblegi = "0.16";
                        double i_umumi_mebleg = i_umumi_mehsul_sayi * d_mehsul_qiymeti;
                        string str_umumi_mehsul_meblegi = Convert.ToString(i_umumi_mebleg);
                        OleDbCommand anliq_artir = access.CreateCommand();
                        anliq_artir.CommandType = CommandType.Text;
                        anliq_artir.CommandText = "insert into anliq_satis values('" + str_anliq_no + "'," +
                            "'" + button + "','" + mehsul_qiymeti + "','" + anliq_mehsul_sayi + "','" + mehsul_meblegi + "')";
                        anliq_artir.ExecuteNonQuery();
                        OleDbCommand umumi_yenile = access.CreateCommand();
                        umumi_yenile.CommandType = CommandType.Text;
                        umumi_yenile.CommandText = "update umumi_satis set Sayi='" + s_umumi_sayi + "' " +
                            ",Meblegi='" + str_umumi_mehsul_meblegi + "' where Adi='" + button + "'";
                        umumi_yenile.ExecuteNonQuery();
                        OleDbCommand mehsul_yenile = access.CreateCommand();
                        mehsul_yenile.CommandType = CommandType.Text;
                        mehsul_yenile.CommandText = "update mehsullar set Say='" + str_h_mehsul_sayi + "' " +
                            "where Adi='" + button + "'";
                        mehsul_yenile.ExecuteNonQuery();
                        access.Close();
                        cedvele_yukle();
                    }
                }
                else
                {
                    umumi_no++;
                    anliq_no++;
                    h_mehsul_sayi--;
                    string mehsul_sayi = "1";
                    string mehsul_qiymeti = mehsul_yoxlama["Qiymet"].ToString();
                    string mehsul_meblegi = mehsul_qiymeti;
                    double d_mehsul_meblegi = Convert.ToDouble(mehsul_meblegi);
                    string str_umumi_no = Convert.ToString(umumi_no);
                    string str_anliq_no = Convert.ToString(anliq_no);
                    string str_h_mehsul_sayi = Convert.ToString(h_mehsul_sayi);
                    OleDbCommand anliq_artir = new OleDbCommand("insert into anliq_satis " +
                        "values ('" + str_anliq_no + "', '" + button + "', '" + mehsul_qiymeti + "'," +
                        "'" + mehsul_sayi + "', '" + mehsul_meblegi + "')", access);
                    anliq_artir.ExecuteNonQuery();
                    OleDbCommand umumi_artir = new OleDbCommand("insert into umumi_satis " +
                        "values ('" + str_umumi_no + "', '" + button + "', '" + mehsul_qiymeti + "'," +
                        "'" + mehsul_sayi + "', '" + mehsul_meblegi + "')", access);
                    umumi_artir.ExecuteNonQuery();
                    OleDbCommand mehsul_yenile = access.CreateCommand();
                    mehsul_yenile.CommandType = CommandType.Text;
                    mehsul_yenile.CommandText = "update mehsullar set Say='" + str_h_mehsul_sayi + "'where Adi='" + button + "'";
                    mehsul_yenile.ExecuteNonQuery();
                    access.Close();
                    cedvele_yukle();
                }
            }
            else
            {
                MessageBox.Show("Mehsul bitib", "Anbar melumati");
                access.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            access.Open();
            string button = button3.Text;
            OleDbCommand mehsul_var = new OleDbCommand("select Adi,Qiymet,Say from mehsullar where " +
                "Adi = '" + button + "' and Say>0", access);
            OleDbDataReader mehsul_yoxlama = mehsul_var.ExecuteReader();
            if (mehsul_yoxlama.Read())
            {
                string s_h_mehsul_sayi = mehsul_yoxlama["Say"].ToString();
                int h_mehsul_sayi = Convert.ToInt32(s_h_mehsul_sayi);
                OleDbCommand umumi_var = new OleDbCommand("select * from umumi_satis where " +
                "Adi = '" + button + "'", access);
                OleDbDataReader umumi_yoxlama = umumi_var.ExecuteReader();
                if (umumi_yoxlama.Read())
                {
                    OleDbCommand anliq_var = new OleDbCommand("select * from anliq_satis where " +
                        "Adi = '" + button + "'", access);
                    OleDbDataReader anliq_yoxlama = anliq_var.ExecuteReader();
                    if (anliq_yoxlama.Read())
                    {
                        h_mehsul_sayi = Convert.ToInt32(s_h_mehsul_sayi) - 1;
                        string umumi_mehsul_sayi = umumi_yoxlama["Sayi"].ToString();
                        int i_umumi_mehsul_sayi = Convert.ToInt32(umumi_mehsul_sayi) + 1;
                        string s_umumi_sayi = Convert.ToString(i_umumi_mehsul_sayi);
                        string str_h_mehsul_sayi = Convert.ToString(h_mehsul_sayi);
                        string s_anliq_mehsul_sayi = anliq_yoxlama["Sayi"].ToString();
                        int i_anliq_mehsul_sayi = Convert.ToInt32(s_anliq_mehsul_sayi) + 1;
                        string anliq_mehsul_sayi = Convert.ToString(i_anliq_mehsul_sayi);
                        string mehsul_qiymeti = mehsul_yoxlama["Qiymet"].ToString();
                        double d_mehsul_qiymeti = Convert.ToDouble(mehsul_qiymeti);
                        double d_mehsul_meblegi = d_mehsul_qiymeti * i_anliq_mehsul_sayi;
                        string mehsul_meblegi = Convert.ToString(d_mehsul_meblegi);
                        double i_umumi_mebleg = i_umumi_mehsul_sayi * d_mehsul_qiymeti;
                        string str_umumi_mehsul_meblegi = Convert.ToString(i_umumi_mebleg);
                        OleDbCommand anliq_yenile = access.CreateCommand();
                        anliq_yenile.CommandType = CommandType.Text;
                        anliq_yenile.CommandText = "update anliq_satis set Sayi='" + anliq_mehsul_sayi + "' " +
                            ",Meblegi='" + mehsul_meblegi + "' where Adi='" + button + "'";
                        anliq_yenile.ExecuteNonQuery();
                        OleDbCommand umumi_yenile = access.CreateCommand();
                        umumi_yenile.CommandType = CommandType.Text;
                        umumi_yenile.CommandText = "update umumi_satis set Sayi='" + s_umumi_sayi + "' " +
                            ",Meblegi='" + str_umumi_mehsul_meblegi + "' where Adi='" + button + "'";
                        umumi_yenile.ExecuteNonQuery();
                        OleDbCommand mehsul_yenile = access.CreateCommand();
                        mehsul_yenile.CommandType = CommandType.Text;
                        mehsul_yenile.CommandText = "update mehsullar set Say='" + str_h_mehsul_sayi + "' " +
                            "where Adi='" + button + "'";
                        mehsul_yenile.ExecuteNonQuery();
                        access.Close();
                        cedvele_yukle();
                    }
                    else
                    {
                        anliq_no++;
                        h_mehsul_sayi = Convert.ToInt32(s_h_mehsul_sayi) - 1;
                        string umumi_mehsul_sayi = umumi_yoxlama["Sayi"].ToString();
                        int i_umumi_mehsul_sayi = Convert.ToInt32(umumi_mehsul_sayi) + 1;
                        string s_umumi_sayi = Convert.ToString(i_umumi_mehsul_sayi);
                        string str_h_mehsul_sayi = Convert.ToString(h_mehsul_sayi);
                        string str_anliq_no = Convert.ToString(anliq_no);
                        string anliq_mehsul_sayi = "1";
                        string mehsul_qiymeti = mehsul_yoxlama["Qiymet"].ToString();
                        double d_mehsul_qiymeti = Convert.ToDouble(mehsul_qiymeti);
                        string mehsul_meblegi = "0.9";
                        double i_umumi_mebleg = i_umumi_mehsul_sayi * d_mehsul_qiymeti;
                        string str_umumi_mehsul_meblegi = Convert.ToString(i_umumi_mebleg);
                        OleDbCommand anliq_artir = access.CreateCommand();
                        anliq_artir.CommandType = CommandType.Text;
                        anliq_artir.CommandText = "insert into anliq_satis values('" + str_anliq_no + "'," +
                            "'" + button + "','" + mehsul_qiymeti + "','" + anliq_mehsul_sayi + "','" + mehsul_meblegi + "')";
                        anliq_artir.ExecuteNonQuery();
                        OleDbCommand umumi_yenile = access.CreateCommand();
                        umumi_yenile.CommandType = CommandType.Text;
                        umumi_yenile.CommandText = "update umumi_satis set Sayi='" + s_umumi_sayi + "' " +
                            ",Meblegi='" + str_umumi_mehsul_meblegi + "' where Adi='" + button + "'";
                        umumi_yenile.ExecuteNonQuery();
                        OleDbCommand mehsul_yenile = access.CreateCommand();
                        mehsul_yenile.CommandType = CommandType.Text;
                        mehsul_yenile.CommandText = "update mehsullar set Say='" + str_h_mehsul_sayi + "' " +
                            "where Adi='" + button + "'";
                        mehsul_yenile.ExecuteNonQuery();
                        access.Close();
                        cedvele_yukle();
                    }
                }
                else
                {
                    umumi_no++;
                    anliq_no++;
                    h_mehsul_sayi--;
                    string mehsul_sayi = "1";
                    string mehsul_qiymeti = mehsul_yoxlama["Qiymet"].ToString();
                    string mehsul_meblegi = mehsul_qiymeti;
                    double d_mehsul_meblegi = Convert.ToDouble(mehsul_meblegi);
                    string str_umumi_no = Convert.ToString(umumi_no);
                    string str_anliq_no = Convert.ToString(anliq_no);
                    string str_h_mehsul_sayi = Convert.ToString(h_mehsul_sayi);
                    OleDbCommand anliq_artir = new OleDbCommand("insert into anliq_satis " +
                        "values ('" + str_anliq_no + "', '" + button + "', '" + mehsul_qiymeti + "'," +
                        "'" + mehsul_sayi + "', '" + mehsul_meblegi + "')", access);
                    anliq_artir.ExecuteNonQuery();
                    OleDbCommand umumi_artir = new OleDbCommand("insert into umumi_satis " +
                        "values ('" + str_umumi_no + "', '" + button + "', '" + mehsul_qiymeti + "'," +
                        "'" + mehsul_sayi + "', '" + mehsul_meblegi + "')", access);
                    umumi_artir.ExecuteNonQuery();
                    OleDbCommand mehsul_yenile = access.CreateCommand();
                    mehsul_yenile.CommandType = CommandType.Text;
                    mehsul_yenile.CommandText = "update mehsullar set Say='" + str_h_mehsul_sayi + "'where Adi='" + button + "'";
                    mehsul_yenile.ExecuteNonQuery();
                    access.Close();
                    cedvele_yukle();
                }
            }
            else
            {
                MessageBox.Show("Mehsul bitib", "Anbar melumati");
                access.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int n = dataGridView1.Rows.Count;
                double cem_anliq_mebleg=0;
                for(int i=0; i<n-1; i++)
                {
                    double d_anliq_mebleg;
                    string s_anliq_mebleg = dataGridView1.Rows[i].Cells["Meblegi"].Value.ToString();
                    d_anliq_mebleg = Convert.ToDouble(s_anliq_mebleg);
                    cem_anliq_mebleg = cem_anliq_mebleg + d_anliq_mebleg;
                }
                textBox2.Text = Convert.ToString(cem_anliq_mebleg);
                textBox3.Enabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Evvelce satis edin", "Satis melumati");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if(textBox3.Text != "")
            {
                double alinan_mebleg = Convert.ToDouble(textBox3.Text);
                double umumi_mebleg = Convert.ToDouble(textBox2.Text);
                double qaytarilan_mebleg = alinan_mebleg - umumi_mebleg;
                textBox4.Text = Convert.ToString(qaytarilan_mebleg);
                button7.Enabled = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            double umumi_mebleg = Convert.ToDouble(textBox2.Text);
            double qaytarilan_mebleg = Convert.ToDouble(textBox4.Text);
            if (qaytarilan_mebleg > 0)
            {
                iscinin_umumi_meblegi += umumi_mebleg;
                access.Open();
                OleDbCommand temizle1 = new OleDbCommand("delete * anliq_satis", access);
                OleDbCommand temizle = access.CreateCommand();
                temizle.CommandType = CommandType.Text;
                temizle.CommandText = "delete * from anliq_satis";
                temizle.ExecuteReader();
                access.Close();
                cedvele_yukle();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Text = Convert.ToString(iscinin_umumi_meblegi);
                anliq_no = 0;
            }
            else
            {
                MessageBox.Show("Alinan pul umumi puldan azdir!", "Xeta!");
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            anliq_no--;
            int i = dataGridView1.CurrentCell.RowIndex;
            string adi = dataGridView1.Rows[i].Cells[1].Value.ToString();
            int h_say = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
            access.Open();
            OleDbCommand mehsul_secme = access.CreateCommand();
            mehsul_secme.CommandType = CommandType.Text;
            mehsul_secme.CommandText = "select * from mehsullar where Adi = '" + adi + "'";
            OleDbDataReader mehsul_melumati = mehsul_secme.ExecuteReader();
            string str_say = "";
            if (mehsul_melumati.Read())
            {
                str_say = mehsul_melumati["Say"].ToString();
            }               
            int say = Convert.ToInt32(str_say) + h_say;
            OleDbCommand mehsul_yenile = access.CreateCommand();             
            mehsul_yenile.CommandType = CommandType.Text;
            mehsul_yenile.CommandText = "update mehsullar set Say='"+say+"' where Adi= '"+adi+"'";
            mehsul_yenile.ExecuteNonQuery();
            OleDbCommand umumi_secme = access.CreateCommand();
            umumi_secme.CommandType = CommandType.Text;
            umumi_secme.CommandText = "select * from umumi_satis where Adi = '" + adi + "'";
            OleDbDataReader umumi_melumati = umumi_secme.ExecuteReader();
            string str_u_say = "";
            if (umumi_melumati.Read())
            {
                str_u_say = umumi_melumati["Sayi"].ToString();
            }
            int u_say = Convert.ToInt32(str_u_say) + h_say;
            OleDbCommand umumi_yenile = access.CreateCommand();
            umumi_yenile.CommandType = CommandType.Text;
            umumi_yenile.CommandText = "update umumi_satis set Sayi='" + u_say + "' where Adi= '" + adi + "'";
            umumi_yenile.ExecuteNonQuery();
            access.Close();
            dataGridView1.Rows.RemoveAt(i);
            int n = dataGridView1.Rows.Count;
            for(int j = i; j < n-1; j++)
            {
                dataGridView1.Rows[j].Cells[0].Value = Convert.ToInt32(dataGridView1.Rows[j].Cells[0].Value.ToString()) - 1;
            }
        }
    }
}
