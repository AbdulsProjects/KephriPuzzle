using KephriClassLibrary;
using System.Runtime.CompilerServices;

namespace KephriWebApp.Pages
{
    public partial class Index
    {
        private bool[] flippedTiles = { false, false, false, false, false, false, false, false, false };
        private bool[] solvedFlipOrder = { false, false, false, false, false, false, false, false, false };

        //Allowing the user to flip tiles
        private void ChangeStartState(int index)
        {
            flippedTiles[index] ^= true;
        }

        //Formatting tiles flipped by the user
        private string StateStyle(bool flipped)
        {
            if (flipped) return "flipped-tile";
            return "";
        }

        //Formatting tiles that need to be flipped to solve the puzzle
        private string BorderStyle(bool requiresFlip)
        {
            if (requiresFlip) return "requires-flip";
            return "";
        }

        private void returnFlipOrder()
        {
            bool[,] formattedFlippedTiles = { { flippedTiles[0], flippedTiles[1], flippedTiles[2] }, { flippedTiles[3], flippedTiles[4], flippedTiles[5] }, { flippedTiles[6], flippedTiles[7], flippedTiles[8] } };
            //Resetting the solved array
            for (int i = 0; i < solvedFlipOrder.Length; ++i) { solvedFlipOrder[i] = false; }

            int[] returnedFlipOder = LibraryProgram.SimulateGame(formattedFlippedTiles).Item2;

            //Converting the solvedFlipOrder into a single dimension array for use in the UI 
            foreach (int value in returnedFlipOder) { solvedFlipOrder[value-1] = true; }
        }
    }
}