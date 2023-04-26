//This program is used to solve the khephri puzzle in the most optimal way for all layouts

namespace KhephriPuzzle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Generating the 3x3 board
            bool[,] board = { { false, false, false }, { true, false, true }, { false, false, false } };
            var simulationResults = SimulateGame(board);
            foreach (int turn in simulationResults.Item2)
            {
                Console.WriteLine(turn);
            }
        }

        //Simulating all possible 10-turn moves for the current board, and returning the best tile order
        static (bool, int[], int) SimulateGame(bool[,] board)
        {
            //Initializing the variables and their default values needed to begin the simulation
            int[] flipOrder = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            bool simulationComplete = false;
            int scope = 0;
            bool[,] savedBoardState = new bool[3, 3];
            Array.Copy(board, savedBoardState, board.Length);

            while (!simulationComplete)
            {
                //Resetting the board state
                Array.Copy(savedBoardState, board, savedBoardState.Length);
                //Testing the current turn combination
                for (int index = 0; index < 10; index++)
                {
                    //Excecuting the turn, and checking if the win condition has been met
                    board = FlipTile(board, flipOrder[index]);
                    if (CheckForWin(board)) { return (true, flipOrder.Take(index+1).ToArray(), index+1); }
                }

                var newOrderReturn = GenerateNewOrder(flipOrder, scope);
                flipOrder = newOrderReturn.Item1;
                scope = newOrderReturn.Item2;
                //All combinations tested
                if (scope == 10) { Console.WriteLine("Error: No winning turn combination found"); simulationComplete = true; }
            }

            return (false, flipOrder, 10);
        }

        static (int[], int) GenerateNewOrder(int[] order, int scope)
        {
            for (int index = 0; index <= scope; index++)
            {
                if (order[index] < 9) 
                { 
                    order[index] = order[index] + 1; 
                    if (order[index] == 5) { order[index] = order[index] + 1; }
                    break;
                }
                else
                {
                    //Index at the end of scope is 9, meaning all combinations at current scope have been tested. Increasing scope
                    if (index == scope)
                    {
                        //After increasing the scope, the new order will be 1 in all positions, except for the scope, which will be 2
                        scope++;
                        for (int revIndex = index; revIndex >= 0; revIndex--) { order[index] = 1; }
                        if (scope==10) { break; }
                        order[scope] = 2;
                        break;
                    }
                    //Index not at the end of scope, setting index to 1 and moving to the next position
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
            if (Index == 5) { return board; }
            //Converting to xy co-ordinates
            int xIndex = (Index-1) % 3;
            int yIndex = (Index-1) / 3;

            //Flipping the correct tiles
            board[yIndex, xIndex] ^= true;

            if (yIndex != 1)
            {
                if (xIndex > 0) { board[yIndex, xIndex - 1] ^= true; }
                if (xIndex < 2) { board[yIndex, xIndex + 1] ^= true; }
            }

            if (xIndex != 1)
            {
                if (yIndex > 0) { board[yIndex - 1, xIndex] ^= true; }
                if (yIndex < 2) { board[yIndex + 1, xIndex] ^= true; }
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