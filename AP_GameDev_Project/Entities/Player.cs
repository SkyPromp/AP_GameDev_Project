using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AP_GameDev_Project.Entities
{
    internal class Player: AEntity
    {
        private readonly MouseHandler mouseHandler;

        private readonly int max_health;
        
        public Player(Vector2 position, Animate stand_animation, float max_speed, Bullet base_bullet, int max_health=3, float speed_damping_factor=0.95f) : 
            base(position, stand_animation, max_speed, new Rectangle(45, 35, 35, 142), base_bullet, 10f, 0.2f, max_health, speed_damping_factor)
        {
            this.mouseHandler = MouseHandler.getInstance;
            this.max_health = max_health;
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle hitbox = base.GetHitbox;
            base.flip_texture = mouseHandler.MousePos.X < hitbox.X + hitbox.Width / 2;

            base.Update(gameTime);
        }

        public void Attack()
        {
            if (base.bullet_cooldown <= 0)
            {
                Vector2 center = base.GetCenter;
                Vector2 bullet_position = center + new Vector2(this.mouseHandler.MousePos.X < center.X ? -40 : 40, -7);  
                Vector2 angle = Vector2.Normalize(this.mouseHandler.MousePos - bullet_position);  // TODO: correct bullet_position to the center of the bullet?

                base.bullets.Add(new Bullet(bullet_position, angle * base.bullet_speed, base.base_bullet));
                base.bullet_cooldown = base.bullet_max_cooldown;
            }
        }

        public void Heal(int heal_amount=1)
        {
            base.health += heal_amount;
            
            if(base.health > this.max_health) base.health = this.max_health;
        }

        public override void Attack(Vector2 player_center)
        {
            throw new System.NotImplementedException();
        }
    }
}
