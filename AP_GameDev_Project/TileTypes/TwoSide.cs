using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AP_GameDev_Project.TileTypes
{
    internal class TwoSide : ITileType
    {
        public (int, int) GetileTile(int i, List<byte> tiles, int room_width)
        {
            // Bugfix, wrong diagonal corners checked

            int image;
            int rotate;
            TileHelper tileHelper = new TileHelper(room_width, tiles, i);

            int left_i = tileHelper.getLeftIndex(i);
            int right_i = tileHelper.getRightIndex(i);
            int top_i = tileHelper.getTopIndex(i);

            Byte left = tileHelper.getTile(left_i);
            Byte right = tileHelper.getTile(right_i);
            Byte top = tileHelper.getTile(top_i);

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
                    int bottom_right_i = tileHelper.getBottomIndex(right_i);
                    Byte bottom_right = tileHelper.getTile(bottom_right_i);


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
                    int top_right_i = tileHelper.getTopIndex(right_i);
                    Byte top_right = tileHelper.getTile(top_right_i);

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
                    int bottom_left_i = tileHelper.getBottomIndex(left_i);
                    Byte bottom_left = tileHelper.getTile(bottom_left_i);

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
                    int top_left_i = tileHelper.getTopIndex(left_i);
                    Byte top_left = tileHelper.getTile(top_left_i);

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
