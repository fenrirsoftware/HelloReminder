namespace temizHCO
{
    partial class HastaSahibiForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtnumara = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtsahipTCkimlik = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtsahipsoyad = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtsahipad = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Soyad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TCKN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Numara = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtnumara);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtsahipTCkimlik);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtsahipsoyad);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtsahipad);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(350, 211);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sahip Bilgi Formu";
            // 
            // txtnumara
            // 
            this.txtnumara.Location = new System.Drawing.Point(131, 173);
            this.txtnumara.Name = "txtnumara";
            this.txtnumara.Size = new System.Drawing.Size(212, 34);
            this.txtnumara.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 29);
            this.label3.TabIndex = 14;
            this.label3.Text = "TCKN:";
            // 
            // txtsahipTCkimlik
            // 
            this.txtsahipTCkimlik.Location = new System.Drawing.Point(131, 130);
            this.txtsahipTCkimlik.Name = "txtsahipTCkimlik";
            this.txtsahipTCkimlik.Size = new System.Drawing.Size(212, 34);
            this.txtsahipTCkimlik.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 29);
            this.label2.TabIndex = 13;
            this.label2.Text = "Soyad:";
            // 
            // txtsahipsoyad
            // 
            this.txtsahipsoyad.Location = new System.Drawing.Point(131, 87);
            this.txtsahipsoyad.Name = "txtsahipsoyad";
            this.txtsahipsoyad.Size = new System.Drawing.Size(212, 34);
            this.txtsahipsoyad.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 29);
            this.label4.TabIndex = 15;
            this.label4.Text = "Numara:";
            // 
            // txtsahipad
            // 
            this.txtsahipad.Location = new System.Drawing.Point(131, 44);
            this.txtsahipad.Name = "txtsahipad";
            this.txtsahipad.Size = new System.Drawing.Size(212, 34);
            this.txtsahipad.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 29);
            this.label1.TabIndex = 12;
            this.label1.Text = "Ad:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(224, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 45);
            this.button1.TabIndex = 8;
            this.button1.Text = "Kaydet";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Ad,
            this.Soyad,
            this.TCKN,
            this.Numara});
            this.dataGridView1.Location = new System.Drawing.Point(363, 58);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(596, 214);
            this.dataGridView1.TabIndex = 9;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 40;
            // 
            // Ad
            // 
            this.Ad.HeaderText = "Ad";
            this.Ad.MinimumWidth = 6;
            this.Ad.Name = "Ad";
            this.Ad.ReadOnly = true;
            this.Ad.Width = 125;
            // 
            // Soyad
            // 
            this.Soyad.HeaderText = "Soyad";
            this.Soyad.MinimumWidth = 6;
            this.Soyad.Name = "Soyad";
            this.Soyad.ReadOnly = true;
            this.Soyad.Width = 125;
            // 
            // TCKN
            // 
            this.TCKN.HeaderText = "TCKN";
            this.TCKN.MinimumWidth = 6;
            this.TCKN.Name = "TCKN";
            this.TCKN.ReadOnly = true;
            this.TCKN.Width = 125;
            // 
            // Numara
            // 
            this.Numara.HeaderText = "Numara";
            this.Numara.MinimumWidth = 6;
            this.Numara.Name = "Numara";
            this.Numara.ReadOnly = true;
            this.Numara.Width = 125;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(103, 227);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 45);
            this.button2.TabIndex = 10;
            this.button2.Text = "Düzenle";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(4, 227);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(92, 45);
            this.button3.TabIndex = 11;
            this.button3.Text = "Sil";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textBox1.Location = new System.Drawing.Point(424, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(535, 30);
            this.textBox1.TabIndex = 12;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(363, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 29);
            this.label5.TabIndex = 13;
            this.label5.Text = "Ara:";
            // 
            // HastaSahibiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 275);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "HastaSahibiForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HSBF";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HastaSahibiForm_FormClosing);
            this.Load += new System.EventHandler(this.HastaSahibiForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox txtnumara;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtsahipTCkimlik;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtsahipsoyad;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtsahipad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Soyad;
        private System.Windows.Forms.DataGridViewTextBoxColumn TCKN;
        private System.Windows.Forms.DataGridViewTextBoxColumn Numara;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
    }
}