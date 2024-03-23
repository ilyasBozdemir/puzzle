using System;
using System.Collections.Generic;
using System.Xml;

namespace puzzle_game
{
    public class PuzzleModel
    {
        public int id { get; set; }
        public string adi { get; set; }
        public string filename { get; set; }
    }
    public static class PuzzleModelList
    {
        public static bool backToTop { get; set; }
        public static List<PuzzleModel> ListeyiGetir()
        {
            List<PuzzleModel> puzzles = new List<PuzzleModel>();
            XmlDocument document = new XmlDocument();
            //System.Windows.Forms.Application.StartupPath\bin\Debug\bulmaca.xml
            document.Load("bulmaca.xml");
            XmlNodeList nodeList = document.SelectNodes("Bulmacalar/Bulmaca");
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode xmlNode = nodeList[i];
                puzzles.Add(new PuzzleModel
                {
                    id = int.Parse(xmlNode.Attributes["id"].Value),
                    adi = xmlNode.Attributes["adi"].Value,
                    filename = xmlNode.Attributes["filename"].Value
                });
            }
            return puzzles;
        }
        public static PuzzleModel BulmacayiGetir(int puzzleIndex)
        {
            int puzzleCount = ListeyiGetir().Count;
            backToTop = true;//false olursa bulmacalar bitince oyun kapanır.
            if (backToTop)
                if (puzzleIndex >= puzzleCount)
                    puzzleIndex = puzzleIndex % puzzleCount;
            else
                if (puzzleIndex >= puzzleCount)
                    System.Windows.Forms.Application.Exit();
            return ListeyiGetir()[puzzleIndex];
        }

    }
}
