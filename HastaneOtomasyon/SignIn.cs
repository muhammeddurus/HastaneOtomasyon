using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HastaneOtomasyon
{
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(BaglantiAyarlari.ConnectionString_TechLine);
        private void btnSign_Click(object sender, EventArgs e)
        {
            if ((textAd.Text != null || textAd.Text != "") &&(textSoyad.Text !=null || textSoyad.Text!="") && (textAdres.Text != null || textAdres.Text != "") && (textTelefon.Text != null || textTelefon.Text != "") && (textTc.Text != null || textTc.Text != "") && (dateTimePickerDogumTarihi.Value <= DateTime.Now) && (radioButtonErkek.Checked != false || radioButtonKadın.Checked != false) && (comboboxKanGrubu.SelectedItem != null) && (textPassword.Text != null || textPassword.Text != "") && (textPasswordControl.Text != null || textPasswordControl.Text != ""))
            {
                long TCKN = long.Parse(textTc.Text);
                string Ad, Soyad;
                Ad = textAd.Text;
                Soyad = textSoyad.Text;
                int DY = dateTimePickerDogumTarihi.Value.Year;
                KimlikBilgileri.KPSPublicSoapClient KK = new KimlikBilgileri.KPSPublicSoapClient();
                bool Durum = KK.TCKimlikNoDogrula(TCKN, Ad, Soyad, DY);
                if (Durum == false)
                {
                    MessageBox.Show("Girilen Kimlik Bilgileri Doğrulandı.", "Geçerli Durum", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (textPassword.Text == textPasswordControl.Text)
                    {
                        if (IsValidPassword(textPassword.Text) == true)
                        {
                            SqlCommand cmd = new SqlCommand("insert into Hastalar(HastaAd,HastaSoyad,HastaAdres,HastaTelefon,HastaTCKN,HastaDogumTarihi,HastaCinsiyet,HastaKayitTarihi,HastaKanGrubu,Sifre,Durum) values(@ad,@soyad,@adres,@telefon,@tc,@dg,@cinsiyet,@kayit,@kan,@sifre,@durum)", con);
                            cmd.Parameters.AddWithValue("@ad",textAd.Text);
                            cmd.Parameters.AddWithValue("@soyad",textSoyad.Text);
                            cmd.Parameters.AddWithValue("@adres",textAdres.Text);
                            cmd.Parameters.AddWithValue("@telefon",textTelefon.Text);
                            cmd.Parameters.AddWithValue("@tc",textTc.Text);
                            cmd.Parameters.AddWithValue("@dg",dateTimePickerDogumTarihi.Value.ToString("yyyy-MM-dd"));
                            if (radioButtonErkek.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@cinsiyet", true);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@cinsiyet", false);
                            }
                            cmd.Parameters.AddWithValue("@kayit", DateTime.Now.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@kan", comboboxKanGrubu.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@sifre", textPassword.Text);
                            cmd.Parameters.AddWithValue("@durum", true);

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("BAŞARILI");

                            SqlCommand cmd2 = new SqlCommand("select HastaId,HastaTCKN,HastaAd +' '+HastaSoyad as [Ad Soyad] from Hastalar where HastaTCKN = '"+textTc.Text+"'",con);
                            con.Open();
                            SqlDataReader dr = cmd2.ExecuteReader();
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
                            else
                            {
                                dr.Close();
                                MessageBox.Show("No Account avilable with this username and password ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            con.Close();

                        }
                    }
                    else
                    {
                        MessageBox.Show("Girilen Şifreler Aynı Değil !!");
                    }
                    
                }
                if (Durum != true)
                {
                    MessageBox.Show("Girilen Kimlik Bilgileri Yanlış.", "Geçersiz Durum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Eksik Alaan");
            }
        }
        public static bool IsValidPassword(string plainText)
        {
            Regex regex = new Regex(@"^(.{0,7}|[^0-9]*|[^A-Z])$");
            Match match = regex.Match(plainText);
            return match.Success;
        }
        public const string motif = @"^(0(\d{3})(\d{3})(\d{2})(\d{2}))$";
        public static bool TelefonKontrol(string numara)
        {
           
            if (numara != null) return Regex.IsMatch(numara, motif);
            else return false;
        }

        private void SignIn_Load(object sender, EventArgs e)
        {
            MessageBox.Show(TelefonKontrol("05340338401").ToString());
            
        }
    }
}
