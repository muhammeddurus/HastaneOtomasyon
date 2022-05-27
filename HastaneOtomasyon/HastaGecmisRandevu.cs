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
    public partial class HastaGecmisRandevu : Form
    {
        public HastaGecmisRandevu()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(BaglantiAyarlari.ConnectionString_TechLine);
        private void HastaGecmisRandevu_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select r.Tarih,r.Saat,h.HastaAd +' '+h.HastaSoyad as [Ad Soyad],p.PoliklinikAdi,c.CalisanAd +' '+c.CalisanSoyad as [Doktor] from Randevular r join Hastalar h on r.Hasta_ID=h.HastaId join Poliklinikler p on r.Poliklinik_ID=p.PoliklinikId join Doktorlar d on r.Doktor_ID=d.DoktorId join Calisanlar c on d.Calisan_ID=c.Calisan_Id where h.HastaId=@ıd and Convert(datetime,r.Tarih,105) <  GETDATE()", con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
                cmd.Parameters.AddWithValue("@ıd", UserLogin.ID);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                if (dr.Read())
                {
                    try
                    {
                        dt.Load(dr);
                        dataGridView1.DataSource = dt;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    con.Close();

                }
            }
        }
    }
}
