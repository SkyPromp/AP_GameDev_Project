using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AP_GameDev_Project.Entities
{
    internal class Enemy1 : AEntity
    {
        private List<Bullet> bullets;
        public List<Bullet> Bullets { get { return bullets; } set { this.bullets = value; } }
        private readonly Bullet base_bullet;
        private readonly float bullet_speed;
        private double bullet_max_cooldown;
        private double bullet_cooldown;

        public Enemy1(Vector2 position, Animate stand_animation, float max_speed, Rectangle normalized_hitbox, int base_health, Bullet base_bullet, float speed_damping_factor=0.95f) : base(position, stand_animation, max_speed, normalized_hitbox, base_health, speed_damping_factor)
        {
            this.bullets = new List<Bullet>();
            this.base_bullet = base_bullet;
            this.bullet_speed = 10f;
            this.bullet_max_cooldown = 1f;
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

        public void Attack(Vector2 player_center)
        {
            if (this.bullet_cooldown <= 0)
            {
                Rectangle enemy_hitbox = base.GetHitbox;
                Vector2 bullet_position = new Vector2(enemy_hitbox.X + enemy_hitbox.Width / 2, enemy_hitbox.Y + enemy_hitbox.Height / 2);  // TODO: find weapon firing tip
                Vector2 angle = Vector2.Normalize(bullet_position - player_center);    // TODO: correct bullet_position to the center of the bullet?

                this.bullets.Add(new Bullet(bullet_position, angle * this.bullet_speed, this.base_bullet));
                this.bullet_cooldown = this.bullet_max_cooldown;
            }
        }
    }
}
