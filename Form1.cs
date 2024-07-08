using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ProLab2.donem1.proje
{
    public partial class Form1 : Form
    {
        private OyunKarakteri karakter;
        private PictureBox karakterPictureBox;
        private PictureBox kusPictureBox;
        private PictureBox ariPictureBox;
        private PictureBox engelPictureBox;

        private List<PictureBox> kusPictureBoxList = new List<PictureBox>();
        private List<PictureBox> ariPictureBoxList = new List<PictureBox>();
        private List<PictureBox> engelPictureBoxListesi = new List<PictureBox>();

        private List<int> originalYList = new List<int>();
        private List<int> originalXList = new List<int>();
        private List<Hazine> hazineler = new List<Hazine>();
        private List<Engel> engeller = new List<Engel>();

        private Timer timer;
        int birimBoyutu = 10;
        private int haritaBoyutu = 100;
        private PictureBox[,] harita;
        int gorusAlani = 30;
        private Panel panel;
        private int adim = 0;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            panel = new Panel();
            panel.Size = new Size(1000, 740);
            panel.Location = new Point(10, 10);
            panel.AutoScroll = true;
            panel.BorderStyle = BorderStyle.Fixed3D;
            this.Controls.Add(panel);


            timer = new Timer();
            timer.Interval = 100; // Timer'ın her 100 milisaniyede bir tetiklenmesi
            timer.Tick += HareketTimer_Tick;

        }
        private void HareketTimer_Tick(object sender, EventArgs e)
        {
            // Kuşlar ve arılar için hareket etme kodlarını burada çağırın
            for (int i = 0; i < 3; i++)
            {
                HareketEtKuslar(i);
                HareketEtArilar(i);
                BoyaYeriKirmizi(karakter.MevcutKonum.X, karakter.MevcutKonum.Y);
                karakterPictureBox.Location = new Point(karakter.MevcutKonum.X, karakter.MevcutKonum.Y);
            }

            KontrolEtVeHareketEt();
            EngelleriListeyeYazdir(karakter.MevcutKonum.X, karakter.MevcutKonum.Y);
            if (TumHazinelerToplandi())
            {
                timer.Stop();
                MessageBox.Show("Tüm hazineler toplandı!");
            }
        }
        private void BoyaYeriYeşil(int x, int y)
        {
            foreach (Control control in this.Controls)
            {
                if (control is PictureBox && control.Bounds.Contains(x, y))
                {
                    control.BackColor = Color.Green;
                }
            }
        }
        private void HareketEtKuslar(int i)
        {
            PictureBox kusPictureBox = kusPictureBoxList[i];

            // İlk hareket, bulundukları yerden 5 birim yukarı çıkmak
            if ((kusPictureBox.Tag == null || (bool)kusPictureBox.Tag == false))
            {
                if (kusPictureBox.Location.Y > originalYList[i] - 5 * birimBoyutu)
                {
                    BoyaYeriYeşil(kusPictureBox.Location.X, kusPictureBox.Location.Y);
                    kusPictureBox.Location = new Point(kusPictureBox.Location.X, kusPictureBox.Location.Y - birimBoyutu);


                }
                else
                {
                    kusPictureBox.Tag = true; // İlk hareket tamamlandı, bir sonraki hareket için tag'i true yap
                }
            }
            // İkinci hareket, 5 birim aşağı inip yerlerine dönmek
            else
            {
                if (kusPictureBox.Location.Y < originalYList[i]) // originalY, kuşun başlangıç Y konumunu temsil eder
                {
                    BoyaYeriYeşil(kusPictureBox.Location.X, kusPictureBox.Location.Y);
                    kusPictureBox.Location = new Point(kusPictureBox.Location.X, kusPictureBox.Location.Y + birimBoyutu);


                }
                else
                {
                    kusPictureBox.Tag = false; // İkinci hareket tamamlandı, bir sonraki hareket için tag'i false yap
                }
            }
        }

        private void HareketEtArilar(int i)
        {
            PictureBox ariPictureBox = ariPictureBoxList[i];
            // İlk hareket, bulundukları yerden 5 birim yukarı çıkmak
            if (ariPictureBox.Tag == null || (bool)ariPictureBox.Tag == false)
            {
                if (ariPictureBox.Location.X > originalXList[i] - 3 * birimBoyutu)
                {
                    ariPictureBox.Location = new Point(ariPictureBox.Location.X - birimBoyutu, ariPictureBox.Location.Y);
                    BoyaYeriYeşil(ariPictureBox.Location.X, ariPictureBox.Location.Y);
                }
                else
                {
                    ariPictureBox.Tag = true; // İlk hareket tamamlandı, bir sonraki hareket için tag'i true yap
                }
            }
            // İkinci hareket, 5 birim aşağı inip yerlerine dönmek
            else
            {
                if (ariPictureBox.Location.X < originalXList[i]) // originalY, kuşun başlangıç Y konumunu temsil eder
                {
                    ariPictureBox.Location = new Point(ariPictureBox.Location.X + birimBoyutu, ariPictureBox.Location.Y);
                    BoyaYeriYeşil(ariPictureBox.Location.X, ariPictureBox.Location.Y);
                }
                else
                {
                    ariPictureBox.Tag = false; // İkinci hareket tamamlandı, bir sonraki hareket için tag'i false yap
                }
            }
        }
        private bool YazMevsimiBelirle(int x, int haritaBoyutu)
        {
            if (x < haritaBoyutu / 2)
                return true;

            return false;

        }

        private bool KontrolEtt(int x, int y)
        {
            for (int i = x - 3; i < x + 3; i++)
            {
                for (int j = y - 3; j < y + 3; j++)
                {
                    if (harita[i, j] != null)
                        return true;
                    foreach (Engel engel in engeller)
                    {
                        for (int k = engel.Pozisyon.X; k <= engel.Pozisyon.X + engel.BoyutX; k += birimBoyutu)
                        {
                            for (int z = engel.Pozisyon.Y; z <= engel.Pozisyon.Y + engel.BoyutY; z += birimBoyutu)
                            {
                                if (i == k || j == z)
                                    return true;
                            }
                        }

                    }

                }
            }
            return false;
        }

        private void OlusturVeKarakteriYerlestir()
        {
            harita = new PictureBox[haritaBoyutu, haritaBoyutu];
            Random random = new Random();

            int agacSayaci = 0;
            int kayaSayaci = 0;
            int duvarSayaci = 0;
            int dagSayaci = 0;
            int kusSayaci = 0;
            int arilarSayaci = 0;
            int zumrutSandikSayaci = 0;
            int altinSandikSayaci = 0;
            int gumusSandikSayaci = 0;
            int bakirSandikSayaci = 0;


            int a = random.Next(10, haritaBoyutu * birimBoyutu - 10);
            int b = random.Next(10, haritaBoyutu * birimBoyutu - 10);

            karakter = new OyunKarakteri(1, "Oyuncu1", new Lokasyon(50, 50));

            karakterPictureBox = new PictureBox();
            karakterPictureBox.Image = Image.FromFile(@"C:\Users\JARVIS\source\repos\pro2Lab1.2\pro2Lab1.2\Resources\karakter.jpg");
            karakterPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            karakterPictureBox.Size = new Size(30, 30);
            karakterPictureBox.Location = new Point(karakter.MevcutKonum.X, karakter.MevcutKonum.Y);
            panel.Controls.Add(karakterPictureBox);


            while (true)
            {
                int x = random.Next(10, haritaBoyutu - 10);
                int y = random.Next(10, haritaBoyutu - 10);


                if (KontrolEtt(x, y))
                {
                    continue;
                }
                bool yazMevsimi = YazMevsimiBelirle(x, haritaBoyutu);
                // Her engelden en fazla 2 adet ekle
                if (agacSayaci < 3)
                {
                    EngeliEkle(new Agac(4, 4, new Lokasyon(x * birimBoyutu, y * birimBoyutu), yazMevsimi), harita);
                    engeller.Add(new Agac(4, 4, new Lokasyon(x * birimBoyutu, y * birimBoyutu), yazMevsimi));
                    agacSayaci++;
                    continue;
                }
                else if (kayaSayaci < 3)
                {
                    EngeliEkle(new Kaya(3, 3, new Lokasyon(x * birimBoyutu, y * birimBoyutu), yazMevsimi), harita);
                    engeller.Add(new Kaya(3, 3, new Lokasyon(x * birimBoyutu, y * birimBoyutu), yazMevsimi));
                    kayaSayaci++;
                    continue;
                }
                else if (duvarSayaci < 2)
                {
                    EngeliEkle(new Duvar(10, 1, new Lokasyon(x * birimBoyutu, y * birimBoyutu), yazMevsimi), harita);
                    engeller.Add(new Duvar(10, 1, new Lokasyon(x * birimBoyutu, y * birimBoyutu), yazMevsimi));
                    duvarSayaci++;
                    continue;
                }
                else if (dagSayaci < 2)
                {
                    EngeliEkle(new Dag(3, 3, new Lokasyon(x * birimBoyutu, y * birimBoyutu), yazMevsimi), harita);
                    engeller.Add(new Dag(3, 3, new Lokasyon(x * birimBoyutu, y * birimBoyutu), yazMevsimi));
                    dagSayaci++;
                    continue;
                }
                else if (kusSayaci < 3)
                {
                    Kus yeniKus = new Kus(3, 3, new Lokasyon(x * birimBoyutu, y * birimBoyutu));
                    engeller.Add(new Kus(3, 3, new Lokasyon(x * birimBoyutu, y * birimBoyutu)));

                    kusPictureBox = new PictureBox
                    {
                        Size = new Size(yeniKus.BoyutX * birimBoyutu, yeniKus.BoyutY * birimBoyutu),
                        Location = new Point(yeniKus.Pozisyon.X, yeniKus.Pozisyon.Y),
                        Image = yeniKus.EngelResmi,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    panel.Controls.Add(kusPictureBox);
                    kusPictureBoxList.Add(kusPictureBox);
                    foreach (PictureBox kusPictureBox in kusPictureBoxList)
                    {
                        originalYList.Add(kusPictureBox.Location.Y);
                    }
                    kusSayaci++;
                    engelPictureBoxListesi.Add(engelPictureBox);
                    continue;
                }
                else if (arilarSayaci < 3)
                {
                    Arilar yeniAri = new Arilar(3, 3, new Lokasyon(x * birimBoyutu, y * birimBoyutu));
                    engeller.Add(new Arilar(3, 3, new Lokasyon(x * birimBoyutu, y * birimBoyutu)));

                    ariPictureBox = new PictureBox
                    {
                        Size = new Size(yeniAri.BoyutX * birimBoyutu, yeniAri.BoyutY * birimBoyutu),
                        Location = new Point(yeniAri.Pozisyon.X, yeniAri.Pozisyon.Y),
                        Image = yeniAri.EngelResmi,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    panel.Controls.Add(ariPictureBox);
                    ariPictureBoxList.Add(ariPictureBox);
                    foreach (PictureBox ariPictureBox in ariPictureBoxList)
                    {
                        originalXList.Add(ariPictureBox.Location.X);
                    }
                    arilarSayaci++;
                    engelPictureBoxListesi.Add(engelPictureBox);
                    continue;
                }
                else if (
                 agacSayaci == 3 &&
                 kayaSayaci == 3 &&
                 duvarSayaci == 2 &&
                 dagSayaci == 2 &&
                 kusSayaci == 3 &&
                 arilarSayaci == 3)
                    break;

            }
            while (true)
            {
                int x = random.Next(10, haritaBoyutu - 10);
                int y = random.Next(10, haritaBoyutu - 10);


                if (KontrolEtt(x, y))
                {
                    continue;
                }
                if (altinSandikSayaci < 4)
                {
                    HazineEkle(new AltinSandik(new Lokasyon(x * birimBoyutu, y * birimBoyutu)));
                    altinSandikSayaci++;
                    continue;
                }
                else if (gumusSandikSayaci < 4)
                {
                    HazineEkle(new GumusSandik(new Lokasyon(x * birimBoyutu, y * birimBoyutu)));
                    gumusSandikSayaci++;
                    continue;
                }
                else if (bakirSandikSayaci < 4)
                {
                    HazineEkle(new BakirSandik(new Lokasyon(x * birimBoyutu, y * birimBoyutu)));
                    bakirSandikSayaci++;
                    continue;
                }
                else if (zumrutSandikSayaci < 4)
                {
                    HazineEkle(new ZumrutSandik(new Lokasyon(x * birimBoyutu, y * birimBoyutu)));
                    zumrutSandikSayaci++;
                    continue;
                }
                else if (
                 zumrutSandikSayaci == 4 &&
                 altinSandikSayaci == 4 &&
                 gumusSandikSayaci == 4 &&
                 bakirSandikSayaci == 4)
                    break;

            }
            for (int i = 0; i < haritaBoyutu; i++)
            {
                for (int j = 0; j < haritaBoyutu; j++)
                {

                    harita[i, j] = new PictureBox
                    {
                        Size = new Size(birimBoyutu, birimBoyutu),
                        Location = new Point(i * birimBoyutu, j * birimBoyutu),
                        BackColor = Color.White,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    panel.Controls.Add(harita[i, j]);
                }
            }
        }
        private void EngeliEkle(Engel engel, PictureBox[,] harita)
        {
            if (engel is Duvar)
            {
                // Eğer engel bir Duvar ise
                Duvar duvarEngel = (Duvar)engel;
                engelPictureBox = duvarEngel.OlusturPictureBox(birimBoyutu);
            }
            else
            {
                engelPictureBox = new PictureBox
                {
                    Size = new Size(engel.BoyutX * birimBoyutu, engel.BoyutY * birimBoyutu),
                    Location = new Point(engel.Pozisyon.X, engel.Pozisyon.Y),
                    Image = engel.EngelResmi,
                    SizeMode = PictureBoxSizeMode.Zoom,
                };
            }
            panel.Controls.Add(engelPictureBox);
            engelPictureBoxListesi.Add(engelPictureBox);
        }
        private void HazineEkle(Hazine hazine)
        {
            hazineler.Add(hazine);

            PictureBox hazinePictureBox = new PictureBox
            {
                Size = new Size(birimBoyutu * 3, birimBoyutu * 3),
                Location = new Point(hazine.Pozisyon.X, hazine.Pozisyon.Y),
                Image = hazine.HazineResmi,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Blue,
            };
            panel.Controls.Add(hazinePictureBox);
        }

        private bool KarakterGidebilirMi(int hedefX, int hedefY)
        {
            // Hedef koordinatların geçerli olup olmadığını ve engel içerip içermediğini kontrol et
            if (hedefX >= 0 && hedefX < haritaBoyutu * birimBoyutu && hedefY >= 0 && hedefY < haritaBoyutu * birimBoyutu)
            {
                //int baslangicX = karakter.MevcutKonum.X;
                int baslangicX = hedefX;
                // int baslangicY = karakter.MevcutKonum.Y;
                int baslangicY = hedefY;

                // Görüş alanındaki her bir kareyi tarayın
                for (int i = baslangicX; i < baslangicX + birimBoyutu * 3; i += birimBoyutu)
                {
                    for (int j = baslangicY; j < baslangicY + birimBoyutu * 3; j += birimBoyutu)
                    {
                        // Eğer taradığımız karede bir engel varsa
                        foreach (Engel engel in engeller)
                        {
                            if (engel.Pozisyon.X == i && engel.Pozisyon.Y == j)
                            {
                                if (kontrol == 0)
                                    HareketEtEngelEtrafinda(engel.Pozisyon.X, engel.Pozisyon.Y, engel.BoyutX, engel.BoyutY);
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            return false; // Hedefe hareket edilemez veya engel var
        }
        int kontrol = 0;
        private void HareketEtEngelEtrafinda(int engelX, int engelY, int boyutX, int boyutY)
        {
            kontrol = 1;
            int karakterX = karakter.MevcutKonum.X;
            int karakterY = karakter.MevcutKonum.Y;

            int mesafeX = engelX + boyutX;
            int mesafeY = engelY + boyutY;

            // Karakterin hareket edebileceği tüm yönleri hesapla
            List<Point> hareketListesi = new List<Point>();
            List<Point> hareketBuyukListesi = new List<Point>();

            // Sağa, sola, yukarıya ve aşağıya hareket etme imkanlarını kontrol et
            if (KarakterGidebilirMi(karakterX + birimBoyutu * 2, karakterY))
                hareketListesi.Add(new Point(birimBoyutu * 2, 0)); // Sağa hareket
            if (KarakterGidebilirMi(karakterX - birimBoyutu * 2, karakterY))
                hareketListesi.Add(new Point(-birimBoyutu * 2, 0)); // Sola hareket
            if (KarakterGidebilirMi(karakterX, karakterY + birimBoyutu * 2))
                hareketListesi.Add(new Point(0, birimBoyutu * 2)); // Aşağı hareket
            if (KarakterGidebilirMi(karakterX, karakterY - birimBoyutu * 2))
                hareketListesi.Add(new Point(0, -birimBoyutu * 2)); // Yukarı hareket


            Random random = new Random();

            Point hareketYonu = hareketListesi[random.Next(hareketListesi.Count)];

            // Karakterin yeni hedefini belirle
            int yeniHedefX = karakterX + hareketYonu.X;
            int yeniHedefY = karakterY + hareketYonu.Y;

            // Eğer yeni hedef engelin olduğu bir konum değilse, hareket et
            if (yeniHedefX != engelX || yeniHedefY != engelY)
            {
                karakter.HareketEt(hareketYonu.X, hareketYonu.Y);

            }
            else
            {
                if (KarakterGidebilirMi(karakterX, karakterY - birimBoyutu * 5))
                    hareketBuyukListesi.Add(new Point(0, -birimBoyutu * 5));
                if (KarakterGidebilirMi(karakterX, karakterY - birimBoyutu * 5))
                    hareketBuyukListesi.Add(new Point(0, +birimBoyutu * 5));
                if (KarakterGidebilirMi(karakterX - birimBoyutu * 5, karakterY))
                    hareketBuyukListesi.Add(new Point(-birimBoyutu * 5, 0));
                if (KarakterGidebilirMi(karakterX + birimBoyutu * 5, karakterY))
                    hareketBuyukListesi.Add(new Point(birimBoyutu * 5, 0));

                hareketYonu = hareketBuyukListesi[random.Next(hareketListesi.Count)];

                int yeniX = karakterX + hareketYonu.X;
                int yeniY = karakterY + hareketYonu.Y;

                // Eğer yeni hedef engelin olduğu bir konum değilse, hareket et
                if (yeniX != engelX || yeniY != engelY)
                {
                    karakter.HareketEt(hareketYonu.X, hareketYonu.Y);

                }
            }
            kontrol = 0;
            hareketBuyukListesi.Clear();
            hareketListesi.Clear();
        }


        private void EngelleriListeyeYazdir(int karakterX, int karakterY)
        { // Görüş alanının sol üst köşesini hesapla
            int baslangicX = karakterX - (birimBoyutu * gorusAlani / 2);
            int baslangicY = karakterY - (birimBoyutu * gorusAlani / 2);

            // Görüş alanındaki her bir kareyi tarayın
            for (int i = baslangicX; i < baslangicX + birimBoyutu * gorusAlani; i += birimBoyutu)
            {
                for (int j = baslangicY; j < baslangicY + birimBoyutu * gorusAlani; j += birimBoyutu)
                {
                    bool engelYazildi = false; // Engelin daha önce yazılıp yazılmadığını kontrol etmek için bayrak

                    // Eğer taradığımız karede bir engel varsa
                    foreach (Engel engel in engeller)
                    {
                        if (engel.Pozisyon.X == i && engel.Pozisyon.Y == j)
                        {
                            // Engelin daha önce yazılmadığını kontrol et
                            if (!engelYazildi)
                            {
                                // Engelin listbox'ta bulunan bir öğe olup olmadığını kontrol et
                                bool engelVarMi = false;
                                foreach (var item in listBox2.Items)
                                {
                                    if (item.ToString() == $"Engel Türü: {engel.GetType().Name}, Konumu: ({i}, {j})")
                                    {
                                        engelVarMi = true;
                                        break;
                                    }
                                }

                                // Eğer engel yoksa listbox'a ekle
                                if (!engelVarMi)
                                {
                                    listBox2.Items.Add($"Engel Türü: {engel.GetType().Name}, Konumu: ({i}, {j})");
                                }
                                engelYazildi = true; // Engelin yazıldığını işaretle
                            }
                            break; // Her bir kare için bir kez yazdırıldığından emin olmak için döngüden çık
                        }
                    }
                }
            }
        }
        private Hazine EnYakinHazineyiBul()
        {
            Hazine enYakinHazine = null;
            double enKisaMesafe = double.MaxValue;

            foreach (Hazine hazine in hazineler)
            {
                // Karakterin daha önce topladığı hazineleri atla
                if (karakter.ToplananHazineListesi.Contains(hazine))
                    continue;

                int karakterX = karakter.MevcutKonum.X;
                int karakterY = karakter.MevcutKonum.Y;

                // Hazineyi 7x7 lik birim karede ara
                if (Hazine7x7BirimKaredeMi(hazine, karakterX, karakterY))
                {
                    // Hazineye olan mesafeyi hesapla
                    double mesafe = Math.Sqrt(Math.Pow(hazine.Pozisyon.X - karakterX, 2) + Math.Pow(hazine.Pozisyon.Y - karakterY, 2));

                    // Eğer bu hazine, en yakın hazineye şu ana kadar bulduğumuzdan daha yakınsa, en yakın hazineyi güncelle
                    if (mesafe < enKisaMesafe)
                    {
                        enKisaMesafe = mesafe;
                        enYakinHazine = hazine;
                    }
                }
            }

            return enYakinHazine;
        }
        private bool Hazine7x7BirimKaredeMi(Hazine hazine, int karakterX, int karakterY)
        {
            // Hazine ve karakter arasındaki mesafeyi kontrol et
            int deltaX = Math.Abs(hazine.Pozisyon.X - karakterX);
            int deltaY = Math.Abs(hazine.Pozisyon.Y - karakterY);

            return deltaX < gorusAlani * birimBoyutu && deltaY < gorusAlani * birimBoyutu;
        }
        private void KontrolEtVeHareketEt()
        {
            // Karakterin en son topladığı hazineyi bul
            Hazine enYakinHazine = EnYakinHazineyiBul();

            int eskiKonumX = karakter.MevcutKonum.X;
            int eskiKonumY = karakter.MevcutKonum.Y;

            // Eğer en yakın hazine varsa
            if (enYakinHazine != null)
            {
                // Karakteri en yakın hazineye yönlendir
                if (!YonlendirKarakter(karakter, enYakinHazine.Pozisyon))
                {
                    RandomHareket(eskiKonumX, eskiKonumY, 4);
                }
                // Eğer karakter hazineyi toplayabiliyorsa
                if (karakter.MevcutKonum.X == enYakinHazine.Pozisyon.X && karakter.MevcutKonum.Y == enYakinHazine.Pozisyon.Y)
                {
                    karakter.HazineTopla(enYakinHazine);
                    BoyaYeriSiyah(enYakinHazine.Pozisyon.X, enYakinHazine.Pozisyon.Y);
                    HazineEkleListBox(enYakinHazine);
                }
            }
            else
                RandomHareket(eskiKonumX, eskiKonumY, 2);
        }
        private void RandomHareket(int eskiKonumX, int eskiKonumY, int mesafe)
        {
            List<Point> hareketListesi = new List<Point>();

            // Sağa, aşağıya, yukarıya ve sola hareket etme imkanlarını kontrol et
            if (KarakterGidebilirMi(eskiKonumX + birimBoyutu * mesafe, eskiKonumY))
                hareketListesi.Add(new Point(birimBoyutu * mesafe, 0)); // Sağa hareket
            if (KarakterGidebilirMi(eskiKonumX, eskiKonumY + birimBoyutu * mesafe))
                hareketListesi.Add(new Point(0, birimBoyutu * mesafe)); // Aşağı hareket
            if (KarakterGidebilirMi(eskiKonumX, eskiKonumY - birimBoyutu * mesafe))
                hareketListesi.Add(new Point(0, -birimBoyutu * mesafe)); // Yukarı hareket
            if (KarakterGidebilirMi(eskiKonumX - birimBoyutu * mesafe, eskiKonumY))
                hareketListesi.Add(new Point(-birimBoyutu * mesafe, 0)); // Sola hareket

            // Rastgele hareket et
            Random random = new Random();
            if (hareketListesi.Count > 0)
            {
                Point rastgeleHareket = hareketListesi[random.Next(hareketListesi.Count)];
                karakter.HareketEt(rastgeleHareket.X, rastgeleHareket.Y);
            }
            hareketListesi.Clear();
        }
        private bool YonlendirKarakter(OyunKarakteri karakter, Lokasyon hedef)
        {
            int karakterX = karakter.MevcutKonum.X;
            int karakterY = karakter.MevcutKonum.Y;

            // Hedef konumu
            int hedefX = hedef.X;
            int hedefY = hedef.Y;

            // Yatay ve dikey hareketi kontrol et
            if (Math.Abs(hedefX - karakterX) > Math.Abs(hedefY - karakterY))
            {
                // Yatay hareket
                if (hedefX >= karakterX && KarakterGidebilirMi(karakterX + birimBoyutu, karakterY))
                {
                    karakter.HareketEt(birimBoyutu, 0); // Sağa hareket
                    return true;
                }
                else if (hedefX < karakterX && KarakterGidebilirMi(karakterX - birimBoyutu, karakterY))
                {
                    karakter.HareketEt(-birimBoyutu, 0); // Sola hareket
                    return true;
                }

            }
            else
            {
                // Dikey hareket
                if (hedefY > karakterY && KarakterGidebilirMi(karakterX, karakterY + birimBoyutu))
                {
                    karakter.HareketEt(0, birimBoyutu); // Aşağı hareket
                    return true;
                }
                else if (hedefY < karakterY && KarakterGidebilirMi(karakterX, karakterY - birimBoyutu))
                {
                    karakter.HareketEt(0, -birimBoyutu); // Yukarı hareket
                    return true;
                }

            }
            return false;
        }

        private void HazineEkleListBox(Hazine hazine)
        {
            hazineler.Add(hazine);

            string yeniHazineMesaji = $"Yeni hazine türü: {hazine.Tur}, Konumu: ({hazine.Pozisyon.X}, {hazine.Pozisyon.Y})";

            // Öncelik sırasına göre sıralı bir şekilde ekleme yapmak için
            bool eklendi = false;
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string eskiHazineMesaji = listBox1.Items[i].ToString();

                if (string.Compare(yeniHazineMesaji, eskiHazineMesaji) <= 0)
                {
                    listBox1.Items.Insert(i, yeniHazineMesaji);
                    eklendi = true;
                    break;
                }
            }
            // Eğer ekleme yapılmadıysa, en sona ekle
            if (!eklendi)
            {
                listBox1.Items.Add(yeniHazineMesaji);
            }
        }

        private void BoyaYeriKirmizi(int x, int y)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is PictureBox && control.Bounds.Contains(x, y))
                {
                    control.BackColor = Color.Red;
                    adim++;
                    int xy = adim / birimBoyutu;
                    label1.Text = xy.ToString();
                }
            }
        }
        private void BoyaYeriSiyah(int x, int y)
        {
            foreach (Control control in this.Controls)
            {
                if (control is PictureBox && control.Bounds.Contains(x, y))
                {
                    control.BackColor = Color.Black;
                    control.ForeColor = Color.Black;
                }
            }
        }


        private bool TumHazinelerToplandi()
        {
            // Tüm hazinelerin toplandığını kontrol etmek için hazineler listesinde gezin
            foreach (Hazine hazine in hazineler)
            {
                // Herhangi bir hazine toplanmamışsa false döndür
                if (!karakter.ToplananHazineListesi.Contains(hazine))
                {
                    return false;
                }
            }
            // Eğer hiçbir hazine toplanmamış değilse true döndür
            return true;
        }


        private void Temizle()
        {
            panel.Controls.Clear();
            hazineler.Clear();
            engeller.Clear();
            kusPictureBoxList.Clear();
            ariPictureBoxList.Clear();
            originalYList.Clear();
            originalXList.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            adim = 0;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            timer.Start();
            adim = 0;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            button2.Enabled = false;
            timer.Stop();
            Temizle();
            if (int.TryParse(txtHaritaBoyutu.Text, out haritaBoyutu) && haritaBoyutu > 0)
            {
                OlusturVeKarakteriYerlestir();
                label.Text = $"{haritaBoyutu}x{haritaBoyutu} boyutunda harita oluşturuldu.";
            }
            else
            {
                label.Text = "Geçerli bir harita boyutu giriniz.";
            }
            button2.Enabled = true;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            panel.Visible = true;
            button2.Enabled = false;
        }
    }
}
