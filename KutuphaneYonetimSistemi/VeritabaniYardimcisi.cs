using Npgsql;
using System;
using System.Data;

public class VeritabaniYardimcisi
{
    private const string BaglantiDizesi = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=KutuphaneYonetim";

    public NpgsqlConnection BaglantiAl()
    {
        return new NpgsqlConnection(BaglantiDizesi);
    }

   

    public DataTable SorguCalistir(string sorgu, params NpgsqlParameter[] parametreler)
    {
        using (var baglanti = BaglantiAl())
        {
            baglanti.Open();
            using (var komut = new NpgsqlCommand(sorgu, baglanti))
            {
                if (parametreler != null)
                {
                    komut.Parameters.AddRange(parametreler);
                }

                using (var adaptor = new NpgsqlDataAdapter(komut))
                {
                    DataTable veriTablosu = new DataTable();
                    adaptor.Fill(veriTablosu);
                    return veriTablosu;
                }
            }
        }
    }

    public int KomutCalistir(string sorgu, params NpgsqlParameter[] parametreler)
    {
        using (var baglanti = BaglantiAl())
        {
            baglanti.Open();
            using (var komut = new NpgsqlCommand(sorgu, baglanti))
            {
                if (parametreler != null)
                {
                    komut.Parameters.AddRange(parametreler);
                }
                return komut.ExecuteNonQuery();
            }
        }
    }
}
