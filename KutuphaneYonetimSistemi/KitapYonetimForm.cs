using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

public partial class KitapYonetimForm : Form
{
    private TextBox txtBaslik, txtYili;
    private ComboBox cmbYazar, cmbTur, cmbYayinEvi;
    private Button btnEkle, btnSil;
    private TextBox txtKitapIdSil;
    private Button btnYazarEkle, btnYayineviEkle;

    public KitapYonetimForm()
    {
        InitializeComponent();
        LoadComboBoxData();
    }

    private void InitializeComponent()
    {

        txtBaslik = new TextBox { PlaceholderText = "Kitap Başlığı", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(200, 22) };
        Controls.Add(txtBaslik);

        cmbYazar = new ComboBox { Location = new System.Drawing.Point(20, 60), Size = new System.Drawing.Size(200, 22), DropDownStyle = ComboBoxStyle.DropDownList };
        Controls.Add(cmbYazar);

        cmbTur = new ComboBox { Location = new System.Drawing.Point(20, 100), Size = new System.Drawing.Size(200, 22), DropDownStyle = ComboBoxStyle.DropDownList };
        Controls.Add(cmbTur);

        cmbYayinEvi = new ComboBox { Location = new System.Drawing.Point(20, 140), Size = new System.Drawing.Size(200, 22), DropDownStyle = ComboBoxStyle.DropDownList };
        Controls.Add(cmbYayinEvi);

        txtYili = new TextBox { PlaceholderText = "Yayın Yılı", Location = new System.Drawing.Point(20, 180), Size = new System.Drawing.Size(200, 22) };
        Controls.Add(txtYili);

        btnEkle = new Button { Text = "Kitap Ekle", Location = new System.Drawing.Point(20, 220), Size = new System.Drawing.Size(200, 30) };
        btnEkle.Click += BtnEkle_Click;
        Controls.Add(btnEkle);

        txtKitapIdSil = new TextBox { PlaceholderText = "Silinecek Kitap ID", Location = new System.Drawing.Point(20, 260), Size = new System.Drawing.Size(200, 22) };
        Controls.Add(txtKitapIdSil);

        btnSil = new Button { Text = "Kitap Sil", Location = new System.Drawing.Point(20, 300), Size = new System.Drawing.Size(200, 30) };
        btnSil.Click += BtnSil_Click;
        Controls.Add(btnSil);

        this.Text = "Kitap Yönetim";
        this.ClientSize = new System.Drawing.Size(300, 400);

        btnYazarEkle = new Button { Text = "Yazar Ekle", Location = new System.Drawing.Point(240, 60), Size = new System.Drawing.Size(120, 30) };
        btnYazarEkle.Click += BtnYazarEkle_Click;
        Controls.Add(btnYazarEkle);

        btnYayineviEkle = new Button { Text = "Yayın Evi Ekle", Location = new System.Drawing.Point(240, 140), Size = new System.Drawing.Size(120, 30) };
        btnYayineviEkle.Click += BtnYayineviEkle_Click;
        Controls.Add(btnYayineviEkle);
    }

    private void LoadComboBoxData()
    {
        try
        {
            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();

            
            string sorguYazarlar = "SELECT yazar_id, ad || ' ' || soyad AS ad_soyad FROM yazarlar";
            DataTable yazarlar = yardimci.SorguCalistir(sorguYazarlar);
            cmbYazar.DataSource = yazarlar;
            cmbYazar.DisplayMember = "ad_soyad";
            cmbYazar.ValueMember = "yazar_id";

            
            string sorguTurler = "SELECT tur_id, tur_adi FROM kitapturleri";
            DataTable turler = yardimci.SorguCalistir(sorguTurler);
            cmbTur.DataSource = turler;
            cmbTur.DisplayMember = "tur_adi";
            cmbTur.ValueMember = "tur_id";

           
            string sorguYayinEvleri = "SELECT yayin_id, ad FROM yayinevleri";
            DataTable yayinEvleri = yardimci.SorguCalistir(sorguYayinEvleri);
            cmbYayinEvi.DataSource = yayinEvleri;
            cmbYayinEvi.DisplayMember = "ad";
            cmbYayinEvi.ValueMember = "yayin_id";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Veriler yüklenirken hata oluştu: {ex.Message}");
        }
    }

    private void BtnEkle_Click(object sender, EventArgs e)
    {
        try
        {
            string baslik = txtBaslik.Text;
            int yazarId = (int)cmbYazar.SelectedValue;
            int turId = (int)cmbTur.SelectedValue;
            int yayinEviId = (int)cmbYayinEvi.SelectedValue;
            int yili = int.Parse(txtYili.Text);

            string sorgu = "INSERT INTO kitaplar (baslik, yazar_id, yayin_id, tur_id, yili) " +
                           "VALUES (@baslik, @yazar_id, @yayin_id, @tur_id, @yili)";

            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
            yardimci.KomutCalistir(sorgu,
                new NpgsqlParameter("@baslik", baslik),
                new NpgsqlParameter("@yazar_id", yazarId),
                new NpgsqlParameter("@yayin_id", yayinEviId),
                new NpgsqlParameter("@tur_id", turId),
                new NpgsqlParameter("@yili", yili)
            );

            MessageBox.Show("Kitap başarıyla eklendi!");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Hata oluştu: {ex.Message}");
        }
    }

    private void BtnSil_Click(object sender, EventArgs e)
    {
        try
        {
            int kitapId = int.Parse(txtKitapIdSil.Text);

            string sorgu = "DELETE FROM kitaplar WHERE kitap_id = @kitap_id";

            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
            yardimci.KomutCalistir(sorgu, new NpgsqlParameter("@kitap_id", kitapId));

            MessageBox.Show("Kitap başarıyla silindi!");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Hata oluştu: {ex.Message}");
        }
    }

    private void BtnYazarEkle_Click(object sender, EventArgs e)
    {
        YazarEkleForm yazarForm = new YazarEkleForm();
        yazarForm.YazarEklendi += (s, ev) => LoadComboBoxData(); 
        yazarForm.ShowDialog();
    }

    private void BtnYayineviEkle_Click(object sender, EventArgs e)
    {
        YayineviEkleForm yayineviForm = new YayineviEkleForm();
        yayineviForm.YayineviEklendi += (s, ev) => LoadComboBoxData(); 
        yayineviForm.ShowDialog();
    }
}
