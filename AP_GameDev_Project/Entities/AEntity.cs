using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace AP_GameDev_Project.Entities
{
    internal abstract class AEntity
    {
        protected Animate stand_animation;
        protected Animate walk_animation;
        protected Animate current_animation;

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
        public Vector2 GetCenter
        {
            get { return this.position + this.normalized_hitbox.Center.ToVector2(); }
        }

        public bool show_hitbox;

        protected List<Bullet> bullets;
        public List<Bullet> Bullets { get { return bullets; } set { this.bullets = value; } }
        protected readonly Bullet base_bullet;
        protected readonly float bullet_speed;
        protected double bullet_max_cooldown;
        protected double bullet_cooldown;

        public AEntity(Vector2 position, float max_speed, Rectangle normalized_hitbox, float bullet_speed, double bullet_max_cooldown, Animate stand_animation, Animate walk_animation=null, int base_health=5, float speed_damping_factor=0.95f)
        {
            this.position = position;

            this.flip_texture = false;

            this.speed = Vector2.Zero;
            this.max_speed = max_speed;
            this.speed_damping_factor = speed_damping_factor;

            this.normalized_hitbox = normalized_hitbox;
            this.show_hitbox = false;

            this.health = base_health;

            this.bullets = new List<Bullet>();
            this.bullet_speed = bullet_speed;
            this.bullet_max_cooldown = bullet_max_cooldown;

            this.stand_animation = stand_animation;
            this.walk_animation = walk_animation != null ? walk_animation : stand_animation;
            this.current_animation = stand_animation;
        }

        public virtual void Update(GameTime gameTime, Vector2 move_direction)
        {
            Rectangle hitbox = this.GetHitbox;
            this.flip_texture = move_direction.X < hitbox.X + hitbox.Width / 2;

            if (Math.Abs(this.speed.X) < 0.1) this.speed.X = 0;
            if (Math.Abs(this.speed.Y) < 0.1) this.speed.Y = 0;

            if (this.speed.Length() < 0.1) this.current_animation = this.stand_animation;
            else this.current_animation = this.walk_animation;

            this.current_animation.Update(gameTime);
            this.position += speed;
            
            this.speed *= (1 - this.speed_damping_factor);

            if (this.bullet_cooldown > 0) this.bullet_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Bullet bullet in this.bullets) bullet.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.current_animation.Draw(spriteBatch, this.position, this.flip_texture);

            foreach (Bullet bullet in this.bullets) bullet.Draw(spriteBatch);

            if (this.show_hitbox)
            {
                spriteBatch.End();  // Required to draw the hitbox on top
                spriteBatch.Begin();
                this.DrawHitbox(spriteBatch.GraphicsDevice);
            }
        }

        private void DrawHitbox(GraphicsDevice graphicsDevice)
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

            graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, 0, 4);

        }

        public virtual void SpeedUp(Vector2 add_speed)
        {
            this.speed += add_speed * (1/5f * this.max_speed) * (1 + this.speed_damping_factor);

            if (this.speed.Length() >= this.max_speed)
            {
                this.speed = Vector2.Normalize(this.speed) * this.max_speed;
            }
        }

        public void HandleCollison(Rectangle wall)
        {
            Vector2 xtest = new Vector2(this.position.X, this.position.Y);
            Vector2 ytest = new Vector2(this.position.X, this.position.Y);

            Rectangle hitbox = this.GetHitbox;

            float horizontal = float.PositiveInfinity;
            float vertical = float.PositiveInfinity;

            if(hitbox.Left < wall.Right)  // Left
            {
                xtest.X = wall.X + wall.Width - this.normalized_hitbox.X;
                horizontal = xtest.X;
            }
            if ((hitbox.Right > wall.Left))  // Right
            {
                xtest.X = wall.X - this.normalized_hitbox.Width - this.normalized_hitbox.X;
                if (Math.Abs(horizontal - this.position.X) < Math.Abs(xtest.X - this.position.X)) xtest.X = horizontal;
            }

            if(hitbox.Top < wall.Bottom)  // Top
            {
                ytest.Y = wall.Y + wall.Height - this.normalized_hitbox.Y;
                vertical = ytest.Y;
            }
            if(hitbox.Bottom > wall.Top)  // Bottom
            {
                ytest.Y = wall.Y - this.normalized_hitbox.Height - this.normalized_hitbox.Y;
                if (Math.Abs(vertical - this.position.Y) < Math.Abs(ytest.Y - this.position.Y)) ytest.Y = vertical;
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

        public virtual int DoDamage(int damage=1)
        {
            this.health -= damage;

            return this.health;
        }

        public abstract void Attack(Vector2 player_center);
    }
}
