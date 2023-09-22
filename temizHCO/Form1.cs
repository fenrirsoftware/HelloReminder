using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class Form1 : Form
    {
        private string connectionString =("server=.; Initial Catalog=HcoDb;Integrated Security=SSPI");
        private int animationSpeed = 50;

        public Form1()
        {
            InitializeComponent();
            InitializeTimers();
        }

        private void InitializeTimers()
        {
            InitializeTimer(label1, GetHastaSahibiSayisi());
            InitializeTimer(label2, GetHayvanSayisi());
            InitializeTimer(label3, GetAsiSayisi());
        }

        private void InitializeTimer(Label label, int targetValue)
        {
            Timer timer = new Timer();
            timer.Interval = animationSpeed;
            timer.Tick += (sender, e) => UpdateValue(label, targetValue, timer);
            timer.Start();
        }

        private void UpdateValue(Label label, int targetValue, Timer timer)
        {
            int currentValue = Convert.ToInt32(label.Text);

            if (currentValue < targetValue)
            {
                currentValue++;
                label.Text = currentValue.ToString();
            }
            else
            {
                timer.Stop();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int hastaSahibiSayisi = GetHastaSahibiSayisi();
            int hayvanSayisi = GetHayvanSayisi();
            int asiSayisi = GetAsiSayisi();

            label1.Text = $"{hastaSahibiSayisi}";
            label2.Text = $"{hayvanSayisi}";
            label3.Text = $"{asiSayisi}";
        }

        private int GetHastaSahibiSayisi()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM HastaSahipleri";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return count;
        }

        private int GetHayvanSayisi()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Hayvanlar";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return count;
        }

        private int GetAsiSayisi()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Asilar";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return count;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            HastaSahibiForm hsb = new HastaSahibiForm();
            hsb.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            HastaForm h = new HastaForm();
            h.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AşıForm a = new AşıForm();
            a.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Görüntüleme g = new Görüntüleme();
            g.Show();
            this.Hide();
        }
        string connection1String = "server=.; Initial Catalog=HcoDb; Integrated Security=SSPI"; 
        string outputDirectory = Path.Combine(Application.StartupPath, "Files");
        private void Form1_Load_1(object sender, EventArgs e)
        {
            acilis a = new acilis();
            a.Close();


            try
            {
                // Metin dosyasının kaydedileceği klasörün yolu


                // Klasörü oluştur (varsa zaten oluşturulmuş olabilir)
                Directory.CreateDirectory(outputDirectory);

                using (SqlConnection connection = new SqlConnection(connection1String))
                {
                    connection.Open();

                    // SQL sorgusu
                    string query = @"
                -- Tüm hayvanlar, aşılar ve sahipleri için ilişkileri ve verileri al
                SELECT 
                    H.HayvanID, H.Ad AS HayvanAdi, H.Tur AS HayvanTuru,
                    A.AsiTakipID, A.AsiAdi AS AsiAdi, A.AsiTarihi, A.AsiTekrarTarihi, A.AsiSeriNo,
                    S.SahipID, S.Ad AS SahipAdi, S.Soyad AS SahipSoyadi, S.TCKimlik, S.TelefonNumarasi
                FROM Hayvanlar H
                JOIN HayvanHastaSahipleri HS ON H.HayvanID = HS.HayvanID
                JOIN HastaSahipleri S ON HS.SahipID = S.SahipID
                JOIN HayvanAsi HA ON H.HayvanID = HA.HayvanID
                JOIN Asilar A ON HA.AsiTakipID = A.AsiTakipID
            ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Verileri metin dosyasına yazma
                            string outputFilePath = Path.Combine(outputDirectory, "veriler.txt");
                            using (StreamWriter writer = new StreamWriter(outputFilePath))
                            {
                                while (reader.Read())
                                {
                                    writer.WriteLine($"Hayvan ID: {reader["HayvanID"]}, Hayvan Adı: {reader["HayvanAdi"]}, Hayvan Türü: {reader["HayvanTuru"]}");
                                    writer.WriteLine($"Asi ID: {reader["AsiTakipID"]}, Asi Adı: {reader["AsiAdi"]}, Asi Tarihi: {reader["AsiTarihi"]}, Asi Tekrar Tarihi: {reader["AsiTekrarTarihi"]}, Asi Seri No: {reader["AsiSeriNo"]}");
                                    writer.WriteLine($"Sahip ID: {reader["SahipID"]}, Sahip Adı: {reader["SahipAdi"]}, Sahip Soyadı: {reader["SahipSoyadi"]}, TCKimlik: {reader["TCKimlik"]}, Telefon Numarası: {reader["TelefonNumarasi"]}");
                                    writer.WriteLine("---------------------------------------------------------");
                                }
                            }
                        }
                    }

                    connection.Close();
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        private void button6_Click(object sender, EventArgs e)
        {
            Reminder r = new Reminder();
            r.Show();
            this.Hide();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

      
    }
}
