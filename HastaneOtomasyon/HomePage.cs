using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RandevuAl randevuAl = new RandevuAl();
            randevuAl.Show();
            MessageBox.Show("Giriş Yapmış Kullanıcı TCKN ve ID" + UserLogin.KullaniciAdi + ">>" + UserLogin.ID.ToString());
        }
    }
}
