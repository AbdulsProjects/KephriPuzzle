namespace KephriWebApp.Pages
{
    public partial class Index
    {
        private bool[] flippedTiles = { false, false, false, false, false, false, false, false, false };

        private void ChangeStartState(int index)
        {
            flippedTiles[index] ^= true;
        }

        private string StateStyle(bool flipped)
        {
            if (flipped) return "flipped-tile";
            return "";
        }
    }
}