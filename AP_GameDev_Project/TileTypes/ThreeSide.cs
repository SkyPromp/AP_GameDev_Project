using System;
using System.Collections.Generic;


namespace AP_GameDev_Project.TileTypes
{
    internal class ThreeSide : ITileType
    {
        public (int, int) GetileTile(int i, List<byte> tiles, int room_width)
        {
            int image = 5;
            int rotate = 2;

            TileHelper tileHelper = new TileHelper(room_width, tiles, i);

            int left = tileHelper.getLeftIndex(i);
            int right = tileHelper.getRightIndex(i);
            int top = tileHelper.getTopIndex(i);
            int bottom = tileHelper.getBottomIndex(i);

            if (tileHelper.getTile(left) == (Byte)0)
            {
                rotate = 3;

                // TODO get correct corner variant
            }
            else if (tileHelper.getTile(right) == (Byte)0)
            {
                rotate = 1;

                // TODO get correct corner variant
            }
            else if (tileHelper.getTile(top) == (Byte)0)
            {
                rotate = 0;

                if (tileHelper.getTile(tileHelper.getLeftIndex(bottom)) == (byte)0)
                {
                    if (tileHelper.getTile(tileHelper.getRightIndex(bottom)) == (byte)0)
                    {
                        image = 8;
                    }
                    else
                    {
                        image = 6;
                    }
                } else if (tileHelper.getTile(tileHelper.getRightIndex(bottom)) == (byte)0)
                {
                    image = 7;
                }
            }

            return (image, rotate);
        }
    }
}
