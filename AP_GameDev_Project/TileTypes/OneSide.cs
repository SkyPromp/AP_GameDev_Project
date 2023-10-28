using System;
using System.Collections.Generic;


namespace AP_GameDev_Project.TileTypes
{
    internal class OneSide : ITileType
    {
        public (int, int) GetileTile(int i, List<byte> tiles, int room_width)
        {
            // TODO, add corner variants
            int image = 1;
            int rotate = 0;
            TileHelper tileHelper = new TileHelper(room_width, tiles);

            int left = tileHelper.getLeftIndex(i);
            int right = tileHelper.getRightIndex(i);
            int top = tileHelper.getTopIndex(i);
            Byte correct_tile = tiles[i];

            if (tileHelper.DoesTileMatch(left, correct_tile) == (Byte)1)
            {
                rotate = 1;
            }
            else if (tileHelper.DoesTileMatch(right, correct_tile) == (Byte)1)
            {
                rotate = 3;
            }
            else if (tileHelper.DoesTileMatch(top, correct_tile) == (Byte)1)
            {
                rotate = 2;
            }

            return (image, rotate);
        }
    }
}
