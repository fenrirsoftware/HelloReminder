using Microsoft.Data.Sqlite;
using System;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=HCO.db;";
        private int animationSpeed = 50;

        public Form1()
        {
            InitializeComponent();
            SQLitePCL.Batteries.Init();
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
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM HastaSahipleri";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
                connection.Close();
            }
            return count;
        }

        private int GetHayvanSayisi()
        {
            int count = 0;
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Hayvanlar";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
                connection.Close();
            }
            return count;
        }

        private int GetAsiSayisi()
        {
            int count = 0;
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Asilar";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
                connection.Close();
            }
            return count;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            HastaSahibiForm hsb = new HastaSahibiForm();
            hsb.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            HastaForm h = new HastaForm();
            h.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AşıForm a = new AşıForm();
            a.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Görüntüleme g = new Görüntüleme();
            g.Show();
        }
    }
}
