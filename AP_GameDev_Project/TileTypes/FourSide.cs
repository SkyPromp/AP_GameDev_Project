using System.Collections.Generic;


namespace AP_GameDev_Project.TileTypes
{
    internal class FourSide : ATileType
    {
        protected override int GetRotation()
        {
            // TODO: get all corner variants
            return 0;
        }

        protected override int GetImage(int rotation)
        {
            // TODO: get all corner variants
            return 9;
        }
    }
}
