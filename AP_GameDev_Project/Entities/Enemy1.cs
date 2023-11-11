using Microsoft.Xna.Framework;


namespace AP_GameDev_Project.Entities
{
    internal class Enemy1 : AEntity
    {
        private ContentManager contentManager;

        public Enemy1(Vector2 position, float max_speed, int base_health, float speed_damping_factor=0.95f) : 
            base(position, max_speed, new Rectangle(22, 10, 17, 43), 10f, 1f, base_health, speed_damping_factor)
        {
            this.contentManager = ContentManager.getInstance;
            base.stand_animation = this.contentManager.GetAnimations["ENEMY1_STANDSTILL"];
        }

        public override void Attack(Vector2 player_center)
        {
            if (base.bullet_cooldown <= 0)  // TODO: if no walls using ray marching
            {
                Vector2 enemy_center = base.GetCenter;
                Vector2 weapon_offset = new Vector2(base.Position.X < player_center.X ? -19.5f: 19.5f, -7);
                Vector2 bullet_position = weapon_offset + enemy_center;
                Vector2 angle = Vector2.Normalize(player_center - bullet_position);    // TODO: correct bullet_position to the center of the bullet?

                base.bullets.Add(new Bullet(bullet_position, angle * base.bullet_speed));
                base.bullet_cooldown = base.bullet_max_cooldown;
            }
        }
    }
}
