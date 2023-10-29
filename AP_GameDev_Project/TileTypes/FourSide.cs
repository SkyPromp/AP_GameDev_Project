using System.Collections.Generic;


namespace AP_GameDev_Project.TileTypes
{
    internal class FourSide : ITileType
    {
        public (int, int) GetileTile(int i, List<byte> tiles, int room_width)
        {
            int image = 9;
            int rotation = 0;
            // TODO, add corner variants
            return (image, rotation);
        }
    }
}
