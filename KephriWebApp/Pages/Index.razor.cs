using KephriClassLibrary;
using System.Runtime.CompilerServices;
using System.Xml;

namespace KephriWebApp.Pages
{
    public partial class Index
    {
        private bool[] flippedTiles = { false, false, false, false, false, false, false, false, false };
        private bool[] solvedFlipOrder = { false, false, false, false, false, false, false, false, false };
        private bool mode;
        private bool loading = false;

        //Allowing the user to flip tiles
        private void ChangeStartState(int index)
        {
            //Toggles the correct tiles based on the mode active
            if (mode) { LibraryProgram.FlipTile(flippedTiles, index); return; }
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
            loading = true;
            //Resetting the solved array
            for (int i = 0; i < solvedFlipOrder.Length; ++i) { solvedFlipOrder[i] = false; }

            //Creating a copy of the board state as the SimulateGame method changes the state of the tiles passed
            bool[] flippedTilesCopy = new bool[9];
			Array.Copy(flippedTiles, flippedTilesCopy, flippedTiles.Length);

			//Exits the method early if no solution was found, as this means all tiles were already flipped by the user
			var fullReturn = LibraryProgram.SimulateGame(flippedTilesCopy);
            if (!fullReturn.Item1) { return; }
            //Converting the solvedFlipOrder into a single dimension array for use in the UI 
            foreach (int value in fullReturn.Item2) { solvedFlipOrder[value] = true; }
            
            //loading = false;
        }
    }
}