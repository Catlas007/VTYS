using Npgsql;
using System;
using System.Windows.Forms;

public partial class YazarEkleForm : Form
{
    private TextBox txtAd, txtSoyad;
    private Button btnEkle;

    public event EventHandler YazarEklendi;

    public YazarEkleForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        txtAd = new TextBox { PlaceholderText = "Ad", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(200, 22) };
        Controls.Add(txtAd);

        txtSoyad = new TextBox { PlaceholderText = "Soyad", Location = new System.Drawing.Point(20, 60), Size = new System.Drawing.Size(200, 22) };
        Controls.Add(txtSoyad);

        btnEkle = new Button { Text = "Yazar Ekle", Location = new System.Drawing.Point(20, 100), Size = new System.Drawing.Size(200, 30) };
        btnEkle.Click += BtnEkle_Click;
        Controls.Add(btnEkle);

        this.Text = "Yazar Ekle";
        this.ClientSize = new System.Drawing.Size(250, 150);
    }

    private void BtnEkle_Click(object sender, EventArgs e)
    {
        string ad = txtAd.Text.Trim();
        string soyad = txtSoyad.Text.Trim();

        if (!string.IsNullOrEmpty(ad) && !string.IsNullOrEmpty(soyad))
        {
            try
            {
                string sorgu = "INSERT INTO yazarlar (ad, soyad) VALUES (@ad, @soyad)";
                VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
                yardimci.KomutCalistir(sorgu,
                    new Npgsql.NpgsqlParameter("@ad", ad),
                    new Npgsql.NpgsqlParameter("@soyad", soyad)
                );

                MessageBox.Show("Yazar başarıyla eklendi!");
                YazarEklendi?.Invoke(this, EventArgs.Empty); 
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("Ad ve Soyad alanları zorunludur.");
        }
    }
}
