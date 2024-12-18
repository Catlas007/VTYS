using Npgsql;
using System.Data;
using System;
using System.Windows.Forms;

namespace KutuphaneYonetimSistemi
{
    public partial class RezervasyonlarForm : Form
    {
        private Button btnMasaRezervasyonlari;
        private Button btnKitapRezervasyonlari;
        private System.Windows.Forms.Button btnMasaRezervasyonEkle;
        private System.Windows.Forms.Button btnKitapRezervasyonEkle;
        private System.Windows.Forms.TextBox txtRezervasyonId;
        private System.Windows.Forms.Button btnMasaRezervasyonSil;
        private System.Windows.Forms.Button btnKitapRezervasyonSil;


        public RezervasyonlarForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.ClientSize = new System.Drawing.Size(1000, 1000);
            this.btnMasaRezervasyonlari = new Button();
            this.btnKitapRezervasyonlari = new Button();
 
            // btnMasaRezervasyonlari 
            this.btnMasaRezervasyonlari.Location = new System.Drawing.Point(50, 50);
            this.btnMasaRezervasyonlari.Name = "btnMasaRezervasyonlari";
            this.btnMasaRezervasyonlari.Size = new System.Drawing.Size(300, 30);
            this.btnMasaRezervasyonlari.Text = "Masa Rezervasyonları";
            this.btnMasaRezervasyonlari.Click += new EventHandler(this.btnMasaRezervasyonlari_Click);
            this.Controls.Add(this.btnMasaRezervasyonlari);

            //MasaRezEkle
            this.btnMasaRezervasyonEkle = new Button();
            this.btnMasaRezervasyonEkle.Text = "Masa Rezervasyon Ekle";
            this.btnMasaRezervasyonEkle.Location = new System.Drawing.Point(50, 100);
            this.btnMasaRezervasyonEkle.Size = new System.Drawing.Size (300,30);
            this.btnMasaRezervasyonEkle.Click += new EventHandler(this.btnMasaRezervasyonEkle_Click);
            this.Controls.Add(this.btnMasaRezervasyonEkle);

            // txtRezervasyonId
            this.txtRezervasyonId = new System.Windows.Forms.TextBox();
            this.txtRezervasyonId.Location = new System.Drawing.Point(50, 150);
            this.txtRezervasyonId.Name = "txtRezervasyonId";
            this.txtRezervasyonId.Size = new System.Drawing.Size(300, 30);
            this.txtRezervasyonId.PlaceholderText = "Silinecek Rezervasyon ID";
            this.Controls.Add(this.txtRezervasyonId);

            // btnMasaRezervasyonSil
            this.btnMasaRezervasyonSil = new System.Windows.Forms.Button();
            this.btnMasaRezervasyonSil.Location = new System.Drawing.Point(50, 200);
            this.btnMasaRezervasyonSil.Name = "btnMasaRezervasyonSil";
            this.btnMasaRezervasyonSil.Size = new System.Drawing.Size(300, 30);
            this.btnMasaRezervasyonSil.Text = "Masa Rezervasyon Sil";
            this.btnMasaRezervasyonSil.Click += new System.EventHandler(this.btnMasaRezervasyonSil_Click);
            this.Controls.Add(this.btnMasaRezervasyonSil);

            // btnKitapRezervasyonSil
            this.btnKitapRezervasyonSil = new System.Windows.Forms.Button();
            this.btnKitapRezervasyonSil.Location = new System.Drawing.Point(500, 200);
            this.btnKitapRezervasyonSil.Name = "btnKitapRezervasyonSil";
            this.btnKitapRezervasyonSil.Size = new System.Drawing.Size(300, 30);
            this.btnKitapRezervasyonSil.Text = "Kitap Rezervasyon Sil";
            this.btnKitapRezervasyonSil.Click += new System.EventHandler(this.btnKitapRezervasyonSil_Click);
            this.Controls.Add(this.btnKitapRezervasyonSil);

 
            // btnKitapRezervasyonlari
            this.btnKitapRezervasyonlari.Location = new System.Drawing.Point(500, 50);
            this.btnKitapRezervasyonlari.Name = "btnKitapRezervasyonlari";
            this.btnKitapRezervasyonlari.Size = new System.Drawing.Size(300, 30);
            this.btnKitapRezervasyonlari.Text = "Kitap Rezervasyonları";
            this.btnKitapRezervasyonlari.Click += new EventHandler(this.btnKitapRezervasyonlari_Click);
            this.Controls.Add(this.btnKitapRezervasyonlari);

            //KitapRezEkle
            this.btnKitapRezervasyonEkle = new Button();
            this.btnKitapRezervasyonEkle.Text = "Kitap Rezervasyon Ekle";
            this.btnKitapRezervasyonEkle.Location = new System.Drawing.Point(500, 100);
            this.btnKitapRezervasyonEkle.Size = new System.Drawing.Size (300,30);
            this.btnKitapRezervasyonEkle.Click += new EventHandler(this.btnKitapRezervasyonEkle_Click);
            this.Controls.Add(this.btnKitapRezervasyonEkle);

            // RezervasyonlarForm

            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.Name = "RezervasyonlarForm";
            this.Text = "Rezervasyonlar";
        }

        private void btnMasaRezervasyonlari_Click(object sender, EventArgs e)
        {
            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
            string sorgu = @"
                SELECT 
                    mr.masa_rezervasyon_id AS ""Rezervasyon ID"",
                    mr.masa_id AS ""Masa ID"",
                    u.uye_id AS ""Üye ID"",
                    u.ad || ' ' || u.soyad AS ""Üye Adı Soyadı"",
                    m.misafir_id AS ""Misafir ID"",
                    m.ad || ' ' || m.soyad AS ""Misafir Adı Soyadı""
                FROM 
                    masarezervasyonlari mr
                LEFT JOIN 
                    uyeler u ON mr.uye_id = u.uye_id
                LEFT JOIN 
                    misafirler m ON mr.misafir_id = m.misafir_id";

            DataTable masaRezervasyonlari = yardimci.SorguCalistir(sorgu);

            DisplayRezervasyonlar(masaRezervasyonlari);

        }

        private void btnKitapRezervasyonlari_Click(object sender, EventArgs e)
        {
            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();

            string sorgu = @"
                SELECT 
                    kr.kitap_rezervasyon_id AS ""Rezervasyon ID"",
                    k.baslik AS ""Kitap Adı"",
                    y.ad || ' ' || y.soyad AS ""Yazar"",
                    u.ad || ' ' || u.soyad AS ""Üye Adı Soyadı"",
                    kr.kopya_numarasi AS ""Kopya Numarası""
                FROM 
                    kitaprezervasyonlari kr
                INNER JOIN 
                    kitapkopyalari kk ON kr.kitap_id = kk.kitap_id AND kr.kopya_numarasi = kk.kopya_numarasi
                INNER JOIN 
                    kitaplar k ON kk.kitap_id = k.kitap_id
                INNER JOIN 
                    yazarlar y ON k.yazar_id = y.yazar_id
                INNER JOIN 
                    uyeler u ON kr.uye_id = u.uye_id
                ORDER BY 
                    kr.kitap_rezervasyon_id;";

            DataTable kitapRezervasyonlari = yardimci.SorguCalistir(sorgu);

            DisplayRezervasyonlar(kitapRezervasyonlari);
        }

        private void btnMasaRezervasyonEkle_Click(object sender, EventArgs e)
        {
             MasaRezervasyonEkleForm form = new MasaRezervasyonEkleForm();
             form.RezervasyonEklendi += (s, ev) => btnMasaRezervasyonlari_Click(null, null); 
             form.ShowDialog();
         }

        private void btnKitapRezervasyonEkle_Click(object sender, EventArgs e)
        {
            KitapRezervasyonEkleForm form = new KitapRezervasyonEkleForm();
            form.RezervasyonEklendi += (s, ev) => btnKitapRezervasyonlari_Click(null, null); 
            form.ShowDialog();
        }


            private void DisplayRezervasyonlar(DataTable rezervasyonlar)
            {
                foreach (Control control in this.Controls.OfType<DataGridView>().ToList())
                {
                    this.Controls.Remove(control);
                    control.Dispose();
                }

                DataGridView dataGridView = new DataGridView
                {
                    Location = new System.Drawing.Point(50, 400),
                    Size = new System.Drawing.Size(1000, 300),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    DataSource = rezervasyonlar
                };

                this.Controls.Add(dataGridView);
            }

        private void btnMasaRezervasyonSil_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtRezervasyonId.Text, out int rezervasyonId))
            {
                VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();

                try
                {
                    string sorgu = "DELETE FROM masarezervasyonlari WHERE masa_rezervasyon_id = @rezervasyonId";
                    int affectedRows = yardimci.KomutCalistir(sorgu, new NpgsqlParameter("@rezervasyonId", rezervasyonId));

                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Masa rezervasyonu başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnMasaRezervasyonlari_Click(null, null); 
                    }
                    else
                    {
                        MessageBox.Show("Rezervasyon bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Geçerli bir Rezervasyon ID giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnKitapRezervasyonSil_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtRezervasyonId.Text, out int rezervasyonId))
            {
                VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();

                try
                {
                    string sorgu = "DELETE FROM kitaprezervasyonlari WHERE kitap_rezervasyon_id = @rezervasyonId";
                    int affectedRows = yardimci.KomutCalistir(sorgu, new NpgsqlParameter("@rezervasyonId", rezervasyonId));

                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Kitap rezervasyonu başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnKitapRezervasyonlari_Click(null, null); 
                    }
                    else
                    {
                        MessageBox.Show("Rezervasyon bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Geçerli bir Rezervasyon ID giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


    }
}
