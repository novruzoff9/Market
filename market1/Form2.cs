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

namespace market1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public string ad;

        OleDbConnection access = new OleDbConnection("Provider= Microsoft.JET.OleDb.4.0; Data Source=" +
            Application.StartupPath + "\\market.mdb");

        private void cedvele_yukle()
        {
            try
            {
                access.Open();
                OleDbDataAdapter yukle = new OleDbDataAdapter("select * from hesablar", access);
                DataSet yaddas = new DataSet();
                yukle.Fill(yaddas);
                dataGridView2.DataSource = yaddas.Tables[0];
                access.Close();
            }
            catch (Exception xatamsj)
            {
                MessageBox.Show(xatamsj.Message);
                access.Close();
            }
        }

        private void mehsullari_cedvele_yukle()
        {
            try
            {
                access.Open();
                OleDbDataAdapter yukle = new OleDbDataAdapter("select * from mehsullar", access);
                DataSet yaddas = new DataSet();
                yukle.Fill(yaddas);
                dataGridView3.DataSource = yaddas.Tables[0];
                access.Close();
            }
            catch (Exception xatamsj)
            {
                MessageBox.Show(xatamsj.Message);
                access.Close();
            }
        }

        private void isci_sayi()
        {
            int isci_say = dataGridView2.Rows.Count - 1;
            label6.Text = isci_say.ToString();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = ad;
            textBox4.Enabled = false;
            radioButton2.Checked = true;
            //Satis melumati sehifesinde umumi satisi gosterir
            try
            {
                access.Open();
                OleDbDataAdapter yukle = new OleDbDataAdapter("select * from umumi_satis", access);
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
            //Isci melumati sehifesinde Iscileri gosterir
            cedvele_yukle();
            //Isci melumati sehifesinde Isci sayini gosterir
            isci_sayi();
            //Anbar melumati sehifesinde anbari gosterir
            try
            {
                access.Open();
                OleDbDataAdapter anbar = new OleDbDataAdapter("select * from mehsullar", access);
                DataSet anbar_melumati = new DataSet();
                anbar.Fill(anbar_melumati);
                dataGridView3.DataSource = anbar_melumati.Tables[0];
                access.Close();
            }
            catch (Exception xatamsj)
            {
                MessageBox.Show(xatamsj.Message);
                access.Close();
            }
            //umumi meblegi goster
            int n = dataGridView1.Rows.Count;
            double umumi_mebleg = 0;
            for (int i = 0; i < n - 1; i++)
            {
                double m_mebleg = Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString());
                umumi_mebleg = umumi_mebleg + m_mebleg;
            }
            textBox4.Text = umumi_mebleg.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            access.Open();
            OleDbCommand temizle = new OleDbCommand("delete * from umumi_satis", access);
            temizle.ExecuteReader();
            access.Close();
            cedvele_yukle();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string adi = textBox2.Text;
            string parol = textBox3.Text;
            string vezife = "";
            if(radioButton1.Checked == true)
            {
                vezife = "Mudur";
            }
            else if (radioButton2.Checked == true)
            {
                vezife = "Isci";
            }
            try
            {
                access.Open();
                OleDbCommand iscini_artir = access.CreateCommand();
                iscini_artir.CommandType = CommandType.Text;
                iscini_artir.CommandText = "insert into hesablar values('" + adi + "', '" + parol + "', '" + vezife + "')";
                iscini_artir.ExecuteNonQuery();
                access.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Eyni Ad ile 2 nefer olmaz!", "Isci melumati");
            }
            
            cedvele_yukle();
            textBox2.Text = "";
            textBox3.Text = "";
            radioButton2.Checked = true;
            isci_sayi();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string adi = textBox2.Text;
            access.Open();
            OleDbDataAdapter isci_melumati = new OleDbDataAdapter("select * from hesablar where Ad = '"+adi+"'", access);
            DataSet isciler = new DataSet();
            isci_melumati.Fill(isciler);
            dataGridView2.DataSource = isciler.Tables[0];
            access.Close();
            isci_sayi();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string vezife = "";
            if(radioButton1.Checked == true)
            {
                vezife = "Mudur";
            }
            else if(radioButton2.Checked == true)
            {
                vezife = "Isci";
            }
            access.Open();
            OleDbDataAdapter isci_melumati = new OleDbDataAdapter("select * from hesablar where Vezife = '"+vezife+"'", access);
            DataSet isciler = new DataSet();
            isci_melumati.Fill(isciler);
            dataGridView2.DataSource = isciler.Tables[0];
            access.Close();
            isci_sayi();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter iscileri_goster = new OleDbDataAdapter("select * from hesablar", access);
            DataSet isciler = new DataSet();
            iscileri_goster.Fill(isciler);
            dataGridView2.DataSource = isciler.Tables[0];
            isci_sayi();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(textBox2.Text != "")
            {
                string adi = textBox2.Text;
                int n = dataGridView2.Rows.Count;
                int i = dataGridView2.CurrentCell.RowIndex;
                string k = Convert.ToString(i + 1);
                string t = Convert.ToString(i + 2);
                access.Open();
                OleDbCommand iscini_sil = access.CreateCommand();
                iscini_sil.CommandType = CommandType.Text;
                iscini_sil.CommandText = "delete from hesablar where Ad='" + adi + "'";
                iscini_sil.ExecuteNonQuery();
                access.Close();
                cedvele_yukle();
                textBox2.Text = "";
                isci_sayi();
                int n1 = dataGridView2.Rows.Count;
            }
            else
            {
                MessageBox.Show("burdadi");
                int i = dataGridView2.CurrentCell.RowIndex;
                string isci_adi = dataGridView2.Rows[i].Cells[0].Value.ToString();
                access.Open();
                OleDbCommand iscini_sil = access.CreateCommand();
                iscini_sil.CommandType = CommandType.Text;
                iscini_sil.CommandText = "delete * from hesablar where Ad='" + isci_adi + "'";
                iscini_sil.ExecuteNonQuery();
                access.Close();
                dataGridView2.Rows.RemoveAt(i);
            }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string mehsul = "";
            if (radioButton3.Checked) { mehsul =  radioButton3.Text; }
            else if (radioButton4.Checked) { mehsul =  radioButton4.Text; }
            else if (radioButton5.Checked) { mehsul =  radioButton5.Text; }
            else
            {
                MessageBox.Show("Məhsul seçin", "Anbar Melumati");
                return;
            }
            access.Open();
            OleDbCommand mehsul_var = new OleDbCommand("select Adi,Qiymet,Say from mehsullar where " +
                "Adi = '" + mehsul + "' and Say>=0", access);
            OleDbDataReader mehsul_yoxlama = mehsul_var.ExecuteReader();
            if (mehsul_yoxlama.Read())
            {
                string s_h_mehsul_sayi = mehsul_yoxlama["Say"].ToString();
                int h_mehsul_sayi = Convert.ToInt32(s_h_mehsul_sayi);
                int count = (int)numericUpDown1.Value + h_mehsul_sayi;
                if (count == 0)
                {
                    MessageBox.Show("Məhsul sayini daxil edin", "Anbar Melumati");
                    return;
                }
                else
                {
                    OleDbCommand anbara_artir = access.CreateCommand();
                    anbara_artir.CommandType = CommandType.Text;
                    anbara_artir.CommandText = "update mehsullar set Say='" + count + "' " +
                                "where Adi='" + mehsul + "'";
                    anbara_artir.ExecuteNonQuery();
                    numericUpDown1.Value = 0;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton5.Checked = false;
                    access.Close();
                    mehsullari_cedvele_yukle();
                }
            }
            
        }
    }
}
