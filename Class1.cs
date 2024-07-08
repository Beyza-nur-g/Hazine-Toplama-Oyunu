using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProLab2.donem1.proje
{
    internal class Class1
    {
    }
    public class Lokasyon
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Lokasyon(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public class OyunKarakteri
    {
        public int ID { get; set; }
        public string Ad { get; set; }
        public Lokasyon MevcutKonum { get; set; }
        public List<Hazine> ToplananHazineListesi { get; private set; }

        public OyunKarakteri(int id, string ad, Lokasyon baslangicKonumu)
        {
            ID = id;
            Ad = ad;
            MevcutKonum = baslangicKonumu;
            ToplananHazineListesi = new List<Hazine>();
        }

        public void HareketEt(int x, int y)
        {
            MevcutKonum.X += x;
            MevcutKonum.Y += y;
        }
        public void HazineTopla(Hazine hazine)
        {
            ToplananHazineListesi.Add(hazine);
            hazine.HazineResmi.Clone();
            hazine.HazineResmi = null;

        }
    }

    public class Hazine
    {
        public string Tur { get; set; }
        public Lokasyon Pozisyon { get; set; }
        public Image HazineResmi { get; set; }

        public Hazine(string tur, Lokasyon pozisyon, Image hazineResmi)
        {
            Tur = tur;
            Pozisyon = pozisyon;
            HazineResmi = hazineResmi;
        }
    }
    public class AltinSandik : Hazine
    {
        public AltinSandik(Lokasyon pozisyon)
            : base("Altin Sandik", pozisyon, Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\altin3.png"))
        {
        }
    }
    public class GumusSandik : Hazine
    {
        public GumusSandik(Lokasyon pozisyon)
            : base("Gumus Sandik", pozisyon, Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\gumus3.png"))
        {
        }
    }
    public class ZumrutSandik : Hazine
    {
        public ZumrutSandik(Lokasyon pozisyon)
            : base("Zumrut Sandik", pozisyon, Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\zumrut2.png"))
        {
        }
    }
    public class BakirSandik : Hazine
    {
        public BakirSandik(Lokasyon pozisyon)
            : base("Bakir Sandik", pozisyon, Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\bakir.png"))
        {
        }
    }
    public class Engel : Control
    {
        public string Tur { get; set; }
        public int BoyutX { get; set; }
        public int BoyutY { get; set; }
        public Lokasyon Pozisyon { get; set; }
        public Image EngelResmi { get; set; }
        public Engel(string tur, int boyutX, int boyutY, Lokasyon pozisyon, Image engelResmi)
        {
            Tur = tur;
            BoyutX = boyutX;
            BoyutY = boyutY;
            Pozisyon = pozisyon;
            EngelResmi = engelResmi;
        }
    }
    public class HareketsizEngel : Engel
    {
        protected Image yazResmi;
        protected Image kisResmi;
        public HareketsizEngel(string tur, int boyutX, int boyutY, Lokasyon pozisyon, Image engelResmi)
             : base("hareketsiz", boyutX, boyutY, pozisyon, engelResmi)
        {
        }
    }
    public abstract class HareketliEngel : Engel
    {
        public int BirimSayisi { get; set; }
        protected int HareketYonu { get; set; }

        public HareketliEngel(string tur, int boyutX, int boyutY, Lokasyon pozisyon, int birimSayisi, Image engelResmi)
            : base("hareketli", boyutX, boyutY, pozisyon, engelResmi)
        {
            BirimSayisi = birimSayisi;
            HareketYonu = 1;
        }

        public void HareketEt()
        {
            Pozisyon.X += HareketYonu * BirimSayisi;
        }
    }
    public class Kus : HareketliEngel
    {
        public Kus(int boyutX, int boyutY, Lokasyon pozisyon)
            : base("Kuş", boyutX, boyutY, pozisyon, 5, Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\kus3.png"))
        {
        }
        public void HareketEtAsagi()
        {
            Pozisyon.Y += BirimSayisi;
            this.Location = new Point(Pozisyon.X, Pozisyon.Y);
        }
        public void HareketEtYukari()
        {
            Pozisyon.Y -= BirimSayisi;
            this.Location = new Point(Pozisyon.X, Pozisyon.Y);
        }
    }


    public class Arilar : HareketliEngel
    {
        public Arilar(int boyutX, int boyutY, Lokasyon pozisyon)
            : base("ari", boyutX, boyutY, pozisyon, 3, Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\ari2.png"))
        {
        }

        public void HareketEtSag()
        {
            Pozisyon.X += BirimSayisi;
            this.Location = new Point(Pozisyon.X, Pozisyon.Y);
        }


    }
    public class Agac : HareketsizEngel
    {
        public bool YazMevsimi { get; set; }

        public Agac(int boyutX, int boyutY, Lokasyon pozisyon, bool yazMevsimi)
            : base("agac", boyutX, boyutY, pozisyon, null)
        {
            YazMevsimi = yazMevsimi;
            MevsimeGoreResimAyarla();
        }

        public void MevsimeGoreResimAyarla()
        {
            yazResmi = Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\agac.png");
            kisResmi = Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\agacKis.jpeg");

            if (YazMevsimi)
            {
                base.EngelResmi = yazResmi;
            }
            else
            {
                base.EngelResmi = kisResmi;
            }
        }
    }
    public class Kaya : HareketsizEngel
    {
        public bool YazMevsimi { get; set; }

        public Kaya(int boyutX, int boyutY, Lokasyon pozisyon, bool yazMevsimi)
            : base("kaya", boyutX, boyutY, pozisyon, null)
        {
            YazMevsimi = yazMevsimi;
            MevsimeGoreResimAyarla();
        }

        public void MevsimeGoreResimAyarla()
        {

            yazResmi = Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\kayaYaz.png");
            kisResmi = Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\kayaKis.png");

            if (YazMevsimi)
            {
                base.EngelResmi = yazResmi;
            }
            else
            {
                base.EngelResmi = kisResmi;
            }
        }
    }
    public class Dag : HareketsizEngel
    {
        public bool YazMevsimi { get; set; }

        public Dag(int boyutX, int boyutY, Lokasyon pozisyon, bool yazMevsimi)
            : base("dag", boyutX, boyutY, pozisyon, null)
        {
            YazMevsimi = yazMevsimi;
            MevsimeGoreResimAyarla();
        }

        public void MevsimeGoreResimAyarla()
        {
            yazResmi = Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\dagYaz.png");
            kisResmi = Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\dagKis.png");

            if (YazMevsimi)
            {
                base.EngelResmi = yazResmi;
            }
            else
            {
                base.EngelResmi = kisResmi;
            }
        }
    }
    public class Duvar : HareketsizEngel
    {
        public bool YazMevsimi { get; set; }

        public Duvar(int boyutX, int boyutY, Lokasyon pozisyon, bool yazMevsimi)
            : base("duvar", boyutX, boyutY, pozisyon, null)
        {
            YazMevsimi = yazMevsimi;
            MevsimeGoreResimAyarla();
        }

        public void MevsimeGoreResimAyarla()
        {
            yazResmi = Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\duvarYaz.png");
            kisResmi = Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\duvarKis.png");

            if (YazMevsimi)
            {
                base.EngelResmi = yazResmi;
            }
            else
            {
                base.EngelResmi = kisResmi;
            }
        }
        public PictureBox OlusturPictureBox(int birimBoyutu)
        {
            PictureBox duvarPictureBox = new PictureBox
            {
                Size = new Size(BoyutX * birimBoyutu, BoyutY * birimBoyutu),
                Location = new Point(Pozisyon.X, Pozisyon.Y),
                Image = EngelResmi,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            return duvarPictureBox;
        }
    }
}