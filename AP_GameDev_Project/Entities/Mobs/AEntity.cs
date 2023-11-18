using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace AP_GameDev_Project.Entities.Mobs
{
    internal abstract class AEntity : ICollidable, ISpawnable
    {
        private HitboxDrawer hitboxDrawer;

        protected Animate stand_animation;
        protected Animate walk_animation;
        protected Animate current_animation;

        protected bool flip_texture;

        protected int health;

        private Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } }

        private readonly float max_speed;
        private Vector2 speed;
        public Vector2 Speed { get { return speed; } set { speed = value; } }
        private float speed_damping_factor;

        private readonly Hitbox hitbox;
        public Rectangle NormalizedHitbox { get { return this.hitbox.GetHitbox; } }
        public Rectangle GetHitbox
        {
            get
            {
                return new Rectangle((int)(position.X + this.NormalizedHitbox.X), (int)(position.Y + this.NormalizedHitbox.Y), this.NormalizedHitbox.Width, this.NormalizedHitbox.Height);
            }
        }
        public Vector2 GetCenter
        {
            get { return position + this.hitbox.GetHitbox.Center.ToVector2(); }
        }

        public bool show_hitbox;

        protected List<Bullet> bullets;
        public List<Bullet> Bullets { get { return bullets; } set { bullets = value; } }
        protected readonly float bullet_speed;
        protected double bullet_max_cooldown;
        protected double bullet_cooldown;

        public AEntity(Vector2 position, float max_speed, Hitbox hitbox, float bullet_speed, double bullet_max_cooldown, Animate stand_animation, Animate walk_animation = null, int base_health = 5, float speed_damping_factor = 0.95f)
        {
            this.hitboxDrawer = HitboxDrawer.getInstance;

            this.position = position;

            flip_texture = false;

            speed = Vector2.Zero;
            this.max_speed = max_speed;
            this.speed_damping_factor = speed_damping_factor;

            this.hitbox = hitbox;
            show_hitbox = false;

            health = base_health;

            bullets = new List<Bullet>();
            this.bullet_speed = bullet_speed;
            this.bullet_max_cooldown = bullet_max_cooldown;

            this.stand_animation = stand_animation;
            this.walk_animation = walk_animation != null ? walk_animation : stand_animation;
            current_animation = stand_animation;
        }

        public virtual void Update(GameTime gameTime, Vector2 move_direction)
        {
            Rectangle hitbox = GetHitbox;
            flip_texture = move_direction.X < hitbox.X + hitbox.Width / 2;

            if (Math.Abs(speed.X) < 0.1) speed.X = 0;
            if (Math.Abs(speed.Y) < 0.1) speed.Y = 0;

            if (speed.Length() < 0.1) current_animation = stand_animation;
            else current_animation = walk_animation;

            current_animation.Update(gameTime);
            position += speed;

            speed *= 1 - speed_damping_factor;

            if (bullet_cooldown > 0) bullet_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Bullet bullet in bullets) bullet.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            current_animation.Draw(spriteBatch, position, flip_texture);

            foreach (Bullet bullet in bullets) bullet.Draw(spriteBatch);

            //if (show_hitbox) this.hitboxDrawer.DrawHitbox(this.GetHitbox, spriteBatch);
            if (this.show_hitbox) { hitbox.DrawHitboxes(spriteBatch, this.position); }
        }


        public virtual void SpeedUp(Vector2 add_speed)
        {
            speed += add_speed * (1 / 5f * max_speed) * (1 + speed_damping_factor);

            if (speed.Length() >= max_speed)
            {
                speed = Vector2.Normalize(speed) * max_speed;
            }
        }

        public virtual int DoDamage(int damage = 1)
        {
            health -= damage;

            return health;
        }

        public abstract void Attack(Vector2 player_center);
    }
}
