using DesktopLibrary;
using DesktopLibrary.DesktopLibrary.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puzzle_game
{
    //amaç sürükle bırak yapmadan dinamik olarak yazmak.
    partial class PuzzleGame//tanım alanı
    {
        private static Form activeForm;
        private IContainer components;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Button button1, button2, button3;
        //"global::" aynı namespace üzerinde aynı class varsa kullanılır.
        //amaç karışıklık önlemektir.
        protected void InitializeComponent()
        {
            // 
            // components
            // 
            this.components = new Container();
            //
            // activeForm
            //
            activeForm.AutoScaleDimensions = new SizeF(6F, 13F);
            activeForm.AutoScaleMode = AutoScaleMode.Font;
            activeForm.ClientSize = new Size(765, 340);
            activeForm.MaximizeBox = false;
            activeForm.MaximumSize = new Size(780, 380);
            activeForm.MinimumSize = new Size(780, 380);
            activeForm.Name = "activeForm";
            activeForm.Text = "Bulmaca Oyunu";
            activeForm.SuspendLayout();
            // 
            // pictureBox1
            //
            this.pictureBox1 = new PictureBox
            {
                BackColor = Color.Gray,
                Location = new Point(20, 40),
                Name = "pictureBox1",
                Size = new Size(225, 225),
                SizeMode = PictureBoxSizeMode.StretchImage,
                TabIndex = 1,
                TabStop = false
            };
            ((ISupportInitialize)(this.pictureBox1)).BeginInit();
            // 
            // panel1
            // 
            this.panel1 = new Panel
            {
                BackColor = Color.LightSeaGreen,
                Location = new Point(300, 40),
                Name = "panel1",
                Size = new Size(458, 225),
                TabIndex = 0,
            };
            this.panel1.Paint += new PaintEventHandler(Panel1_Paint);
            // 
            // button1
            //
            this.button1 = new Button
            {
                Location = new Point(20, 282),
                Name = "button1",
                Size = new Size(70, 47),
                TabIndex = 1,
                Text = "Çözümü Göster",
                UseVisualStyleBackColor = true
            };
            this.button1.Click += new EventHandler(button1_Click);
            // 
            // button2
            // 
            this.button2 = new Button
            {

                Location = new Point(175, 282),
                Name = "button2",
                Size = new Size(70, 47),
                TabIndex = 3,
                Text = "Sonraki Bulmaca",
                UseVisualStyleBackColor = true
            };
            this.button2.Click += new EventHandler(button2_Click);
            // 
            // button3
            // 
            this.button3 = new Button
            {
                Location = new Point(95, 282),
                Name = "button3",
                Size = new Size(70, 47),
                Text = "İpucu Ver",
                TabIndex = 2,
                UseVisualStyleBackColor = true
            };
            this.button3.Click += new EventHandler(button3_Click);
            // 
            // activeForm.Controls.Add
            // 
            Control[] controls =
                {
                this.panel1,
                this.pictureBox1,
                this.button1,
                this.button2,
                this.button3
            };
            foreach (var control in controls)
            {
                activeForm.Controls.Add(control);
            }
        }
    }
    partial class PuzzleGame//temel ayarlar
    {
        private int PuzzleIndex { get; set; }
        public static int yatayParcaSayisi { get; private set; }
        public static int dikeyParcaSayisi { get; private set; }
        public static int ekrandakiKareBoyutu { get; private set; }
        //
        private PuzzleHelper puzzleHelper;
        private PuzzleModel puzzleModel;
        private static PuzzleGame puzzleGame;
        public static PictureBox[,] ResimParcalariBox;
        public static List<Puzzle> puzzles;
        private DraggableControl draggableControl;
        private readonly Point ParcalarinBaslangicPointi;
        private Point ParcaEtkiAlani;
        public static Dictionary<PictureBox, bool> ParcaYerlestiMiDict;
        public static PuzzleGame NewGame(Form form)
        {
            return (puzzleGame == null) ? puzzleGame = new PuzzleGame(form) : puzzleGame;
        }
        private PuzzleGame(Form form)
        {
            activeForm = form;
            Control.CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();

            ParcalarinBaslangicPointi = new Point(x: (panel1.Width / 2) + 10, y: 0);
            ParcaEtkiAlani = new Point(15, 15);
        }
        public void StartGame()
        {
            PuzzleIndex = 0;
            PuzzleGetir(PuzzleIndex);
        }
        public void SiradakiPuzzle()
        {
            PuzzleGetir(PuzzleIndex += 1);
        }
        private void PuzzleGetir(int PuzzleIndex)
        {
            puzzleHelper = new PuzzleHelper();
            puzzleModel = PuzzleModelList.BulmacayiGetir(PuzzleIndex);
            puzzleParcaEbatAyarla();
            pictureBox1.ImageLocation = puzzleModel.filename;
            BulmacaParcalariniGetir();
        }
        private void puzzleParcaEbatAyarla()
        {
            yatayParcaSayisi = 4;
            dikeyParcaSayisi = 4;
            ekrandakiKareBoyutu = 56;
        }
        private void BulmacaParcalariniGetir()
        {
            puzzles = puzzleHelper.ParcalaVeGetir(puzzleImage: new Bitmap(puzzleModel.filename));
            ResimParcalariBox = new PictureBox[yatayParcaSayisi, dikeyParcaSayisi];
            this.panel1.Controls.Clear();
            draggableControl = new DraggableControl();

            ParcaYerlestiMiDict = new Dictionary<PictureBox, bool>();

            for (int i = 0; i < puzzles.Count; i++)
            {
                int d = puzzles[i].PuzzleIcindekiKoordinati.Y;
                int y = puzzles[i].PuzzleIcindekiKoordinati.X;
                ResimParcalariBox[d, y] = new PictureBox
                {
                    Size = new Size(ekrandakiKareBoyutu, ekrandakiKareBoyutu),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = puzzles[i].puzzleImage,
                    Name = puzzles[i].Name,
                };
                ResimParcalariBox[d, y].MouseUp += new MouseEventHandler(ResimParcasiBox_MouseUp);
             
                ParcaDurumunuAyarla(ResimParcalariBox[d, y], false);
                
                this.panel1.Controls.Add(ResimParcalariBox[d, y]);
                draggableControl.addControl(ResimParcalariBox[d, y], true);
            }
            BulmacaParcalariDagit();
        }

        private void ParcaDurumunuAyarla(PictureBox pictureBox, bool state)
        {
            ParcaYerlestiMiDict.Add(pictureBox, state);
        }
        private void BulmacaParcalariDagit()
        {
            int sayiAraligi = (yatayParcaSayisi * yatayParcaSayisi);

            UniqueRandom uniqueRandom = new UniqueRandom(DateTime.Now.Second)
            {
                Capacity = sayiAraligi
            };
            List<int> RandomBenzersizSayilarListesi = uniqueRandom.getUniqueList(sayiAraligi);

            for (int i = 0; i < puzzles.Count; i++)
            {
                int x = puzzles[i].PuzzleIcindekiKoordinati.X;
                int y = puzzles[i].PuzzleIcindekiKoordinati.Y;
                int randomSayi = RandomBenzersizSayilarListesi[i];

                // d , y puzzles[i]ye baglı olarak  normal indeks içinde gidicek
                // ama locationu  random randomSayidaki indeksten gidicek.
                ResimParcalariBox[y, x].Location = new Point
                {
                    X = (puzzles[randomSayi].PuzzleParcasiRect.X + ParcalarinBaslangicPointi.X),
                    Y = (puzzles[randomSayi].PuzzleParcasiRect.Y + ParcalarinBaslangicPointi.Y)
                };
            }
        }
        private void CozumuGoster()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < puzzles.Count; i++)
                {
                    Point point = puzzles[i].PuzzleParcasiRect.Location;
                    Point point2 = puzzles[i].PuzzleIcindekiKoordinati;

                    int d = point2.Y;
                    int y = point2.X;

                    if (ResimParcalariBox[d, y].Location != point)
                    {
                        ResimParcalariBox[d, y].Location = point;
                        PuzzleGame.ParcaYerlestiMiDict[ResimParcalariBox[d, y]] = true;
                        Thread.Sleep(200);
                    }
                }
            });
        }
        private void GrafikCiz(Graphics graphics)
        {
            Size size = new Size(ekrandakiKareBoyutu, ekrandakiKareBoyutu);
            Point point = new Point(0, 0);
            Rectangle rectangle = new Rectangle(point, size);
            for (int d = 0; d < yatayParcaSayisi; d++)
            {
                for (int y = 0; y < dikeyParcaSayisi; y++)
                {
                    rectangle.Y = (d * rectangle.Size.Height);
                    rectangle.X = (y * rectangle.Size.Width);
                    graphics.DrawRectangle(Pens.Black, rectangle);
                }
            }
        }
        ~PuzzleGame()//Yıkıcı Method
        {
            // Buraya yazılan kod Bellekten atılmadan önce
            // GC (çöp toplayıcılar) tarafından derlenir ve Dispose edilir.

        }
    }
    partial class PuzzleGame//form bileşenlerinin olay yönetim alanı
    {
        private void button1_Click(object sender, EventArgs e)
        {
            CozumuGoster();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SiradakiPuzzle();
        }
        int a = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            bool ipucuVarMi = true;
            if (ipucuVarMi)
            {
                puzzleHelper.IpucuVer();
                a += 1;
            }
            else
            {
                button3.Enabled = false;
            }
            activeForm.Text = "Deneme : " + a;
        }
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            GrafikCiz(e.Graphics);
        }
        private void ResimParcasiBox_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox pictureBox = ((PictureBox)sender);
            puzzleHelper.ParcayiAlanaBirak(pictureBox, ParcaEtkiAlani);
            if (puzzleHelper.BulmacaTamalandiMi())
            {
                MessageBox.Show("Bulmacayı Tamamladınız tebrikler!");
            }
        }
    }
}
