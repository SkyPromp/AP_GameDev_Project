using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace AP_GameDev_Project.Entities
{
    internal class Player: AEntity
    {
        private List<Bullet> bullets;
        public List<Bullet> Bullets { get { return bullets; } set { this.bullets = value; } }
        private readonly Bullet base_bullet;
        private readonly float bullet_speed;
        private double bullet_max_cooldown;
        private double bullet_cooldown;

        private readonly MouseHandler mouseHandler;

        private readonly int max_health;
        
        public Player(Vector2 position, Animate stand_animation, float max_speed, Bullet base_bullet, int max_health=3, float speed_damping_factor=0.95f) : 
            base(position, stand_animation, max_speed, new Rectangle(56, 35, 35, 142), base_bullet, 10f, 0.2f, max_health, speed_damping_factor)
        {
            this.bullets = new List<Bullet>();
            this.mouseHandler = MouseHandler.getInstance;
            this.base_bullet = base_bullet;
            this.bullet_speed = 10f;
            this.bullet_max_cooldown = 0.2f;
            this.max_health = max_health;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.bullet_cooldown > 0) this.bullet_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Bullet bullet in this.bullets) bullet.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Bullet bullet in this.bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public void Attack()
        {
            if (this.bullet_cooldown <= 0)
            {
                Rectangle hitbox = this.GetHitbox;
                Vector2 center = new Vector2(hitbox.X + hitbox.Width / 2, hitbox.Y + hitbox.Height / 2);
                Vector2 bullet_position = center + new Vector2(this.mouseHandler.MousePos.X < center.X ? -40 : 40, -7);  
                Vector2 angle = Vector2.Normalize(this.mouseHandler.MousePos - bullet_position);  // TODO: correct bullet_position to the center of the bullet?

                this.bullets.Add(new Bullet(bullet_position, angle * this.bullet_speed, this.base_bullet));
                this.bullet_cooldown = this.bullet_max_cooldown;
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
