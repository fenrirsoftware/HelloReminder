using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net.Mail;
using System.Net;
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

            SendReminderEmails();
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



        private void SendReminderEmails()
        {
            DataTable dataTable = (DataTable)dataGridView1.DataSource;

            foreach (DataRow row in dataTable.Rows)
            {
                string recipientEmail = row["SahipEmail"].ToString(); // Alıcı e-posta adresi
                string subject = "Hatırlatma Mesajı: " + row["AsiAdi"].ToString(); // E-posta konusu
                string message = "Merhaba,\n\n" +
                                 "Unutmayın, " + row["AsiAdi"].ToString() + " aşısı için " +
                                 "son tarih " + ((DateTime)row["AsiTarihi"]).ToShortDateString() + ". " +
                                 "Lütfen hatırlatmayı göz önünde bulundurun.\n\n" +
                                 "Saygılarımla,\n" +
                                 "Sizin Veteriner Hekiminiz";

                // E-posta gönderme işlemi
                SendEmail(recipientEmail, subject, message);
            }
        }

        private void SendEmail(string recipient, string subject, string message)
        {
            using (SmtpClient smtpClient = new SmtpClient("mail.fenrirsoftware.com"))
            {
                smtpClient.Port = 587; // E-posta sunucu portu (SSL kullanıyorsanız 465 veya 587 gibi olabilir)
                smtpClient.Credentials = new NetworkCredential("info@fenrirsoftware.com", "TheRedBaron37"); // Gönderen e-posta hesap bilgileri
                smtpClient.EnableSsl = true; // SSL kullanıyorsanız true, kullanmıyorsanız false

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("ozhan-yildirim@hotmail.com"); // Gönderen e-posta adresi
                    mail.To.Add(recipient); // Alıcı e-posta adresi
                    mail.Subject = subject; // E-posta konusu
                    mail.Body = message; // E-posta mesajı

                    try
                    {
                        smtpClient.Send(mail);
                        Console.WriteLine("E-posta gönderildi: " + recipient);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("E-posta gönderme hatası: " + ex.Message);
                    }
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
