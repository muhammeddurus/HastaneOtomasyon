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
    public partial class PersonelLogin : Form
    {
        public PersonelLogin()
        {
            InitializeComponent();
        }
        bool kontrolKayit = false;
        bool kontrolTahlil = false;
        private void PersonelLogin_Load(object sender, EventArgs e)
        {

        }
        HastaKayit hastaKayit = new HastaKayit();
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (kontrolKayit == false)
            {
               

                hastaKayit.MdiParent = this;
                hastaKayit.Show();
                kontrolKayit = true;
                if (kontrolTahlil == true)
                {
                    tahliller.Hide();
                    kontrolTahlil = false;
                }
                
            }
            else if(kontrolKayit == true)
            {
                MessageBox.Show("Zaten Açık");
            }
            
        }
        Tahliller tahliller = new Tahliller();
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (kontrolTahlil == false)
            {
               
                tahliller.MdiParent = this;
                tahliller.Show();
                kontrolTahlil = true;
                if (kontrolKayit==true)
                {
                    hastaKayit.Hide();
                    kontrolKayit = false;
                }
                
            }
            else if (kontrolTahlil == true)
            {
                MessageBox.Show("Zaten Açık");
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
