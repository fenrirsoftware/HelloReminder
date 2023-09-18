using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
    
namespace temizHCO
{
    public partial class Reminder : Form
    {
        private SqliteConnection connection;
        private string connectionString = "Data Source=HCO.db;";
        private DataGridView dataGridView1;
        private long chatId =1058087196; // Telegram chat ID'sini buraya ekleyin
        private string botApiToken = "6022733274:AAEo67PBt9cnSrffmtQ3DZRjUop6wgRExwk";

        public Reminder()
        {
            InitializeComponent();
            InitializeDataGridView();
            PopulateDataGridView();
            SendReminderMessages(); // Form yüklendiğinde otomatik olarak mesaj gönderme işlemi
        }

        private void InitializeDataGridView()
        {
            dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill;
            Controls.Add(dataGridView1);

            dataGridView1.AutoResizeColumns();
        }

        private void PopulateDataGridView()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("AsiAdi", typeof(string));
            dataTable.Columns.Add("AsiTarihi", typeof(DateTime));
            dataTable.Columns.Add("AsiTekrarTarihi", typeof(DateTime));
            dataTable.Columns.Add("AsiSeriNo",typeof(string));
            dataTable.Columns.Add("PasaportNumarasi", typeof(string));
            dataTable.Columns.Add("CipNumarasi", typeof(string));
            dataTable.Columns.Add("HayvanAdi", typeof(string));
            dataTable.Columns.Add("SahipAdi", typeof(string));
            dataTable.Columns.Add("SahipSoyadi", typeof(string));
            dataTable.Columns.Add("SahipTelefon", typeof(string));
            dataTable.Columns.Add("SahipTCKimlik", typeof(string));

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT a.AsiAdi, a.AsiTarihi, a.AsiTekrarTarihi,AsiSeriNo,
                           h.PasaportNumarasi, h.CipNumarasi, h.Ad AS HayvanAdi,
                           hs.Ad AS SahipAdi, hs.Soyad AS SahipSoyadi, hs.TelefonNumarasi AS SahipTelefon, hs.TCKimlik AS SahipTCKimlik
                    FROM Asilar AS a
                    INNER JOIN HayvanAsi AS ha ON a.AsiTakipID = ha.AsiTakipID
                    INNER JOIN Hayvanlar AS h ON ha.HayvanID = h.HayvanID
                    INNER JOIN HayvanHastaSahipleri AS hhs ON h.HayvanID = hhs.HayvanID
                    INNER JOIN HastaSahipleri AS hs ON hhs.SahipID = hs.SahipID
                    WHERE julianday(a.AsiTekrarTarihi) - julianday('now') < 7;
                ";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
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
                            reader["SahipTCKimlik"]
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
                Console.WriteLine($"Mesaj gönderme hatası: {ex.Message}");
            }
        }


        private List<string> PrepareMessages(DataTable dataTable)
        {

            List<string> messages = new List<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                string phoneNumber = row["SahipTelefon"].ToString(); // Telefon numarasını alın

                // Telefon numarasını kullanarak bir Inline Keyboard Button oluşturun
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

        private void Reminder_Load(object sender, EventArgs e)
        {
            // Load event handler kodu buraya gelecek
        }

        private void Reminder_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
        }
    }
}
