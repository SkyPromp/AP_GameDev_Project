using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace AP_GameDev_Project.Entities.Mobs
{
    internal class Player : AEntity
    {
        private ContentManager contentManager;
        private readonly int max_health;
        private bool has_invincibility;
        private Vector2 mouse_position;

        public Player(Vector2 position, ContentManager contentManager, float max_speed = 5f, int max_health = 3, float speed_damping_factor = 0.10f) :
            base(position, max_speed, new Rectangle(47, 35, 35, 107), 10f, 0.2f, contentManager.GetAnimations["PLAYER_STANDSTILL"], contentManager.GetAnimations["PLAYER_WALK"], max_health, speed_damping_factor)
        {
            this.contentManager = ContentManager.getInstance;
            this.max_health = max_health;
            has_invincibility = true;  // TODO set to false
        }

        public override void Update(GameTime gameTime, Vector2 move_direction)
        {
            mouse_position = move_direction;

            base.Update(gameTime, move_direction);
        }

        public override void SpeedUp(Vector2 add_speed)
        {
            base.SpeedUp(add_speed);
        }

        public override void Attack(Vector2 mouse_pos)
        {
            if (bullet_cooldown <= 0)
            {
                Vector2 center = GetCenter;
                Vector2 bullet_position = center + new Vector2(mouse_pos.X < center.X ? -36 : 36, 3);
                Vector2 angle = Vector2.Normalize(mouse_pos - bullet_position);  // TODO: correct bullet_position to the center of the bullet?

                bullets.Add(new Bullet(bullet_position, angle * bullet_speed));
                bullet_cooldown = bullet_max_cooldown;
                contentManager.GetSoundEffects["BULLET_SHOOT"].Play();
            }
        }

        public void Heal(int heal_amount = 1)
        {
            health += heal_amount;

            if (health > max_health) health = max_health;
        }


        public override int DoDamage(int damage = 1)
        {
            if (!has_invincibility) base.DoDamage(damage);

            return health;
        }
    }
}
