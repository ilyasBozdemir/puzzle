using System.Collections.Generic;
using System.Drawing;

namespace puzzle_game
{
    public class Puzzle
    {
        public int ID { get; set; }
        public Rectangle PuzzleParcasiRect { get; set; }
        public Point PuzzleIcindekiKoordinati { get; set; }
        public Bitmap puzzleImage { get; set; }
        public string Name { get; set; }
        public Puzzle()
        {
            PuzzleParcasiRect = new Rectangle(0, 0, 0, 0);
            PuzzleIcindekiKoordinati = new Point(0, 0);
        }
    }
    public class PuzzleCollection
    {
        private readonly List<Puzzle> puzzles;

        public PuzzleCollection() => puzzles = new List<Puzzle>();

        public void AddPuzzle(Puzzle puzzle) => puzzles.Add(puzzle);

        public List<Puzzle> GetPuzzles() => puzzles;

    }

}
