using System.Drawing;

namespace Tetris {
    class GUI {
        private int width;
        private int height;
        private int blockSize;
        
        public GUI(int width, int height, int blockSize) {
            this.width = width;
            this.height = height;
            this.blockSize = blockSize;
        }
        
        public void DrawLayout(Graphics graphics) {
            Pen outline = new Pen(Color.Black, 2);
            graphics.DrawRectangle(outline, 29, 29, width * blockSize + 2, height * blockSize + 2);
        }
        
        public void DrawScreen(Graphics graphics, Screen screen, int x, int y) {
            for(int i = 0; i < screen.GetWidth(); ++i) {
                for(int j = 0; j < screen.GetHeight(); ++j) {
                    if(screen.BlockExists(i, j)) {
                        DrawBlock(graphics, screen.GetColor(i, j), x + i * blockSize, y + j * blockSize);
                    }
                }
            }
        }
        
        public void DrawPiece(Graphics graphics, Piece piece, int x, int y) {
            foreach(Point block in piece.GetBlocks()) {
                DrawBlock(graphics, piece.color, x + block.X * blockSize, y + block.Y * blockSize);
            }
        }
        
        public void DrawBlock(Graphics graphics, Color color, int x, int y) {
            graphics.FillRectangle(new SolidBrush(color), x, y, blockSize, blockSize);
            graphics.DrawRectangle(new Pen(Color.Black), x, y, blockSize, blockSize);
        }
    }
}