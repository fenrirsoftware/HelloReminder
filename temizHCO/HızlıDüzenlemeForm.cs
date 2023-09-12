using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class HızlıDüzenlemeForm : Form
    {
       



        public HızlıDüzenlemeForm()
        {
            InitializeComponent();
        }
        public string veri { get; set; }
       

        private void HızlıDüzenlemeForm_Load(object sender, EventArgs e)
        {
            // HızlıDüzenlemeForm yüklendiğinde, SecilenHayvanID değerini kullanarak SQL sorgusunu çalıştırabilirsiniz.
            string connectionString = "Data Source=HCO.db;";
            string query = $"SELECT Hayvanlar.*, HastaSahipleri.* " +
                           $"FROM Hayvanlar " +
                           $"INNER JOIN HayvanHastaSahipleri ON Hayvanlar.HayvanID = HayvanHastaSahipleri.HayvanID " +
                           $"INNER JOIN HastaSahipleri ON HayvanHastaSahipleri.SahipID = HastaSahipleri.SahipID " +
                           $"WHERE Hayvanlar.HayvanID = {veri}";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();

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

                    // Hasta Sahibi bilgilerini TextBox'lara yerleştirin
                    txtsahipad.Text = reader["Ad"].ToString(); // Örnek olarak, Hasta Sahibinin Adını aldık
                    txtsahipsoyad.Text = reader["Soyad"].ToString(); // Hasta Sahibinin Soyadını aldık
                    txtsahipTCkimlik.Text = reader["TCKimlik"].ToString(); // Hasta Sahibinin TC Kimlik Numarasını aldık
                    txtnumara.Text = reader["TelefonNumarasi"].ToString(); // Hasta Sahibinin Telefon Numarasını aldık
                }

                reader.Close();
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
            string connectionString = "Data Source=HCO.db;";
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

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(updateQuery, connection))
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
