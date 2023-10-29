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

            if (!this.tileHelper.IsCorrectTileAtPos(left))
            {
                if (!this.tileHelper.IsCorrectTileAtPos(top)) rotation = 0;
                // Parallel and non-parallel options rotate the same in this case
                else rotation = 3;

            } else 
            {
                if (!this.tileHelper.IsCorrectTileAtPos(top))
                {
                    // if non-parallel
                    if (!this.tileHelper.IsCorrectTileAtPos(right)) rotation = 1;
                    else rotation = 0;
                }
                else rotation = 2;
            }

            return rotation;
        }

        public override int GetImage(int rotation)
        {
            int left = this.tileHelper.getLeftIndex(this.i);
            int right = this.tileHelper.getRightIndex(this.i);

            if (this.tileHelper.IsCorrectTileAtPos(left) == this.tileHelper.IsCorrectTileAtPos(right)) return 2;

            int bottom_right = this.tileHelper.getRotatedCorner((int)TileHelper.corners.BOTTOM_RIGHT, this.i, rotation);

            if (!this.tileHelper.IsCorrectTileAtPos(bottom_right)) return 4;

            return 3;
        }
    }
}
