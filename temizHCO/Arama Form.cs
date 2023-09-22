using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class Arama_Form : Form
    {
        public Arama_Form()
        {
            InitializeComponent();
            // Veritabanı bağlantısını açın
            connection = new SqlConnection(connectionString);
        }

        public SqlConnection connection;
        private string connectionString = ("server=.; Initial Catalog=HcoDb;Integrated Security=SSPI");

        private void Arama_Form_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                string belirliHayvanAdi = textBox1.Text.Trim();
                string query = $"SELECT Hayvanlar.HayvanID FROM Hayvanlar WHERE Hayvanlar.Ad LIKE '%{belirliHayvanAdi}%'";

                ExecuteQueryAndShowResult(query);
            }
            else if (radioButton2.Checked)
            {
                string tcKimlik = textBox1.Text.Trim();
                string query = "SELECT Hayvanlar.HayvanID, Hayvanlar.Ad AS HayvanAdi " +
                               "FROM HastaSahipleri " +
                               "INNER JOIN HayvanHastaSahipleri ON HastaSahipleri.SahipID = HayvanHastaSahipleri.SahipID " +
                               "INNER JOIN Hayvanlar ON HayvanHastaSahipleri.HayvanID = Hayvanlar.HayvanID " +
                               $"WHERE HastaSahipleri.TCKimlik = '{tcKimlik}'";

                List<int> hayvanIDList = new List<int>();
                List<string> hayvanAdList = new List<string>();

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int hayvanID = Convert.ToInt32(reader["HayvanID"]);
                        hayvanIDList.Add(hayvanID);

                        string hayvanAdi = reader["HayvanAdi"].ToString();
                        hayvanAdList.Add(hayvanAdi);
                    }

                    reader.Close();
                }

                int secilenHayvanID = -1;
                if (hayvanIDList.Count > 1)
                {
                    Form secimForm = new Form();
                    secimForm.StartPosition = FormStartPosition.CenterScreen;
                    secimForm.Text = "Hayvan Seçimi";
                    secimForm.Size = new System.Drawing.Size(300, 150);

                    Label label = new Label();
                    label.Text = "Lütfen bir hayvan seçin (Hayvan Adı):";
                    label.Location = new System.Drawing.Point(10, 10);
                    secimForm.Controls.Add(label);

                    ComboBox comboBox = new ComboBox();
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBox.Location = new System.Drawing.Point(10, 40);
                    foreach (string ad in hayvanAdList)
                    {
                        comboBox.Items.Add(ad);
                    }
                    secimForm.Controls.Add(comboBox);

                    Button button = new Button();
                    button.Text = "Seç";
                    button.Location = new System.Drawing.Point(10, 70);
                    button.Click += (s, ev) =>
                    {
                        if (comboBox.SelectedItem != null)
                        {
                            secilenHayvanID = hayvanIDList[hayvanAdList.IndexOf(comboBox.SelectedItem.ToString())];
                            secimForm.Close();
                        }
                        else
                        {
                            MessageBox.Show("Geçersiz Hayvan Seçimi. Lütfen geçerli bir seçenek seçin.");
                        }
                    };
                    secimForm.Controls.Add(button);

                    secimForm.ShowDialog();
                }
                else if (hayvanIDList.Count == 1)
                {
                    secilenHayvanID = hayvanIDList[0];
                }
                else
                {
                    MessageBox.Show("Sonuç bulunamadı.", "Sonuç Yok");
                }

                if (secilenHayvanID != -1)
                {
                    MessageBox.Show($"Seçilen Hayvan ID: {secilenHayvanID}", "Seçilen Hayvan");
                    HızlıDüzenlemeForm fr = new HızlıDüzenlemeForm();
                    fr.veri = secilenHayvanID.ToString();
                    fr.Show();
                }
            }
            else if (radioButton3.Checked)
            {
                string pasaportNumarasi = textBox1.Text.Trim();
                string query = $"SELECT Hayvanlar.HayvanID FROM Hayvanlar WHERE Hayvanlar.PasaportNumarasi = '{pasaportNumarasi}'";
                ExecuteQueryAndShowResult(query);
            }
            else if (radioButton4.Checked)
            {
                string cipNumarasi = textBox1.Text.Trim();
                string query = $"SELECT Hayvanlar.HayvanID FROM Hayvanlar WHERE Hayvanlar.CipNumarasi = '{cipNumarasi}'";
                ExecuteQueryAndShowResult(query);
            }
            else if (radioButton5.Checked)
            {
                string adsoyad = textBox1.Text.Trim();
                string query = $"SELECT Hayvanlar.HayvanID, Hayvanlar.Ad AS HayvanAdi, HastaSahipleri.Ad AS SahipAdi, HastaSahipleri.Soyad AS SahipSoyadi, " +
                               "HastaSahipleri.TCKimlik, HastaSahipleri.TelefonNumarasi, " +
                               "Asilar.AsiAdi, Asilar.AsiTarihi, Asilar.AsiTekrarTarihi " +
                               "FROM Hayvanlar " +
                               "INNER JOIN HayvanHastaSahipleri ON Hayvanlar.HayvanID = HayvanHastaSahipleri.HayvanID " +
                               "INNER JOIN HastaSahipleri ON HayvanHastaSahipleri.SahipID = HastaSahipleri.SahipID " +
                               "LEFT JOIN HayvanAsi ON Hayvanlar.HayvanID = HayvanAsi.HayvanID " +
                               "LEFT JOIN Asilar ON HayvanAsi.AsiTakipID = Asilar.AsiTakipID " +
                               $"WHERE HastaSahipleri.Ad LIKE '%{adsoyad}%' OR HastaSahipleri.Soyad LIKE '%{adsoyad}%' " +
                               $"OR (HastaSahipleri.Ad + ' ' + HastaSahipleri.Soyad) LIKE '%{adsoyad}%'";

                List<int> hayvanIDList = new List<int>();
                List<string> hayvanAdList = new List<string>();

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int hayvanID = Convert.ToInt32(reader["HayvanID"]);
                        hayvanIDList.Add(hayvanID);

                        string hayvanAdi = reader["HayvanAdi"].ToString();
                        hayvanAdList.Add(hayvanAdi);
                    }

                    reader.Close();
                }

                int secilenHayvanID = -1;
                if (hayvanIDList.Count > 1)
                {
                    Form secimForm = new Form();
                    secimForm.Text = "Hayvan Seçimi";
                    secimForm.StartPosition = FormStartPosition.CenterScreen;
                    secimForm.Size = new System.Drawing.Size(300, 150);

                    Label label = new Label();
                    label.Text = "Lütfen bir hayvan seçin:";
                    label.Location = new System.Drawing.Point(10, 10);
                    secimForm.Controls.Add(label);

                    ComboBox comboBox = new ComboBox();
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBox.Location = new System.Drawing.Point(10, 40);
                    foreach (string ad in hayvanAdList)
                    {
                        comboBox.Items.Add(ad);
                    }
                    secimForm.Controls.Add(comboBox);

                    Button button = new Button();
                    button.Text = "Seç";
                    button.Location = new System.Drawing.Point(10, 70);
                    button.Click += (s, ev) =>
                    {
                        if (comboBox.SelectedItem != null)
                        {
                            secilenHayvanID = hayvanIDList[hayvanAdList.IndexOf(comboBox.SelectedItem.ToString())];
                            secimForm.Close();
                        }
                        else
                        {
                            MessageBox.Show("Geçersiz Hayvan Seçimi. Lütfen geçerli bir seçenek seçin.");
                        }
                    };
                    secimForm.Controls.Add(button);

                    secimForm.ShowDialog();

                    if (comboBox.SelectedItem != null)
                    {
                        secilenHayvanID = hayvanIDList[hayvanAdList.IndexOf(comboBox.SelectedItem.ToString())];
                        secimForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Geçersiz Hayvan Seçimi. Lütfen geçerli bir seçenek seçin.");
                    }
                }
                else if (hayvanIDList.Count == 1)
                {
                    secilenHayvanID = hayvanIDList[0];
                }
                else
                {
                    MessageBox.Show("Sonuç bulunamadı.", "Sonuç Yok");
                }

                if (secilenHayvanID != -1)
                {
                    MessageBox.Show($"Seçilen Hayvan ID: {secilenHayvanID}", "Seçilen Hayvan");
                    HızlıDüzenlemeForm fr = new HızlıDüzenlemeForm();
                    fr.veri = secilenHayvanID.ToString();
                    fr.Show();
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir seçim yapınız.");
            }

        }

        private void ExecuteQueryAndShowResult(string query)
        {
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                string sonuc = "Hasta Sahibi ve Aşı Bilgileri:\n";

                while (reader.Read())
                {
                    string hayvanAdi = reader["HayvanID"].ToString();
                    sonuc += "Hayvan Adı: " + hayvanAdi + "\n";

                    HızlıDüzenlemeForm fr = new HızlıDüzenlemeForm();
                    fr.veri = hayvanAdi;
                    fr.Show();
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

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Arama_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            Görüntüleme g = new Görüntüleme();
            g.Show();
        }
    }
}
