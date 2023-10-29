using System;
using System.Collections.Generic;


namespace AP_GameDev_Project.TileTypes
{
    internal class ThreeSide : ATileType
    {
        /*private int i;
        private TileHelper tileHelper;

        public (int, int) GetileTile(int i, List<byte> tiles, int room_width)
        {
            this.tileHelper = new TileHelper(room_width, tiles, i);
            this.i = i;

            int rotate = this.GetRotation();

            return (this.GetImage(rotate), rotate);
        }*/

        public override int GetRotation()
        {
            int rotation = 2;

            int left = this.tileHelper.getLeftIndex(i);
            int right = this.tileHelper.getRightIndex(i);
            int top = this.tileHelper.getTopIndex(i);

            if (this.tileHelper.getTile(left) == (Byte)0)
            {
                rotation = 3;
            }
            else if (this.tileHelper.getTile(right) == (Byte)0)
            {
                rotation = 1;
            }
            else if (this.tileHelper.getTile(top) == (Byte)0)
            {
                rotation = 0;
            }

            return rotation;
        }

        public override int GetImage(int rotation)
        {
            int image = 5;
            int bottom_left = this.tileHelper.getRotatedCorner((int)TileHelper.corners.BOTTOM_LEFT, this.i, rotation);
            int bottom_right = this.tileHelper.getRotatedCorner((int)TileHelper.corners.BOTTOM_RIGHT, this.i, rotation);

            if (this.tileHelper.getTile(bottom_left) == (byte)0)
            {
                if (this.tileHelper.getTile(bottom_right) == (byte)0)
                {
                    image = 8;
                }
                else
                {
                    image = 6;
                }
            }
            else if (this.tileHelper.getTile(bottom_right) == (byte)0)
            {
                image = 7;
            }

            return image;
        }
    }
}
