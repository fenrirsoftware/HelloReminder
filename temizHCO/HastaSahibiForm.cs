using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class HastaSahibiForm : Form
    {
        private string connectionString = ("server=.; Initial Catalog=HcoDb;Integrated Security=SSPI");
        public HastaSahibiForm()
        {
            InitializeComponent();
        }

        private void HastaSahibiForm_Load(object sender, EventArgs e)
        {
            LoadHastaSahipleriData();
        }

        private void LoadHastaSahipleriData()
        {
            // DataGridView'i temizle
            dataGridView1.Rows.Clear();

            // Veritabanından hasta sahibi bilgilerini al
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT SahipID, Ad, Soyad, TCKimlik, TelefonNumarasi FROM HastaSahipleri";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Verileri DataGridView'e ekleyin
                            int sahipID = reader.GetInt32(0);
                            string ad = reader.GetString(1);
                            string soyad = reader.GetString(2);
                            string tcKimlik = reader.GetString(3);
                            string telefon = reader.GetString(4);

                            dataGridView1.Rows.Add(sahipID, ad, soyad, tcKimlik, telefon);
                        }
                    }
                }

                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kullanıcının girdiği verileri al
            string sahipAd = txtsahipad.Text;
            string sahipSoyad = txtsahipsoyad.Text;
            string sahipTCKimlik = txtsahipTCkimlik.Text;
            string sahipTelefon = txtnumara.Text;

            // Veriyi kullanıcıya göster
            string message = $"Eklemek istediğiniz hasta sahibi bilgileri:\n\nAd: {sahipAd}\nSoyad: {sahipSoyad}\nTC Kimlik No: {sahipTCKimlik}\nTelefon: {sahipTelefon}\n\nBu veriyi eklemek istiyor musunuz?";
            DialogResult result = MessageBox.Show(message, "Hasta Sahibi Ekleme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Veriyi SQL Server veritabanına eklemek için SQL sorgusu oluştur
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Hasta sahibi ekleme işlemi
                    string insertSahipQuery = "INSERT INTO HastaSahipleri (Ad, Soyad, TCKimlik, TelefonNumarasi) VALUES (@Ad, @Soyad, @TCKimlik, @Telefon)";
                    using (SqlCommand insertSahipCommand = new SqlCommand(insertSahipQuery, connection))
                    {
                        insertSahipCommand.Parameters.AddWithValue("@Ad", sahipAd);
                        insertSahipCommand.Parameters.AddWithValue("@Soyad", sahipSoyad);
                        insertSahipCommand.Parameters.AddWithValue("@TCKimlik", sahipTCKimlik);
                        insertSahipCommand.Parameters.AddWithValue("@Telefon", sahipTelefon);
                        insertSahipCommand.ExecuteNonQuery();
                    }

                    connection.Close();
                }
                LoadHastaSahipleriData();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satır tıklanmışsa
            {
                // Seçilen satırın verilerini TextBox'lara aktar
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                txtsahipad.Text = selectedRow.Cells["Ad"].Value.ToString();
                txtsahipsoyad.Text = selectedRow.Cells["Soyad"].Value.ToString();
                txtsahipTCkimlik.Text = selectedRow.Cells["TCKN"].Value.ToString();
                txtnumara.Text = selectedRow.Cells["Numara"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Seçilen satırın indeksini al
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

            // Eğer hiçbir satır seçilmediyse (indeks -1 ise) çıkış yap
            if (selectedRowIndex == -1)
            {
                MessageBox.Show("Lütfen düzenlemek istediğiniz bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Seçilen satırın SahipID'sini al
            int sahipID = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["ID"].Value);

            // Yeni verileri al
            string yeniAd = txtsahipad.Text;
            string yeniSoyad = txtsahipsoyad.Text;
            string yeniTCKimlik = txtsahipTCkimlik.Text;
            string yeniTelefon = txtnumara.Text;

            // Veriyi kullanıcıya onaylat
            string message = $"Sahip ID: {sahipID}\n\nYeni Bilgiler:\n\nAd: {yeniAd}\nSoyad: {yeniSoyad}\nTC Kimlik No: {yeniTCKimlik}\nTelefon: {yeniTelefon}\n\nBu veriyi güncellemek istiyor musunuz?";
            DialogResult result = MessageBox.Show(message, "Bilgi Güncelleme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Veriyi güncelle
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    string updateQuery = "UPDATE HastaSahipleri SET Ad = @Ad, Soyad = @Soyad, TCKimlik = @TCKimlik, TelefonNumarasi = @Telefon WHERE SahipID = @SahipID";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@SahipID", sahipID);
                        updateCommand.Parameters.AddWithValue("@Ad", yeniAd);
                        updateCommand.Parameters.AddWithValue("@Soyad", yeniSoyad);
                        updateCommand.Parameters.AddWithValue("@TCKimlik", yeniTCKimlik);
                        updateCommand.Parameters.AddWithValue("@Telefon", yeniTelefon);
                        updateCommand.ExecuteNonQuery();
                        LoadHastaSahipleriData();
                    }

                    connection.Close();
                }

                // TextBox'lardaki verileri temizle
                txtsahipad.Clear();
                txtsahipsoyad.Clear();
                txtsahipTCkimlik.Clear();
                txtnumara.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // DataGridView'den seçilen satırın indeksini al
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

            // Eğer hiçbir satır seçilmediyse (indeks -1 ise) çıkış yap
            if (selectedRowIndex == -1)
            {
                MessageBox.Show("Lütfen silmek istediğiniz bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Seçilen satırın SahipID'sini al
            int sahipID = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["ID"].Value);

            // Silme işlemi için onay al
            string message = $"Sahip ID: {sahipID} olan kaydı silmek istiyor musunuz?";
            DialogResult result = MessageBox.Show(message, "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // İlgili hasta sahibinin tüm hayvan ilişkilerini kaldırma sorgusu
                    string deleteHayvanSahipleriQuery = "DELETE FROM HayvanHastaSahipleri WHERE SahipID = @SahipID";
                    using (SqlCommand deleteHayvanSahipleriCommand = new SqlCommand(deleteHayvanSahipleriQuery, connection))
                    {
                        deleteHayvanSahipleriCommand.Parameters.AddWithValue("@SahipID", sahipID);
                        deleteHayvanSahipleriCommand.ExecuteNonQuery();
                    }

                    // Sahip kaydını silme sorgusu
                    string deleteQuery = "DELETE FROM HastaSahipleri WHERE SahipID = @SahipID";

                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@SahipID", sahipID);
                        deleteCommand.ExecuteNonQuery();
                    }



                 

                    connection.Close();



                    connection.Close();
                }

                // DataGridView'i yeniden yükle
                LoadHastaSahipleriData();
            }
        }


        private void HastaSahibiForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string aramaKelimesi = textBox1.Text.Trim(); // TextBox'tan gelen arama kelimesini al

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Arama sorgusu
                string query = "SELECT SahipID, Ad, Soyad, TCKimlik, TelefonNumarasi FROM HastaSahipleri WHERE Ad LIKE @AramaKelimesi OR Soyad LIKE @AramaKelimesi OR TCKimlik LIKE @AramaKelimesi OR TelefonNumarasi LIKE @AramaKelimesi";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AramaKelimesi", "%" + aramaKelimesi + "%"); // LIKE kullanarak benzer sonuçları al
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataGridView1.Rows.Clear(); // DataGridView'i temizle

                        while (reader.Read())
                        {
                            // Verileri DataGridView'e ekleyin
                            int sahipID = reader.GetInt32(0);
                            string ad = reader.GetString(1);
                            string soyad = reader.GetString(2);
                            string tcKimlik = reader.GetString(3);
                            string telefon = reader.GetString(4);

                            dataGridView1.Rows.Add(sahipID, ad, soyad, tcKimlik, telefon);
                        }
                    }
                }

                connection.Close();
            }
        }

    }
}
