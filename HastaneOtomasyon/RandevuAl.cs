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
    public partial class RandevuAl : Form
    {
        public RandevuAl()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(BaglantiAyarlari.ConnectionString_TechLine);
        DataTable dtPoliklinik;
        private void RandevuAl_Load(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("select PoliklinikId,PoliklinikAdi from Poliklinikler", con);
            con.Open();

            SqlDataReader dr = command.ExecuteReader();
            dtPoliklinik = new DataTable();
            if (dr.HasRows)
            {
                dtPoliklinik.Load(dr);
                MessageBox.Show("Poliklinik Geldi ");
            }
            comboBoxPoliklinik.DataSource = dtPoliklinik;
            comboBoxPoliklinik.DisplayMember = "PoliklinikAdi";
            comboBoxPoliklinik.ValueMember = "PoliklinikId";
            con.Close();
            //foreach (DataRow item in dtPoliklinik.Rows)
            //{
            //    comboBoxPoliklinik.Items.Add(item["PoliklinikAdi"]);
            //}
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            MessageBox.Show("Valuechanged");
            ResetCombobox(comboBox1);

        }

        private void comboBoxSaatlerDuzenle()
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";

            if (con.State == ConnectionState.Closed)
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("select Saat from Randevular where Tarih='" + dateTimePicker1.Text + "' and Poliklinik_ID = '" + comboBoxPoliklinik.SelectedValue + "' and Doktor_ID = '" + comboBoxDoktor.SelectedValue + "'", con);

                //cmd.Parameters.AddWithValue();
                MessageBox.Show("Test" + dateTimePicker1.Text);

                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                if (dr.HasRows)
                {
                    dt.Load(dr);

                    MessageBox.Show("İşlem Başarılı");

                }

                foreach (DataRow item in dt.Rows)
                {

                    MessageBox.Show("Test" + item["Saat"].ToString());
                    for (int i = 0; i < comboBox1.Items.Count; i++)
                    {

                        MessageBox.Show("for");
                        if (comboBox1.Items[i].ToString() == item["Saat"].ToString())
                        {
                            MessageBox.Show("If statement");
                            comboBox1.Items.RemoveAt(i);
                        }
                    }
                }




                con.Close();
            }
            else
            {
                MessageBox.Show("Test çalışmadı.");
            }
        }

        string[] comboboxSaatler = { "08:00", "08:15", "08:30", "08:45", "09:00", "09:15", "09:30", "09:45", "10:00", "10:15", "10:30", "10:45", "11:00", "11:15", "11:30", "11:45", "13:00", "13:15", "13:30", "13:45", "14:00", "14:15", "14:30", "14:45", "15:00", "15:15", "15:30", "15:45", "16:00", "16:15", "16:30", "16:45" };
        private void ResetCombobox(ComboBox comboBox)
        {
            int countCombo = comboBox.Items.Count;
            MessageBox.Show(comboBox.Items.Count.ToString());
            comboBox.Items.Clear();


            for (int i = 0; i < comboboxSaatler.Length; i++)
            {
                comboBox.Items.Add(comboboxSaatler[i]);
            }

            comboBoxSaatlerDuzenle();
        }

        private void comboBoxPoliklinik_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ResetCombobox(comboBox1);
            SqlCommand cmd = new SqlCommand("select d.DoktorId,d.Poliklinik_ID,(c.CalisanAd + ' ' +c.CalisanSoyad)  as adSoyad  from Calisanlar c join Doktorlar d on d.Calisan_ID = c.Calisan_Id where Poliklinik_ID = '"+comboBoxPoliklinik.SelectedValue+"'",con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            if (dr.HasRows)
            {
                dt.Load(dr);

                MessageBox.Show("İşlem Başarılı");

            }

            comboBoxDoktor.DataSource = dt;
            comboBoxDoktor.DisplayMember = "adSoyad";
            //comboBoxDoktor.DisplayMember = "CalisanSoyad";
            comboBoxDoktor.ValueMember = "DoktorId";
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Test"+ comboBox1.SelectedItem);
            if (comboBoxDoktor.SelectedItem != null && comboBoxPoliklinik.SelectedItem != null && comboBox1.SelectedItem != null && dateTimePicker1.Value != null && dateTimePicker1.Value > DateTime.Now)
            { 
                SqlCommand cmd = new SqlCommand("insert into Randevular(Tarih,Saat,Hasta_ID,Poliklinik_ID,Doktor_ID,Durum) values(@tarih,@saat,@hId,@pId,@dId,@durum ) set @ID = SCOPE_IDENTITY()", con);
                
                con.Open();
                
                cmd.Parameters.AddWithValue("@tarih", dateTimePicker1.Text);
                cmd.Parameters.AddWithValue("@saat", comboBox1.SelectedItem);
                cmd.Parameters.AddWithValue("@hId", 1);
                cmd.Parameters.AddWithValue("@pId", comboBoxPoliklinik.SelectedValue);
                cmd.Parameters.AddWithValue("@dId", comboBoxDoktor.SelectedValue);
                cmd.Parameters.AddWithValue("@durum", true);
                cmd.Parameters.AddWithValue("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int id = Convert.ToInt32(cmd.Parameters["@ID"].Value);
                SqlCommand cmd2 = new SqlCommand("insert into HastaKabuller(Sikayet,Randevu_ID,Durum) values (@sikayet,@rndId,@durum)", con);
                    try
                    {
                        
                        cmd2.Parameters.AddWithValue("@sikayet", "Belirtilmedi");
                        cmd2.Parameters.AddWithValue("@rndId", id);
                        cmd2.Parameters.AddWithValue("@durum", false);
                        cmd2.ExecuteNonQuery();
                       
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hasta Kabul Başarısız. ''' " + ex.Message);
                        throw;
                    }

                
                con.Close();
            }
            else
            {
                MessageBox.Show("Hatalı Seçim");
            }
        }

        private void comboBoxDoktor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetCombobox(comboBox1);
        }
    }
}

