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
        private VScrollBar vScrollBar;

        private int buttonHeight = 80;
        private int buttonsPerRow = 4;
        private int rowCount = 0;
        private int maxRowCount = 0;

        public Görüntüleme()
        {
            InitializeComponent();
            // Form özelliklerini ayarlayın
            this.Text = "Hasta Sahipleri ve Dikey ScrollBar Örneği";
            this.Size = new System.Drawing.Size(400, 400);

            // FlowLayoutPanel oluşturun ve ayarlarını yapın
            flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.AutoScroll = true;
            flowLayoutPanel.Dock = DockStyle.Fill; // Formun tüm alanını kaplayacak şekilde ayarlayın
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight; // Öğeleri sola doğru sırala

            // Dikey kaydırma çubuğu (VScrollBar) oluşturun ve ayarlarını yapın
      

            // Forma FlowLayoutPanel'i ve dikey kaydırma çubuğunu ekleyin
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

        private void VScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            // Scroll çubuğunun değeri değiştiğinde FlowLayoutPanel'ı kaydırın
            flowLayoutPanel.VerticalScroll.Value = vScrollBar.Value;
        }


        private void VeritabanindanHastaSahipleriniAlVeButtonlariOlustur()
        {
            string query = "SELECT Ad, Soyad FROM HastaSahipleri"; // İlgili sorguyu veritabanınıza uyarlayın
            SqliteCommand command = new SqliteCommand(query, connection);

            try
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string ad = reader["Ad"].ToString();
                    string soyad = reader["Soyad"].ToString();

                    // Button oluşturun ve hasta sahibinin adını atayın
                    Button button = new Button();
                    button.Text = ad + " " + soyad;
                    button.Width = 80;
                    button.Height = buttonHeight;

                    flowLayoutPanel.Controls.Add(button);
                }

                reader.Close();

                // Buttonlar eklenince scrollbar'ı ayarlayın
             
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hasta sahipleri alınırken bir hata oluştu: " + ex.Message);
            }
        }

     

        private void Görüntüleme_Load(object sender, EventArgs e)
        {

        }

       
    }
}
