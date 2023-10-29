using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AP_GameDev_Project.TileTypes
{
    internal class TwoSide : ATileType
    {
        public override int GetRotation()
        {
            int left = this.tileHelper.getLeftIndex(this.i);
            int right = this.tileHelper.getRightIndex(this.i);
            int top = this.tileHelper.getTopIndex(this.i);

            int rotation;

            if (this.tileHelper.getTile(left) == (Byte)0)
            {
                if (this.tileHelper.getTile(top) == (Byte)0)
                {
                    rotation = 0;
                }
                else
                {  // Parallel and non-parallel options rotate the same in this case
                    rotation = 3;
                }
            } else 
            {
                if (this.tileHelper.getTile(top) == (Byte)0)
                {
                    if(this.tileHelper.getTile(right) == (Byte)0)  // if non-parallel
                    {
                        rotation = 1;
                    }
                    else
                    {
                        rotation = 0;
                    }
                }
                else
                {
                    rotation = 2;
                }
            }

            return rotation;
        }

        public override int GetImage(int rotation)
        {
            int left = this.tileHelper.getLeftIndex(this.i);
            int right = this.tileHelper.getRightIndex(this.i);

            if (this.tileHelper.getTile(left) == this.tileHelper.getTile(right)) return 2;

            int bottom_right = this.tileHelper.getRotatedCorner((int)TileHelper.corners.BOTTOM_RIGHT, this.i, rotation);

            if (this.tileHelper.getTile(bottom_right) == (byte)0)
            {
                return 4;
            }

            return 3;
        }
    }
}
