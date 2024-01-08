using AP_GameDev_Project.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace AP_GameDev_Project.Entities.Mobs
{
    internal abstract class AEntity : ICollidable, ISpawnable
    {
        protected Animate stand_animation;
        protected Animate walk_animation;
        protected Animate current_animation;

        protected bool is_standing;

        protected bool flip_texture;

        protected int health;
        public int Health { get { return this.health; } }

        private Vector2 position;
        public Vector2 Position { get { return this.position; } set { this.position = value; this.hitbox.Position = value; } }

        private readonly float max_speed;
        private Vector2 speed;
        public Vector2 Speed { get { return this.speed; } set { this.speed = value; } }
        private float speed_damping_factor;

        private readonly Hitbox hitbox;
        public Hitbox GetHitboxHitbox { get { return this.hitbox; } }
        public Rectangle NormalizedHitbox { get { return this.hitbox.GetHitbox; } }
        private readonly Vector2 hitbox_center;
        private int damage;
        public int Damage { get { return this.damage; }  set { this.damage = value; } }
        public Rectangle GetHitbox
        {
            get
            {
                return new Rectangle((int)(this.position.X + this.NormalizedHitbox.X), (int)(this.position.Y + this.NormalizedHitbox.Y), this.NormalizedHitbox.Width, this.NormalizedHitbox.Height);
            }
        }
        public Vector2 GetCenter
        {
            get { return this.hitbox.GetHitbox.Center.ToVector2(); }
        }

        public bool show_hitbox;

        protected List<Bullet> bullets;
        public List<Bullet> Bullets { get { return bullets; } set { bullets = value; } }
        protected readonly float bullet_speed;
        protected double bullet_max_cooldown;
        protected double bullet_cooldown;

        public AEntity(Vector2 position, float max_speed, Hitbox hitbox, float bullet_speed, double bullet_max_cooldown, Animate stand_animation, Animate walk_animation = null, int base_health = 5, float speed_damping_factor = 0.95f, Vector2 hitbox_center = default, int damage = 1)
        {
            this.position = position;

            this.flip_texture = false;

            this.speed = Vector2.Zero;
            this.max_speed = max_speed;
            this.speed_damping_factor = speed_damping_factor;

            this.hitbox = hitbox;
            this.hitbox.Position = position;
            this.show_hitbox = false;

            this.health = base_health;

            this.bullets = new List<Bullet>();
            this.bullet_speed = bullet_speed;
            this.bullet_max_cooldown = bullet_max_cooldown;

            this.stand_animation = stand_animation;
            this.walk_animation = walk_animation != null ? walk_animation : stand_animation;
            this.current_animation = stand_animation;
            this.hitbox_center = hitbox_center;

            if (this.hitbox_center == default) throw new NotImplementedException();//this.hitbox_center = new Vector2(this.hitbox.GetHitbox.Left + (float) this.hitbox.GetHitbox.Width / 2, this.hitbox.GetHitbox.Top + (float)this.hitbox.GetHitbox.Height / 2);
            this.damage = damage;
            this.is_standing = false;
        }

        public virtual void Update(GameTime gameTime, Vector2 move_direction)
        {
            this.hitbox.Position = this.position;
            this.flip_texture = move_direction.X < this.hitbox_center.X + (int)this.Position.X && (move_direction - this.GetCenter).Length() > 10;

            if ((this.flip_texture != this.GetHitboxHitbox.is_flipped))
                this.hitbox.Flip(this.hitbox_center.X + (int)this.Position.X);

            if (Math.Abs(this.speed.X) < 0.1) this.speed.X = 0;
            if (Math.Abs(this.speed.Y) < 0.1) this.speed.Y = 0;

            if (this.speed.Length() < 0.1  || this.is_standing || (move_direction - this.GetCenter).Length() < 3) this.current_animation = this.stand_animation;
            else this.current_animation = this.walk_animation;

            this.current_animation.Update(gameTime);
            this.position += this.speed;

            this.speed *= 1 - this.speed_damping_factor;

            if (this.bullet_cooldown > 0) this.bullet_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Bullet bullet in this.bullets) bullet.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.current_animation.Draw(spriteBatch, this.position, this.flip_texture);

            foreach (Bullet bullet in this.bullets) bullet.Draw(spriteBatch);

            if (this.show_hitbox) { this.hitbox.Draw(spriteBatch); }
        }


        public virtual void SpeedUp(Vector2 add_speed)
        {
            this.speed += add_speed * (1 / 5f * this.max_speed) * (1 + this.speed_damping_factor);

            if (this.speed.Length() >= this.max_speed)
            {
                this.speed = Vector2.Normalize(this.speed) * this.max_speed;
            }
        }

        public virtual int DoDamage(int damage = 1)
        {
            this.health -= damage;

            return this.health;
        }

        public virtual void Die(ContentManager contentManager)
        {
            contentManager.GetSoundEffects["PLAYER_DEATH"].Play();
        }

        public abstract void Attack(Vector2 player_center);
    }
}
