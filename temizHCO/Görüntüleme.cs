using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class Görüntüleme : Form
    {
        public SqlConnection connection;
        private string connectionString = ("server=.; Initial Catalog=HcoDb;Integrated Security=SSPI");

        private FlowLayoutPanel flowLayoutPanel;
        private int buttonHeight = 80;

        public Görüntüleme()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(400, 400);

            flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.AutoScroll = true;
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;

            this.Controls.Add(flowLayoutPanel);

            connection = new SqlConnection(connectionString);

            try
            {
                connection.Open(); // Bağlantıyı açık tutun
                VeritabanindanHastaSahipleriniAlVeButtonlariOlustur();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantısında bir hata oluştu: " + ex.Message);
            }
        }

        private void VeritabanindanHastaSahipleriniAlVeButtonlariOlustur()
        {
            string query = "SELECT Ad, Soyad FROM HastaSahipleri";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string ad = reader["Ad"].ToString();
                    string soyad = reader["Soyad"].ToString();

                    Button button = new Button();
                    button.Text = ad + " " + soyad;
                    button.Width = 80;
                    button.Height = buttonHeight;

                    button.Click += (sender, e) =>
                    {
                        string sahipAdSoyad = ((Button)sender).Text;
                        string[] sahipAdSoyadParts = sahipAdSoyad.Split(' ');

                        int sahipID = GetSahipID(sahipAdSoyadParts[0], sahipAdSoyadParts[1]);

                        if (sahipID != -1)
                        {
                            List<int> hayvanIDList = GetHayvanlarBySahipID(sahipID);

                            if (hayvanIDList.Count > 0)
                            {
                                int secilenHayvanID = ShowHayvanSecimForm(hayvanIDList);

                                if (secilenHayvanID != -1)
                                {
                                   
                                }
                                else
                                {
                                    MessageBox.Show("Hayvan seçimi iptal edildi.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Bu sahibe ait kayıtlı hayvan bulunmuyor.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Sahip bulunamadı.");
                        }
                    };

                    flowLayoutPanel.Controls.Add(button);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hasta sahipleri alınırken bir hata oluştu: " + ex.Message);
            }
        }

        private int GetSahipID(string ad, string soyad)
        {
            int sahipID = -1;
            string sahipQuery = "SELECT SahipID FROM HastaSahipleri WHERE Ad = @Ad AND Soyad = @Soyad";

            using (SqlCommand sahipCommand = new SqlCommand(sahipQuery, connection))
            {
                sahipCommand.Parameters.AddWithValue("@Ad", ad);
                sahipCommand.Parameters.AddWithValue("@Soyad", soyad);

                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = sahipCommand.ExecuteReader())
                    {
                        if (reader.Read()) // Veri bulunursa
                        {
                            sahipID = Convert.ToInt32(reader["SahipID"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sahip ID'si alınırken bir hata oluştu: " + ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return sahipID;
        }



        private List<int> GetHayvanlarBySahipID(int sahipID)
        {
            List<int> hayvanIDList = new List<int>();
            string hayvanQuery = "SELECT HayvanID FROM HayvanHastaSahipleri WHERE SahipID = @SahipID";

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open(); // Bağlantıyı açın (eğer kapalı ise)
                }

                using (SqlCommand hayvanCommand = new SqlCommand(hayvanQuery, connection))
                {
                    hayvanCommand.Parameters.AddWithValue("@SahipID", sahipID);

                    using (SqlDataReader reader = hayvanCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            hayvanIDList.Add(Convert.ToInt32(reader["HayvanID"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hayvanlar alınırken bir hata oluştu: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close(); // Bağlantıyı kapatın (eğer açık ise)
                }
            }

            return hayvanIDList;
        }




        private int ShowHayvanSecimForm(List<int> hayvanIDList)
        {
            int secilenHayvanID = -1;

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

            foreach (int hayvanID in hayvanIDList)
            {
                string hayvanAdi = GetHayvanAdiByID(hayvanID);
                comboBox.Items.Add(hayvanAdi);
            }
            secimForm.Controls.Add(comboBox);

            Button button = new Button();
            button.Text = "Seç";
            button.Location = new System.Drawing.Point(10, 70);
            button.Click += (s, ev) =>
            {
                if (comboBox.SelectedItem != null)
                {
                    secilenHayvanID = GetHayvanIDByAdi(comboBox.SelectedItem.ToString());
                    secimForm.Close();
                }
                else
                {
                    MessageBox.Show("Lütfen bir hayvan seçin.");
                }
            };
            secimForm.Controls.Add(button);

            secimForm.ShowDialog();

            return secilenHayvanID;
        }

        private string GetHayvanAdiByID(int hayvanID)
        {

            string hayvanAdi = "";
            string hayvanAdiQuery = "SELECT Ad FROM Hayvanlar WHERE HayvanID = @HayvanID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Bağlantıyı açın

                    using (SqlCommand hayvanAdiCommand = new SqlCommand(hayvanAdiQuery, connection))
                    {
                        hayvanAdiCommand.Parameters.AddWithValue("@HayvanID", hayvanID);

                        object hayvanAdiResult = hayvanAdiCommand.ExecuteScalar();

                        if (hayvanAdiResult != null)
                        {
                            hayvanAdi = hayvanAdiResult.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hayvan adı alınırken bir hata oluştu: " + ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close(); // Bağlantıyı kapatın (eğer açık ise)
                    }
                }
            }

            return hayvanAdi;
        }



        private int GetHayvanIDByAdi(string hayvanAdi)
        {
            int hayvanID = -1;
            string hayvanIDQuery = "SELECT HayvanID FROM Hayvanlar WHERE Ad = @Ad";

            using (SqlConnection newConnection = new SqlConnection(connectionString))
            {
                newConnection.Open(); // Yeni bir bağlantı açın

                using (SqlCommand hayvanIDCommand = new SqlCommand(hayvanIDQuery, newConnection))
                {
                    hayvanIDCommand.Parameters.AddWithValue("@Ad", hayvanAdi);

                    try
                    {
                        object hayvanIDResult = hayvanIDCommand.ExecuteScalar();

                        if (hayvanIDResult != null)
                        {
                            hayvanID = Convert.ToInt32(hayvanIDResult);
                            HızlıDüzenlemeForm fr = new HızlıDüzenlemeForm();
                            fr.veri = hayvanID.ToString();
                            this.Hide();
                            fr.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hayvan ID'si alınırken bir hata oluştu: " + ex.Message);
                    }
                }

                newConnection.Close(); // Yeni bağlantıyı kapatın
            }

            return hayvanID;
        }

        private void Görüntüleme_Load(object sender, EventArgs e)
        {
            // Diğer kodlar burada...
        }

        private void Görüntüleme_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            Arama_Form af = new Arama_Form();
            this.Hide();
            af.Show();
          
        }

        private void Görüntüleme_Load_1(object sender, EventArgs e)
        {

        }

        // Diğer olaylar ve metotlar
    }
}
