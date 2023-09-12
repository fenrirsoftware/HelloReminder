using Microsoft.Data.Sqlite;
using System;
using System.Linq;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class AşıForm : Form
    {
        private string connectionString = "Data Source=HCO.db;";

        public AşıForm()
        {
            InitializeComponent();
            SQLitePCL.Batteries.Init();
        }

        private void AşıForm_Load(object sender, EventArgs e)
        {
            LoadAşıData();
            LoadHayvanComboBox();
        }

        private void LoadAşıData()
        {
            dataGridView1.Rows.Clear();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT Asilar.AsiTakipID, Asilar.AsiAdi, Asilar.AsiTarihi, Asilar.AsiTekrarTarihi, 
                                Hayvanlar.Ad AS HayvanAdi, Hayvanlar.PasaportNumarasi
                         FROM Asilar
                         LEFT JOIN HayvanAsi ON Asilar.AsiTakipID = HayvanAsi.AsiTakipID
                         LEFT JOIN Hayvanlar ON HayvanAsi.HayvanID = Hayvanlar.HayvanID";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int asiTakipID = reader.GetInt32(0);
                            string asiAdi = reader.GetString(1);
                            string asiTarihi = reader.GetString(2);
                            string asiTekrarTarihi = reader.GetString(3);


                            string hayvanAdi = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            string pasaportNumarasi = reader.IsDBNull(5) ? "" : reader.GetString(5);

                            dataGridView1.Rows.Add(asiTakipID, asiAdi, asiTarihi, asiTekrarTarihi, hayvanAdi, pasaportNumarasi);
                        }
                    }
                }
                
                connection.Close();
            }

          
        }

        private void LoadHayvanComboBox()
        {
            comboBox1.Items.Clear();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT HayvanID, Ad, PasaportNumarasi FROM Hayvanlar";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int hayvanID = reader.GetInt32(0);
                            string hayvanAdi = reader.GetString(1);
                            string pasaportNumarasi = reader.GetString(2);

                            ComboBoxItem comboBoxItem = new ComboBoxItem
                            {
                                HayvanID = hayvanID,
                                DisplayText = $"{hayvanAdi} - {pasaportNumarasi}"
                            };

                            comboBox1.Items.Add(comboBoxItem);
                        }
                    }
                }
                connection.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = selectedRow.Cells["AşıAd"].Value.ToString();
                maskedTextBox1.Text = selectedRow.Cells["ABT"].Value.ToString();
                maskedTextBox2.Text = selectedRow.Cells["ATT"].Value.ToString();
                comboBox1.Text = "";
                comboBox1.Items.Clear();
                LoadHayvanComboBox();

                int aşıTakipID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT HayvanAsi.HayvanID FROM HayvanAsi " +
                                   "WHERE HayvanAsi.AsiTakipID = @AsiTakipID";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AsiTakipID", aşıTakipID);

                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int hayvanID = reader.GetInt32(0);
                                ComboBoxItem selectedComboBoxItem = comboBox1.Items.OfType<ComboBoxItem>()
                                    .FirstOrDefault(item => item.HayvanID == hayvanID);

                                if (selectedComboBoxItem != null)
                                {
                                    comboBox1.SelectedItem = selectedComboBoxItem;
                                   
                                }
                            }
                        }
                    }
                   
                    connection.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string isim = textBox1.Text;
            string baslangic = maskedTextBox1.Text;
            string tekerrür = maskedTextBox2.Text;

            string message = $"Eklemek istediğiniz Aşı bilgileri:\n\nAd: {isim}\nBaşlangıç: {baslangic}\nTekerrür: {tekerrür}\n\n\nBu veriyi eklemek istiyor musunuz?";
            DialogResult result = MessageBox.Show(message, "Aşı Ekleme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Asilar (AsiAdi, AsiTarihi, AsiTekrarTarihi) VALUES (@AsiAdi, @AsiTarihi, @AsiTekrarTarihi)";
                    using (SqliteCommand insertCommand = new SqliteCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@AsiAdi", isim);
                        insertCommand.Parameters.AddWithValue("@AsiTarihi", baslangic);
                        insertCommand.Parameters.AddWithValue("@AsiTekrarTarihi", tekerrür);
                        insertCommand.ExecuteNonQuery();

                        string selectLastInsertID = "SELECT last_insert_rowid()";
                        using (SqliteCommand selectCommand = new SqliteCommand(selectLastInsertID, connection))
                        {
                            int asiTakipID = Convert.ToInt32(selectCommand.ExecuteScalar());

                            ComboBoxItem selectedComboBoxItem = comboBox1.SelectedItem as ComboBoxItem;

                            if (selectedComboBoxItem != null)
                            {
                                int hayvanID = selectedComboBoxItem.HayvanID;

                                string ilişkilendirmeQuery = "INSERT INTO HayvanAsi (HayvanID, AsiTakipID) VALUES (@HayvanID, @AsiTakipID)";
                                using (SqliteCommand ilişkilendirmeCommand = new SqliteCommand(ilişkilendirmeQuery, connection))
                                {
                                    ilişkilendirmeCommand.Parameters.AddWithValue("@HayvanID", hayvanID);
                                    ilişkilendirmeCommand.Parameters.AddWithValue("@AsiTakipID", asiTakipID);
                                    ilişkilendirmeCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        connection.Close();
                    }
                }

                LoadAşıData();
                textBox1.Clear();
                maskedTextBox1.Clear();
                maskedTextBox2.Clear();
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

            // Seçilen satırın AsiTakipID'sini al
            int asiTakipID = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["ID"].Value);

            // Yeni hayvan seçimini al
            ComboBoxItem selectedComboBoxItem = comboBox1.SelectedItem as ComboBoxItem;
            if (selectedComboBoxItem == null)
            {
                MessageBox.Show("Lütfen bir hayvan seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int yeniHayvanID = selectedComboBoxItem.HayvanID;

            // Aşı adı ve diğer bilgileri al
            string yeniAsiAdi = textBox1.Text;
            string yeniAsiTarihi = maskedTextBox1.Text;
            string yeniAsiTekrarTarihi = maskedTextBox2.Text;

            // Veriyi kullanıcıya onaylat
            string message = $"Seçilen aşının bilgilerini güncellemek istiyor musunuz?";
            DialogResult result = MessageBox.Show(message, "Bilgi Güncelleme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    // Mevcut ilişkilendirmeyi kaldır
                    string removeRelationQuery = "DELETE FROM HayvanAsi WHERE AsiTakipID = @AsiTakipID";
                    using (SqliteCommand removeRelationCommand = new SqliteCommand(removeRelationQuery, connection))
                    {
                        removeRelationCommand.Parameters.AddWithValue("@AsiTakipID", asiTakipID);
                        removeRelationCommand.ExecuteNonQuery();
                    }

                    // Aşı adı ve diğer bilgileri güncelle
                    string updateQuery = "UPDATE Asilar SET AsiAdi = @YeniAsiAdi, AsiTarihi = @YeniAsiTarihi, AsiTekrarTarihi = @YeniAsiTekrarTarihi WHERE AsiTakipID = @AsiTakipID";
                    using (SqliteCommand updateCommand = new SqliteCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@YeniAsiAdi", yeniAsiAdi);
                        updateCommand.Parameters.AddWithValue("@YeniAsiTarihi", yeniAsiTarihi);
                        updateCommand.Parameters.AddWithValue("@YeniAsiTekrarTarihi", yeniAsiTekrarTarihi);
                        updateCommand.Parameters.AddWithValue("@AsiTakipID", asiTakipID);

                        updateCommand.ExecuteNonQuery();
                    }

                    // Yeni ilişkilendirmeyi ekle
                    string addRelationQuery = "INSERT INTO HayvanAsi (HayvanID, AsiTakipID) VALUES (@HayvanID, @AsiTakipID)";
                    using (SqliteCommand addRelationCommand = new SqliteCommand(addRelationQuery, connection))
                    {
                        addRelationCommand.Parameters.AddWithValue("@HayvanID", yeniHayvanID);
                        addRelationCommand.Parameters.AddWithValue("@AsiTakipID", asiTakipID);
                        addRelationCommand.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                LoadAşıData();
                textBox1.Clear();
                maskedTextBox1.Clear();
                maskedTextBox2.Clear();

                // ComboBox ve DataGridView'i temizle
                comboBox1.SelectedIndex = -1;
                dataGridView1.ClearSelection();
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

            if (selectedRowIndex == -1)
            {
                MessageBox.Show("Lütfen silmek istediğiniz bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int asiTakipID = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["ID"].Value);

            string message = $"Aşı Takip ID: {asiTakipID} olan kaydı silmek istiyor musunuz?";
            DialogResult result = MessageBox.Show(message, "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Asilar WHERE AsiTakipID = @AsiTakipID";
                    using (SqliteCommand deleteCommand = new SqliteCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@AsiTakipID", asiTakipID);
                        deleteCommand.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                LoadAşıData();
            }
        }

        public class ComboBoxItem
        {
            public int HayvanID { get; set; }
            public string DisplayText { get; set; }

            public override string ToString()
            {
                return DisplayText;
            }
        }

        private void AşıForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
        }
    }
}
