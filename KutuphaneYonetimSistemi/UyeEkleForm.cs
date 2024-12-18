using System;
using System.Windows.Forms;

public partial class UyeEkleForm : Form
{
    private TextBox txtAd, txtSoyad, txtEmail, txtTelefon;
    private Button btnEkle;

    public UyeEkleForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.txtAd = new TextBox();
        this.txtSoyad = new TextBox();
        this.txtEmail = new TextBox();
        this.txtTelefon = new TextBox();
        this.btnEkle = new Button();

        // txtAd
        this.txtAd.Location = new System.Drawing.Point(20, 20);
        this.txtAd.Size = new System.Drawing.Size(200, 22);
        this.txtAd.PlaceholderText = "Ad";

        // txtSoyad
        this.txtSoyad.Location = new System.Drawing.Point(20, 60);
        this.txtSoyad.Size = new System.Drawing.Size(200, 22);
        this.txtSoyad.PlaceholderText = "Soyad";

        // txtEmail
        this.txtEmail.Location = new System.Drawing.Point(20, 100);
        this.txtEmail.Size = new System.Drawing.Size(200, 22);
        this.txtEmail.PlaceholderText = "Email";

        // txtTelefon
        this.txtTelefon.Location = new System.Drawing.Point(20, 140);
        this.txtTelefon.Size = new System.Drawing.Size(200, 22);
        this.txtTelefon.PlaceholderText = "Telefon";

        // btnEkle
        this.btnEkle.Location = new System.Drawing.Point(20, 180);
        this.btnEkle.Size = new System.Drawing.Size(200, 30);
        this.btnEkle.Text = "Üye Ekle";
        this.btnEkle.Click += new EventHandler(this.btnEkle_Click);

        // UyeEkleForm
        this.Controls.Add(this.txtAd);
        this.Controls.Add(this.txtSoyad);
        this.Controls.Add(this.txtEmail);
        this.Controls.Add(this.txtTelefon);
        this.Controls.Add(this.btnEkle);
        this.ClientSize = new System.Drawing.Size(250, 250);
        this.Text = "Yeni Üye Ekle";
    }

    private void btnEkle_Click(object sender, EventArgs e)
    {
        string ad = txtAd.Text.Trim();
        string soyad = txtSoyad.Text.Trim();
        string email = txtEmail.Text.Trim();
        string telefon = txtTelefon.Text.Trim();

        if (!string.IsNullOrEmpty(ad) && !string.IsNullOrEmpty(soyad) &&
            !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(telefon))
        {
            try
            {
                
                VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
                string sorgu = $"INSERT INTO uyeler (ad, soyad, email, phone) VALUES ('{ad}', '{soyad}', '{email}', '{telefon}')";
                yardimci.KomutCalistir(sorgu);

                MessageBox.Show("Yeni üye başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
