using System;
using System.Windows.Forms;

public partial class MisafirLogEkleForm : Form
{
    public MisafirLogEkleForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.txtAd = new TextBox();
        this.txtSoyad = new TextBox();
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

        // txtTelefon
        this.txtTelefon.Location = new System.Drawing.Point(20, 100);
        this.txtTelefon.Size = new System.Drawing.Size(200, 22);
        this.txtTelefon.PlaceholderText = "Telefon";

        // btnEkle
        this.btnEkle.Location = new System.Drawing.Point(20, 140);
        this.btnEkle.Size = new System.Drawing.Size(200, 30);
        this.btnEkle.Text = "Ekle";
        this.btnEkle.Click += new EventHandler(this.btnEkle_Click);

        // MisafirLogEkleForm
        this.Controls.Add(this.txtAd);
        this.Controls.Add(this.txtSoyad);
        this.Controls.Add(this.txtTelefon);
        this.Controls.Add(this.btnEkle);
        this.ClientSize = new System.Drawing.Size(250, 200);
        this.Text = "Misafir Log Ekle";
    }

    private TextBox txtAd, txtSoyad, txtTelefon;
    private Button btnEkle;

    private void btnEkle_Click(object sender, EventArgs e)
    {
        string ad = txtAd.Text;
        string soyad = txtSoyad.Text;
        string telefon = txtTelefon.Text;

        if (!string.IsNullOrEmpty(ad) && !string.IsNullOrEmpty(soyad))
        {
            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
            string sorgu = $"INSERT INTO misafirler (ad, soyad, phone, ziyaret_tarihi) VALUES ('{ad}', '{soyad}', '{telefon}', NOW())";
            yardimci.KomutCalistir(sorgu);

            MessageBox.Show("Misafir logu eklendi.");
            this.Close();
        }
        else
        {
            MessageBox.Show("Ad ve Soyad alanları zorunludur.");
        }
    }
}
