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
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
            MessageBox.Show(UserLogin.Ad.ToString());
            label1.Text = "Hoşgeldiniz " + UserLogin.Ad.ToString() ;
        }
        SqlConnection con = new SqlConnection(BaglantiAyarlari.ConnectionString_TechLine);
        bool gecmisKontrol = false;
        bool randevuKontrol = false;
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (randevuKontrol == false)
            {
                RandevuAl randevuAl = new RandevuAl();
                randevuAl.MdiParent = this;
                randevuAl.Show();
                randevuKontrol = true;
                randevuAl.FormClosed += frmRandevu_FormClosed;
                if (gecmisKontrol == true)
                {
                    gecmisKontrol = false;
                }
            }


            MessageBox.Show("Giriş Yapmış Kullanıcı TCKN ve ID" + UserLogin.KullaniciAdi + ">>" + UserLogin.ID.ToString());
        }

        private void frmRandevu_FormClosed(object sender, FormClosedEventArgs e)
        {
            randevuKontrol = false;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (gecmisKontrol == false)
            {
                HastaGecmisRandevu hastaGecmis = new HastaGecmisRandevu();
                hastaGecmis.MdiParent = this;
                hastaGecmis.Show();
                gecmisKontrol = true;
                hastaGecmis.FormClosed += frm2_FormClosed;
                if (randevuKontrol == true)
                {
                    randevuKontrol = false;
                }
                
            }


        }

        private void frm2_FormClosed(object sender, FormClosedEventArgs e)
        {
            gecmisKontrol = false;
        }
    }
}
