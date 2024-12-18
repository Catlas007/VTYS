using System;
using System.Windows.Forms;
using Npgsql;

public partial class UyeDuzenleForm : Form
{
    private int uyeId;
    private TextBox txtAd, txtSoyad, txtEmail, txtTelefon;
    private Button btnKaydet;

    public UyeDuzenleForm(int uyeId)
    {
        this.uyeId = uyeId;
        InitializeComponent();
        LoadUyeDetails();
    }

    private void InitializeComponent()
    {
        txtAd = new TextBox { PlaceholderText = "Ad", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(200, 22) };
        Controls.Add(txtAd);

        txtSoyad = new TextBox { PlaceholderText = "Soyad", Location = new System.Drawing.Point(20, 60), Size = new System.Drawing.Size(200, 22) };
        Controls.Add(txtSoyad);

        txtEmail = new TextBox { PlaceholderText = "Email", Location = new System.Drawing.Point(20, 100), Size = new System.Drawing.Size(200, 22) };
        Controls.Add(txtEmail);

        txtTelefon = new TextBox { PlaceholderText = "Telefon", Location = new System.Drawing.Point(20, 140), Size = new System.Drawing.Size(200, 22) };
        Controls.Add(txtTelefon);

        btnKaydet = new Button { Text = "Kaydet", Location = new System.Drawing.Point(20, 180), Size = new System.Drawing.Size(200, 30) };
        btnKaydet.Click += BtnKaydet_Click;
        Controls.Add(btnKaydet);

        this.Text = "Üye Düzenle";
        this.ClientSize = new System.Drawing.Size(250, 250);
    }

    private void LoadUyeDetails()
    {
        VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
        string sorgu = "SELECT ad, soyad, email, phone FROM uyeler WHERE uye_id = @uye_id";
        var uyeDetails = yardimci.SorguCalistir(sorgu, new Npgsql.NpgsqlParameter("@uye_id", uyeId)).Rows[0];

        txtAd.Text = uyeDetails["ad"].ToString();
        txtSoyad.Text = uyeDetails["soyad"].ToString();
        txtEmail.Text = uyeDetails["email"].ToString();
        txtTelefon.Text = uyeDetails["phone"].ToString();
    }

    private void BtnKaydet_Click(object sender, EventArgs e)
    {
        string ad = txtAd.Text;
        string soyad = txtSoyad.Text;
        string email = txtEmail.Text;
        string telefon = txtTelefon.Text;

        if (!string.IsNullOrEmpty(ad) && !string.IsNullOrEmpty(soyad) &&
            !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(telefon))
        {
            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
            string sorgu = "UPDATE uyeler SET ad = @ad, soyad = @soyad, email = @email, phone = @phone WHERE uye_id = @uye_id";
            yardimci.KomutCalistir(
                sorgu,
                new Npgsql.NpgsqlParameter("@ad", ad),
                new Npgsql.NpgsqlParameter("@soyad", soyad),
                new Npgsql.NpgsqlParameter("@email", email),
                new Npgsql.NpgsqlParameter("@phone", telefon),
                new Npgsql.NpgsqlParameter("@uye_id", uyeId)
            );

            MessageBox.Show("Üye bilgileri başarıyla güncellendi.");
            this.Close();
        }
        else
        {
            MessageBox.Show("Lütfen tüm alanları doldurunuz.");
        }
    }
}
