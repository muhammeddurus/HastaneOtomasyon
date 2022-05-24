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
            SqlCommand cmd = new SqlCommand("select r.RandevuId as [Randevu No],p.PoliklinikAdi as [Poliklinik Adı],(c.CalisanAd + ' ' +c.CalisanSoyad)  as [Doktor],(h.HastaAd + ' ' + h.HastaSoyad) as [Hasta],(r.Tarih + ' '+r.Saat) as [Tarih Saat],hk.Durum as[Hasta Kabul] from Randevular r join Doktorlar d on d.DoktorId = r.Doktor_ID join Calisanlar c on c.Calisan_Id = d.Calisan_ID join Poliklinikler p on p.PoliklinikId = d.Poliklinik_ID join Hastalar h on h.HastaId = r.Hasta_ID join HastaKabuller hk on hk.Randevu_ID=r.RandevuId where  Convert(datetime,R.Tarih,105) >=  GETDATE()", con);
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

       
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            rndId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            HastaAdSoyad = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("Değişti aminyüm" + Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[5].Value));
            if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[5].Value) == true) //checkbox seçiliyse 
            {

                SqlCommand cmd = new SqlCommand("update HastaKabuller set Sikayet=@sikayet, Durum=@durum where Randevu_ID=@rndId", con);
                if (con.State == ConnectionState.Closed)
                {
                    //    List<string> selectedItem = new List<string>();
                    //    DataGridViewRow drow = new DataGridViewRow();
                    //    for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                    //{
                    //        drow = dataGridView1.Rows[i];
                    //        if (Convert.ToBoolean(drow.Cells[4].Value) == true) //checkbox seçiliyse 
                    //        {
                    //            string id = drow.Cells[0].Value.ToString();
                    //            selectedItem.Add(id); //seçiliyse listeye ekle
                    //        }
                    //    }
                    try
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@sikayet", "Belirtilmedi");
                        cmd.Parameters.AddWithValue("@rndId", rndId);
                        cmd.Parameters.AddWithValue("@durum", true);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show(HastaAdSoyad + "adlı kişinin randevusu kabul edildi.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hasta Kabul Başarısız. ''' " + ex.Message);
                        throw;
                    }

                }
            }
            else if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[5].Value) == false)
            {
                SqlCommand cmd = new SqlCommand("update HastaKabuller set Sikayet=@sikayet, Durum=@durum where Randevu_ID=@rndId", con);
                if (con.State == ConnectionState.Closed)
                {
                    //    List<string> selectedItem = new List<string>();
                    //    DataGridViewRow drow = new DataGridViewRow();
                    //    for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                    //{
                    //        drow = dataGridView1.Rows[i];
                    //        if (Convert.ToBoolean(drow.Cells[4].Value) == true) //checkbox seçiliyse 
                    //        {
                    //            string id = drow.Cells[0].Value.ToString();
                    //            selectedItem.Add(id); //seçiliyse listeye ekle
                    //        }
                    //    }
                    try
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@sikayet", "Belirtilmedi");
                        cmd.Parameters.AddWithValue("@rndId", rndId);
                        cmd.Parameters.AddWithValue("@durum", false);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show(HastaAdSoyad + "adlı kişinin randevusu iptal edildi.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hasta Kabul Başarısız. ''' " + ex.Message);
                        throw;
                    }

                }
            }
        }
    }
}
