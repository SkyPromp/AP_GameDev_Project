using System.Collections.Generic;


namespace AP_GameDev_Project.TileTypes
{
    internal class FourSide : ATileType
    {
        /*public (int, int) GetileTile(int i, List<byte> tiles, int room_width)
        {
            int image = 9;
            int rotation = 0;
            // TODO, add corner variants
            return (image, rotation);
        }*/

        public override int GetRotation()
        {
            return 0;
        }

        public override int GetImage(int rotation)
        {
            // TODO: get all corner variants
            return 9;
        }
    }
}
