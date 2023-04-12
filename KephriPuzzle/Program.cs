//This program is used to solve the khephri puzzle in the most optimal way for all layouts
using static System.Formats.Asn1.AsnWriter;

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
            //Initializing the variables and their default values needed to begin the simulation
            int[] flipOrder = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int[] optimalFlipOrder = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int turnsToWin = 10;
            bool simulationComplete = false;
            int scope = 0;
            bool[,] savedBoardState = new bool[3, 3];
            Array.Copy(board, savedBoardState, board.Length);

            //NEED TO MAKE IT LOOP THROUGH AND CHANGE THE TURN combination

            while (!simulationComplete)
            {
                Array.Copy(savedBoardState, board, savedBoardState.Length);
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

                Console.WriteLine("\r\n10 turns simulated:");
                foreach (int turn in flipOrder) { Console.Write(turn + ","); }
                Console.WriteLine("\r\n" + turnsToWin);
                DisplayBoard(board);

                //This while loop is purely for testing. Currently, when going from a scope of 1 to 2, only the [0] index is set to 1. Need to iterate and set all to 1
                //var newOrderReturn = GenerateNewOrder(flipOrder, scope);
                do { var newOrderReturn = GenerateNewOrder(flipOrder, scope); scope = newOrderReturn.Item2; } while (true);
                //flipOrder = newOrderReturn.Item1;
                //scope = newOrderReturn.Item2;

            }

            Console.WriteLine(turnsToWin);
            foreach (int turn in optimalFlipOrder) { Console.Write(turn); }
            return optimalFlipOrder;
        }

        static (int[], int) GenerateNewOrder(int[] order, int scope)
        {
            for (int index = 0; index <= scope; index++)
            {
                if (order[index] < 9) { order[index] = order[index] + 1; break; }
                else
                {
                    //Index at the end of scope is 9, meaning all combinations at current scope have been tested. Increasing scope
                    if (index == scope)
                    {
                        scope++;
                        for (int revIndex = index; revIndex == 0; revIndex--) { order[index] = 1; }
                        //Setting the value of the last element in the new scope to 2, as all combinations with 1 will have been tested prior
                        order[scope] = 2;
                        break;
                    }
                    else
                    {
                        order[index] = 1;
                    }
                }
            }
            return (order, scope);
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