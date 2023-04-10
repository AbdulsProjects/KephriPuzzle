//This program is used to solve the khephri puzzle in the most optimal way for all layouts
namespace KhephriPuzzle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Generating the 3x3 board
            bool[,] board = { { true, true, false }, { true, false, true }, { true, true, true } };
            int[] bestTurns = SimulateGame(board);
            foreach (int turn in bestTurns)
            {
                Console.WriteLine(turn);
            }
        }

        //Simulating all possible 10-turn moves for the current board, and returning the best tile order
        static int[] SimulateGame(bool[,] board)
        {
            //Creating an array that stores the tiles flipped for each turn. The same tile can't be flipped on 2 consecutive 
            int[] flipOrder = { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int[] optimalFlipOrder = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int turnsToWin = 10;
            bool simulationComplete = false;

            //NEED TO MAKE IT LOOP THROUGH AND CHANGE THE TURN combination

            while (!simulationComplete)
            {
                //Testing the current turn combination
                for (int index = 0; index < 10; index++)
                {
                    //Excecuting the turn, and checking if the win condition has been met
                    board = FlipTile(board, flipOrder[index]);
                    if (CheckForWin(board))
                    {
                        //Checking if the current flip order is more efficient than those tested before, and reassigning the benchmarks if so
                        if (index + 1 < turnsToWin)
                        {
                            turnsToWin = index + 1;
                            optimalFlipOrder = flipOrder;
                        }
                        continue;
                    }
                }
                
                //Generating the next turn combination by iterating through the turn combination backwards, and incrementing by 1 if less than 9
                //If the tile flipped at the current index is 9, the value is set to 1, and the iteration looks at the previous turn
                //The tile flipped can't be set to the same tile as the previous turn, as this would be a waste of 2 turns. Loops may still occur at a larger scale (e.g. over 4 turns)
                for (int index = 9; index >= 0; index--)
                {
                    flipOrder[index] = flipOrder[index] + 1;
                    if (index != 0) { if (flipOrder[index] == flipOrder[index - 1]) { flipOrder[index] = flipOrder[index] + 1; } }
                    //Exits the loop if all permutations have been tested
                    if (index == 0 && flipOrder[index] == 10) 
                    { 
                        simulationComplete = true;
                        break;
                    }
                    if (flipOrder[index] > 9) { flipOrder[index] = 1; }
                    else { break; }
                }

            }

            Console.WriteLine(turnsToWin);
            return flipOrder;
        }


        //Simuating flipping a tile
        static bool[,] FlipTile(bool[,] board, int Index)
        {
            //Returns early if trying to flip the middle tile
            if (Index == 4) { return board; }
            //Converting to xy co-ordinates
            int xIndex = (Index-1) % 3;
            int yIndex = (Index-1) / 3;

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

        //Displaying the board as a 3x3 grid
        static void DisplayBoard(bool[,] board)
        {
            for (int index = 0; index < 3; index++)
            {
                Console.WriteLine(Convert.ToString(board[0, index]) + Convert.ToString(board[1, index]) + Convert.ToString(board[2, index]));
            }
        }

        //Indentifying if the win condition has been met
        static bool CheckForWin(bool[,] board)
        {
            for (int xIndex = 0; xIndex < 3; xIndex++)
            { 
                for (int yIndex = 0; yIndex < 3; yIndex++)
                {
                    if (xIndex == 1 && yIndex == 1) { continue; }
                    if (!board[xIndex, yIndex]) { return false; }
                }
            }

            return true;
        }
    }
}