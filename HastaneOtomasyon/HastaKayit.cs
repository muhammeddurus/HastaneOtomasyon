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
    public partial class HastaKayit : Form
    {
        public HastaKayit()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(BaglantiAyarlari.ConnectionString_TechLine);
        private void HastaKayit_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select r.RandevuId as [Randevu No],p.PoliklinikAdi as [Poliklinik Adı],(c.CalisanAd + ' ' +c.CalisanSoyad)  as [Doktor],(h.HastaAd + ' ' + h.HastaSoyad) as [Hasta],(r.Tarih + ' '+r.Saat) as [Tarih Saat] from Randevular r join Doktorlar d on d.DoktorId = r.Doktor_ID join Calisanlar c on c.Calisan_Id = d.Calisan_ID join Poliklinikler p on p.PoliklinikId = d.Poliklinik_ID join Hastalar h on h.HastaId = r.Hasta_ID where  Convert(datetime,R.Tarih,105) >=  GETDATE()", con);
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
        int rndId;
        string HastaAdSoyad;

        private void hastaKabulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            SqlCommand cmd = new SqlCommand("insert into HastaKabuller(Sikayet,Randevu_ID,Durum) values (@sikayet,@rndId,@durum)", con);
            if (con.State == ConnectionState.Closed)
            {
                try
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@sikayet", "Belirtilmedi");
                    cmd.Parameters.AddWithValue("@rndId", rndId);
                    cmd.Parameters.AddWithValue("@durum", true);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show(HastaAdSoyad + "adlı kişinin randevusu başarıyla kaydedildi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hasta Kabul Başarısız. ''' "+ex.Message);
                    throw;
                }
               
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            rndId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            HastaAdSoyad = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }
    }
}
