namespace temizHCO
{
    partial class HastaForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hayvanad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pasaport = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Çip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Renk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Irk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Yaş = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cinsiyet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tür = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notlar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sahipad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sahipsoyad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TCKN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtnot = new System.Windows.Forms.TextBox();
            this.txtür = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtcins = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtyas = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textırk = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtrenk = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtcip = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtpass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtad = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(14, 517);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(132, 45);
            this.button3.TabIndex = 16;
            this.button3.Text = "Sil";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(152, 517);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 45);
            this.button2.TabIndex = 15;
            this.button2.Text = "Düzenle";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Hayvanad,
            this.Pasaport,
            this.Çip,
            this.Renk,
            this.Irk,
            this.Yaş,
            this.Cinsiyet,
            this.Tür,
            this.Notlar,
            this.Sahipad,
            this.Sahipsoyad,
            this.TCKN});
            this.dataGridView1.Location = new System.Drawing.Point(412, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(962, 535);
            this.dataGridView1.TabIndex = 14;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 45;
            // 
            // Hayvanad
            // 
            this.Hayvanad.HeaderText = "HayvanAd";
            this.Hayvanad.MinimumWidth = 6;
            this.Hayvanad.Name = "Hayvanad";
            this.Hayvanad.ReadOnly = true;
            this.Hayvanad.Width = 125;
            // 
            // Pasaport
            // 
            this.Pasaport.HeaderText = "Pasaport";
            this.Pasaport.MinimumWidth = 6;
            this.Pasaport.Name = "Pasaport";
            this.Pasaport.ReadOnly = true;
            this.Pasaport.Width = 125;
            // 
            // Çip
            // 
            this.Çip.HeaderText = "Çip";
            this.Çip.MinimumWidth = 6;
            this.Çip.Name = "Çip";
            this.Çip.ReadOnly = true;
            this.Çip.Width = 125;
            // 
            // Renk
            // 
            this.Renk.HeaderText = "Renk";
            this.Renk.MinimumWidth = 6;
            this.Renk.Name = "Renk";
            this.Renk.ReadOnly = true;
            this.Renk.Width = 125;
            // 
            // Irk
            // 
            this.Irk.HeaderText = "Irk";
            this.Irk.MinimumWidth = 6;
            this.Irk.Name = "Irk";
            this.Irk.ReadOnly = true;
            this.Irk.Width = 125;
            // 
            // Yaş
            // 
            this.Yaş.HeaderText = "Yaş";
            this.Yaş.MinimumWidth = 6;
            this.Yaş.Name = "Yaş";
            this.Yaş.ReadOnly = true;
            this.Yaş.Width = 45;
            // 
            // Cinsiyet
            // 
            this.Cinsiyet.HeaderText = "Cinsiyet";
            this.Cinsiyet.MinimumWidth = 6;
            this.Cinsiyet.Name = "Cinsiyet";
            this.Cinsiyet.ReadOnly = true;
            this.Cinsiyet.Width = 80;
            // 
            // Tür
            // 
            this.Tür.HeaderText = "Tür";
            this.Tür.MinimumWidth = 6;
            this.Tür.Name = "Tür";
            this.Tür.ReadOnly = true;
            this.Tür.Width = 125;
            // 
            // Notlar
            // 
            this.Notlar.HeaderText = "Notlar";
            this.Notlar.MinimumWidth = 6;
            this.Notlar.Name = "Notlar";
            this.Notlar.ReadOnly = true;
            this.Notlar.Width = 125;
            // 
            // Sahipad
            // 
            this.Sahipad.HeaderText = "Sahip Ad";
            this.Sahipad.MinimumWidth = 6;
            this.Sahipad.Name = "Sahipad";
            this.Sahipad.ReadOnly = true;
            this.Sahipad.Width = 125;
            // 
            // Sahipsoyad
            // 
            this.Sahipsoyad.HeaderText = "Sahip Soyad";
            this.Sahipsoyad.MinimumWidth = 6;
            this.Sahipsoyad.Name = "Sahipsoyad";
            this.Sahipsoyad.ReadOnly = true;
            this.Sahipsoyad.Width = 125;
            // 
            // TCKN
            // 
            this.TCKN.HeaderText = "TCKN";
            this.TCKN.MinimumWidth = 6;
            this.TCKN.Name = "TCKN";
            this.TCKN.ReadOnly = true;
            this.TCKN.Width = 125;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(273, 517);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 45);
            this.button1.TabIndex = 13;
            this.button1.Text = "Kaydet";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtnot);
            this.groupBox1.Controls.Add(this.txtür);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtcins);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtyas);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textırk);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtrenk);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtcip);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtpass);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtad);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(14, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(390, 502);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hasta Bilgi Formu";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 437);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 29);
            this.label10.TabIndex = 28;
            this.label10.Text = "Sahibi:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(173, 437);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(212, 37);
            this.comboBox1.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 380);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 29);
            this.label9.TabIndex = 26;
            this.label9.Text = "Notlar:";
            // 
            // txtnot
            // 
            this.txtnot.Location = new System.Drawing.Point(172, 372);
            this.txtnot.Multiline = true;
            this.txtnot.Name = "txtnot";
            this.txtnot.Size = new System.Drawing.Size(212, 58);
            this.txtnot.TabIndex = 24;
            // 
            // txtür
            // 
            this.txtür.Location = new System.Drawing.Point(173, 331);
            this.txtür.Name = "txtür";
            this.txtür.Size = new System.Drawing.Size(212, 34);
            this.txtür.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 296);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 29);
            this.label5.TabIndex = 22;
            this.label5.Text = "Cinsiyet:";
            // 
            // txtcins
            // 
            this.txtcins.Location = new System.Drawing.Point(173, 290);
            this.txtcins.Name = "txtcins";
            this.txtcins.Size = new System.Drawing.Size(212, 34);
            this.txtcins.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 254);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 29);
            this.label6.TabIndex = 21;
            this.label6.Text = "Yaş:";
            // 
            // txtyas
            // 
            this.txtyas.Location = new System.Drawing.Point(173, 249);
            this.txtyas.Name = "txtyas";
            this.txtyas.Size = new System.Drawing.Size(212, 34);
            this.txtyas.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 338);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 29);
            this.label7.TabIndex = 23;
            this.label7.Text = "Tür:";
            // 
            // textırk
            // 
            this.textırk.Location = new System.Drawing.Point(173, 208);
            this.textırk.Name = "textırk";
            this.textırk.Size = new System.Drawing.Size(212, 34);
            this.textırk.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 212);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 29);
            this.label8.TabIndex = 20;
            this.label8.Text = "Irk:";
            // 
            // txtrenk
            // 
            this.txtrenk.Location = new System.Drawing.Point(173, 167);
            this.txtrenk.Name = "txtrenk";
            this.txtrenk.Size = new System.Drawing.Size(212, 34);
            this.txtrenk.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 29);
            this.label3.TabIndex = 14;
            this.label3.Text = "Çip NO:";
            // 
            // txtcip
            // 
            this.txtcip.Location = new System.Drawing.Point(173, 126);
            this.txtcip.Name = "txtcip";
            this.txtcip.Size = new System.Drawing.Size(212, 34);
            this.txtcip.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 29);
            this.label2.TabIndex = 13;
            this.label2.Text = "Pasaport NO:";
            // 
            // txtpass
            // 
            this.txtpass.Location = new System.Drawing.Point(173, 85);
            this.txtpass.Name = "txtpass";
            this.txtpass.Size = new System.Drawing.Size(212, 34);
            this.txtpass.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 29);
            this.label4.TabIndex = 15;
            this.label4.Text = "Renk:";
            // 
            // txtad
            // 
            this.txtad.Location = new System.Drawing.Point(173, 44);
            this.txtad.Name = "txtad";
            this.txtad.Size = new System.Drawing.Size(212, 34);
            this.txtad.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 29);
            this.label1.TabIndex = 12;
            this.label1.Text = "Ad:";
            // 
            // HastaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1377, 562);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "HastaForm";
            this.Text = "HastaForm";
            this.Load += new System.EventHandler(this.HastaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtrenk;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtcip;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtpass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtnot;
        private System.Windows.Forms.TextBox txtür;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtcins;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtyas;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textırk;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hayvanad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pasaport;
        private System.Windows.Forms.DataGridViewTextBoxColumn Çip;
        private System.Windows.Forms.DataGridViewTextBoxColumn Renk;
        private System.Windows.Forms.DataGridViewTextBoxColumn Irk;
        private System.Windows.Forms.DataGridViewTextBoxColumn Yaş;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cinsiyet;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tür;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notlar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sahipad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sahipsoyad;
        private System.Windows.Forms.DataGridViewTextBoxColumn TCKN;
    }
}