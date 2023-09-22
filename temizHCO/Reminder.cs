using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace temizHCO
{
    public partial class Reminder : Form
    {
        private SqlConnection connection;
        private string connectionString = ("server=.; Initial Catalog=HcoDb;Integrated Security=SSPI");
        private DataGridView dataGridView1;
        private long chatId = 1058087196; // Telegram chat ID'sini buraya ekleyin
        private string botApiToken = "6022733274:AAEo67PBt9cnSrffmtQ3DZRjUop6wgRExwk";

        public Reminder()
        {
            InitializeComponent();
            InitializeDataGridView();
            PopulateDataGridView();
            SendReminderMessages();
        }

        private void InitializeDataGridView()
        {
            dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 12);
        Controls.Add(dataGridView1);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // DataGridView'e Kalan Gün sütunu ekleyin

            dataGridView1.AutoResizeColumns();
        }

        private void PopulateDataGridView()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("AsiAdi", typeof(string));
            dataTable.Columns.Add("AsiTarihi", typeof(DateTime));
            dataTable.Columns.Add("AsiTekrarTarihi", typeof(DateTime));
            dataTable.Columns.Add("AsiSeriNo", typeof(string));
            dataTable.Columns.Add("PasaportNumarasi", typeof(string));
            dataTable.Columns.Add("CipNumarasi", typeof(string));
            dataTable.Columns.Add("HayvanAdi", typeof(string));
            dataTable.Columns.Add("SahipAdi", typeof(string));
            dataTable.Columns.Add("SahipSoyadi", typeof(string));
            dataTable.Columns.Add("SahipTelefon", typeof(string));
            dataTable.Columns.Add("SahipTCKimlik", typeof(string));
            dataTable.Columns.Add("KalanGun", typeof(int)); // Kalan Gün sütunu

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT a.AsiAdi, a.AsiTarihi, a.AsiTekrarTarihi, AsiSeriNo,
                           h.PasaportNumarasi, h.CipNumarasi, h.Ad AS HayvanAdi,
                           hs.Ad AS SahipAdi, hs.Soyad AS SahipSoyadi, hs.TelefonNumarasi AS SahipTelefon, hs.TCKimlik AS SahipTCKimlik,
                           DATEDIFF(DAY, GETDATE(), a.AsiTekrarTarihi) AS KalanGun
                    FROM Asilar AS a
                    INNER JOIN HayvanAsi AS ha ON a.AsiTakipID = ha.AsiTakipID
                    INNER JOIN Hayvanlar AS h ON ha.HayvanID = h.HayvanID
                    INNER JOIN HayvanHastaSahipleri AS hhs ON h.HayvanID = hhs.HayvanID
                    INNER JOIN HastaSahipleri AS hs ON hhs.SahipID = hs.SahipID
                    WHERE DATEDIFF(DAY, GETDATE(), a.AsiTekrarTarihi) BETWEEN 0 AND 7; -- 0-7 gün arası
                ";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int remainingDays = reader.GetInt32(reader.GetOrdinal("KalanGun"));
                        string rowColor = "White"; // Varsayılan renk

                        if (remainingDays <= 7 && remainingDays >= 5)
                            rowColor = "Yellow"; // Sarı
                        else if (remainingDays < 5 && remainingDays >= 2)
                            rowColor = "Orange"; // Turuncu
                        else if (remainingDays < 2)
                            rowColor = "Red"; // Kırmızı

                        DataGridViewRow row = new DataGridViewRow();
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.FromName(rowColor);

                        dataTable.Rows.Add(
                            reader["AsiAdi"],
                            reader.GetDateTime(reader.GetOrdinal("AsiTarihi")),
                            reader.GetDateTime(reader.GetOrdinal("AsiTekrarTarihi")),
                            reader["AsiSeriNo"],
                            reader["PasaportNumarasi"],
                            reader["CipNumarasi"],
                            reader["HayvanAdi"],
                            reader["SahipAdi"],
                            reader["SahipSoyadi"],
                            reader["SahipTelefon"],
                            reader["SahipTCKimlik"],
                            remainingDays // Kalan Gün sütunu
                        );
                    }
                }

                connection.Close();
            }

            dataGridView1.DataSource = dataTable;
        }

        private async void SendReminderMessages()
        {
            DataTable dataTable = (DataTable)dataGridView1.DataSource;
            List<string> messages = PrepareMessages(dataTable);

            try
            {
                var botClient = new TelegramBotClient(botApiToken);

                foreach (var message in messages)
                {
                    await botClient.SendTextMessageAsync(chatId, message, parseMode: ParseMode.Html);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mesaj gönderme hatası: {ex.Message}");
            }
        }

        private List<string> PrepareMessages(DataTable dataTable)
        {
            List<string> messages = new List<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                string phoneNumber = row["SahipTelefon"].ToString();

                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Aramayı Başlat", $"tel:{phoneNumber}")
                    }
                });

                string message = $"<b>{DateTime.Now.ToString("D")} Asi Bilgileri:</b>\n" +
                    $"<i>Asi Adı:</i> {row["AsiAdi"]}\n" +
                    $"<i>Asi Tarihi:</i> {((DateTime)row["AsiTarihi"]).ToShortDateString()}\n" +
                    $"<i>Asi Tekrar Tarihi:</i> {((DateTime)row["AsiTekrarTarihi"]).ToShortDateString()}\n" +
                    $"<i>Pasaport Numarası:</i> {row["PasaportNumarasi"]}\n" +
                    $"<i>Cip Numarası:</i> {row["CipNumarasi"]}\n" +
                    $"<i>Hayvan Adı:</i> {row["HayvanAdi"]}\n" +
                    $"<i>Sahip Adı:</i> {row["SahipAdi"]} {row["SahipSoyadi"]}\n" +
                    $"<i>Sahip Telefon:</i> <a href='tel:{phoneNumber}'>{phoneNumber}</a>\n" +
                    $"<i>Sahip TCKimlik:</i> {row["SahipTCKimlik"]}\n" +
                    "\n------------------------\n" +
                    $"<b>Kalan Gün Sayısı:</b> {((DateTime)row["AsiTekrarTarihi"] - DateTime.Now).Days} GÜN\n" +
                    "\n------------------------\n";

                messages.Add(message);
            }

            return messages;
        }
        private void SetDataGridViewRowColors()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int remainingDays = Convert.ToInt32(row.Cells["KalanGun"].Value);
                string rowColor = "White"; // Varsayılan renk

                 


                if (remainingDays <= 7 && remainingDays >= 5) 
                { 
                    rowColor = "Yellow";

                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromName(rowColor);
                }
                else if (remainingDays < 5 && remainingDays >= 2) 
                { 
                    rowColor = "Orange"; // Turuncu

                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromName(rowColor);

                }
                else if (remainingDays < 2) 
                {
                
                    rowColor = "Red"; // Kırmızı

                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromName(rowColor);
                }

                else if (remainingDays < 0)
                {

                    rowColor = "White"; // Kırmızı

                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromName(rowColor);
                }






            }
        }

   


        private void Reminder_Load(object sender, EventArgs e)
        {
            // Load event handler kodu buraya gelecek
            SetDataGridViewRowColors(); // DataGridView satır renklerini ayarla
        }

       

        private void Reminder_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
        }
    }
}
