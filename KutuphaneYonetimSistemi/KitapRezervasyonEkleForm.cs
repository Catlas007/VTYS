using System;
using System.Windows.Forms;

public partial class KitapRezervasyonEkleForm : Form
{
    public event EventHandler RezervasyonEklendi;

    private TextBox txtKitapID, txtKopyaNumarasi, txtUyeID;
    private Button btnEkle;

    public KitapRezervasyonEkleForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.txtKitapID = new TextBox();
        this.txtKopyaNumarasi = new TextBox();
        this.txtUyeID = new TextBox();
        this.btnEkle = new Button();

        // txtKitapID
        this.txtKitapID.Location = new System.Drawing.Point(20, 20);
        this.txtKitapID.Size = new System.Drawing.Size(200, 22);
        this.txtKitapID.PlaceholderText = "Kitap ID";

        // txtKopyaNumarasi
        this.txtKopyaNumarasi.Location = new System.Drawing.Point(20, 60);
        this.txtKopyaNumarasi.Size = new System.Drawing.Size(200, 22);
        this.txtKopyaNumarasi.PlaceholderText = "Kopya Numarası";

        // txtUyeID
        this.txtUyeID.Location = new System.Drawing.Point(20, 100);
        this.txtUyeID.Size = new System.Drawing.Size(200, 22);
        this.txtUyeID.PlaceholderText = "Üye ID";

        // btnEkle
        this.btnEkle.Location = new System.Drawing.Point(20, 140);
        this.btnEkle.Size = new System.Drawing.Size(200, 30);
        this.btnEkle.Text = "Rezervasyon Ekle";
        this.btnEkle.Click += new EventHandler(this.btnEkle_Click);

        // Form
        this.Controls.Add(this.txtKitapID);
        this.Controls.Add(this.txtKopyaNumarasi);
        this.Controls.Add(this.txtUyeID);
        this.Controls.Add(this.btnEkle);
        this.ClientSize = new System.Drawing.Size(250, 200);
        this.Text = "Kitap Rezervasyon Ekle";
    }

    private void btnEkle_Click(object sender, EventArgs e)
    {
        int kitapID, kopyaNumarasi, uyeID;

        if (int.TryParse(txtKitapID.Text, out kitapID) &&
            int.TryParse(txtKopyaNumarasi.Text, out kopyaNumarasi) &&
            int.TryParse(txtUyeID.Text, out uyeID))
        {
            string sorgu = $"INSERT INTO kitaprezervasyonlari (kitap_id, kopya_numarasi, uye_id) " +
                           $"VALUES ({kitapID}, {kopyaNumarasi}, {uyeID})";

            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
            yardimci.KomutCalistir(sorgu);

            RezervasyonEklendi?.Invoke(this, EventArgs.Empty); // Fire the event
            MessageBox.Show("Kitap rezervasyonu başarıyla eklendi.");
            this.Close();
        }
        else
        {
            MessageBox.Show("Tüm alanları doğru bir şekilde doldurunuz.");
        }
    }
}
