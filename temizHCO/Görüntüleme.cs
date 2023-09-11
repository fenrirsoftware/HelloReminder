using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class Görüntüleme : Form
    {
        public SqliteConnection connection;
        private string connectionString = "Data Source=HCO.db;"; // Veritabanı bağlantı dizesini buraya ekleyin

        private FlowLayoutPanel flowLayoutPanel;






        private int buttonHeight = 80;
        private int buttonsPerRow = 4;
        private int rowCount = 0;
        private int maxRowCount = 0;

        public Görüntüleme()
        {
            InitializeComponent();
            // Form özelliklerini ayarlayın
            this.Size = new System.Drawing.Size(400, 400);



            // FlowLayoutPanel oluşturun ve ayarlarını yapın
            flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.AutoScroll = true;
            flowLayoutPanel.Dock = DockStyle.Fill; // Formun tüm alanını kaplayacak şekilde ayarlayın
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight; // Öğeleri sola doğru sırala

            // Forma "Arama" butonunu ve FlowLayoutPanel'i ekleyin
         
            this.Controls.Add(flowLayoutPanel);

            // Veritabanı bağlantısını açın
            connection = new SqliteConnection(connectionString);

            try
            {
                connection.Open();
                // Veritabanından hasta sahiplerini alıp buttonları oluşturun
                VeritabanindanHastaSahipleriniAlVeButtonlariOlustur();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantısında bir hata oluştu: " + ex.Message);
            }
            finally
            {
                // Veritabanı bağlantısını kapatın
                connection.Close();
            }
        }





        private void VeritabanindanHastaSahipleriniAlVeButtonlariOlustur()
        {
          

            string query = "SELECT Ad, Soyad FROM HastaSahipleri"; // Hasta sahiplerini almak için SQL sorgusu

            SqliteCommand command = new SqliteCommand(query, connection);

            try
            {
                connection.Open();
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string ad = reader["Ad"].ToString();
                    string soyad = reader["Soyad"].ToString();

                    // Hasta sahibinin adını kullanarak bir buton oluşturun
                    Button button = new Button();
                    button.Text = ad + " " + soyad;
                    button.Width = 80;
                    button.Height = buttonHeight;

                    // Butonun Click olayını tanımlayın (istersek her butonun farklı bir işlem yapmasını sağlayabiliriz)
                    button.Click += (sender, e) => {
                        // Butona tıklandığında yapılacak işlemleri buraya ekleyebilirsiniz
                        MessageBox.Show("Hasta Sahibi: " + ad + " " + soyad);
                    };

                    // FlowLayoutPanel'e butonu ekleyin (arama butonundan sonra)
                    flowLayoutPanel.Controls.Add(button);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hasta sahipleri alınırken bir hata oluştu: " + ex.Message);
            }
            finally
            {
                // Veritabanı bağlantısını kapatın
                connection.Close();
            }
        }



        

        private void Görüntüleme_Load(object sender, EventArgs e)
        {
            // Diğer kodlar burada...
        }

        private void Görüntüleme_Load_1(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            // "AramaFormu" adlı formu oluşturun ve açın
            Arama_Form af = new Arama_Form();
            af.Show(); // veya ShowDialog() kullanabilirsiniz
        }
    }
}
