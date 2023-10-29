using System;
using System.Collections.Generic;


namespace AP_GameDev_Project.TileTypes
{
    internal class OneSide : ATileType
    {
        public override int GetRotation()
        {
            int rotation = 0;

            int left = this.tileHelper.getLeftIndex(this.i);
            int right = this.tileHelper.getRightIndex(this.i);
            int top = this.tileHelper.getTopIndex(this.i);

            if (tileHelper.getTile(left) == (Byte)1)
            {
                rotation = 1;
            }
            else if (tileHelper.getTile(right) == (Byte)1)
            {
                rotation = 3;
            }
            else if (tileHelper.getTile(top) == (Byte)1)
            {
                rotation = 2;
            }

            return rotation;
        }

        public override int GetImage(int rotation)
        {
            return 1;
        }
    }
}
