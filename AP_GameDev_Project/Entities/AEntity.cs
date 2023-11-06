using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;

namespace AP_GameDev_Project.Entities
{
    internal abstract class AEntity
    {
        private readonly Animate stand_animation;
        protected bool flip_texture;

        protected int health;

        private Vector2 position;
        public Vector2 Position { get { return this.position; } }

        private readonly float max_speed;
        private Vector2 speed;
        protected Vector2 Speed { get { return this.speed; } }
        private float speed_damping_factor;

        private readonly Rectangle normalized_hitbox;
        public Rectangle GetHitbox
        {
            get
            {
                return new Rectangle((int)(this.position.X + this.normalized_hitbox.X), (int)(this.position.Y + this.normalized_hitbox.Y), this.normalized_hitbox.Width, this.normalized_hitbox.Height);
            }
        }

        protected List<Bullet> bullets;
        public List<Bullet> Bullets { get { return bullets; } set { this.bullets = value; } }
        protected readonly Bullet base_bullet;
        protected readonly float bullet_speed;
        protected double bullet_max_cooldown;
        protected double bullet_cooldown;

        public AEntity(Vector2 position, Animate stand_animation, float max_speed, Rectangle normalized_hitbox, Bullet base_bullet, float bullet_speed, double bullet_max_cooldown, int base_health = 5, float speed_damping_factor=0.95f)
        {
            this.position = position;

            this.stand_animation = stand_animation;
            this.flip_texture = false;

            this.speed = Vector2.Zero;
            this.max_speed = max_speed;
            this.speed_damping_factor = speed_damping_factor;

            this.normalized_hitbox = normalized_hitbox;

            this.health = base_health;

            this.bullets = new List<Bullet>();
            this.base_bullet = base_bullet;
            this.bullet_speed = bullet_speed;
            this.bullet_max_cooldown = bullet_max_cooldown;
        }

        public virtual void Update(GameTime gameTime)
        {
            this.speed *= this.speed_damping_factor;
            if (Math.Abs(this.speed.X) < 0.1) this.speed.X = 0;
            if (Math.Abs(this.speed.Y) < 0.1) this.speed.Y = 0;

            this.stand_animation.Update(gameTime);
            this.position += speed;

            if (this.bullet_cooldown > 0) this.bullet_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Bullet bullet in this.bullets) bullet.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.stand_animation.Draw(spriteBatch, this.position, this.flip_texture);

            foreach (Bullet bullet in this.bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public void DrawHitbox(GraphicsDevice graphicsDevice)
        {
            Rectangle hitbox = this.GetHitbox;

            // DRAW VERTICES SETUP
            BasicEffect basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
            (0, graphicsDevice.Viewport.Width,     // left, right
            graphicsDevice.Viewport.Height, 0,    // bottom, top
            0, 1);

            basicEffect.CurrentTechnique.Passes[0].Apply();
            var vertices = new VertexPositionColor[5];
            vertices[0].Position = new Vector3(hitbox.X, hitbox.Y, 0);
            vertices[0].Color = Color.Red;
            vertices[1].Position = new Vector3(hitbox.X + hitbox.Width, hitbox.Y, 0);
            vertices[1].Color = Color.Red;
            vertices[2].Position = new Vector3(hitbox.X + hitbox.Width, hitbox.Y + hitbox.Height, 0);
            vertices[2].Color = Color.Red;
            vertices[3].Position = new Vector3(hitbox.X, hitbox.Y + hitbox.Height, 0);
            vertices[3].Color = Color.Red;
            vertices[4] = vertices[0];

            graphicsDevice.DrawUserPrimitives<VertexPositionColor>(Microsoft.Xna.Framework.Graphics.PrimitiveType.LineStrip, vertices, 0, 4);

        }

        public void SpeedUp(Vector2 add_speed)
        {
            this.speed += add_speed;

            if (this.speed.Length() >= this.max_speed)
            {
                this.speed = Vector2.Normalize(this.speed) * this.max_speed;
            }
        }

        public void HandleCollison(Rectangle wall)
        {
            Vector2 xtest = new Vector2(this.position.X, this.position.Y);
            Vector2 ytest = new Vector2(this.position.X, this.position.Y);

            if (this.speed.X < 0 && wall.X + wall.Width > this.position.X + this.normalized_hitbox.X) // Left
            {
                xtest.X = wall.X + wall.Width - this.normalized_hitbox.X;
            }
            else if (this.speed.X > 0 && wall.X < this.position.X + this.normalized_hitbox.X + this.normalized_hitbox.Width)  // Right
            {
                xtest.X = wall.X - this.normalized_hitbox.Width - this.normalized_hitbox.X;
            }

            if (this.speed.Y < 0 && wall.Y + wall.Height > this.position.Y + this.normalized_hitbox.Y)  // Top
            {
                ytest.Y = wall.Y + wall.Height - this.normalized_hitbox.Y;
            }
            else if (this.speed.Y > 0 && wall.Y < this.position.Y + this.normalized_hitbox.Y + this.normalized_hitbox.Height)  // Bottom
            {
                ytest.Y = wall.Y - this.normalized_hitbox.Height - this.normalized_hitbox.Y;
            }

            if (((xtest != this.position) && (this.position - xtest).Length() < (this.position - ytest).Length()) || ((ytest == this.position)))
            {
                this.position = xtest;
                this.speed.X = 0;
            }
            else
            {
                this.position = ytest;
                this.speed.Y = 0;
            }
        }

        public int DoDamage(int damage=1)
        {
            this.health -= damage;

            return this.health;
        }

        public abstract void Attack(Vector2 player_center);
    }
}
