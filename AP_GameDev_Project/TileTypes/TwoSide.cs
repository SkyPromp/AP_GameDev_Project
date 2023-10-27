using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.TileTypes
{
    internal class TwoSide : ITileType
    {
        public (int, int) GetileTile(int i, List<byte> tiles, int room_width)
        {
            int image;
            int rotate;
            TileHelper tileHelper = new TileHelper(room_width, tiles);

            Byte correct_tile = tiles[i];

            int left_i = tileHelper.getLeftIndex(i);
            int right_i = tileHelper.getRightIndex(i);
            int top_i = tileHelper.getTopIndex(i);

            Byte left = tileHelper.DoesTileMatch(left_i, correct_tile);
            Byte right = tileHelper.DoesTileMatch(right_i, correct_tile);
            Byte top = tileHelper.DoesTileMatch(top_i, correct_tile);

            if (left == (Byte)0)
            {
                if (right == left)  // Parallel
                {
                    image = 2;
                    rotate = 1;
                }
                else if (top == (Byte)0)
                {
                    rotate = 0;
                    int bottom_right_i = tileHelper.getBottomIndex(right);
                    Byte bottom_right = tileHelper.DoesTileMatch(bottom_right_i, correct_tile);

                    if (bottom_right == (Byte)0)
                    {
                        image = 4;
                    }
                    else
                    {
                        image = 3;
                    }
                }
                else
                {
                    rotate = 3;
                    int top_right_i = tileHelper.getTopIndex(right);
                    Byte top_right = tileHelper.DoesTileMatch(top_right_i, correct_tile);

                    if (top_right == (Byte)0)
                    {
                        image = 4;
                    }
                    else
                    {
                        image = 3;
                    }
                }
            }
            else
            {
                if (left == right)  // Parallel
                {
                    image = 2;
                    rotate = 0;
                }
                else if (top == (Byte)0)
                {
                    rotate = 1;
                    int bottom_left_i = tileHelper.getBottomIndex(left);
                    Byte bottom_left = tileHelper.DoesTileMatch(bottom_left_i, correct_tile);

                    if (bottom_left == (Byte)0)
                    {
                        image = 4;
                    }
                    else
                    {
                        image = 3;
                    }
                }
                else
                {
                    rotate = 2;
                    int top_left_i = tileHelper.getTopIndex(left);
                    Byte top_left = tileHelper.DoesTileMatch(top_left_i, correct_tile);

                    if (top_left == (Byte)0)
                    {
                        image = 4;
                    }
                    else
                    {
                        image = 3;
                    }
                }
            }

            return (image, rotate);
        }
    }
}
