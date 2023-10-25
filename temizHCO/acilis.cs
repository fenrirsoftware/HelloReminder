using System;
using System.Windows.Forms;

namespace temizHCO
{
    public partial class acilis : Form
    {
        private Timer fadeTimer = new Timer();
        private double opacity = 1.0;

        public acilis()
        {
            InitializeComponent();

            fadeTimer.Interval = 100; // Her 50 milisaniyede bir çalışacak
            fadeTimer.Tick += FadeTimer_Tick;
        }

        private void acilis_Load_1(object sender, EventArgs e)
        {
            // Formu başlangıçta görünür yap
            this.Opacity = 1.0;

            // Zamanlayıcıyı başlat
            fadeTimer.Start();
        }

        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            // Opaklığı azalt
            opacity -= 0.05;
            if (opacity <= 0.0)
            {
                // Opaklık 0.0 olduğunda formu tamamen kapat
                fadeTimer.Stop(); // Zamanlayıcıyı durdur
                this.Hide();
                Form1 f = new Form1();
                f.Show();
                
            }
            else
            {
                this.Opacity = opacity;
            }
        }
    }
}
