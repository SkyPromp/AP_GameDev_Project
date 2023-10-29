using System.Collections.Generic;


namespace AP_GameDev_Project.TileTypes
{
    internal class FourSide : ATileType
    {
        public override int GetRotation()
        {
            return 0;
        }

        public override int GetImage(int rotation)
        {
            // TODO: get all corner variants
            return 9;
        }
    }
}
