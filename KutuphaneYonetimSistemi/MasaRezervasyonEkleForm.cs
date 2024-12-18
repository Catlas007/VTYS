using System;
using System.Windows.Forms;

public partial class MasaRezervasyonEkleForm : Form
{
    public event EventHandler RezervasyonEklendi; 

    private TextBox txtMasaID, txtUyeID, txtMisafirID;
    private Button btnEkle;

    public MasaRezervasyonEkleForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.txtMasaID = new TextBox();
        this.txtUyeID = new TextBox();
        this.txtMisafirID = new TextBox();
        this.btnEkle = new Button();

        // txtMasaID
        this.txtMasaID.Location = new System.Drawing.Point(20, 20);
        this.txtMasaID.Size = new System.Drawing.Size(200, 22);
        this.txtMasaID.PlaceholderText = "Masa ID";

        // txtUyeID
        this.txtUyeID.Location = new System.Drawing.Point(20, 60);
        this.txtUyeID.Size = new System.Drawing.Size(200, 22);
        this.txtUyeID.PlaceholderText = "Üye ID (Opsiyonel)";

        // txtMisafirID
        this.txtMisafirID.Location = new System.Drawing.Point(20, 100);
        this.txtMisafirID.Size = new System.Drawing.Size(200, 22);
        this.txtMisafirID.PlaceholderText = "Misafir ID (Opsiyonel)";

        // btnEkle
        this.btnEkle.Location = new System.Drawing.Point(20, 140);
        this.btnEkle.Size = new System.Drawing.Size(200, 30);
        this.btnEkle.Text = "Rezervasyon Ekle";
        this.btnEkle.Click += new EventHandler(this.btnEkle_Click);

        // Form
        this.Controls.Add(this.txtMasaID);
        this.Controls.Add(this.txtUyeID);
        this.Controls.Add(this.txtMisafirID);
        this.Controls.Add(this.btnEkle);
        this.ClientSize = new System.Drawing.Size(250, 200);
        this.Text = "Masa Rezervasyon Ekle";
    }

    private void btnEkle_Click(object sender, EventArgs e)
    {
        int masaID;
        int? uyeID = null, misafirID = null;

        if (int.TryParse(txtMasaID.Text, out masaID))
        {
            if (!string.IsNullOrEmpty(txtUyeID.Text)) uyeID = int.Parse(txtUyeID.Text);
            if (!string.IsNullOrEmpty(txtMisafirID.Text)) misafirID = int.Parse(txtMisafirID.Text);

            string sorgu = "INSERT INTO masarezervasyonlari (masa_id, uye_id, misafir_id) " +
                           $"VALUES ({masaID}, {uyeID?.ToString() ?? "NULL"}, {misafirID?.ToString() ?? "NULL"})";

            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
            yardimci.KomutCalistir(sorgu);

            RezervasyonEklendi?.Invoke(this, EventArgs.Empty); 
            MessageBox.Show("Masa rezervasyonu başarıyla eklendi.");
            this.Close();
        }
        else
        {
            MessageBox.Show("Geçerli bir Masa ID giriniz.");
        }
    }
}
