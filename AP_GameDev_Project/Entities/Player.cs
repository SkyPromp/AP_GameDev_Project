using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace AP_GameDev_Project.Entities
{
    internal class Player: AEntity
    {
        List<Bullet> bullets;
        private readonly Bullet base_bullet;
        private readonly float bullet_speed;
        private double bullet_cooldown;
        
        public Player(Vector2 position, Animate stand_animation, float max_speed, Bullet base_bullet, float speed_damping_factor=0.95f): base(position, stand_animation, max_speed, new Rectangle(56, 35, 35, 142), speed_damping_factor)
        {
            bullets = new List<Bullet>();
            this.base_bullet = base_bullet;
            this.bullet_speed = 10f;
            this.bullet_cooldown = 0.2f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.bullet_cooldown > 0) bullet_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
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

        public void Attack(Vector2 angle)
        {
            if (this.bullet_cooldown <= 0)
            {
                Rectangle hitbox = base.GetHitbox;
                Vector2 center = new Vector2(hitbox.X + hitbox.Width / 2, hitbox.Y + hitbox.Height / 2);

                bullets.Add(new Bullet(center + new Vector2(angle.X < 0 ? -40: 40, -7), angle * this.bullet_speed, this.base_bullet));
                this.bullet_cooldown = 0.2f;
            }
        }
    }
}
