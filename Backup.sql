--
-- PostgreSQL database dump
--

-- Dumped from database version 15.10
-- Dumped by pg_dump version 15.10

-- Started on 2024-12-17 18:10:34

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 252 (class 1255 OID 17021)
-- Name: iade_edildiginde_uygunluk(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.iade_edildiginde_uygunluk() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE kitapkopyalari
    SET uygunluk = TRUE
    WHERE kitap_id = OLD.kitap_id AND kopya_numarasi = OLD.kopya_numarasi;
    RETURN OLD;
END;
$$;


ALTER FUNCTION public.iade_edildiginde_uygunluk() OWNER TO postgres;

--
-- TOC entry 251 (class 1255 OID 17047)
-- Name: kiralama_uygunluk_guncelle(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.kiralama_uygunluk_guncelle() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE kitapkopyalari
    SET uygunluk = FALSE
    WHERE kitap_id = NEW.kitap_id AND kopya_numarasi = NEW.kopya_numarasi;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.kiralama_uygunluk_guncelle() OWNER TO postgres;

--
-- TOC entry 249 (class 1255 OID 17019)
-- Name: kiralama_uygunluk_kontrolu(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.kiralama_uygunluk_kontrolu() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF (SELECT uygunluk FROM kitapkopyalari WHERE kitap_id = NEW.kitap_id AND kopya_numarasi = NEW.kopya_numarasi) = FALSE THEN
        RAISE EXCEPTION 'Kitap kopyası uygun değil.';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.kiralama_uygunluk_kontrolu() OWNER TO postgres;

--
-- TOC entry 247 (class 1255 OID 17008)
-- Name: kitap_ekle(character varying, integer, integer, integer, integer, integer); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.kitap_ekle(IN p_baslik character varying, IN p_yili integer, IN p_yayin_id integer, IN p_tur_id integer, IN p_yazar_id integer, IN p_kopya_sayisi integer)
    LANGUAGE plpgsql
    AS $$
DECLARE
    yeni_kitap_id INT;
BEGIN
    INSERT INTO kitaplar (baslik, yili, yayin_id, tur_id, yazar_id)
    VALUES (p_baslik, p_yili, p_yayin_id, p_tur_id, p_yazar_id)
    RETURNING kitap_id INTO yeni_kitap_id;

    FOR i IN 1..p_kopya_sayisi LOOP
        INSERT INTO kitapkopyalari (kitap_id, kopya_numarasi, uygunluk)
        VALUES (yeni_kitap_id, DEFAULT, TRUE);
    END LOOP;
END;
$$;


ALTER PROCEDURE public.kitap_ekle(IN p_baslik character varying, IN p_yili integer, IN p_yayin_id integer, IN p_tur_id integer, IN p_yazar_id integer, IN p_kopya_sayisi integer) OWNER TO postgres;

--
-- TOC entry 246 (class 1255 OID 17007)
-- Name: kitap_iade_et(integer); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.kitap_iade_et(IN p_kira_id integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE kiralamalar
    SET teslim_tarihi = CURRENT_DATE
    WHERE kira_id = p_kira_id AND teslim_tarihi IS NULL;

    UPDATE kitapkopyalari
    SET uygunluk = TRUE
    WHERE kitap_id = (SELECT kitap_id FROM kiralamalar WHERE kira_id = p_kira_id)
      AND kopya_numarasi = (SELECT kopya_numarasi FROM kiralamalar WHERE kira_id = p_kira_id);
END;
$$;


ALTER PROCEDURE public.kitap_iade_et(IN p_kira_id integer) OWNER TO postgres;

--
-- TOC entry 245 (class 1255 OID 17006)
-- Name: kitap_kirala(integer, integer, integer); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.kitap_kirala(IN p_kitap_id integer, IN p_kopya_numarasi integer, IN p_uye_id integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM kitapkopyalari 
        WHERE kitap_id = p_kitap_id AND kopya_numarasi = p_kopya_numarasi AND uygunluk = TRUE
    ) THEN
        RAISE EXCEPTION 'Kitap kopyası uygun değil.';
    ELSE
        INSERT INTO kiralamalar (kitap_id, kopya_numarasi, uye_id, verilme_tarihi) 
        VALUES (p_kitap_id, p_kopya_numarasi, p_uye_id, CURRENT_DATE);

        UPDATE kitapkopyalari 
        SET uygunluk = FALSE 
        WHERE kitap_id = p_kitap_id AND kopya_numarasi = p_kopya_numarasi;
    END IF;
END;
$$;


ALTER PROCEDURE public.kitap_kirala(IN p_kitap_id integer, IN p_kopya_numarasi integer, IN p_uye_id integer) OWNER TO postgres;

--
-- TOC entry 255 (class 1255 OID 17078)
-- Name: kitap_kopya_durum_guncelle_delete(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.kitap_kopya_durum_guncelle_delete() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE kitapkopyalari
    SET uygunluk = true
    WHERE kitap_id = OLD.kitap_id AND kopya_numarasi = OLD.kopya_numarasi;
    RETURN OLD;
END;
$$;


ALTER FUNCTION public.kitap_kopya_durum_guncelle_delete() OWNER TO postgres;

--
-- TOC entry 254 (class 1255 OID 17076)
-- Name: kitap_kopya_durum_guncelle_insert(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.kitap_kopya_durum_guncelle_insert() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE kitapkopyalari
    SET uygunluk = false
    WHERE kitap_id = NEW.kitap_id AND kopya_numarasi = NEW.kopya_numarasi;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.kitap_kopya_durum_guncelle_insert() OWNER TO postgres;

--
-- TOC entry 253 (class 1255 OID 17050)
-- Name: masa_uygunluk_guncelle(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.masa_uygunluk_guncelle() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        UPDATE masalar
        SET uygunluk = FALSE
        WHERE masa_id = NEW.masa_id;
    ELSIF TG_OP = 'DELETE' THEN
        IF NOT EXISTS (
            SELECT 1 
            FROM masarezervasyonlari 
            WHERE masa_id = OLD.masa_id
        ) THEN
            UPDATE masalar
            SET uygunluk = TRUE
            WHERE masa_id = OLD.masa_id;
        END IF;
    END IF;
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.masa_uygunluk_guncelle() OWNER TO postgres;

--
-- TOC entry 250 (class 1255 OID 17023)
-- Name: rezervasyon_tekrari_engelle(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.rezervasyon_tekrari_engelle() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM kitaprezervasyonlari
        WHERE kitap_id = NEW.kitap_id AND kopya_numarasi = NEW.kopya_numarasi AND uye_id = NEW.uye_id
    ) THEN
        RAISE EXCEPTION 'Aynı kitap kopyası için birden fazla rezervasyon yapılamaz.';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.rezervasyon_tekrari_engelle() OWNER TO postgres;

--
-- TOC entry 244 (class 1255 OID 17005)
-- Name: uye_ekle(character varying, character varying, character varying, character varying); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.uye_ekle(IN p_ad character varying, IN p_soyad character varying, IN p_email character varying, IN p_telefon character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF EXISTS (SELECT 1 FROM uyeler WHERE email = p_email) THEN
        RAISE EXCEPTION 'E-posta % ile zaten bir üye var.', p_email;
    ELSE
        INSERT INTO uyeler (ad, soyad, email, phone) VALUES (p_ad, p_soyad, p_email, p_telefon);
    END IF;
END;
$$;


ALTER PROCEDURE public.uye_ekle(IN p_ad character varying, IN p_soyad character varying, IN p_email character varying, IN p_telefon character varying) OWNER TO postgres;

--
-- TOC entry 248 (class 1255 OID 17017)
-- Name: yeni_uye_gunlugu(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.yeni_uye_gunlugu() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO uye_log (uye_id) VALUES (NEW.uye_id);
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.yeni_uye_gunlugu() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 215 (class 1259 OID 16823)
-- Name: calisanlar; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.calisanlar (
    calisan_id integer NOT NULL,
    ad character varying(100) NOT NULL,
    soyad character varying(100) NOT NULL,
    email character varying(100) NOT NULL,
    phone character varying(15),
    sifre character varying(255)
);


ALTER TABLE public.calisanlar OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 16822)
-- Name: calisanlar_calisan_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.calisanlar_calisan_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.calisanlar_calisan_id_seq OWNER TO postgres;

--
-- TOC entry 3514 (class 0 OID 0)
-- Dependencies: 214
-- Name: calisanlar_calisan_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.calisanlar_calisan_id_seq OWNED BY public.calisanlar.calisan_id;


--
-- TOC entry 235 (class 1259 OID 16932)
-- Name: kiralamalar; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.kiralamalar (
    kira_id integer NOT NULL,
    kitap_id integer NOT NULL,
    kopya_numarasi integer NOT NULL,
    uye_id integer NOT NULL,
    verilme_tarihi date NOT NULL,
    teslim_tarihi date,
    kutuphaneci_id integer
);


ALTER TABLE public.kiralamalar OWNER TO postgres;

--
-- TOC entry 234 (class 1259 OID 16931)
-- Name: kiralamalar_kira_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.kiralamalar_kira_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.kiralamalar_kira_id_seq OWNER TO postgres;

--
-- TOC entry 3515 (class 0 OID 0)
-- Dependencies: 234
-- Name: kiralamalar_kira_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.kiralamalar_kira_id_seq OWNED BY public.kiralamalar.kira_id;


--
-- TOC entry 231 (class 1259 OID 16911)
-- Name: kitapkopyalari; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.kitapkopyalari (
    kitap_id integer NOT NULL,
    kopya_numarasi integer NOT NULL,
    uygunluk boolean DEFAULT true
);


ALTER TABLE public.kitapkopyalari OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 16910)
-- Name: kitapkopyalari_kopya_numarasi_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.kitapkopyalari_kopya_numarasi_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.kitapkopyalari_kopya_numarasi_seq OWNER TO postgres;

--
-- TOC entry 3516 (class 0 OID 0)
-- Dependencies: 230
-- Name: kitapkopyalari_kopya_numarasi_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.kitapkopyalari_kopya_numarasi_seq OWNED BY public.kitapkopyalari.kopya_numarasi;


--
-- TOC entry 229 (class 1259 OID 16889)
-- Name: kitaplar; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.kitaplar (
    kitap_id integer NOT NULL,
    baslik character varying(100) NOT NULL,
    yili integer,
    yayin_id integer,
    tur_id integer,
    yazar_id integer
);


ALTER TABLE public.kitaplar OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 16888)
-- Name: kitaplar_kitap_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.kitaplar_kitap_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.kitaplar_kitap_id_seq OWNER TO postgres;

--
-- TOC entry 3517 (class 0 OID 0)
-- Dependencies: 228
-- Name: kitaplar_kitap_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.kitaplar_kitap_id_seq OWNED BY public.kitaplar.kitap_id;


--
-- TOC entry 239 (class 1259 OID 16985)
-- Name: kitaprezervasyonlari; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.kitaprezervasyonlari (
    kitap_id integer NOT NULL,
    kopya_numarasi integer NOT NULL,
    uye_id integer NOT NULL,
    kitap_rezervasyon_id integer NOT NULL
);


ALTER TABLE public.kitaprezervasyonlari OWNER TO postgres;

--
-- TOC entry 243 (class 1259 OID 17067)
-- Name: kitaprezervasyonlari_kitap_rezervasyon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.kitaprezervasyonlari_kitap_rezervasyon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.kitaprezervasyonlari_kitap_rezervasyon_id_seq OWNER TO postgres;

--
-- TOC entry 3518 (class 0 OID 0)
-- Dependencies: 243
-- Name: kitaprezervasyonlari_kitap_rezervasyon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.kitaprezervasyonlari_kitap_rezervasyon_id_seq OWNED BY public.kitaprezervasyonlari.kitap_rezervasyon_id;


--
-- TOC entry 227 (class 1259 OID 16882)
-- Name: kitapturleri; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.kitapturleri (
    tur_id integer NOT NULL,
    tur_adi character varying(100) NOT NULL
);


ALTER TABLE public.kitapturleri OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 16881)
-- Name: kitapturleri_tur_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.kitapturleri_tur_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.kitapturleri_tur_id_seq OWNER TO postgres;

--
-- TOC entry 3519 (class 0 OID 0)
-- Dependencies: 226
-- Name: kitapturleri_tur_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.kitapturleri_tur_id_seq OWNED BY public.kitapturleri.tur_id;


--
-- TOC entry 217 (class 1259 OID 16841)
-- Name: kutuphaneciler; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.kutuphaneciler (
    kutuphaneci_id integer NOT NULL,
    yonetici_id integer
);


ALTER TABLE public.kutuphaneciler OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 16924)
-- Name: masalar; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.masalar (
    masa_id integer NOT NULL,
    uygunluk boolean DEFAULT true
);


ALTER TABLE public.masalar OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 16923)
-- Name: masalar_masa_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.masalar_masa_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.masalar_masa_id_seq OWNER TO postgres;

--
-- TOC entry 3520 (class 0 OID 0)
-- Dependencies: 232
-- Name: masalar_masa_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.masalar_masa_id_seq OWNED BY public.masalar.masa_id;


--
-- TOC entry 238 (class 1259 OID 16960)
-- Name: masarezervasyonlari; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.masarezervasyonlari (
    masa_id integer NOT NULL,
    uye_id integer,
    misafir_id integer,
    masa_rezervasyon_id integer NOT NULL
);


ALTER TABLE public.masarezervasyonlari OWNER TO postgres;

--
-- TOC entry 242 (class 1259 OID 17059)
-- Name: masarezervasyonlari_masa_rezervasyon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.masarezervasyonlari_masa_rezervasyon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.masarezervasyonlari_masa_rezervasyon_id_seq OWNER TO postgres;

--
-- TOC entry 3521 (class 0 OID 0)
-- Dependencies: 242
-- Name: masarezervasyonlari_masa_rezervasyon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.masarezervasyonlari_masa_rezervasyon_id_seq OWNED BY public.masarezervasyonlari.masa_rezervasyon_id;


--
-- TOC entry 221 (class 1259 OID 16861)
-- Name: misafirler; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.misafirler (
    misafir_id integer NOT NULL,
    ad character varying(100) NOT NULL,
    soyad character varying(100) NOT NULL,
    phone character varying(15) NOT NULL,
    ziyaret_tarihi date NOT NULL
);


ALTER TABLE public.misafirler OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 16860)
-- Name: misafirler_misafir_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.misafirler_misafir_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.misafirler_misafir_id_seq OWNER TO postgres;

--
-- TOC entry 3522 (class 0 OID 0)
-- Dependencies: 220
-- Name: misafirler_misafir_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.misafirler_misafir_id_seq OWNED BY public.misafirler.misafir_id;


--
-- TOC entry 237 (class 1259 OID 16949)
-- Name: rezervasyonlar; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.rezervasyonlar (
    rezervasyon_id integer NOT NULL,
    uye_id integer,
    rezervasyon_tarihi date NOT NULL,
    kutuphaneci_id integer
);


ALTER TABLE public.rezervasyonlar OWNER TO postgres;

--
-- TOC entry 236 (class 1259 OID 16948)
-- Name: rezervasyonlar_rezervasyon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.rezervasyonlar_rezervasyon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.rezervasyonlar_rezervasyon_id_seq OWNER TO postgres;

--
-- TOC entry 3523 (class 0 OID 0)
-- Dependencies: 236
-- Name: rezervasyonlar_rezervasyon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.rezervasyonlar_rezervasyon_id_seq OWNED BY public.rezervasyonlar.rezervasyon_id;


--
-- TOC entry 241 (class 1259 OID 17010)
-- Name: uye_log; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.uye_log (
    log_id integer NOT NULL,
    uye_id integer,
    log_tarihi timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.uye_log OWNER TO postgres;

--
-- TOC entry 240 (class 1259 OID 17009)
-- Name: uye_log_log_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.uye_log_log_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.uye_log_log_id_seq OWNER TO postgres;

--
-- TOC entry 3524 (class 0 OID 0)
-- Dependencies: 240
-- Name: uye_log_log_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.uye_log_log_id_seq OWNED BY public.uye_log.log_id;


--
-- TOC entry 219 (class 1259 OID 16852)
-- Name: uyeler; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.uyeler (
    uye_id integer NOT NULL,
    ad character varying(100) NOT NULL,
    soyad character varying(100) NOT NULL,
    email character varying(100) NOT NULL,
    phone character varying(15)
);


ALTER TABLE public.uyeler OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 16851)
-- Name: uyeler_uye_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.uyeler_uye_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.uyeler_uye_id_seq OWNER TO postgres;

--
-- TOC entry 3525 (class 0 OID 0)
-- Dependencies: 218
-- Name: uyeler_uye_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.uyeler_uye_id_seq OWNED BY public.uyeler.uye_id;


--
-- TOC entry 225 (class 1259 OID 16875)
-- Name: yayinevleri; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.yayinevleri (
    yayin_id integer NOT NULL,
    ad character varying(100) NOT NULL,
    email character varying(100) NOT NULL
);


ALTER TABLE public.yayinevleri OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 16874)
-- Name: yayinevleri_yayin_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.yayinevleri_yayin_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.yayinevleri_yayin_id_seq OWNER TO postgres;

--
-- TOC entry 3526 (class 0 OID 0)
-- Dependencies: 224
-- Name: yayinevleri_yayin_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.yayinevleri_yayin_id_seq OWNED BY public.yayinevleri.yayin_id;


--
-- TOC entry 223 (class 1259 OID 16868)
-- Name: yazarlar; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.yazarlar (
    yazar_id integer NOT NULL,
    ad character varying(100) NOT NULL,
    soyad character varying(100) NOT NULL
);


ALTER TABLE public.yazarlar OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 16867)
-- Name: yazarlar_yazar_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.yazarlar_yazar_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.yazarlar_yazar_id_seq OWNER TO postgres;

--
-- TOC entry 3527 (class 0 OID 0)
-- Dependencies: 222
-- Name: yazarlar_yazar_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.yazarlar_yazar_id_seq OWNED BY public.yazarlar.yazar_id;


--
-- TOC entry 216 (class 1259 OID 16831)
-- Name: yoneticiler; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.yoneticiler (
    yonetici_id integer NOT NULL
);


ALTER TABLE public.yoneticiler OWNER TO postgres;

--
-- TOC entry 3258 (class 2604 OID 16826)
-- Name: calisanlar calisan_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.calisanlar ALTER COLUMN calisan_id SET DEFAULT nextval('public.calisanlar_calisan_id_seq'::regclass);


--
-- TOC entry 3269 (class 2604 OID 16935)
-- Name: kiralamalar kira_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kiralamalar ALTER COLUMN kira_id SET DEFAULT nextval('public.kiralamalar_kira_id_seq'::regclass);


--
-- TOC entry 3265 (class 2604 OID 16914)
-- Name: kitapkopyalari kopya_numarasi; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitapkopyalari ALTER COLUMN kopya_numarasi SET DEFAULT nextval('public.kitapkopyalari_kopya_numarasi_seq'::regclass);


--
-- TOC entry 3264 (class 2604 OID 16892)
-- Name: kitaplar kitap_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitaplar ALTER COLUMN kitap_id SET DEFAULT nextval('public.kitaplar_kitap_id_seq'::regclass);


--
-- TOC entry 3272 (class 2604 OID 17068)
-- Name: kitaprezervasyonlari kitap_rezervasyon_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitaprezervasyonlari ALTER COLUMN kitap_rezervasyon_id SET DEFAULT nextval('public.kitaprezervasyonlari_kitap_rezervasyon_id_seq'::regclass);


--
-- TOC entry 3263 (class 2604 OID 16885)
-- Name: kitapturleri tur_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitapturleri ALTER COLUMN tur_id SET DEFAULT nextval('public.kitapturleri_tur_id_seq'::regclass);


--
-- TOC entry 3267 (class 2604 OID 16927)
-- Name: masalar masa_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.masalar ALTER COLUMN masa_id SET DEFAULT nextval('public.masalar_masa_id_seq'::regclass);


--
-- TOC entry 3271 (class 2604 OID 17060)
-- Name: masarezervasyonlari masa_rezervasyon_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.masarezervasyonlari ALTER COLUMN masa_rezervasyon_id SET DEFAULT nextval('public.masarezervasyonlari_masa_rezervasyon_id_seq'::regclass);


--
-- TOC entry 3260 (class 2604 OID 16864)
-- Name: misafirler misafir_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.misafirler ALTER COLUMN misafir_id SET DEFAULT nextval('public.misafirler_misafir_id_seq'::regclass);


--
-- TOC entry 3270 (class 2604 OID 16952)
-- Name: rezervasyonlar rezervasyon_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyonlar ALTER COLUMN rezervasyon_id SET DEFAULT nextval('public.rezervasyonlar_rezervasyon_id_seq'::regclass);


--
-- TOC entry 3273 (class 2604 OID 17013)
-- Name: uye_log log_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.uye_log ALTER COLUMN log_id SET DEFAULT nextval('public.uye_log_log_id_seq'::regclass);


--
-- TOC entry 3259 (class 2604 OID 16855)
-- Name: uyeler uye_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.uyeler ALTER COLUMN uye_id SET DEFAULT nextval('public.uyeler_uye_id_seq'::regclass);


--
-- TOC entry 3262 (class 2604 OID 16878)
-- Name: yayinevleri yayin_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.yayinevleri ALTER COLUMN yayin_id SET DEFAULT nextval('public.yayinevleri_yayin_id_seq'::regclass);


--
-- TOC entry 3261 (class 2604 OID 16871)
-- Name: yazarlar yazar_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.yazarlar ALTER COLUMN yazar_id SET DEFAULT nextval('public.yazarlar_yazar_id_seq'::regclass);


--
-- TOC entry 3276 (class 2606 OID 16830)
-- Name: calisanlar calisanlar_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.calisanlar
    ADD CONSTRAINT calisanlar_email_key UNIQUE (email);


--
-- TOC entry 3278 (class 2606 OID 16828)
-- Name: calisanlar calisanlar_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.calisanlar
    ADD CONSTRAINT calisanlar_pkey PRIMARY KEY (calisan_id);


--
-- TOC entry 3302 (class 2606 OID 16937)
-- Name: kiralamalar kiralamalar_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kiralamalar
    ADD CONSTRAINT kiralamalar_pkey PRIMARY KEY (kira_id);


--
-- TOC entry 3298 (class 2606 OID 16917)
-- Name: kitapkopyalari kitapkopyalari_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitapkopyalari
    ADD CONSTRAINT kitapkopyalari_pkey PRIMARY KEY (kitap_id, kopya_numarasi);


--
-- TOC entry 3296 (class 2606 OID 16894)
-- Name: kitaplar kitaplar_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitaplar
    ADD CONSTRAINT kitaplar_pkey PRIMARY KEY (kitap_id);


--
-- TOC entry 3308 (class 2606 OID 17070)
-- Name: kitaprezervasyonlari kitaprezervasyonlari_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitaprezervasyonlari
    ADD CONSTRAINT kitaprezervasyonlari_pkey PRIMARY KEY (kitap_rezervasyon_id);


--
-- TOC entry 3294 (class 2606 OID 16887)
-- Name: kitapturleri kitapturleri_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitapturleri
    ADD CONSTRAINT kitapturleri_pkey PRIMARY KEY (tur_id);


--
-- TOC entry 3282 (class 2606 OID 16845)
-- Name: kutuphaneciler kutuphaneciler_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kutuphaneciler
    ADD CONSTRAINT kutuphaneciler_pkey PRIMARY KEY (kutuphaneci_id);


--
-- TOC entry 3300 (class 2606 OID 16930)
-- Name: masalar masalar_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.masalar
    ADD CONSTRAINT masalar_pkey PRIMARY KEY (masa_id);


--
-- TOC entry 3306 (class 2606 OID 17062)
-- Name: masarezervasyonlari masarezervasyonlari_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.masarezervasyonlari
    ADD CONSTRAINT masarezervasyonlari_pkey PRIMARY KEY (masa_rezervasyon_id);


--
-- TOC entry 3288 (class 2606 OID 16866)
-- Name: misafirler misafirler_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.misafirler
    ADD CONSTRAINT misafirler_pkey PRIMARY KEY (misafir_id);


--
-- TOC entry 3304 (class 2606 OID 16954)
-- Name: rezervasyonlar rezervasyonlar_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyonlar
    ADD CONSTRAINT rezervasyonlar_pkey PRIMARY KEY (rezervasyon_id);


--
-- TOC entry 3310 (class 2606 OID 17016)
-- Name: uye_log uye_log_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.uye_log
    ADD CONSTRAINT uye_log_pkey PRIMARY KEY (log_id);


--
-- TOC entry 3284 (class 2606 OID 16859)
-- Name: uyeler uyeler_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.uyeler
    ADD CONSTRAINT uyeler_email_key UNIQUE (email);


--
-- TOC entry 3286 (class 2606 OID 16857)
-- Name: uyeler uyeler_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.uyeler
    ADD CONSTRAINT uyeler_pkey PRIMARY KEY (uye_id);


--
-- TOC entry 3292 (class 2606 OID 16880)
-- Name: yayinevleri yayinevleri_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.yayinevleri
    ADD CONSTRAINT yayinevleri_pkey PRIMARY KEY (yayin_id);


--
-- TOC entry 3290 (class 2606 OID 16873)
-- Name: yazarlar yazarlar_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.yazarlar
    ADD CONSTRAINT yazarlar_pkey PRIMARY KEY (yazar_id);


--
-- TOC entry 3280 (class 2606 OID 16835)
-- Name: yoneticiler yoneticiler_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.yoneticiler
    ADD CONSTRAINT yoneticiler_pkey PRIMARY KEY (yonetici_id);


--
-- TOC entry 3329 (class 2620 OID 17049)
-- Name: kiralamalar iade_sonrasi_tetikleyici; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER iade_sonrasi_tetikleyici AFTER UPDATE OF teslim_tarihi ON public.kiralamalar FOR EACH ROW WHEN (((old.teslim_tarihi IS NULL) AND (new.teslim_tarihi IS NOT NULL))) EXECUTE FUNCTION public.iade_edildiginde_uygunluk();


--
-- TOC entry 3330 (class 2620 OID 17020)
-- Name: kiralamalar kiralama_oncesi_tetikleyici; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER kiralama_oncesi_tetikleyici BEFORE INSERT ON public.kiralamalar FOR EACH ROW EXECUTE FUNCTION public.kiralama_uygunluk_kontrolu();


--
-- TOC entry 3331 (class 2620 OID 17048)
-- Name: kiralamalar kiralama_sonrasi_tetikleyici; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER kiralama_sonrasi_tetikleyici AFTER INSERT ON public.kiralamalar FOR EACH ROW EXECUTE FUNCTION public.kiralama_uygunluk_guncelle();


--
-- TOC entry 3332 (class 2620 OID 17051)
-- Name: masarezervasyonlari masa_rezervasyon_eklendi; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER masa_rezervasyon_eklendi AFTER INSERT ON public.masarezervasyonlari FOR EACH ROW EXECUTE FUNCTION public.masa_uygunluk_guncelle();


--
-- TOC entry 3333 (class 2620 OID 17052)
-- Name: masarezervasyonlari masa_rezervasyon_silindi; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER masa_rezervasyon_silindi AFTER DELETE ON public.masarezervasyonlari FOR EACH ROW EXECUTE FUNCTION public.masa_uygunluk_guncelle();


--
-- TOC entry 3334 (class 2620 OID 17024)
-- Name: kitaprezervasyonlari rezervasyon_oncesi_tetikleyici; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER rezervasyon_oncesi_tetikleyici BEFORE INSERT ON public.kitaprezervasyonlari FOR EACH ROW EXECUTE FUNCTION public.rezervasyon_tekrari_engelle();


--
-- TOC entry 3335 (class 2620 OID 17077)
-- Name: kitaprezervasyonlari trg_kitap_rezervasyon_ekle; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER trg_kitap_rezervasyon_ekle AFTER INSERT ON public.kitaprezervasyonlari FOR EACH ROW EXECUTE FUNCTION public.kitap_kopya_durum_guncelle_insert();


--
-- TOC entry 3336 (class 2620 OID 17079)
-- Name: kitaprezervasyonlari trg_kitap_rezervasyon_sil; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER trg_kitap_rezervasyon_sil AFTER DELETE ON public.kitaprezervasyonlari FOR EACH ROW EXECUTE FUNCTION public.kitap_kopya_durum_guncelle_delete();


--
-- TOC entry 3328 (class 2620 OID 17018)
-- Name: uyeler uye_ekleme_tetikleyici; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER uye_ekleme_tetikleyici AFTER INSERT ON public.uyeler FOR EACH ROW EXECUTE FUNCTION public.yeni_uye_gunlugu();


--
-- TOC entry 3318 (class 2606 OID 16943)
-- Name: kiralamalar kiralamalar_kitap_id_kopya_numarasi_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kiralamalar
    ADD CONSTRAINT kiralamalar_kitap_id_kopya_numarasi_fkey FOREIGN KEY (kitap_id, kopya_numarasi) REFERENCES public.kitapkopyalari(kitap_id, kopya_numarasi);


--
-- TOC entry 3319 (class 2606 OID 17035)
-- Name: kiralamalar kiralamalar_kutuphaneci_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kiralamalar
    ADD CONSTRAINT kiralamalar_kutuphaneci_id_fkey FOREIGN KEY (kutuphaneci_id) REFERENCES public.kutuphaneciler(kutuphaneci_id);


--
-- TOC entry 3320 (class 2606 OID 16938)
-- Name: kiralamalar kiralamalar_uye_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kiralamalar
    ADD CONSTRAINT kiralamalar_uye_id_fkey FOREIGN KEY (uye_id) REFERENCES public.uyeler(uye_id);


--
-- TOC entry 3317 (class 2606 OID 16918)
-- Name: kitapkopyalari kitapkopyalari_kitap_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitapkopyalari
    ADD CONSTRAINT kitapkopyalari_kitap_id_fkey FOREIGN KEY (kitap_id) REFERENCES public.kitaplar(kitap_id);


--
-- TOC entry 3314 (class 2606 OID 16900)
-- Name: kitaplar kitaplar_tur_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitaplar
    ADD CONSTRAINT kitaplar_tur_id_fkey FOREIGN KEY (tur_id) REFERENCES public.kitapturleri(tur_id);


--
-- TOC entry 3315 (class 2606 OID 16895)
-- Name: kitaplar kitaplar_yayin_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitaplar
    ADD CONSTRAINT kitaplar_yayin_id_fkey FOREIGN KEY (yayin_id) REFERENCES public.yayinevleri(yayin_id);


--
-- TOC entry 3316 (class 2606 OID 16905)
-- Name: kitaplar kitaplar_yazar_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitaplar
    ADD CONSTRAINT kitaplar_yazar_id_fkey FOREIGN KEY (yazar_id) REFERENCES public.yazarlar(yazar_id);


--
-- TOC entry 3326 (class 2606 OID 17000)
-- Name: kitaprezervasyonlari kitaprezervasyonlari_kitap_id_kopya_numarasi_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitaprezervasyonlari
    ADD CONSTRAINT kitaprezervasyonlari_kitap_id_kopya_numarasi_fkey FOREIGN KEY (kitap_id, kopya_numarasi) REFERENCES public.kitapkopyalari(kitap_id, kopya_numarasi);


--
-- TOC entry 3327 (class 2606 OID 16995)
-- Name: kitaprezervasyonlari kitaprezervasyonlari_uye_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kitaprezervasyonlari
    ADD CONSTRAINT kitaprezervasyonlari_uye_id_fkey FOREIGN KEY (uye_id) REFERENCES public.uyeler(uye_id);


--
-- TOC entry 3312 (class 2606 OID 16846)
-- Name: kutuphaneciler kutuphaneciler_kutuphaneci_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kutuphaneciler
    ADD CONSTRAINT kutuphaneciler_kutuphaneci_id_fkey FOREIGN KEY (kutuphaneci_id) REFERENCES public.calisanlar(calisan_id);


--
-- TOC entry 3313 (class 2606 OID 17025)
-- Name: kutuphaneciler kutuphaneciler_yonetici_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kutuphaneciler
    ADD CONSTRAINT kutuphaneciler_yonetici_id_fkey FOREIGN KEY (yonetici_id) REFERENCES public.yoneticiler(yonetici_id);


--
-- TOC entry 3323 (class 2606 OID 16970)
-- Name: masarezervasyonlari masarezervasyonlari_masa_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.masarezervasyonlari
    ADD CONSTRAINT masarezervasyonlari_masa_id_fkey FOREIGN KEY (masa_id) REFERENCES public.masalar(masa_id);


--
-- TOC entry 3324 (class 2606 OID 16980)
-- Name: masarezervasyonlari masarezervasyonlari_misafir_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.masarezervasyonlari
    ADD CONSTRAINT masarezervasyonlari_misafir_id_fkey FOREIGN KEY (misafir_id) REFERENCES public.misafirler(misafir_id);


--
-- TOC entry 3325 (class 2606 OID 16975)
-- Name: masarezervasyonlari masarezervasyonlari_uye_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.masarezervasyonlari
    ADD CONSTRAINT masarezervasyonlari_uye_id_fkey FOREIGN KEY (uye_id) REFERENCES public.uyeler(uye_id);


--
-- TOC entry 3321 (class 2606 OID 17030)
-- Name: rezervasyonlar rezervasyonlar_kutuphaneci_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyonlar
    ADD CONSTRAINT rezervasyonlar_kutuphaneci_id_fkey FOREIGN KEY (kutuphaneci_id) REFERENCES public.kutuphaneciler(kutuphaneci_id);


--
-- TOC entry 3322 (class 2606 OID 16955)
-- Name: rezervasyonlar rezervasyonlar_uye_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyonlar
    ADD CONSTRAINT rezervasyonlar_uye_id_fkey FOREIGN KEY (uye_id) REFERENCES public.uyeler(uye_id);


--
-- TOC entry 3311 (class 2606 OID 16836)
-- Name: yoneticiler yoneticiler_yonetici_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.yoneticiler
    ADD CONSTRAINT yoneticiler_yonetici_id_fkey FOREIGN KEY (yonetici_id) REFERENCES public.calisanlar(calisan_id);


-- Completed on 2024-12-17 18:10:34

--
-- PostgreSQL database dump complete
--

