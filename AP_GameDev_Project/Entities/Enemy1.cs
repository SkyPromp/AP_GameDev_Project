using Microsoft.Xna.Framework;


namespace AP_GameDev_Project.Entities
{
    internal class Enemy1 : AEntity
    {
        public Enemy1(Vector2 position, Animate stand_animation, float max_speed, Rectangle normalized_hitbox, int base_health, float speed_damping_factor=0.95f) : base(position, stand_animation, max_speed, normalized_hitbox, base_health, speed_damping_factor)
        {
        }
    }
}
