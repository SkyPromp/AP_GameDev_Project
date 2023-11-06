using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AP_GameDev_Project.Entities
{
    internal class Enemy1 : AEntity
    {
        public Enemy1(Vector2 position, Animate stand_animation, float max_speed, Rectangle normalized_hitbox, int base_health, Bullet base_bullet, float speed_damping_factor=0.95f) : 
            base(position, stand_animation, max_speed, normalized_hitbox, base_bullet, 10f, 1f, base_health, speed_damping_factor)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Bullet bullet in this.bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public override void Attack(Vector2 player_center)
        {
            if (base.bullet_cooldown <= 0)  // TODO: if no walls
            {
                Rectangle enemy_hitbox = base.GetHitbox;
                Vector2 enemy_center = new Vector2(enemy_hitbox.X + enemy_hitbox.Width / 2, enemy_hitbox.Y + enemy_hitbox.Height / 2);
                Vector2 weapon_offset = new Vector2(base.Position.X < player_center.X ? -19.5f: 19.5f, -7);
                Vector2 bullet_position = weapon_offset + enemy_center;
                Vector2 angle = Vector2.Normalize(player_center - bullet_position);    // TODO: correct bullet_position to the center of the bullet?

                base.bullets.Add(new Bullet(bullet_position, angle * base.bullet_speed, base.base_bullet));
                base.bullet_cooldown = base.bullet_max_cooldown;
            }
        }
    }
}
