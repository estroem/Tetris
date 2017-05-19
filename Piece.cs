using System;
using System.Drawing;

namespace Tetris {
    class Piece {
        private bool[][,] grid;
        public Point pos { get; set; }
        public Color color { get; set; }
        private int orientation;
        
        private Piece(Color color, bool[][,] grid) {
            this.color = color;
            this.grid = grid;
        }
        
        public Point[] GetBlocks() {	
            Point[] blocks = new Point[grid[orientation].GetLength(0) * grid[orientation].GetLength(1)];
            int count = 0;
            
            for(int i = 0; i < grid[orientation].GetLength(0); ++i) {
                for(int j = 0; j < grid[orientation].GetLength(1); ++j) {
                    if(grid[orientation][i, j]) blocks[count++] = pos + new Size(i, j);
                }
            }
            
            Point[] ret = new Point[count];
            Array.Copy(blocks, 0, ret, 0, count);
            
            return ret;
        }
        
        public void SnapDown(Screen screen) {
            int min = 0;
            
            foreach(Point block in GetBlocks()) {
                for(int i = 1;; ++i) {
                    if(screen.BlockExists(block.X, block.Y + i) || block.Y + i >= screen.GetHeight()) {
                        if(min == 0 || i - 1 < min)
                            min = i - 1;
                        break;
                    }
                }
            }
            
            pos = new Point(pos.X, pos.Y + min);
        }
        
        public bool MoveDown(Screen screen) {
            pos = new Point(pos.X, pos.Y + 1);
            bool ret = false;
            
            foreach(Point block in GetBlocks()) {
                if(block.Y >= screen.GetHeight() || Overlaps(screen)) {
                    pos = new Point(pos.X, pos.Y - 1);
                    return true;
                }
            }
            
            return ret;
        }
        
        public bool MoveRight(Screen screen) {
            pos = new Point(pos.X + 1, pos.Y);
            bool ret = false;
            
            foreach(Point block in GetBlocks()) {			
                if(block.X >= screen.GetWidth() || Overlaps(screen)) {
                    pos = new Point(pos.X - 1, pos.Y);
                    return true;
                }
            }
            
            return ret;
        }
        
        public bool MoveLeft(Screen screen) {
            pos = new Point(pos.X - 1, pos.Y);
            bool ret = false;
            
            foreach(Point block in GetBlocks()) {
                if(block.X < 0 || Overlaps(screen)) {
                    pos = new Point(pos.X + 1, pos.Y);
                    return true;
                }
            }
            
            return ret;
        }
        
        public void Rotate(Screen screen) {
            if(++orientation == grid.Length)
                orientation = 0;
            
            if(!IsInside(screen) || Overlaps(screen)) {
                if(orientation == 0)
                    orientation = grid.Length - 1;
                else
                    --orientation;
            }
        }
        
        public bool IsInside(Screen screen) {
            foreach(Point block in GetBlocks()) {
                if(block.X < 0 || block.X >= screen.GetWidth() || block.Y < 0 || block.Y >= screen.GetHeight()) return false;
            }
            
            return true;
        }
        
        public bool Overlaps(Screen screen) {
            foreach(Point block in GetBlocks()) {
                if(screen.BlockExists(block.X, block.Y)) return true;
            }
            
            return false;
        }
        
        public static Piece GetPiece(int num) {
            Color color = Color.Black;
            bool[][,] grid = new bool[0][,];
            
            switch(num) {
            case 0:
                color = Color.Blue;
                grid = new bool[1][,];
                grid[0] = new bool[4,4];
                grid[0][1, 1] = grid[0][1, 2] = grid[0][2, 1] = grid[0][2, 2] = true;
                break;
            
            case 1:
                color = Color.Yellow;
                grid = new bool[4][,];
                grid[0] = new bool[4,4];
                grid[1] = new bool[4,4];
                grid[2] = new bool[4,4];
                grid[3] = new bool[4,4];
                
                grid[0][1, 1] = grid[0][0, 2] = grid[0][1, 2] = grid[0][2, 2] = true;
                grid[1][1, 1] = grid[1][1, 3] = grid[1][1, 2] = grid[1][2, 2] = true;
                grid[2][0, 2] = grid[2][1, 3] = grid[2][1, 2] = grid[2][2, 2] = true;
                grid[3][0, 2] = grid[3][1, 3] = grid[3][1, 2] = grid[3][1, 1] = true;
                break;
            
            case 2:
                color = Color.Green;
                grid = new bool[2][,];
                grid[0] = new bool[4,4];
                grid[1] = new bool[4,4];
                
                grid[0][1, 1] = grid[0][2, 1] = grid[0][0, 2] = grid[0][1, 2] = true;
                grid[1][1, 1] = grid[1][2, 1] = grid[1][1, 0] = grid[1][2, 2] = true;
                break;
            
            case 3:
                color = Color.Teal;
                grid = new bool[2][,];
                grid[0] = new bool[4,4];
                grid[1] = new bool[4,4];
                
                grid[0][0, 1] = grid[0][1, 1] = grid[0][1, 2] = grid[0][2, 2] = true;
                grid[1][2, 1] = grid[1][1, 1] = grid[1][1, 2] = grid[1][2, 0] = true;
                break;
            
            case 4:
                color = Color.Purple;
                grid = new bool[4][,];
                grid[0] = new bool[4,4];
                grid[1] = new bool[4,4];
                grid[2] = new bool[4,4];
                grid[3] = new bool[4,4];
                
                grid[0][0, 1] = grid[0][1, 1] = grid[0][2, 1] = grid[0][0, 2] = true;
                grid[1][0, 0] = grid[1][1, 1] = grid[1][1, 2] = grid[1][1, 0] = true;
                grid[2][2, 0] = grid[2][1, 1] = grid[2][0, 1] = grid[2][2, 1] = true;
                grid[3][1, 0] = grid[3][1, 1] = grid[3][2, 2] = grid[3][1, 2] = true;
                break;
            
            case 5:
                color = Color.Red;
                grid = new bool[4][,];
                grid[0] = new bool[4,4];
                grid[1] = new bool[4,4];
                grid[2] = new bool[4,4];
                grid[3] = new bool[4,4];
                
                grid[0][0, 1] = grid[0][1, 1] = grid[0][2, 1] = grid[0][2, 2] = true;
                grid[1][0, 2] = grid[1][1, 1] = grid[1][1, 2] = grid[1][1, 0] = true;
                grid[2][0, 0] = grid[2][1, 1] = grid[2][0, 1] = grid[2][2, 1] = true;
                grid[3][1, 0] = grid[3][1, 1] = grid[3][2, 0] = grid[3][1, 2] = true;
                break;
            
            case 6:
                color = Color.Orange;
                grid = new bool[2][,];
                grid[0] = new bool[4,4];
                grid[1] = new bool[4,4];
                
                grid[0][0, 1] = grid[0][1, 1] = grid[0][2, 1] = grid[0][3, 1] = true;
                grid[1][1, 0] = grid[1][1, 1] = grid[1][1, 2] = grid[1][1, 3] = true;
                break;
            }
            
            return new Piece(color, grid);
        }
    }
}