using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puzzle_game
{
    public partial class PuzzleHelper
    {
        private PuzzleCollection puzzleCollection;
        public PuzzleHelper()
        {
            puzzleCollection = new PuzzleCollection();
        }
        public List<Puzzle> ParcalaVeGetir(Bitmap puzzleImage)
        {
            if (puzzleImage.Height == puzzleImage.Width)
            {
                int kareBoyutu = puzzleImage.Width / PuzzleGame.yatayParcaSayisi;

                Rectangle kirpmaAlani = new Rectangle(0, 0, kareBoyutu, kareBoyutu);
                int id = 0;
                for (int y = 0; y < PuzzleGame.yatayParcaSayisi; y++)
                {
                    for (int x = 0; x < PuzzleGame.dikeyParcaSayisi; x++)
                    {
                        kirpmaAlani.X = (x * kareBoyutu);//X: yatay eksendir ondan x*
                        kirpmaAlani.Y = (y * kareBoyutu);//y: dikey eksendir ondan y*
                        puzzleCollection.AddPuzzle(new Puzzle
                        {
                            ID = id,
                            PuzzleIcindekiKoordinati = new Point(x: x, y: y),//burda da aynı kurala uyduk.
                            puzzleImage = puzzleImage.Clone(kirpmaAlani, puzzleImage.PixelFormat),
                            PuzzleParcasiRect = new Rectangle
                            {
                                Location = new Point
                                {
                                    X = (x * PuzzleGame.ekrandakiKareBoyutu),
                                    Y = (y * PuzzleGame.ekrandakiKareBoyutu)
                                },
                                Size = new Size
                                {
                                    Width = PuzzleGame.ekrandakiKareBoyutu,
                                    Height = PuzzleGame.ekrandakiKareBoyutu
                                }
                            },
                            Name = $"x{x}y{y}",
                        });
                        id += 1;
                    }
                }
                return puzzleCollection.GetPuzzles();
            }
            else
            {
                throw new PuzzleException($"verilen {nameof(puzzleImage)} bir kare özelliği taşımıyor.", new PuzzleException());
            }
        }
        public void ParcayiAlanaBirak(PictureBox pictureBox, Point parcaEtkiAlani)
        {
            for (int d = 0; d < PuzzleGame.yatayParcaSayisi; d++)
            {
                for (int y = 0; y < PuzzleGame.dikeyParcaSayisi; y++)
                {
                    Point altSinir = new Point
                    {
                        X = (y * PuzzleGame.ekrandakiKareBoyutu),
                        Y = (d * PuzzleGame.ekrandakiKareBoyutu)
                    };
                    Point ustSinir = new Point
                    {
                        X = (altSinir.X + parcaEtkiAlani.X),
                        Y = (altSinir.Y + parcaEtkiAlani.Y)
                    };
                    if (PointKarsilastir(pictureBox.Location, altSinir, ustSinir))
                    {
                        pictureBox.Location = altSinir;
                    }
                    PuzzleGame.ParcaYerlestiMiDict[pictureBox] = ParcayiAlandaMi(pictureBox);
                }
            }
        }
        private bool PointKarsilastir(Point p1, Point left, Point right)
        {
            if (p1.X >= left.X && p1.X <= right.X)
            {
                if (p1.Y >= left.Y && p1.Y <= right.Y)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ParcayiAlandaMi(PictureBox pictureBox)
        {
            for (int i = 0; i < PuzzleGame.puzzles.Count; i++)
            {
                if (pictureBox.Name == PuzzleGame.puzzles[i].Name)
                {
                    if (pictureBox.Location == PuzzleGame.puzzles[i].PuzzleParcasiRect.Location)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void IpucuVer()//daha da geliştirilecektir.d
        {
            int sayiAraligi = int.Parse(Math.Pow(PuzzleGame.yatayParcaSayisi, 2).ToString());
            List<int> KullanilabilirSayilar = new List<int>(sayiAraligi);

            Task.Run(() =>
            {
                Dictionary<PictureBox, bool>.ValueCollection values = PuzzleGame.ParcaYerlestiMiDict.Values;
                //Dictionary<PictureBox, bool>.KeyCollection keys = PuzzleGame.ParcaYerlestiMiDict.Keys;

                Random random = new Random(DateTime.Now.Millisecond);

                bool[] ParcaDurumlari = new bool[sayiAraligi];

                values.CopyTo(ParcaDurumlari, 0);

                for (int i = 0; i < sayiAraligi; i++)
                {
                    if (ParcaDurumlari[i] == false)
                    {
                        KullanilabilirSayilar.Add(i);
                    }
                }

                bool durum = true;

                while (durum)
                {
                    int Ipucu = 0;
                    Ipucu = random.Next(sayiAraligi);
                    Puzzle puzzle = PuzzleGame.puzzles[Ipucu];
                    int y = puzzle.PuzzleIcindekiKoordinati.Y;
                    int x = puzzle.PuzzleIcindekiKoordinati.X;

                    if (KullanilabilirSayilar.Contains(Ipucu))
                    {
                        PuzzleGame.ResimParcalariBox[y, x].Location = puzzle.PuzzleParcasiRect.Location;
                        durum = false;
                        //break;
                    }
                }
            });
        }

        public bool BulmacaTamalandiMi()
        {
            int counter = 0;
            for (int i = 0; i < PuzzleGame.puzzles.Count; i++)
            {
                Puzzle puzzle = PuzzleGame.puzzles[i];
                int y = puzzle.PuzzleIcindekiKoordinati.Y;
                int x = puzzle.PuzzleIcindekiKoordinati.X;
                if (PuzzleGame.ResimParcalariBox[y, x].Location == puzzle.PuzzleParcasiRect.Location)
                {
                    counter += 1;
                }
            }
            if (counter == PuzzleGame.puzzles.Count)
            {
                return true;
            }
            //ParcayiAlandaMi
            return false;
        }
    }
}
