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
        private DataGridView dataGridView1;


        public Reminder()
        {
            InitializeComponent();
            InitializeDataGridView();
            PopulateDataGridView();

           
        }
        /// <summary>
        /// reminder sorunlu sql sorgusu düzeltilecek.
        /// </summary>

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
                    SELECT a.AsiAdi, a.AsiTarihi, a.AsiTekrarTarihi,
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
    }
}
