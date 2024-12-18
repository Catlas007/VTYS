using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace KutuphaneYonetimSistemi
{
    public partial class AnaForm : Form  
    {
        private System.Windows.Forms.Button btnListeleKitapKopyalari;

            public AnaForm()
            {
                InitializeComponent();
            }

            private void ClearOldDataGridView()
            {   
                dataGridViewKitaplar.DataSource = null;
                dataGridViewKitaplar.Refresh();
            }

            private void btnKitaplariYukle_Click(object sender, EventArgs e)
            {
                VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
                string sorgu = @"
                    SELECT 
                        k.kitap_id AS ""ID"",
                        k.baslik AS ""Kitap Başlığı"",
                        y.ad || ' ' || y.soyad AS ""Yazar"",
                        t.tur_adi AS ""Tür"",
                        ye.ad AS ""Yayın Evi"",
                        k.yili AS ""Yayın Yılı""
                    FROM 
                        kitaplar k
                    JOIN 
                        yazarlar y ON k.yazar_id = y.yazar_id
                    JOIN 
                        kitapturleri t ON k.tur_id = t.tur_id
                    JOIN 
                        yayinevleri ye ON k.yayin_id = ye.yayin_id";

                
                DataTable kitapListesi = yardimci.SorguCalistir(sorgu);

                
                dataGridViewKitaplar.DataSource = kitapListesi;
                dataGridViewKitaplar.Refresh();
            }

            private void btnListeleKitapKopyalari_Click(object sender, EventArgs e)
            {
                VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();

                string sorgu = @"
                    SELECT 
                        k.kitap_id AS ""Kitap ID"",
                        k.baslik AS ""Kitap Adı"",
                        y.ad || ' ' || y.soyad AS ""Yazar"",
                        kk.kopya_numarasi AS ""Kopya Numarası"",
                        CASE 
                            WHEN kk.uygunluk = true THEN 'Uygun'
                            ELSE 'Uygun Değil'
                        END AS ""Durum""
                    FROM 
                        kitapkopyalari kk
                    INNER JOIN 
                        kitaplar k ON kk.kitap_id = k.kitap_id
                    INNER JOIN 
                        yazarlar y ON k.yazar_id = y.yazar_id
                    ORDER BY 
                        k.baslik, kk.kopya_numarasi;";

                
                DataTable kitapKopyalari = yardimci.SorguCalistir(sorgu);
                dataGridViewKitaplar.DataSource = kitapKopyalari;
                dataGridViewKitaplar.Refresh();
            }

            private void btnUyeListesi_Click(object sender, EventArgs e)
            {
                VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
                string sorgu = "SELECT * FROM uyeler"; // Query for members
                DataTable uyeler = yardimci.SorguCalistir(sorgu);
                dataGridViewKitaplar.DataSource = uyeler; 
                dataGridViewKitaplar.Refresh();
            }

            private void btnUyeSil_Click(object sender, EventArgs e)
            {
                if (int.TryParse(txtUyeIdSil.Text, out int uyeId))
                {
                    VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();

                    try
                    {
                        string sorgu = "DELETE FROM uyeler WHERE uye_id = @uye_id";
                        int affectedRows = yardimci.KomutCalistir(sorgu, new NpgsqlParameter("@uye_id", uyeId));

                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Üye başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Üye bulunamadı. Geçerli bir Üye ID giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        
                        btnUyeListesi_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen geçerli bir Üye ID giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            private void btnMisafirListesi_Click(object sender, EventArgs e)
            {
                VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
                string sorgu = "SELECT * FROM misafirler"; // Query for guests
                DataTable misafirler = yardimci.SorguCalistir(sorgu);
                dataGridViewKitaplar.DataSource = misafirler; 
                dataGridViewKitaplar.Refresh();
            }

            private void btnRezervasyonlar_Click(object sender, EventArgs e)
            {
                RezervasyonlarForm rezervasyonlarForm = new RezervasyonlarForm();
                rezervasyonlarForm.Show(); 
            }

            private void btnUyeLogEkle_Click(object sender, EventArgs e)
            {
                UyeLogEkleForm uyeForm = new UyeLogEkleForm();
                uyeForm.ShowDialog();
            }

            private void btnYeniUyeEkle_Click(object sender, EventArgs e)
            {
                UyeEkleForm uyeEkleForm = new UyeEkleForm();
                uyeEkleForm.ShowDialog();
            }

            private void btnMisafirLogEkle_Click(object sender, EventArgs e)
            {
                MisafirLogEkleForm misafirForm = new MisafirLogEkleForm();
                misafirForm.ShowDialog();
            }

            private void btnCalisanlarYonet_Click(object sender, EventArgs e)
            {
                CalisanlarForm calisanlarForm = new CalisanlarForm();
                calisanlarForm.ShowDialog();
            }

            private void btnKitapYonetim_Click(object sender, EventArgs e)
            {
                KitapYonetimForm kitapForm = new KitapYonetimForm();
                kitapForm.ShowDialog();
            }

            private void BtnKitapAra_Click(object sender, EventArgs e)
            {
                string kitapAdi = txtKitapAra.Text.Trim();

                if (string.IsNullOrEmpty(kitapAdi))
                {
                    MessageBox.Show("Lütfen aramak istediğiniz kitabın adını giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    VeritabaniYardimcisi yardimci = new VeritabaniYardimcisi();
                    string sorgu = @"
                        SELECT 
                            k.kitap_id AS ""Kitap ID"",
                            k.baslik AS ""Kitap Başlığı"",
                            y.ad || ' ' || y.soyad AS ""Yazar"",
                            t.tur_adi AS ""Tür"",
                            ye.ad AS ""Yayın Evi"",
                            k.yili AS ""Yayın Yılı"",
                            kk.kopya_numarasi AS ""Kopya Numarası"",
                            CASE
                                WHEN kk.uygunluk = TRUE THEN 'Uygun'
                                ELSE 'Uygun Değil'
                            END AS ""Durum""
                        FROM 
                            kitaplar k
                        JOIN 
                            yazarlar y ON k.yazar_id = y.yazar_id
                        JOIN 
                            kitapturleri t ON k.tur_id = t.tur_id
                        JOIN 
                            yayinevleri ye ON k.yayin_id = ye.yayin_id
                        LEFT JOIN 
                            kitapkopyalari kk ON k.kitap_id = kk.kitap_id
                        WHERE 
                            k.baslik ILIKE @kitapAdi
                        ORDER BY 
                            k.kitap_id, kk.kopya_numarasi";

                    
                    DataTable sonuc = yardimci.SorguCalistir(sorgu, new NpgsqlParameter("@kitapAdi", "%" + kitapAdi + "%"));

                    if (sonuc.Rows.Count > 0)
                    {
                        dataGridViewKitaplar.DataSource = sonuc;
                    }
                    else
                    {
                        MessageBox.Show("Kitap bulunamadı.", "Sonuç", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
            }

            private void btnEditUye_Click(object sender, EventArgs e)
            {
                if (dataGridViewKitaplar.SelectedRows.Count > 0)
                {
                    int uyeId = Convert.ToInt32(dataGridViewKitaplar.SelectedRows[0].Cells["uye_id"].Value);
                    UyeDuzenleForm uyeDuzenleForm = new UyeDuzenleForm(uyeId);
                    uyeDuzenleForm.ShowDialog();

                    
                    btnUyeListesi_Click(null, null);
                }
                else
                {
                    MessageBox.Show("Lütfen bir üye seçiniz.");
                }
            }






    }
}