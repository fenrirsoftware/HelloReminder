using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class HızlıDüzenlemeForm : Form
    {
        public string veri { get; set; }
        private string connectionString = ("server=.; Initial Catalog=HcoDb;Integrated Security=SSPI");
        public HızlıDüzenlemeForm()
        {
            InitializeComponent();
        }

        private void HızlıDüzenlemeForm_Load(object sender, EventArgs e)
        {
            // HızlıDüzenlemeForm yüklendiğinde, SecilenHayvanID değerini kullanarak SQL sorgusunu çalıştırabilirsiniz.
            string query = $"SELECT Hayvanlar.*, HastaSahipleri.Ad AS SahipAd, HastaSahipleri.Soyad AS SahipSoyad, HastaSahipleri.TCKimlik, HastaSahipleri.TelefonNumarasi " +
                           $"FROM Hayvanlar " +
                           $"INNER JOIN HayvanHastaSahipleri ON Hayvanlar.HayvanID = HayvanHastaSahipleri.HayvanID " +
                           $"INNER JOIN HastaSahipleri ON HayvanHastaSahipleri.SahipID = HastaSahipleri.SahipID " +
                           $"WHERE Hayvanlar.HayvanID = @HayvanID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parametreyi ekleyin
                    command.Parameters.AddWithValue("@HayvanID", veri);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Hayvanın bilgilerini TextBox'lara yerleştirin
                        txtpass.Text = reader["PasaportNumarasi"].ToString();
                        txtcip.Text = reader["CipNumarasi"].ToString();
                        txtrenk.Text = reader["Renk"].ToString();
                        textırk.Text = reader["Irk"].ToString();
                        txtyas.Text = reader["Yas"].ToString();
                        txtcins.Text = reader["Cinsiyet"].ToString();
                        txtnot.Text = reader["Notlar"].ToString();
                        txtür.Text = reader["Tur"].ToString();
                        textBox5.Text = reader["Ad"].ToString(); // Hayvan Adı buradan gelsin

                        // Hasta Sahibi bilgilerini TextBox'lara yerleştirin
                        txtsahipad.Text = reader["SahipAd"].ToString(); // Sahip Adı buradan gelsin
                        txtsahipsoyad.Text = reader["SahipSoyad"].ToString(); // Sahip Soyadı buradan gelsin
                        txtsahipTCkimlik.Text = reader["TCKimlik"].ToString();
                        txtnumara.Text = reader["TelefonNumarasi"].ToString();
                    }

                    reader.Close();
                }
            }
        }


        private void HızlıDüzenlemeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Görüntüleme g = new Görüntüleme();
            g.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Kullanıcı tarafından girilen verileri alın
            string yeniPasaportNumarasi = txtpass.Text;
            string yeniCipNumarasi = txtcip.Text;
            string yeniRenk = txtrenk.Text;
            string yeniIrk = textırk.Text;
            int yeniYas = Convert.ToInt32(txtyas.Text);
            string yeniCinsiyet = txtcins.Text;
            string yeniNotlar = txtnot.Text;
            string yeniTur = txtür.Text;

            // Kullanıcı tarafından girilen hasta sahibi bilgilerini alın
            string yeniAd = txtsahipad.Text;
            string yeniSoyad = txtsahipsoyad.Text;
            string yeniTCKimlik = txtsahipTCkimlik.Text;
            string yeniTelefonNumarasi = txtnumara.Text;

            // Güncelleme sorgusu oluşturun
            string updateQuery = @"UPDATE Hayvanlar
                            SET PasaportNumarasi = @PasaportNumarasi,
                                CipNumarasi = @CipNumarasi,
                                Renk = @Renk,
                                Irk = @Irk,
                                Yas = @Yas,
                                Cinsiyet = @Cinsiyet,
                                Notlar = @Notlar,
                                Tur = @Tur
                            WHERE HayvanID = @HayvanID;

                            UPDATE HastaSahipleri
                            SET Ad = @Ad,
                                Soyad = @Soyad,
                                TCKimlik = @TCKimlik,
                                TelefonNumarasi = @TelefonNumarasi
                            WHERE SahipID = (SELECT SahipID FROM HayvanHastaSahipleri WHERE HayvanID = @HayvanID);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // Parametreleri ekleyin
                    command.Parameters.AddWithValue("@PasaportNumarasi", yeniPasaportNumarasi);
                    command.Parameters.AddWithValue("@CipNumarasi", yeniCipNumarasi);
                    command.Parameters.AddWithValue("@Renk", yeniRenk);
                    command.Parameters.AddWithValue("@Irk", yeniIrk);
                    command.Parameters.AddWithValue("@Yas", yeniYas);
                    command.Parameters.AddWithValue("@Cinsiyet", yeniCinsiyet);
                    command.Parameters.AddWithValue("@Notlar", yeniNotlar);
                    command.Parameters.AddWithValue("@Tur", yeniTur);
                    command.Parameters.AddWithValue("@Ad", yeniAd);
                    command.Parameters.AddWithValue("@Soyad", yeniSoyad);
                    command.Parameters.AddWithValue("@TCKimlik", yeniTCKimlik);
                    command.Parameters.AddWithValue("@TelefonNumarasi", yeniTelefonNumarasi);
                    command.Parameters.AddWithValue("@HayvanID", veri);

                    try
                    {
                        int affectedRows = command.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Veriler başarıyla güncellendi.");
                        }
                        else
                        {
                            MessageBox.Show("Veriler güncellenemedi.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veriler güncellenirken bir hata oluştu: " + ex.Message);
                    }
                }
            }
        }
    }
}
