using System;
using System.Data;
using System.Windows.Forms;

namespace KutuphaneYonetimSistemi
{
    public partial class GirisForm : Form
    {
        public GirisForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtKullaniciAdi = new TextBox();
            this.txtSifre = new TextBox();
            this.btnGiris = new Button();
            this.SuspendLayout();

            // txtKullaniciAdi 
            this.txtKullaniciAdi.Location = new System.Drawing.Point(50, 30);
            this.txtKullaniciAdi.Name = "txtKullaniciAdi";
            this.txtKullaniciAdi.PlaceholderText = "Kullanıcı Adı";
            this.txtKullaniciAdi.Size = new System.Drawing.Size(200, 22);

            // txtSifre
            this.txtSifre.Location = new System.Drawing.Point(50, 70);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.PlaceholderText = "Şifre";
            this.txtSifre.UseSystemPasswordChar = true;
            this.txtSifre.Size = new System.Drawing.Size(200, 22);

            // btnGiris
            this.btnGiris.Location = new System.Drawing.Point(50, 110);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.Size = new System.Drawing.Size(200, 30);
            this.btnGiris.Text = "Giriş";
            this.btnGiris.Click += new EventHandler(this.btnGiris_Click);

            // GirisForm 
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.txtKullaniciAdi);
            this.Controls.Add(this.txtSifre);
            this.Controls.Add(this.btnGiris);
            this.Name = "GirisForm";
            this.Text = "Giriş";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox txtKullaniciAdi;
        private TextBox txtSifre;
        private Button btnGiris;

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text;
            string sifre = txtSifre.Text;

            if (GirisDogrula(kullaniciAdi, sifre))
            {
                MessageBox.Show("Giriş Başarılı!");
                this.Hide();
                AnaForm anaForm = new AnaForm();
                anaForm.Show();
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı adı veya şifre.");
            }
        }

        private bool GirisDogrula(string kullaniciAdi, string sifre)
        {
            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
            string sorgu = $"SELECT COUNT(*) FROM calisanlar WHERE ad = @ad AND sifre = @sifre";

            using (var baglanti = yardimci.BaglantiAl())
            {
                baglanti.Open();
                using (var komut = new Npgsql.NpgsqlCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@ad", kullaniciAdi);
                    komut.Parameters.AddWithValue("@sifre", sifre);

                    int count = Convert.ToInt32(komut.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}
