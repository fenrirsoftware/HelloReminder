using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class AşıForm : Form
    {
        private string connectionString = ("server=.; Initial Catalog=HcoDb;Integrated Security=SSPI");
        private int selectedAsiTakipID = -1;

        public AşıForm()
        {
            InitializeComponent();
        }

        private void AşıForm_Load(object sender, EventArgs e)
        {
            LoadAşıData();
            LoadHayvanComboBox();
        }

        private void LoadAşıData()
        {
            dataGridView1.Rows.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT Asilar.AsiTakipID, Asilar.AsiAdi, Asilar.AsiTarihi, Asilar.AsiTekrarTarihi, 
                                Asilar.AsiSeriNo, Hayvanlar.Ad AS HayvanAdi, Hayvanlar.PasaportNumarasi
                         FROM Asilar
                         LEFT JOIN HayvanAsi ON Asilar.AsiTakipID = HayvanAsi.AsiTakipID
                         LEFT JOIN Hayvanlar ON HayvanAsi.HayvanID = Hayvanlar.HayvanID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                        int asiTakipID = reader.GetInt32(0);
string asiAdi = reader.GetString(1);
DateTime asiTarihi = reader.GetDateTime(2); // DateTime olarak al
DateTime asiTekrarTarihi = reader.GetDateTime(3); // DateTime olarak al
string hayvanAdi = reader.IsDBNull(5) ? "" : reader.GetString(5);
string pasaportNumarasi = reader.IsDBNull(6) ? "" : reader.GetString(6);
string asiSeriNo = reader.IsDBNull(4) ? "" : reader.GetString(4);

// DataGridView'e eklerken DateTime verilerini uygun bir biçimde biçimlendirin
dataGridView1.Rows.Add(asiTakipID, asiAdi, asiTarihi.ToShortDateString(), asiTekrarTarihi.ToShortDateString(), hayvanAdi, pasaportNumarasi, asiSeriNo);

                        }
                    }
                }

                connection.Close();
            }
        }

        private void LoadHayvanComboBox()
        {
            comboBox1.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT HayvanID, Ad, PasaportNumarasi FROM Hayvanlar";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
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
                dateTimePicker1.Text = selectedRow.Cells["ABT"].Value.ToString();
                dateTimePicker2.Text = selectedRow.Cells["ATT"].Value.ToString();
                textBox2.Text = selectedRow.Cells["AsiSeriNo"].Value.ToString();
                comboBox1.Text = "";

                comboBox1.Items.Clear();
                LoadHayvanComboBox();

                selectedAsiTakipID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT HayvanAsi.HayvanID FROM HayvanAsi " +
                                   "WHERE HayvanAsi.AsiTakipID = @AsiTakipID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AsiTakipID", selectedAsiTakipID);

                        using (SqlDataReader reader = command.ExecuteReader())
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
            DateTime baslangic = dateTimePicker1.Value;
            DateTime tekerrür = dateTimePicker2.Value;
            string asiSeriNo = textBox2.Text;

            string message = $"Eklemek istediğiniz Aşı bilgileri:\n\nAd: {isim}\nBaşlangıç: {baslangic}\nTekerrür: {tekerrür}\nSeri No: {asiSeriNo}\n\n\nBu veriyi eklemek istiyor musunuz?";
            DialogResult result = MessageBox.Show(message, "Aşı Ekleme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                long lastInsertedAsiTakipID = -1;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Asilar (AsiAdi, AsiTarihi, AsiTekrarTarihi, AsiSeriNo) VALUES (@AsiAdi, @AsiTarihi, @AsiTekrarTarihi, @AsiSeriNo)";
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@AsiAdi", isim);
                        insertCommand.Parameters.AddWithValue("@AsiTarihi", baslangic);
                        insertCommand.Parameters.AddWithValue("@AsiTekrarTarihi", tekerrür);
                        insertCommand.Parameters.AddWithValue("@AsiSeriNo", asiSeriNo);
                        insertCommand.ExecuteNonQuery();

                        // Son eklenen AsiTakipID değerini almak için sorguyu kullan
                        string selectLastInsertIDQuery = "SELECT IDENT_CURRENT('Asilar')"; // 'Asilar' tablo adınıza uygun olacak
                        using (SqlCommand selectLastInsertIDCommand = new SqlCommand(selectLastInsertIDQuery, connection))
                        {
                            lastInsertedAsiTakipID = Convert.ToInt32(selectLastInsertIDCommand.ExecuteScalar());
                        }
                    }

                    ComboBoxItem selectedComboBoxItem = comboBox1.SelectedItem as ComboBoxItem;
                    if (selectedComboBoxItem != null)
                    {
                        int hayvanID = selectedComboBoxItem.HayvanID;

                        string ilişkilendirmeQuery = "INSERT INTO HayvanAsi (HayvanID, AsiTakipID) VALUES (@HayvanID, @AsiTakipID)";
                        using (SqlCommand ilişkilendirmeCommand = new SqlCommand(ilişkilendirmeQuery, connection))
                        {
                            ilişkilendirmeCommand.Parameters.AddWithValue("@HayvanID", hayvanID);
                            ilişkilendirmeCommand.Parameters.AddWithValue("@AsiTakipID", lastInsertedAsiTakipID);
                            ilişkilendirmeCommand.ExecuteNonQuery();
                        }
                    }

                    connection.Close();
                }

                LoadAşıData();
                textBox1.Clear();
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
                textBox2.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

            if (selectedRowIndex == -1)
            {
                MessageBox.Show("Lütfen düzenlemek istediğiniz bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int asiTakipID = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["ID"].Value);

            ComboBoxItem selectedComboBoxItem = comboBox1.SelectedItem as ComboBoxItem;
            if (selectedComboBoxItem == null)
            {
                MessageBox.Show("Lütfen bir hayvan seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int yeniHayvanID = selectedComboBoxItem.HayvanID;

            string yeniAsiAdi = textBox1.Text;
            DateTime yeniAsiTarihi = dateTimePicker1.Value; // Tarih bilgisini DateTime olarak al
            DateTime yeniAsiTekrarTarihi = dateTimePicker2.Value; // Tarih bilgisini DateTime olarak al
            string yeniAsiSeriNo = textBox2.Text;

            string message = $"Seçilen aşının bilgilerini güncellemek istiyor musunuz?";
            DialogResult result = MessageBox.Show(message, "Bilgi Güncelleme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string removeRelationQuery = "DELETE FROM HayvanAsi WHERE AsiTakipID = @AsiTakipID";
                    using (SqlCommand removeRelationCommand = new SqlCommand(removeRelationQuery, connection))
                    {
                        removeRelationCommand.Parameters.AddWithValue("@AsiTakipID", asiTakipID);
                        removeRelationCommand.ExecuteNonQuery();
                    }

                    string updateQuery = "UPDATE Asilar SET AsiAdi = @YeniAsiAdi, AsiTarihi = @YeniAsiTarihi, AsiTekrarTarihi = @YeniAsiTekrarTarihi, AsiSeriNo = @YeniAsiSeriNo WHERE AsiTakipID = @AsiTakipID";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@YeniAsiAdi", yeniAsiAdi);
                        updateCommand.Parameters.AddWithValue("@YeniAsiTarihi", yeniAsiTarihi);
                        updateCommand.Parameters.AddWithValue("@YeniAsiTekrarTarihi", yeniAsiTekrarTarihi);
                        updateCommand.Parameters.AddWithValue("@YeniAsiSeriNo", yeniAsiSeriNo);
                        updateCommand.Parameters.AddWithValue("@AsiTakipID", asiTakipID);

                        updateCommand.ExecuteNonQuery();
                    }

                    string addRelationQuery = "INSERT INTO HayvanAsi (HayvanID, AsiTakipID) VALUES (@HayvanID, @AsiTakipID)";
                    using (SqlCommand addRelationCommand = new SqlCommand(addRelationQuery, connection))
                    {
                        addRelationCommand.Parameters.AddWithValue("@HayvanID", yeniHayvanID);
                        addRelationCommand.Parameters.AddWithValue("@AsiTakipID", asiTakipID);
                        addRelationCommand.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                LoadAşıData();
                textBox1.Clear();
                textBox2.Clear();

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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Önce aşı-hayvan ilişkisini kaldır
                    string deleteHayvanAsiQuery = "DELETE FROM HayvanAsi WHERE AsiTakipID = @AsiTakipID";
                    using (SqlCommand deleteHayvanAsiCommand = new SqlCommand(deleteHayvanAsiQuery, connection))
                    {
                        deleteHayvanAsiCommand.Parameters.AddWithValue("@AsiTakipID", asiTakipID);
                        deleteHayvanAsiCommand.ExecuteNonQuery();
                    }

                    // Sonra aşı kaydını sil
                    string deleteQuery = "DELETE FROM Asilar WHERE AsiTakipID = @AsiTakipID";
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox3.Text.Trim(); // TextBox'tan metni alın

            // SqlConnection ve SqlCommand kullanarak sorguyu çalıştırın
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
            SELECT 
                Asilar.AsiTakipID AS ID,
                Asilar.AsiAdi AS [Aşı Adı],
                Asilar.AsiTarihi AS [Aşı Tarihi],
                Asilar.AsiTekrarTarihi AS [Aşı Tekrar Tarihi],
                Asilar.AsiSeriNo AS [Aşı Seri No],
                Hayvanlar.Ad AS [Hayvan Adı],
                Hayvanlar.PasaportNumarasi AS [Hayvan Pasaport No]
            FROM
                Asilar
            LEFT JOIN
                HayvanAsi ON Asilar.AsiTakipID = HayvanAsi.AsiTakipID
            LEFT JOIN
                Hayvanlar ON HayvanAsi.HayvanID = Hayvanlar.HayvanID
            WHERE
                Asilar.AsiAdi LIKE @SearchText + '%' OR
                Asilar.AsiSeriNo LIKE @SearchText + '%' OR
                Hayvanlar.Ad LIKE @SearchText + '%' OR
                Hayvanlar.PasaportNumarasi LIKE @SearchText + '%'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchText", searchText);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataGridView1.Rows.Clear(); // DataGridView'i temizle

                        while (reader.Read())
                        {
                            int asiTakipID = reader.GetInt32(reader.GetOrdinal("ID"));
                            string asiAdi = reader.GetString(reader.GetOrdinal("Aşı Adı"));
                            DateTime asiTarihi = reader.GetDateTime(reader.GetOrdinal("Aşı Tarihi"));
                            DateTime asiTekrarTarihi = reader.GetDateTime(reader.GetOrdinal("Aşı Tekrar Tarihi"));
                            string asiSeriNo = reader.GetString(reader.GetOrdinal("Aşı Seri No"));
                            string hayvanAdi = reader.IsDBNull(reader.GetOrdinal("Hayvan Adı")) ? "" : reader.GetString(reader.GetOrdinal("Hayvan Adı"));
                            string pasaportNumarasi = reader.IsDBNull(reader.GetOrdinal("Hayvan Pasaport No")) ? "" : reader.GetString(reader.GetOrdinal("Hayvan Pasaport No"));

                            // Sonuçları DataGridView'e ekleyin
                            dataGridView1.Rows.Add(asiTakipID, asiAdi, asiTarihi.ToShortDateString(), asiTekrarTarihi.ToShortDateString(), asiSeriNo, hayvanAdi, pasaportNumarasi);
                        }
                    }
                }

                connection.Close();
            }
        }

    }
}
