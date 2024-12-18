namespace KutuphaneYonetimSistemi
{
    partial class AnaForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnKitaplariYukle;
        private System.Windows.Forms.Button btnUyeListesi;
        private System.Windows.Forms.Button btnMisafirListesi;
        private System.Windows.Forms.Button btnRezervasyonlar;
        private System.Windows.Forms.Button btnUyeLogEkle;
        private System.Windows.Forms.Button btnMisafirLogEkle;
        private System.Windows.Forms.Button btnYeniUyeEkle;
        private System.Windows.Forms.Button btnUyeSil;
        private System.Windows.Forms.TextBox txtUyeIdSil;
        private System.Windows.Forms.Button btnCalisanlarYonet;
        private System.Windows.Forms.Button btnKitapYonetim;
        private System.Windows.Forms.TextBox txtKitapAra;
        private System.Windows.Forms.Button btnKitapAra;
        private System.Windows.Forms.Button btnEditUye;

        private System.Windows.Forms.DataGridView dataGridViewKitaplar;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.btnKitaplariYukle = new System.Windows.Forms.Button();
            this.dataGridViewKitaplar = new System.Windows.Forms.DataGridView();
 
            // btnUyeListesi
            this.btnUyeListesi = new System.Windows.Forms.Button();
            this.btnUyeListesi.Location = new System.Drawing.Point(450, 20);
            this.btnUyeListesi.Name = "btnUyeListesi";
            this.btnUyeListesi.Size = new System.Drawing.Size(150, 30);
            this.btnUyeListesi.Text = "Üye Listesi";
            this.btnUyeListesi.Click += new System.EventHandler(this.btnUyeListesi_Click);
            this.Controls.Add(this.btnUyeListesi);

            // btnYeniUyeEkle
            this.btnYeniUyeEkle = new System.Windows.Forms.Button();
            this.btnYeniUyeEkle.Location = new System.Drawing.Point(450, 70);
            this.btnYeniUyeEkle.Name = "btnYeniUyeEkle";
            this.btnYeniUyeEkle.Size = new System.Drawing.Size(150, 30);
            this.btnYeniUyeEkle.Text = "Yeni Üye Ekle";
            this.btnYeniUyeEkle.Click += new System.EventHandler(this.btnYeniUyeEkle_Click);
            this.Controls.Add(this.btnYeniUyeEkle);

            // txtUyeIdSil
            this.txtUyeIdSil = new System.Windows.Forms.TextBox();
            this.txtUyeIdSil.Location = new System.Drawing.Point(650, 20);
            this.txtUyeIdSil.Name = "txtUyeIdSil";
            this.txtUyeIdSil.Size = new System.Drawing.Size(150, 30);
            this.txtUyeIdSil.PlaceholderText = "Üye ID Giriniz";
            this.Controls.Add(this.txtUyeIdSil);

            // btnUyeSil
            this.btnUyeSil = new System.Windows.Forms.Button();
            this.btnUyeSil.Location = new System.Drawing.Point(650, 70);
            this.btnUyeSil.Name = "btnUyeSil";
            this.btnUyeSil.Size = new System.Drawing.Size(150, 30);
            this.btnUyeSil.Text = "Üye Sil";
            this.btnUyeSil.UseVisualStyleBackColor = true;
            this.btnUyeSil.Click += new System.EventHandler(this.btnUyeSil_Click);
            this.Controls.Add(this.btnUyeSil);

            // btnUyeEdit
            this.btnEditUye = new System.Windows.Forms.Button();
            this.btnEditUye.Location = new System.Drawing.Point(450, 120); // Adjust position
            this.btnEditUye.Name = "btnEditUye";
            this.btnEditUye.Size = new System.Drawing.Size(150, 30);
            this.btnEditUye.Text = "Üye Düzenle";
            this.btnEditUye.UseVisualStyleBackColor = true;
            this.btnEditUye.Click += new System.EventHandler(this.btnEditUye_Click);
            this.Controls.Add(this.btnEditUye);

            // btnMisafirListesi
            this.btnMisafirListesi = new System.Windows.Forms.Button();
            this.btnMisafirListesi.Location = new System.Drawing.Point(250, 20);
            this.btnMisafirListesi.Name = "btnMisafirListesi";
            this.btnMisafirListesi.Size = new System.Drawing.Size(150, 30);
            this.btnMisafirListesi.Text = "Misafirler";
            this.btnMisafirListesi.Click += new System.EventHandler(this.btnMisafirListesi_Click);
            this.Controls.Add(this.btnMisafirListesi);

            // btnMisafirLogEkle
            this.btnMisafirLogEkle = new System.Windows.Forms.Button();
            this.btnMisafirLogEkle.Location = new System.Drawing.Point(250, 70);
            this.btnMisafirLogEkle.Name = "btnMisafirLogEkle";
            this.btnMisafirLogEkle.Size = new System.Drawing.Size(150, 30);
            this.btnMisafirLogEkle.Text = "Misafir Log Ekle";
            this.btnMisafirLogEkle.Click += new System.EventHandler(this.btnMisafirLogEkle_Click);
            this.Controls.Add(this.btnMisafirLogEkle);

            // btnRezervasyonlar
            this.btnRezervasyonlar = new System.Windows.Forms.Button();
            this.btnRezervasyonlar.Location = new System.Drawing.Point(850, 20);
            this.btnRezervasyonlar.Name = "btnRezervasyonlar";
            this.btnRezervasyonlar.Size = new System.Drawing.Size(150, 30);
            this.btnRezervasyonlar.Text = "Rezervasyonlar";
            this.btnRezervasyonlar.Click += new System.EventHandler(this.btnRezervasyonlar_Click);
            this.Controls.Add(this.btnRezervasyonlar);

            // btnCalisanlarYonet
            this.btnCalisanlarYonet = new System.Windows.Forms.Button();
            this.btnCalisanlarYonet.Text = "Çalışanları Yönet";
            this.btnCalisanlarYonet.Location = new System.Drawing.Point(850, 70);
            this.btnCalisanlarYonet.Size = new System.Drawing.Size(200, 30);
            this.btnCalisanlarYonet.Click += new System.EventHandler(this.btnCalisanlarYonet_Click);
            this.Controls.Add(this.btnCalisanlarYonet);

 
            // btnKitaplariYukle 
            this.btnKitaplariYukle.Location = new System.Drawing.Point(50, 20);
            this.btnKitaplariYukle.Name = "btnKitaplariYukle";
            this.btnKitaplariYukle.Size = new System.Drawing.Size(120, 30);
            this.btnKitaplariYukle.Text = "Kitap Listesi";
            this.btnKitaplariYukle.Click += new System.EventHandler(this.btnKitaplariYukle_Click);

            // btnListeleKitapKopyalari
            this.btnListeleKitapKopyalari = new Button();
            this.btnListeleKitapKopyalari.Location = new System.Drawing.Point(60, 70);
            this.btnListeleKitapKopyalari.Name = "btnListeleKitapKopyalari";
            this.btnListeleKitapKopyalari.Size = new System.Drawing.Size(150, 30);
            this.btnListeleKitapKopyalari.Text = "Kopya Listesi";
            this.btnListeleKitapKopyalari.UseVisualStyleBackColor = true;
            this.btnListeleKitapKopyalari.Click += new EventHandler(this.btnListeleKitapKopyalari_Click);
            this.Controls.Add(this.btnListeleKitapKopyalari);

            // btnKitapYonetim
            this.btnKitapYonetim = new System.Windows.Forms.Button();
            this.btnKitapYonetim.Location = new System.Drawing.Point(60, 120);
            this.btnKitapYonetim.Name = "btnKitapYonetim";
            this.btnKitapYonetim.Size = new System.Drawing.Size(150, 30);
            this.btnKitapYonetim.Text = "Kitap Yönetim";
            this.btnKitapYonetim.UseVisualStyleBackColor = true;
            this.btnKitapYonetim.Click += new System.EventHandler(this.btnKitapYonetim_Click);
            this.Controls.Add(this.btnKitapYonetim);


            // dataGridViewKitaplar 
            this.dataGridViewKitaplar.Location = new System.Drawing.Point(20, 200);
            this.dataGridViewKitaplar.Name = "dataGridViewKitaplar";
            this.dataGridViewKitaplar.Size = new System.Drawing.Size(1000, 300);
            this.dataGridViewKitaplar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // txtKitapAra
            this.txtKitapAra = new System.Windows.Forms.TextBox();
            this.txtKitapAra.Location = new System.Drawing.Point(1200, 20);
            this.txtKitapAra.Name = "txtKitapAra";
            this.txtKitapAra.Size = new System.Drawing.Size(200, 22);
            this.txtKitapAra.PlaceholderText = "Kitap Adını Giriniz";
            this.Controls.Add(this.txtKitapAra);

            // btnKitapAra
            this.btnKitapAra = new System.Windows.Forms.Button();
            this.btnKitapAra.Location = new System.Drawing.Point(1200, 70);
            this.btnKitapAra.Name = "btnKitapAra";
            this.btnKitapAra.Size = new System.Drawing.Size(100, 30);
            this.btnKitapAra.Text = "Ara";
            this.btnKitapAra.UseVisualStyleBackColor = true;
            this.btnKitapAra.Click += new System.EventHandler(this.BtnKitapAra_Click);
            this.Controls.Add(this.btnKitapAra);

            // AnaForm 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1500, 500);
            this.Controls.Add(this.btnKitaplariYukle);
            this.Controls.Add(this.dataGridViewKitaplar);
            this.Name = "AnaForm";
            this.Text = "Kütüphane Yönetim Sistemi";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewKitaplar)).EndInit();
            this.ResumeLayout(false);
        }
    }
}

