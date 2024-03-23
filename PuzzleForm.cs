using System;
using System.Drawing;
using System.Windows.Forms;

namespace puzzle_game
{
    partial class PuzzleForm
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PuzzleForm
            // 
            this.ClientSize = new System.Drawing.Size(228, 234);
            this.MaximizeBox = false;
            this.Name = "PuzzleForm";
            this.Load += new System.EventHandler(this.PuzzleForm_Load);
            this.ResumeLayout(false);

        }
    }
    partial class PuzzleForm : Form
    {
        public PuzzleForm()
        {
            InitializeComponent();
        }
        private void PuzzleForm_Load(object sender, EventArgs e)
        {
            PuzzleGame puzzleGame1 = PuzzleGame.NewGame(this);
            puzzleGame1.StartGame();
        }
    }
}
