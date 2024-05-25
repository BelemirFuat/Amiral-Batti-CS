using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;



namespace amiralBatti
{

    public partial class Form1 : Form
    {
        private const int tahtaBoyutu = 15;
        int hucreBoyutu = 30;
        private Button[,] dugmeler;
        private List<Gemi> gemiler;
        private int toplamHamle = 0;
        private Stopwatch kronometre = new Stopwatch();

        public Form1()
        {
            InitializeComponent();
            TahtayiOlustur();
            GemileriYerlestir();
            pictureBox1.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"..\..\rec\1_chadweisser-sunsetthailand (Large).jpg");

        }
        private void OyunuBaslat()
        {

            toplamHamle = 0;
            kronometre.Reset();


            kronometre.Start();
        }

        private void TahtayiOlustur()
        {
            dugmeler = new Button[tahtaBoyutu, tahtaBoyutu];
            gemiler = new List<Gemi>();
            hucreBoyutu = 30 * this.ClientSize.Width / 1008;
            int x = this.ClientSize.Width / 4;
            int y = this.ClientSize.Height / 25;


            //MessageBox.Show($"hucre boyutu : {hucreBoyutu}");
            //MessageBox.Show($"x : {x}, y : {y}, xx : {xx}, yy : {yy}");
            for (int i = 0; i < tahtaBoyutu; i++)
            {
                for (int j = 0; j < tahtaBoyutu; j++)
                {
                    Button dugme = new Button();

                    dugme.Width = hucreBoyutu;
                    dugme.Height = hucreBoyutu;

                    // Düğmelerin tahta içinde yerleşimini sağlamak için farklı bir hesaplama yapmalısınız.
                    dugme.Location = new Point((((j + 1) * hucreBoyutu)) + x, ((i + 1) * hucreBoyutu) + y);
                    dugme.Tag = new Point(i, j); // Bu, düğmenin konumunu temsil edebilir.
                    dugme.Click += Dugme_Tiklandi;
                    dugmeler[i, j] = dugme;
                    dugmeler[i, j].BringToFront();

                    dugmeler[i, j].FlatStyle = FlatStyle.Flat;
                    // dugmeler[i, j].Margin = new Padding(5,5,5,5);
                    dugmeler[i, j].Enabled = true;
                    dugmeler[i, j].Visible = true;
                    dugmeler[i, j].BackColor = Color.LightGray;
                    dugmeler[i, j].FlatAppearance.BorderSize = 0;

                    Controls.Add(dugmeler[i, j]);
                }
            }
            pictureBox1.Size = new Size(this.Size.Width, this.Size.Height);
            pictureBox1.SendToBack();
        }

        private void GemileriYerlestir()
        {
            Random rastgele = new Random();


            GemiYerlestir(rastgele, 4);


            GemiYerlestir(rastgele, 3);
            GemiYerlestir(rastgele, 3);


            GemiYerlestir(rastgele, 2);
            GemiYerlestir(rastgele, 2);
            GemiYerlestir(rastgele, 2);


            GemiYerlestir(rastgele, 1);
            GemiYerlestir(rastgele, 1);
            GemiYerlestir(rastgele, 1);
            GemiYerlestir(rastgele, 1);
        }

        private void GemiYerlestir(Random rastgele, int uzunluk)
        {
            bool yerlestirildi = false;

            while (!yerlestirildi)
            {
                int satir = rastgele.Next(0, tahtaBoyutu);
                int sutun = rastgele.Next(0, tahtaBoyutu);

                bool yatayMi = rastgele.Next(2) == 0;

                Gemi yeniGemi = new Gemi(satir, sutun, uzunluk, yatayMi);

                if (GemiYerlestirmeGecerliMi(yeniGemi))
                {
                    gemiler.Add(yeniGemi);
                    yerlestirildi = true;
                }
            }
        }
        private bool GemiParcasiKomsuMu(int satir, int sutun)
        {
            for (int i = Math.Max(0, satir - 1); i <= Math.Min(tahtaBoyutu - 1, satir + 1); i++)
            {
                for (int j = Math.Max(0, sutun - 1); j <= Math.Min(tahtaBoyutu - 1, sutun + 1); j++)
                {
                    if (HucreDolmusMu(i, j))
                        return true;
                }
            }
            return false;
        }
        private bool GemiYerlestirmeGecerliMi(Gemi gemi)
        {
            if (gemi.YatayMi)
            {
                if (gemi.Sutun + gemi.Uzunluk > tahtaBoyutu)
                    return false;

                for (int i = gemi.Sutun; i < gemi.Sutun + gemi.Uzunluk; i++)
                {
                    if (HucreDolmusMu(gemi.Satir, i) || GemiParcasiKomsuMu(gemi.Satir, i))
                        return false;
                }
            }
            else
            {
                if (gemi.Satir + gemi.Uzunluk > tahtaBoyutu)
                    return false;

                for (int i = gemi.Satir; i < gemi.Satir + gemi.Uzunluk; i++)
                {
                    if (HucreDolmusMu(i, gemi.Sutun) || GemiParcasiKomsuMu(i, gemi.Sutun))
                        return false;
                }
            }

            return true;
        }

        private bool HucreDolmusMu(int satir, int sutun)
        {
            return gemiler.Any(gemi => gemi.HucreyiKapsiyorMu(satir, sutun));
        }
        private bool TumGemilerBatirildiMi()
        {
            return gemiler.All(gemi => gemi.TumParcalarVurulduMu());
        }
        private void OyunuSifirla()
        {
            kronometre.Reset();
            gemiler.Clear();
            foreach (Button button in dugmeler)
            {
                this.Controls.Remove(button);
            }
            button4.Enabled = false;
            button4.Visible = false;
            TahtayiOlustur();
            GemileriYerlestir();
            OyunuBaslat();
        }
        private void Dugme_Tiklandi(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(AppDomain.CurrentDomain.BaseDirectory+@"..\..\rec\sniper-rifle-firing-shot-1-39789.wav");
            player.Play();
            Button dugme = (Button)sender;
            Point konum = (Point)dugme.Tag;

            if (HucreDolmusMu(konum.X, konum.Y))
            {
                Gemi vurulanGemi = GetGemiByHucre(konum.X, konum.Y);

                if (vurulanGemi != null)
                {
                    if (vurulanGemi.BirParcaVuruldu(konum.X, konum.Y) && vurulanGemi.TumParcalarVurulduMu())
                    {
                        MarkSunkShip(vurulanGemi);
                    }
                }
                else
                {
                    dugme.BackColor = Color.Red;
                }
            }
            else
            {
                dugme.BackColor = Color.Blue;
            }

            dugme.Enabled = false;
            toplamHamle++;


            if (TumGemilerBatirildiMi())
            {

                kronometre.Stop();

                MessageBox.Show($"Toplam Hamle Sayısı: {toplamHamle}\nToplam Geçen Süre: {Convert.ToInt32(kronometre.Elapsed.TotalSeconds)} saniye");
                foreach (Button button in dugmeler)
                {
                    this.Controls.Remove(button);
                }
                foreach (Button buton in dugmeler)
                {
                    buton.Enabled = false;
                }
                button3.Enabled = true;
                button3.Visible = true;
            }
        }

        private Gemi GetGemiByHucre(int satir, int sutun)
        {
            return gemiler.FirstOrDefault(gemi => gemi.HucreyiKapsiyorMu(satir, sutun));
        }
        private void MarkSunkShip(Gemi gemi)
        {
            foreach (var parcasi in gemi.VurulanParcalar)
            {
                for (int i = Math.Max(0, parcasi.X - 1); i <= Math.Min(tahtaBoyutu - 1, parcasi.X + 1); i++)
                {
                    for (int j = Math.Max(0, parcasi.Y - 1); j <= Math.Min(tahtaBoyutu - 1, parcasi.Y + 1); j++)
                    {
                        Button dugme = dugmeler[i, j];


                        if (gemi.TumParcalarVurulduMu())
                        {
                            dugme.Enabled = false;
                            if (!HucreDolmusMu(i, j))
                            {
                                dugme.BackColor = Color.Gray;
                            }
                        }
                    }
                }
            }
        }

        private class Gemi
        {
            public int Satir { get; }
            public int Sutun { get; }
            public int Uzunluk { get; }
            public bool YatayMi { get; }
            public List<Point> VurulanParcalar { get; } = new List<Point>();

            public bool BirParcaVuruldu(int satir, int sutun)
            {
                Point vurulanParca = new Point(satir, sutun);

                if (!VurulanParcalar.Contains(vurulanParca))
                {
                    VurulanParcalar.Add(vurulanParca);
                    return true;
                }

                return false;
            }
            public bool TumParcalarVurulduMu()
            {
                return VurulanParcalar.Count == Uzunluk;
            }
            public Gemi() { }
            public Gemi(int satir, int sutun, int uzunluk, bool yatayMi)
            {
                Satir = satir;
                Sutun = sutun;
                Uzunluk = uzunluk;
                YatayMi = yatayMi;
            }

            public bool HucreyiKapsiyorMu(int satir, int sutun)
            {
                if (YatayMi)
                {
                    return satir == Satir && sutun >= Sutun && sutun < Sutun + Uzunluk;
                }
                else
                {
                    return sutun == Sutun && satir >= Satir && satir < Satir + Uzunluk;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            clickedButton.Enabled = false;
            clickedButton.Visible = false;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;

            foreach (Button dugme in dugmeler)
            {
                dugme.Visible = true;
            }

            OyunuSifirla();

            clickedButton.Text = "Yeniden Oyna";
            clickedButton.Size = new Size(92 * this.ClientSize.Width / 1008, 38 * this.ClientSize.Height / 614);
            clickedButton.Location = new Point(this.ClientSize.Width / 2 - 46 * this.ClientSize.Width / 1008, this.ClientSize.Height / 2 - 19 * this.ClientSize.Height / 614);
            clickedButton.Visible = false;
            clickedButton.Enabled = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 yeniForm = new Form2();

            yeniForm.Show();
        }
    }
}