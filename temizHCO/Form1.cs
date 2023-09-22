using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Xml;

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
        //...

        private void Form1_Load_1(object sender, EventArgs e)
        {
         
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
