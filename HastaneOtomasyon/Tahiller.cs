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
    public partial class Tahliller : Form
    {
        public Tahliller()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(BaglantiAyarlari.ConnectionString_TechLine);
        private void Tahliller_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select (h.HastaAd +' '+h.HastaSoyad) as [Hasta Bilgileri],t.TahlilAdi as [Tahlil Adı],ts.Sonuc as Sonuc from TahlillerSonuc ts join Tahliller t on t.Tahlil_ID=ts.Tahlil_ID join Hastalar h on h.HastaId=ts.Hasta_ID", con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();

                if (dr.Read())
                {
                    dt.Load(dr);
                }
                dataGridView1.DataSource = dt;
                con.Close();
            }
            
        }
    }
}
