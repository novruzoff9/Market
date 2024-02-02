using System;
using System.Collections;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection access = new OleDbConnection("Provider= Microsoft.JET.OleDb.4.0;Data Source=" +
            Application.StartupPath + "\\market.mdb");

        private Form2 form2 = new Form2();
        private Form3 form3 = new Form3();

        private void form1_Load(object sender, EventArgs e)
        {
            access.Open();
            this.AcceptButton = button1;
            this.StartPosition = FormStartPosition.CenterScreen;
            
            textBox1.Text = "Yagmur";
            textBox2.Text = "2005";
            OleDbCommand temizle = new OleDbCommand("delete * from anliq_satis", access);
            temizle.ExecuteReader();
            access.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                access.Open();
                OleDbCommand hesab = new OleDbCommand("Select Ad,Parol,Vezife from hesablar where Ad='" + textBox1.Text + 
                    "'and Parol='"+ textBox2.Text+"'", access);
                OleDbDataReader parol = hesab.ExecuteReader();
                if (parol.Read())
                {
                    if (parol["Vezife"].ToString() == "Mudur")
                    {                       
                        form2.ad = textBox1.Text;
                        this.Hide();
                        form2.Show();
                    }
                    else if(parol["Vezife"].ToString() == "Isci")
                    {
                        form3.ad = textBox1.Text;
                        this.Hide();
                        form3.Show();
                    }
                    access.Close();
                }
                else
                {
                    MessageBox.Show("Ad və ya parol səhvdir!", "Giris Melumati");
                }
                access.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Nese səhvdir!", "Giris Melumati");
                access.Close();
            }

        }
    }
}
