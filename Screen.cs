using System.Drawing;

namespace Tetris {
    class Screen {
        Color[,] grid;
        
        public Screen(int width, int height) {
            grid = new Color[width, height];
        }
        
        public int GetWidth() {
            return grid.GetLength(0);
        }
        
        public int GetHeight() {
            return grid.GetLength(1);
        }
        
        public void SetBlock(int x, int y, Color value) {
            grid[x, y] = value;
        }
        
        public void RemoveBlock(int x, int y) {
            grid[x, y] = Color.Empty;
        }
        
        public bool BlockExists(int x, int y) {
            if(x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1)) return false;
            return grid[x, y] != Color.Empty;
        }
        
        public bool IsInside(int x, int y) {
            if(x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1)) return true;
            else return false;
        }
        
        public bool IsAvailable(int x, int y) {
            return IsInside(x, y) && !BlockExists(x, y);
        }
        
        public Color GetColor(int x, int y) {
            return grid[x, y];
        }
        
        public void AddPiece(Piece piece) {
            foreach(Point block in piece.GetBlocks()) {
                SetBlock(block.X, block.Y, piece.color);
            }
        }
        
        private bool RowIsFull(int y) {
            for(int i = 0; i < GetWidth(); ++i) {
                if(!BlockExists(i, y)) return false;
            }
            
            return true;
        }
        
        private void RemoveRow(int y) {
            for(int i = y; i >= 0; --i) {
                for(int j = 0; j < GetWidth(); ++j) {
                    if(i == 0)
                        RemoveBlock(j, i);
                    else
                        SetBlock(j, i, GetColor(j, i - 1));
                }
            }
        }
        
        public int RemoveFullRows() {
            int count = 0;
            
            for(int i = 0; i < Tetris.SCREEN_HEIGHT; ++i) {
                if(RowIsFull(i)) {
                    RemoveRow(i);
                    count++;
                }
            }
            
            return count;
        }
    }
}