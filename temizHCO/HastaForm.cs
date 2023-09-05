using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class HastaForm : Form
    {
        private string connectionString = "Data Source=HCO.db;";
        private int hayvanID = -1; // Seçilen hayvanın ID'sini saklamak için kullanılacak

        public HastaForm()
        {
            InitializeComponent();
            SQLitePCL.Batteries.Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TextBox'lardan hayvan bilgilerini al
            string hayvanAd = txtad.Text;
            string pasaportNumarasi = txtpass.Text;
            string cipNumarasi = txtcip.Text;
            string renk = txtrenk.Text;
            string irk = textırk.Text;
            int yas = Convert.ToInt32(txtyas.Text);
            string cinsiyet = txtcins.Text;
            string notlar = txtnot.Text;
            string tur = txtür.Text;

            // ComboBox'tan seçilen hastasahibinin adını ve soyadını ayırın
            string selectedSahip = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedSahip))
            {
                MessageBox.Show("Lütfen bir hastasahibi seçin.");
                return;
            }

            string[] sahipAdSoyad = selectedSahip.Split(' '); // Ad ve soyadı ayır
            string sahipAd = sahipAdSoyad[0];
            string sahipSoyad = sahipAdSoyad[1];

            // Veritabanına hayvan bilgilerini ekleyin
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Eklenen veya mevcut olan hasta sahibinin ID'sini alın
                string getSahipIDQuery = "SELECT SahipID FROM HastaSahipleri WHERE Ad = @Ad AND Soyad = @Soyad";
                int sahipID;
                using (SqliteCommand getSahipIDCommand = new SqliteCommand(getSahipIDQuery, connection))
                {
                    getSahipIDCommand.Parameters.AddWithValue("@Ad", sahipAd);
                    getSahipIDCommand.Parameters.AddWithValue("@Soyad", sahipSoyad);

                    object result = getSahipIDCommand.ExecuteScalar();
                    if (result != null)
                    {
                        sahipID = Convert.ToInt32(result);
                    }
                    else
                    {
                        // SahipID bulunamadı, hata işleme kodunu ekleyin
                        MessageBox.Show("Hasta sahibi bulunamadı.");
                        return;
                    }
                }

                // Hayvanı veritabanına ekleyin
                string insertHayvanQuery = "INSERT INTO Hayvanlar (Ad, PasaportNumarasi, CipNumarasi, Renk, Irk, Yas, Cinsiyet, Notlar, Tur) " +
                                           "VALUES (@Ad, @PasaportNumarasi, @CipNumarasi, @Renk, @Irk, @Yas, @Cinsiyet, @Notlar, @Tur)";
                using (SqliteCommand insertHayvanCommand = new SqliteCommand(insertHayvanQuery, connection))
                {
                    insertHayvanCommand.Parameters.AddWithValue("@Ad", hayvanAd);
                    insertHayvanCommand.Parameters.AddWithValue("@PasaportNumarasi", pasaportNumarasi);
                    insertHayvanCommand.Parameters.AddWithValue("@CipNumarasi", cipNumarasi);
                    insertHayvanCommand.Parameters.AddWithValue("@Renk", renk);
                    insertHayvanCommand.Parameters.AddWithValue("@Irk", irk);
                    insertHayvanCommand.Parameters.AddWithValue("@Yas", yas);
                    insertHayvanCommand.Parameters.AddWithValue("@Cinsiyet", cinsiyet);
                    insertHayvanCommand.Parameters.AddWithValue("@Notlar", notlar);
                    insertHayvanCommand.Parameters.AddWithValue("@Tur", tur);

                    insertHayvanCommand.ExecuteNonQuery();
                }

                // Hayvanı ve hastasahibini ilişkilendirin
                string getLastInsertedHayvanIDQuery = "SELECT last_insert_rowid()";
                int hayvanID;
                using (SqliteCommand getLastInsertedHayvanIDCommand = new SqliteCommand(getLastInsertedHayvanIDQuery, connection))
                {
                    hayvanID = Convert.ToInt32(getLastInsertedHayvanIDCommand.ExecuteScalar());
                }

                string insertHayvanSahipQuery = "INSERT INTO HayvanHastaSahipleri (HayvanID, SahipID) VALUES (@HayvanID, @SahipID)";
                using (SqliteCommand insertHayvanSahipCommand = new SqliteCommand(insertHayvanSahipQuery, connection))
                {
                    insertHayvanSahipCommand.Parameters.AddWithValue("@HayvanID", hayvanID);
                    insertHayvanSahipCommand.Parameters.AddWithValue("@SahipID", sahipID);
                    insertHayvanSahipCommand.ExecuteNonQuery();
                }

                connection.Close();

              
            }

            LoadVeriler();

            // Verileri temizle
            TemizleForm();
        }
        private void TemizleForm()
        {
            // Tüm girdi kutularını temizle
            txtad.Clear();
            txtpass.Clear();
            txtcip.Clear();
            txtrenk.Clear();
            textırk.Clear();
            txtyas.Clear();
            txtcins.Clear(); // Cinsiyet seçimini temizle
            txtnot.Clear();
            txtür.Clear();
            comboBox1.SelectedIndex = -1;
        }

        private void LoadVeriler()
        {
            // DataGridView'i temizle
            dataGridView1.Rows.Clear();

            // Veritabanından hayvan ve sahip bilgilerini al
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT
                    Hayvanlar.HayvanID,
                    Hayvanlar.Ad AS HayvanAdi,
                    Hayvanlar.PasaportNumarasi,
                    Hayvanlar.CipNumarasi,
                    Hayvanlar.Renk,
                    Hayvanlar.Irk,
                    Hayvanlar.Yas,
                    Hayvanlar.Cinsiyet,
                    Hayvanlar.Notlar,
                    Hayvanlar.Tur,
                    HastaSahipleri.Ad AS SahipAdi,
                    HastaSahipleri.Soyad AS SahipSoyadi,
                    HastaSahipleri.TCKimlik AS SahipTCKimlik
                FROM
                    Hayvanlar
                LEFT JOIN
                    HayvanHastaSahipleri ON Hayvanlar.HayvanID = HayvanHastaSahipleri.HayvanID
                LEFT JOIN
                    HastaSahipleri ON HayvanHastaSahipleri.SahipID = HastaSahipleri.SahipID;";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Verileri okurken NULL kontrolü yapın ve sütun adlarına göre okuyun
                            int hayvanID = reader.IsDBNull(reader.GetOrdinal("HayvanID")) ? -1 : reader.GetInt32(reader.GetOrdinal("HayvanID"));

                            string hayvanAdi = reader.IsDBNull(reader.GetOrdinal("HayvanAdi")) ? string.Empty : reader.GetString(reader.GetOrdinal("HayvanAdi"));
                            string pasaportNumarasi = reader.IsDBNull(reader.GetOrdinal("PasaportNumarasi")) ? string.Empty : reader.GetString(reader.GetOrdinal("PasaportNumarasi"));
                            string cipNumarasi = reader.IsDBNull(reader.GetOrdinal("CipNumarasi")) ? string.Empty : reader.GetString(reader.GetOrdinal("CipNumarasi"));
                            string renk = reader.IsDBNull(reader.GetOrdinal("Renk")) ? string.Empty : reader.GetString(reader.GetOrdinal("Renk"));
                            string irk = reader.IsDBNull(reader.GetOrdinal("Irk")) ? string.Empty : reader.GetString(reader.GetOrdinal("Irk"));
                            int yas = reader.IsDBNull(reader.GetOrdinal("Yas")) ? -1 : reader.GetInt32(reader.GetOrdinal("Yas"));
                            string cinsiyet = reader.IsDBNull(reader.GetOrdinal("Cinsiyet")) ? string.Empty : reader.GetString(reader.GetOrdinal("Cinsiyet"));
                            string tur = reader.IsDBNull(reader.GetOrdinal("Tur")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tur"));
                            string notlar = reader.IsDBNull(reader.GetOrdinal("Notlar")) ? string.Empty : reader.GetString(reader.GetOrdinal("Notlar"));
                            string sahipAdi = reader.IsDBNull(reader.GetOrdinal("SahipAdi")) ? string.Empty : reader.GetString(reader.GetOrdinal("SahipAdi"));
                            string sahipSoyadi = reader.IsDBNull(reader.GetOrdinal("SahipSoyadi")) ? string.Empty : reader.GetString(reader.GetOrdinal("SahipSoyadi"));
                            string sahipTCKimlik = reader.IsDBNull(reader.GetOrdinal("SahipTCKimlik")) ? string.Empty : reader.GetString(reader.GetOrdinal("SahipTCKimlik"));

                            dataGridView1.Rows.Add(hayvanID, hayvanAdi, pasaportNumarasi, cipNumarasi, renk, irk, yas, cinsiyet, tur, notlar, sahipAdi, sahipSoyadi, sahipTCKimlik);
                        }
                    }
                }

                connection.Close();
            }
        }

        private void HastaForm_Load(object sender, EventArgs e)
        {
            LoadVeriler();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Ad, Soyad FROM HastaSahipleri";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Hasta sahibinin adını ve soyadını alın
                            string ad = reader.GetString(0);
                            string soyad = reader.GetString(1);

                            // Ad ve soyadı birleştirerek ComboBox'a ekleyin
                            string sahipAdSoyad = $"{ad} {soyad}";
                            comboBox1.Items.Add(sahipAdSoyad);
                        }
                    }
                }

                connection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Kullanıcının girdiği verileri al
            int hayvanID1 = hayvanID; // Önceki kodda seçilen Hayvan ID değerini kullan
            string hayvanAd = txtad.Text;
            string pasaportNumarasi = txtpass.Text;
            string cipNumarasi = txtcip.Text;
            string renk = txtrenk.Text;
            string irk = textırk.Text;
            int yas = Convert.ToInt32(txtyas.Text);
            string cinsiyet = txtcins.Text;
            string notlar = txtnot.Text;
            string tur = txtür.Text;

            // Seçilen hastasahibi adını ve soyadını ayır
            string secilenSahipAdSoyad = comboBox1.SelectedItem.ToString();
            string[] sahipAdSoyadParcalari = secilenSahipAdSoyad.Split(' ');
            string sahipAd = sahipAdSoyadParcalari[0];
            string sahipSoyad = sahipAdSoyadParcalari[1];

            // Veriyi kullanıcıya göster
            string message = $"Güncellemek istediğiniz hayvan bilgileri:\n\nHayvan Adı: {hayvanAd}\nPasaport Numarası: {pasaportNumarasi}\nCip Numarası: {cipNumarasi}\nRenk: {renk}\nIrk: {irk}\nYaş: {yas}\nCinsiyet: {cinsiyet}\nNotlar: {notlar}\nTür: {tur}\nSahip Adı: {sahipAd}\nSahip Soyadı: {sahipSoyad}\n\nBu veriyi güncellemek istiyor musunuz?";
            DialogResult result = MessageBox.Show(message, "Hayvan Güncelleme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Veriyi SQLite veritabanında güncelleme için SQL sorgularını kullan
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    // Hayvanı güncelleme işlemi
                    string updateHayvanQuery = "UPDATE Hayvanlar SET Ad = @Ad, PasaportNumarasi = @PasaportNumarasi, CipNumarasi = @CipNumarasi, Renk = @Renk, Irk = @Irk, Yas = @Yas, Cinsiyet = @Cinsiyet, Notlar = @Notlar, Tur = @Tur WHERE HayvanID = @HayvanID";
                    using (SqliteCommand updateHayvanCommand = new SqliteCommand(updateHayvanQuery, connection))
                    {
                        updateHayvanCommand.Parameters.AddWithValue("@HayvanID", hayvanID);
                        updateHayvanCommand.Parameters.AddWithValue("@Ad", hayvanAd);
                        updateHayvanCommand.Parameters.AddWithValue("@PasaportNumarasi", pasaportNumarasi);
                        updateHayvanCommand.Parameters.AddWithValue("@CipNumarasi", cipNumarasi);
                        updateHayvanCommand.Parameters.AddWithValue("@Renk", renk);
                        updateHayvanCommand.Parameters.AddWithValue("@Irk", irk);
                        updateHayvanCommand.Parameters.AddWithValue("@Yas", yas);
                        updateHayvanCommand.Parameters.AddWithValue("@Cinsiyet", cinsiyet);
                        updateHayvanCommand.Parameters.AddWithValue("@Notlar", notlar);
                        updateHayvanCommand.Parameters.AddWithValue("@Tur", tur);

                        updateHayvanCommand.ExecuteNonQuery();
                    }

                    // Hastasahibi bağlama işlemi
                    string updateHayvanSahipQuery = "UPDATE HayvanHastaSahipleri SET SahipID = (SELECT SahipID FROM HastaSahipleri WHERE Ad = @SahipAd AND Soyad = @SahipSoyad) WHERE HayvanID = @HayvanID";
                    using (SqliteCommand updateHayvanSahipCommand = new SqliteCommand(updateHayvanSahipQuery, connection))
                    {
                        updateHayvanSahipCommand.Parameters.AddWithValue("@HayvanID", hayvanID);
                        updateHayvanSahipCommand.Parameters.AddWithValue("@SahipAd", sahipAd);
                        updateHayvanSahipCommand.Parameters.AddWithValue("@SahipSoyad", sahipSoyad);

                        updateHayvanSahipCommand.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                // Verileri temizleme işlemi
                TemizleForm();
                LoadVeriler(); // Yeniden verileri yükle
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                if (int.TryParse(row.Cells["ID"].Value.ToString(), out hayvanID))
                {
                    // Hayvan ID değerini değişkene atar
                }

                // Seçilen hücrelerin değerlerini TextBox'lara geri getir
                txtad.Text = row.Cells["HayvanAd"].Value.ToString();
                txtpass.Text = row.Cells["Pasaport"].Value.ToString();
                txtcip.Text = row.Cells["Çip"].Value.ToString();
                txtrenk.Text = row.Cells["Renk"].Value.ToString();
                textırk.Text = row.Cells["Irk"].Value.ToString();
                txtyas.Text = row.Cells["Yaş"].Value.ToString();
                txtcins.Text = row.Cells["Cinsiyet"].Value.ToString();
                txtnot.Text = row.Cells["Notlar"].Value.ToString();
                txtür.Text = row.Cells["Tür"].Value.ToString();

                // Hasta sahibini ComboBox'ta seç
                string sahipAdi = row.Cells["SahipAd"].Value.ToString();
                string sahipSoyadi = row.Cells["SahipSoyad"].Value.ToString();
                string sahipAdSoyad = $"{sahipAdi} {sahipSoyadi}";

                if (comboBox1.Items.Contains(sahipAdSoyad))
                {
                    comboBox1.SelectedItem = sahipAdSoyad;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (hayvanID > 0)
            {
                // Kullanıcıdan onay al
                DialogResult result = MessageBox.Show("Hayvanı silmek istiyor musunuz?", "Hayvan Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Hayvanı veritabanından silme işlemi
                    using (SqliteConnection connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();

                        string deleteHayvanQuery = "DELETE FROM Hayvanlar WHERE HayvanID = @HayvanID";
                        using (SqliteCommand deleteHayvanCommand = new SqliteCommand(deleteHayvanQuery, connection))
                        {
                            deleteHayvanCommand.Parameters.AddWithValue("@HayvanID", hayvanID);
                            deleteHayvanCommand.ExecuteNonQuery();
                        }

                        connection.Close();

                        // Verileri temizleme işlemi
                        TemizleForm();
                        LoadVeriler();
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir hayvan seçin.", "Hayvan Silme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
