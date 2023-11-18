using Microsoft.Xna.Framework;


namespace AP_GameDev_Project.Entities
{
    internal interface ICollidable
    {
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public Rectangle GetHitbox { get; }
        public Rectangle NormalizedHitbox { get; }
    }
}
