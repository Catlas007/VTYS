using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace KutuphaneYonetimSistemi
{
    public partial class CalisanlarForm : Form
    {
        private System.Windows.Forms.DataGridView dataGridViewCalisanlar;
        private System.Windows.Forms.Button btnListeleCalisanlar;
        private System.Windows.Forms.Button btnSilCalisan;
        private System.Windows.Forms.Button btnEkleCalisan;
        private System.Windows.Forms.TextBox txtCalisanIdSil;
        private System.Windows.Forms.TextBox txtAd, txtSoyad, txtEmail, txtTelefon, txtSifre;

        public CalisanlarForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
    
            this.dataGridViewCalisanlar = new System.Windows.Forms.DataGridView();
            this.dataGridViewCalisanlar.Location = new System.Drawing.Point(20, 20);
            this.dataGridViewCalisanlar.Size = new System.Drawing.Size(1000, 300);
            this.dataGridViewCalisanlar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

          
            this.btnListeleCalisanlar = new System.Windows.Forms.Button();
            this.btnListeleCalisanlar.Text = "Çalışanları Listele";
            this.btnListeleCalisanlar.Location = new System.Drawing.Point(20, 340);
            this.btnListeleCalisanlar.Size = new System.Drawing.Size(150, 30);
            this.btnListeleCalisanlar.Click += new System.EventHandler(this.btnListeleCalisanlar_Click);

           
            this.txtCalisanIdSil = new System.Windows.Forms.TextBox();
            this.txtCalisanIdSil.PlaceholderText = "Çalışan ID";
            this.txtCalisanIdSil.Location = new System.Drawing.Point(200, 340);
            this.txtCalisanIdSil.Size = new System.Drawing.Size(100, 22);

         
            this.btnSilCalisan = new System.Windows.Forms.Button();
            this.btnSilCalisan.Text = "Çalışan Sil";
            this.btnSilCalisan.Location = new System.Drawing.Point(320, 340);
            this.btnSilCalisan.Size = new System.Drawing.Size(100, 30);
            this.btnSilCalisan.Click += new System.EventHandler(this.btnSilCalisan_Click);

           
            this.txtAd = CreateTextBox("Ad", 20, 400);
            this.txtSoyad = CreateTextBox("Soyad", 150, 400);
            this.txtEmail = CreateTextBox("Email", 280, 400);
            this.txtTelefon = CreateTextBox("Telefon", 410, 400);
            this.txtSifre = CreateTextBox("Şifre", 540, 400);

          
            this.btnEkleCalisan = new System.Windows.Forms.Button();
            this.btnEkleCalisan.Text = "Çalışan Ekle";
            this.btnEkleCalisan.Location = new System.Drawing.Point(20, 450);
            this.btnEkleCalisan.Size = new System.Drawing.Size(150, 30);
            this.btnEkleCalisan.Click += new System.EventHandler(this.btnEkleCalisan_Click);

            this.Controls.Add(this.dataGridViewCalisanlar);
            this.Controls.Add(this.btnListeleCalisanlar);
            this.Controls.Add(this.txtCalisanIdSil);
            this.Controls.Add(this.btnSilCalisan);
            this.Controls.Add(this.btnEkleCalisan);
            this.Controls.Add(this.txtAd);
            this.Controls.Add(this.txtSoyad);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtTelefon);
            this.Controls.Add(this.txtSifre);

            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Text = "Çalışanlar Yönetimi";
        }
        private TextBox CreateTextBox(string placeholder, int x, int y)
        {
            var txtBox = new System.Windows.Forms.TextBox();
            txtBox.PlaceholderText = placeholder;
            txtBox.Location = new System.Drawing.Point(x, y);
            txtBox.Size = new System.Drawing.Size(120, 22);
            return txtBox;
        }

        private void btnListeleCalisanlar_Click(object sender, EventArgs e)
        {
            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
            string sorgu = @"
                SELECT 
                    calisan_id AS ""ID"", 
                    ad AS ""Ad"", 
                    soyad AS ""Soyad"", 
                    email AS ""Email"", 
                    phone AS ""Telefon"", 
                    sifre AS ""Şifre""
                FROM calisanlar";

            try
            {
                DataTable calisanlar = yardimci.SorguCalistir(sorgu);
                dataGridViewCalisanlar.DataSource = calisanlar;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSilCalisan_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtCalisanIdSil.Text, out int calisanId))
            {
                VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
                string sorgu = "DELETE FROM calisanlar WHERE calisan_id = @calisan_id";

                try
                {
                    int affectedRows = yardimci.KomutCalistir(sorgu, new NpgsqlParameter("@calisan_id", calisanId));

                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Çalışan başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnListeleCalisanlar_Click(null, null); // Refresh list
                    }
                    else
                    {
                        MessageBox.Show("Çalışan bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Geçerli bir Çalışan ID giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEkleCalisan_Click(object sender, EventArgs e)
        {
            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();

            string sorgu = @"
                INSERT INTO calisanlar (ad, soyad, email, phone, sifre) 
                VALUES (@ad, @soyad, @email, @phone, @sifre)";

            try
            {
                yardimci.KomutCalistir(sorgu,
                    new NpgsqlParameter("@ad", txtAd.Text),
                    new NpgsqlParameter("@soyad", txtSoyad.Text),
                    new NpgsqlParameter("@email", txtEmail.Text),
                    new NpgsqlParameter("@phone", txtTelefon.Text),
                    new NpgsqlParameter("@sifre", txtSifre.Text)
                );

                MessageBox.Show("Çalışan başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnListeleCalisanlar_Click(null, null); // Refresh list
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
