using System;
using System.Windows.Forms;
using Npgsql;

public partial class YayineviEkleForm : Form
{
    private TextBox txtAd, txtEmail;
    private Button btnEkle;

    public event EventHandler YayineviEklendi; // Event to notify the main form

    public YayineviEkleForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        txtAd = new TextBox { PlaceholderText = "Yayın Evi Adı", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(200, 22) };
        Controls.Add(txtAd);

        txtEmail = new TextBox { PlaceholderText = "E-mail", Location = new System.Drawing.Point(20, 60), Size = new System.Drawing.Size(200, 22) };
        Controls.Add(txtEmail);

        btnEkle = new Button { Text = "Yayın Evi Ekle", Location = new System.Drawing.Point(20, 100), Size = new System.Drawing.Size(200, 30) };
        btnEkle.Click += BtnEkle_Click;
        Controls.Add(btnEkle);

        this.Text = "Yayın Evi Ekle";
        this.ClientSize = new System.Drawing.Size(250, 150);
    }

    private void BtnEkle_Click(object sender, EventArgs e)
    {
        string ad = txtAd.Text.Trim();
        string email = txtEmail.Text.Trim();

        if (!string.IsNullOrEmpty(ad) && !string.IsNullOrEmpty(email))
        {
            try
            {
                string sorgu = "INSERT INTO yayinevleri (ad, email) VALUES (@ad, @email)";
                VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
                yardimci.KomutCalistir(
                    sorgu,
                    new Npgsql.NpgsqlParameter("@ad", ad),
                    new Npgsql.NpgsqlParameter("@email", email)
                );

                MessageBox.Show("Yayın Evi başarıyla eklendi!");
                YayineviEklendi?.Invoke(this, EventArgs.Empty); // Notify the main form
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("Lütfen tüm alanları doldurunuz.");
        }
    }
}
