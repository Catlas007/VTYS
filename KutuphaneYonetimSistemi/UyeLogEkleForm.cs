using System;
using System.Windows.Forms;

public partial class UyeLogEkleForm : Form
{
    public UyeLogEkleForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.txtUyeID = new TextBox();
        this.btnEkle = new Button();

        // txtUyeID
        this.txtUyeID.Location = new System.Drawing.Point(20, 20);
        this.txtUyeID.Size = new System.Drawing.Size(200, 22);
        this.txtUyeID.PlaceholderText = "Üye ID";

        // btnEkle
        this.btnEkle.Location = new System.Drawing.Point(20, 60);
        this.btnEkle.Size = new System.Drawing.Size(200, 30);
        this.btnEkle.Text = "Ekle";
        this.btnEkle.Click += new EventHandler(this.btnEkle_Click);

        // UyeLogEkleForm
        this.Controls.Add(this.txtUyeID);
        this.Controls.Add(this.btnEkle);
        this.ClientSize = new System.Drawing.Size(250, 120);
        this.Text = "Üye Log Ekle";
    }

    private TextBox txtUyeID;
    private Button btnEkle;

    private void btnEkle_Click(object sender, EventArgs e)
    {
        int uyeID;
        if (int.TryParse(txtUyeID.Text, out uyeID))
        {
            VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
            string sorgu = $"INSERT INTO uye_log (uye_id, log_tarihi) VALUES ({uyeID}, NOW())";
            yardimci.KomutCalistir(sorgu);

            MessageBox.Show("Üye logu eklendi.");
            this.Close();
        }
        else
        {
            MessageBox.Show("Geçerli bir Üye ID giriniz.");
        }
    }
}
