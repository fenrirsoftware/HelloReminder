using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class Reminder : Form
    {
        private SqliteConnection connection;
        private string connectionString = "Data Source=HCO.db;";
        private DataGridView dataGridView;

        public Reminder()
        {
            InitializeComponent();

            // DataGridView oluşturma ve ayarları
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            this.Controls.Add(dataGridView);

            // DataGridView için sütunları tanımlama
            dataGridView.Columns.Add("AsiAdi", "Aşı Adı");
            dataGridView.Columns.Add("AsiTarihi", "Aşı Tarihi");
            dataGridView.Columns.Add("AsiTekrarTarihi", "Aşı Tekrar Tarihi");
            dataGridView.Columns.Add("HayvanAdi", "Hayvan Adı");
            dataGridView.Columns.Add("CipNumarasi", "Çip Numarası");
            dataGridView.Columns.Add("PasaportNumarasi", "Pasaport Numarası");
            dataGridView.Columns.Add("SahipAdi", "Sahip Adı");
            dataGridView.Columns.Add("SahipSoyadi", "Sahip Soyadı");
            dataGridView.Columns.Add("SahipTCKimlik", "Sahip TC Kimlik");
            dataGridView.Columns.Add("SahipTelefon", "Sahip Telefon");

            // Verileri çekme ve DataGridView'e ekleme
            LoadData();
        }
        /// <summary>
        /// reminder sorunlu sql sorgusu düzeltilecek.
        /// </summary>
        private void LoadData()
        {
            // Veritabanı bağlantısını açma
            using (connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // SQL sorgusu hazırlama ve verileri çekme (Aşıları 7 günden daha az süre kalanları seçme)
                string query = "SELECT a.AsiAdi, a.AsiTarihi, a.AsiTekrarTarihi, " +
                             "h.Ad AS HayvanAdi, h.CipNumarasi, h.PasaportNumarasi, " +
                             "hs.Ad AS SahipAdi, hs.Soyad AS SahipSoyadi, hs.TCKimlik AS SahipTCKimlik, hs.TelefonNumarasi AS SahipTelefon " +
                             "FROM HayvanAsi AS ha " +
                             "INNER JOIN Asilar AS a ON ha.AsiTakipID = a.AsiTakipID " +
                             "INNER JOIN Hayvanlar AS h ON ha.HayvanID = h.HayvanID " +
                             "INNER JOIN HastaSahipleri AS hs ON h.SahipID = hs.SahipID " +
                             "WHERE (julianday(a.AsiTekrarTarihi) - julianday('now')) < 7";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        // Verileri DataGridView'e ekleme
                        while (reader.Read())
                        {
                            string asiAdi = reader.GetString(0);
                            string asiTarihiStr = reader.GetString(1);
                            string asiTekrarTarihiStr = reader.GetString(2);
                            string hayvanAdi = reader.GetString(3);
                            string cipNumarasi = reader.GetString(4);
                            string pasaportNumarasi = reader.GetString(5);
                            string sahipAdi = reader.GetString(6);
                            string sahipSoyadi = reader.GetString(7);
                            string sahipTCKimlik = reader.GetString(8);
                            string sahipTelefon = reader.GetString(9);

                            // Tarihleri ayrıştırma işlemi
                            DateTime asiTarihi;
                            DateTime asiTekrarTarihi;

                            if (DateTime.TryParseExact(asiTarihiStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out asiTarihi) &&
                                DateTime.TryParseExact(asiTekrarTarihiStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out asiTekrarTarihi))
                            {
                                dataGridView.Rows.Add(asiAdi, asiTarihi.ToShortDateString(), asiTekrarTarihi.ToShortDateString(),
                                    hayvanAdi, cipNumarasi, pasaportNumarasi, sahipAdi, sahipSoyadi, sahipTCKimlik, sahipTelefon);
                            }
                            else
                            {
                                // Tarih ayrıştırma hatası
                                MessageBox.Show($"Geçersiz tarih formatı: {asiTarihiStr} veya {asiTekrarTarihiStr}");
                            }
                        }
                    }
                }
            }
        }
    }
}
