using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.TileTypes
{
    internal class ThreeSide : ITileType
    {
        public (int, int) GetileTile(int i, List<byte> tiles, int room_width)
        {
            int image = 5;
            int rotate = 2;

            TileHelper tileHelper = new TileHelper(room_width, tiles);

            Byte correct_tile = tiles[i];

            int left = tileHelper.getLeftIndex(i);
            int right = tileHelper.getRightIndex(i);
            int top = tileHelper.getTopIndex(i);

            if (tileHelper.DoesTileMatch(left, correct_tile) == (Byte)0)
            {
                rotate = 3;
            }
            else if (tileHelper.DoesTileMatch(right, correct_tile) == (Byte)0)
            {
                rotate = 1;
            }
            else if (tileHelper.DoesTileMatch(top, correct_tile) == (Byte)0)
            {
                rotate = 0;
            }


            return (image, rotate);
        }
    }
}
