﻿namespace KephriClassLibrary
{
    public static class LibraryProgram
    {
        //Simulating all possible 10-turn moves for the current board. 1st return identifies if a solution was found, 2nd return is the solution as an array
        public static (bool, int[]) SimulateGame(bool[] board)
        {
            //Returns 0 if the entire board is already flipped
            if (CheckForWin(board)) { return (false, new int[] { 0 }); }
            //Initializing the variables and their default values needed to begin the simulation
            int[] flipOrder = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            bool simulationComplete = false;
            int scope = 0;
            bool[] savedBoardState = new bool[9];
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
                    if (CheckForWin(board)) { return (true, flipOrder.Take(index + 1).ToArray()); }
                }

                //Generating the new flip order. Orders with duplicates are skipped to reduce runtime
                bool duplicatesInOrder = true;
                do
                {
                    var newOrderReturn = GenerateNewOrder(flipOrder, scope);
                    flipOrder = newOrderReturn.Item1;
                    scope = newOrderReturn.Item2;
                    //Testimg if the new flip order has duplicates
                    int[] scopedFlipOrder = new int[scope + 1];
					Array.Copy(flipOrder, scopedFlipOrder, scopedFlipOrder.Length);
                    if (scopedFlipOrder.Length == scopedFlipOrder.Distinct().Count()) { duplicatesInOrder = false; }

				} while (duplicatesInOrder);

                //All combinations tested
                if (scope == 9) { simulationComplete = true; }
            }

            return (false, flipOrder);
        }

        static (int[], int) GenerateNewOrder(int[] order, int scope)
        {
            for (int index = 0; index <= scope; index++)
            {
                if (order[index] < 8)
                {
                    order[index] = order[index] + 1;
                    if (order[index] == 4) { order[index] = order[index] + 1; }
                    break;
                }
                else
                {
                    //Index at the end of scope is 8, meaning all combinations at current scope have been tested. Increasing scope
                    if (index == scope)
                    {
                        //After increasing the scope, the new order will be 1 in all positions, except for the scope, which will be 2
                        scope++;
                        for (int revIndex = index; revIndex >= 0; revIndex--) { order[index] = 0; }
                        if (scope == 9) { break; }
                        order[scope] = 1;
                        break;
                    }
                    //Index not at the end of scope, setting index to 1 and moving to the next position
                    else
                    {
                        order[index] = 0;
                    }
                }
            }
            return (order, scope);
        }

        //Simuating flipping a tile
        static public bool[] FlipTile(bool[] board, int index)
        {
            //Returns early if trying to flip the middle tile
            if (index == 4) { return board; }

            //Flipping the correct tiles
            board[index] ^= true;
            //Middle Column
            if((index + 1) % 3 == 2)
            {
                board[index + 1] ^= true;
                board[index - 1] ^= true;
				return board;
			}
            //Far right column
            if((index + 1) % 3 == 0) { board[index - 1] ^= true;}
            //Far left column
            if((index + 1) % 3 == 1) { board[index + 1] ^= true;}
            //Both Columns
            if((index + 3) < board.Length) { board[index + 3] ^= true;}
			if ((index - 3) >= 0) { board[index - 3] ^= true; }
            return board;
        }

        //Indentifying if the win condition has been met
        static bool CheckForWin(bool[] board)
        {
            for (int index = 0; index < board.Length; index++)
            {
                //4th index is skipped as this is the middle invisible tile
                if (index == 4) { continue; }
                if (!board[index]) { return false; }
            }
            return true;
        }
    }
}