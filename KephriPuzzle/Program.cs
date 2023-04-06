//This program is used to solve the khephri puzzle in the most optimal way for all layouts
namespace KhephriPuzzle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Generating the 3x3 board
            bool[,] board = { { false, false, false }, { false, false, false }, { false, false, false } };
            board = FlipTile(board, 0, 0);
        }

        static bool[,] FlipTile(bool[,] board, int xIndex, int yIndex)
        {
            //Returns early if trying to flip the middle tile
            if (xIndex == 1 && yIndex == 1) { return board; }
            //Flipping the correct tiles
            board[xIndex, yIndex] ^= true;
            
            if (yIndex != 1)
            {
                if (xIndex > 0) { board[xIndex - 1, yIndex] ^= true; }
                if (xIndex < 2) { board[xIndex + 1, yIndex] ^= true; }
            }

            if (xIndex != 1)
            {
                if (yIndex > 0) { board[xIndex, yIndex - 1] ^= true; }
                if (yIndex < 2) { board[xIndex, yIndex + 1] ^= true; }
            }
            return board;
        }

    }
}