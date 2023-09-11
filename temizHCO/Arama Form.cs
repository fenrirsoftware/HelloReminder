using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class Arama_Form : Form
    {
        public Arama_Form()
        {
          
            InitializeComponent();
            // Veritabanı bağlantısını açın
            connection = new SqliteConnection(connectionString);
        }
        public SqliteConnection connection;
        private string connectionString = "Data Source=HCO.db;"; // Veritabanı bağlantı dizesini buraya ekleyin

        private void Arama_Form_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            //to lower yapacan unutma
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                string belirliHayvanAdi = textBox1.Text.Trim(); // TextBox'tan alınan değeri kullanın
                string query = "SELECT Hayvanlar.Ad AS HayvanAdi, HastaSahipleri.Ad AS SahipAdi, HastaSahipleri.Soyad AS SahipSoyadi, " +
                               "HastaSahipleri.TCKimlik, HastaSahipleri.TelefonNumarasi, " +
                               "Asilar.AsiAdi, Asilar.AsiTarihi, Asilar.AsiTekrarTarihi " +
                               "FROM Hayvanlar " +
                               "INNER JOIN HayvanHastaSahipleri ON Hayvanlar.HayvanID = HayvanHastaSahipleri.HayvanID " +
                               "INNER JOIN HastaSahipleri ON HayvanHastaSahipleri.SahipID = HastaSahipleri.SahipID " +
                               "LEFT JOIN HayvanAsi ON Hayvanlar.HayvanID = HayvanAsi.HayvanID " +
                               "LEFT JOIN Asilar ON HayvanAsi.AsiTakipID = Asilar.AsiTakipID " +
                               $"WHERE Hayvanlar.Ad LIKE '%{belirliHayvanAdi}%'"; // LIKE kullanıldı

                ExecuteQueryAndShowResult(query);
            }
            else if (radioButton2.Checked)
            {
                string tcKimlik = textBox1.Text.Trim(); // TextBox'tan alınan TC Kimlik değerini kullanın
                string query = "SELECT Hayvanlar.Ad AS HayvanAdi, HastaSahipleri.Ad AS SahipAdi, HastaSahipleri.Soyad AS SahipSoyadi, " +
                               "HastaSahipleri.TCKimlik, HastaSahipleri.TelefonNumarasi, " +
                               "Asilar.AsiAdi, Asilar.AsiTarihi, Asilar.AsiTekrarTarihi " +
                               "FROM HastaSahipleri " +
                               "INNER JOIN HayvanHastaSahipleri ON HastaSahipleri.SahipID = HayvanHastaSahipleri.SahipID " +
                               "INNER JOIN Hayvanlar ON HayvanHastaSahipleri.HayvanID = Hayvanlar.HayvanID " +
                               "LEFT JOIN HayvanAsi ON Hayvanlar.HayvanID = HayvanAsi.HayvanID " +
                               "LEFT JOIN Asilar ON HayvanAsi.AsiTakipID = Asilar.AsiTakipID " +
                               $"WHERE HastaSahipleri.TCKimlik = '{tcKimlik}'"; // TC Kimlik ile sorgu

                ExecuteQueryAndShowResult(query);
            }
            else if (radioButton3.Checked)
            {
                string pasaportNumarasi = textBox1.Text.Trim(); // TextBox'tan alınan pasaport numarasını kullanın
                string query = "SELECT Hayvanlar.Ad AS HayvanAdi, HastaSahipleri.Ad AS SahipAdi, HastaSahipleri.Soyad AS SahipSoyadi, " +
                               "HastaSahipleri.TCKimlik, HastaSahipleri.TelefonNumarasi, " +
                               "Asilar.AsiAdi, Asilar.AsiTarihi, Asilar.AsiTekrarTarihi " +
                               "FROM Hayvanlar " +
                               "INNER JOIN HayvanHastaSahipleri ON Hayvanlar.HayvanID = HayvanHastaSahipleri.HayvanID " +
                               "INNER JOIN HastaSahipleri ON HayvanHastaSahipleri.SahipID = HastaSahipleri.SahipID " +
                               "LEFT JOIN HayvanAsi ON Hayvanlar.HayvanID = HayvanAsi.HayvanID " +
                               "LEFT JOIN Asilar ON HayvanAsi.AsiTakipID = Asilar.AsiTakipID " +
                               $"WHERE Hayvanlar.PasaportNumarasi = '{pasaportNumarasi}'"; // Pasaport numarası ile sorgu

                ExecuteQueryAndShowResult(query);
            }
            else if (radioButton4.Checked)
            {
                string cipNumarasi = textBox1.Text.Trim(); // TextBox'tan alınan çip numarasını kullanın
                string query = "SELECT Hayvanlar.Ad AS HayvanAdi, HastaSahipleri.Ad AS SahipAdi, HastaSahipleri.Soyad AS SahipSoyadi, " +
                               "HastaSahipleri.TCKimlik, HastaSahipleri.TelefonNumarasi, " +
                               "Asilar.AsiAdi, Asilar.AsiTarihi, Asilar.AsiTekrarTarihi " +
                               "FROM Hayvanlar " +
                               "INNER JOIN HayvanHastaSahipleri ON Hayvanlar.HayvanID = HayvanHastaSahipleri.HayvanID " +
                               "INNER JOIN HastaSahipleri ON HayvanHastaSahipleri.SahipID = HastaSahipleri.SahipID " +
                               "LEFT JOIN HayvanAsi ON Hayvanlar.HayvanID = HayvanAsi.HayvanID " +
                               "LEFT JOIN Asilar ON HayvanAsi.AsiTakipID = Asilar.AsiTakipID " +
                               $"WHERE Hayvanlar.CipNumarasi = '{cipNumarasi}'"; // Çip numarası ile sorgu

                ExecuteQueryAndShowResult(query);
            }


            else if (radioButton5.Checked)
            {
               
                    string adsoyad = textBox1.Text.Trim();
                  
                    string query = $"SELECT Hayvanlar.Ad AS HayvanAdi, HastaSahipleri.Ad AS SahipAdi, HastaSahipleri.Soyad AS SahipSoyadi, " +
                            "HastaSahipleri.TCKimlik, HastaSahipleri.TelefonNumarasi, " +
                            "Asilar.AsiAdi, Asilar.AsiTarihi, Asilar.AsiTekrarTarihi " +
                            "FROM Hayvanlar " +
                            "INNER JOIN HayvanHastaSahipleri ON Hayvanlar.HayvanID = HayvanHastaSahipleri.HayvanID " +
                            "INNER JOIN HastaSahipleri ON HayvanHastaSahipleri.SahipID = HastaSahipleri.SahipID " +
                            "LEFT JOIN HayvanAsi ON Hayvanlar.HayvanID = HayvanAsi.HayvanID " +
                            "LEFT JOIN Asilar ON HayvanAsi.AsiTakipID = Asilar.AsiTakipID " +
                            $"WHERE HastaSahipleri.Ad LIKE '%{adsoyad}%' OR HastaSahipleri.Soyad LIKE '%{adsoyad}%' " +
                            $"OR (HastaSahipleri.Ad || ' ' || HastaSahipleri.Soyad) LIKE '%{adsoyad}%'"; // LIKE kullanıldı
                

                ExecuteQueryAndShowResult(query);
            }
            else
            {
                MessageBox.Show("Lütfen bir seçim yapınız.");
            }
        }

        private void ExecuteQueryAndShowResult(string query)
        {
            SqliteCommand command = new SqliteCommand(query, connection);

            try
            {
                connection.Open();
                SqliteDataReader reader = command.ExecuteReader();

                string sonuc = "Hasta Sahibi ve Aşı Bilgileri:\n";

                while (reader.Read())
                {
                    string hayvanAdi = reader["HayvanAdi"].ToString();
                    string sahipAdi = reader["SahipAdi"].ToString();
                    string sahipSoyadi = reader["SahipSoyadi"].ToString();
                    string tcKimlik = reader["TCKimlik"].ToString();
                    string telefonNumarasi = reader["TelefonNumarasi"].ToString();
                    string asiAdi = reader["AsiAdi"].ToString();
                    string asiTarihi = reader["AsiTarihi"].ToString();
                    string asiTekrarTarihi = reader["AsiTekrarTarihi"].ToString();

                    sonuc += "Hayvan Adı: " + hayvanAdi + "\n";
                    sonuc += "Sahip Adı: " + sahipAdi + "\n";
                    sonuc += "Sahip Soyadı: " + sahipSoyadi + "\n";
                    sonuc += "TC Kimlik: " + tcKimlik + "\n";
                    sonuc += "Telefon Numarası: " + telefonNumarasi + "\n";
                    sonuc += "Aşı Adı: " + asiAdi + "\n";
                    sonuc += "Aşı Tarihi: " + asiTarihi + "\n";
                    sonuc += "Aşı Tekrar Tarihi: " + asiTekrarTarihi + "\n\n";
                }

                reader.Close();

                if (!string.IsNullOrEmpty(sonuc))
                {
                    MessageBox.Show(sonuc);
                }
                else
                {
                    MessageBox.Show("Belirtilen kriterlere uygun hasta sahibi ve aşı bilgileri bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arama yapılırken bir hata oluştu: " + ex.Message);
            }
            finally
            {
                // Veritabanı bağlantısını kapatın
                connection.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
