using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HastaneOtomasyon
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SignIn signin = new SignIn();
            signin.Show();
            this.Hide();
        }
        SqlConnection con = new SqlConnection(BaglantiAyarlari.ConnectionString_TechLine);
        private void btnLogin_Click(object sender, EventArgs e)
        {
            con.Open();
            MessageBox.Show(con.State.ToString());
            if (txtPassword.Text != string.Empty || txtUsername.Text != string.Empty)
            {

                SqlCommand cmd = new SqlCommand("select HastaId,HastaTCKN,HastaAd + ' ' +HastaSoyad as [Ad Soyad] from Hastalar where HastaTCKN='" + txtUsername.Text + "' and Sifre='" + txtPassword.Text + "'", con);
                SqlCommand cmd2 = new SqlCommand("select Calisan_Id,SicilNo from Calisanlar where SicilNo='" + txtUsername.Text + "' and Sifre='" + txtPassword.Text + "'", con);
                
                SqlDataReader dr = cmd.ExecuteReader();
                SqlDataReader dr2 = cmd2.ExecuteReader();
                if (dr.Read())
                {
                    UserLogin.KullaniciAdi = dr[1].ToString();
                    UserLogin.ID = Convert.ToInt32(dr[0]);
                    UserLogin.Ad = dr[2].ToString();
                    
                    dr.Close();
                    HomePage home = new HomePage();
                    home.Show();
                    this.Hide();
                }
                else if (dr2.Read())
                {
                    UserLogin.KullaniciAdi = dr2[1].ToString();
                    UserLogin.ID = Convert.ToInt32(dr2[0]);
                    UserLogin.Ad = dr2[2].ToString();

                    dr.Close();
                    PersonelLogin PersonelHome = new PersonelLogin();
                    PersonelHome.Show();
                    this.Hide();
                }
                else
                {
                    dr.Close();
                    MessageBox.Show("No Account avilable with this username and password ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                con.Close();
                MessageBox.Show("Please enter value in all field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
