using System;
using System.Media;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Tetris {
    public class Tetris : Form {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);

        public const int BLOCK_SIZE = 25;
        public const int SCREEN_WIDTH = 10;
        public const int SCREEN_HEIGHT = 20;
        
        private static System.Threading.Timer timer;
        private static Tetris instance;
        
        private GUI gui;
        private Random rand;
        private Screen screen;
        private Piece curPiece;
        private Piece nextPiece;
        private Label lineCounter;
        private Label gameOverLabel;
        private int linesRemoved = 0;
        private bool gameOver = false;

        public Tetris() {
            InitializeComponent();
            
            gui = new GUI(SCREEN_WIDTH, SCREEN_HEIGHT, BLOCK_SIZE);
            rand = new Random();
            screen = new Screen(SCREEN_WIDTH, SCREEN_HEIGHT);
            
            AddLabels();
        }
        
        private void InitializeComponent() {
            this.DoubleBuffered = true;
        
            this.Size = new Size(SCREEN_WIDTH * BLOCK_SIZE + 200, SCREEN_HEIGHT * BLOCK_SIZE + 100);
            this.BackColor = System.Drawing.Color.White;
            
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Tetris_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tetris_KeyDown);
        }
        
        private void AddLabels() {
            lineCounter = new Label();
            lineCounter.Location = new Point(345, 100);
            lineCounter.Font = new Font(FontFamily.GenericSansSerif, 18);
            this.Controls.Add(lineCounter);
            
            Label countLabel = new Label();
            countLabel.Location = new Point(305, 80);
            countLabel.Size = new Size(200, 50);
            countLabel.Text = "LINE COUNT";
            countLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
            this.Controls.Add(countLabel);
            
            Label nextLabel = new Label();
            nextLabel.Location = new Point(330, 190);
            nextLabel.Text = "NEXT";
            nextLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
            this.Controls.Add(nextLabel);
            
            gameOverLabel = new Label();
            gameOverLabel.Location = new Point(85, 200);
            gameOverLabel.Size = new Size(200, 300);
            gameOverLabel.Font = new Font(FontFamily.GenericSansSerif, 32);
            gameOverLabel.BackColor = Color.Transparent;
            this.Controls.Add(gameOverLabel);
        }
        
        private Piece randPiece() {
            return Piece.GetPiece(rand.Next(0, 7));
        }
        
        private void UpdateGame() {
            if(gameOver) return;
            
            if(curPiece == null) {
                if(nextPiece == null) // start of game
                    nextPiece = randPiece();
                
                curPiece = nextPiece;
                curPiece.pos = new Point(3, -1);
                
                if(curPiece.Overlaps(screen)) { // game over
                    gameOver = true;
                    gameOverLabel.Text = "GAME OVER";
                }
                
                nextPiece = randPiece();
            } else {
                bool stop = curPiece.MoveDown(screen);
                
                if(stop) { // piece hit bottom
                    screen.AddPiece(curPiece);
                    linesRemoved += screen.RemoveFullRows();
                    
                    curPiece = null;
                }
            }
        }
        
        public void Tetris_KeyDown(object sender, KeyEventArgs e) {
            if(curPiece == null) return;
            
            switch(e.KeyCode) {
                case Keys.A:
                    curPiece.MoveLeft(screen);
                    break;
                case Keys.D:
                    curPiece.MoveRight(screen);
                    break;
                case Keys.W:
                    curPiece.Rotate(screen);
                    break;
                case Keys.Space:
                    curPiece.SnapDown(screen);
                    break;
            }
            
            Invalidate();
        }
        
        public void Tetris_Paint(object sender, PaintEventArgs e) {
            Graphics graphics = e.Graphics;
            
            gui.DrawLayout(graphics);
            gui.DrawScreen(graphics, screen, 30, 30);
            
            if(curPiece != null)
                gui.DrawPiece(graphics, curPiece, 30, 30);
            if(nextPiece != null)
                gui.DrawPiece(graphics, nextPiece, 310, 200);
            
            lineCounter.Text = linesRemoved.ToString();
        }
        
        private static void Redraw(Object stateInfo) {
            instance.UpdateGame();
            instance.Invalidate();
        }

        public static int Main() {
            AttachConsole(-1);
            
            //SoundPlayer player = new SoundPlayer("Tetris_quiet.wav");
            //player.PlayLooping();
            
            instance = new Tetris();
            
            timer = new System.Threading.Timer(Tetris.Redraw, new AutoResetEvent(false), 0, 100);
            
            Application.Run(instance);
            return 0;
        }
    }
}