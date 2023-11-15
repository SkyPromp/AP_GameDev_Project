using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace AP_GameDev_Project.Entities
{
    internal abstract class AEntity: ICollidable
    {
        protected Animate stand_animation;
        protected Animate walk_animation;
        protected Animate current_animation;

        protected bool flip_texture;

        protected int health;

        private Vector2 position;
        public Vector2 Position { get { return this.position; } set { this.position = value; } }

        private readonly float max_speed;
        private Vector2 speed;
        public Vector2 Speed { get { return this.speed; } set { this.speed = value; } }
        private float speed_damping_factor;

        private readonly Rectangle normalized_hitbox;
        public Rectangle GetNormalizedHitbox { get {  return this.normalized_hitbox; } }
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

        public void HandleHardCollison(Rectangle hitbox)
        {
            if (this.GetHitbox.Intersects(hitbox)) this.HardCollide(hitbox);
        }

        public void HardCollide(Rectangle wall)
        {
            Vector2 test = new Vector2(this.position.X, this.position.Y);

            Rectangle hitbox = this.GetHitbox;

            float horizontal = float.PositiveInfinity;
            float vertical = float.PositiveInfinity;

            if(hitbox.Left < wall.Right)  // Left
            {
                test.X = wall.Right- this.normalized_hitbox.Left;
                horizontal = test.X;
            }
            if ((hitbox.Right > wall.Left))  // Right
            {
                test.X = wall.Left - this.normalized_hitbox.Right;
                if (Math.Abs(horizontal - this.position.X) < Math.Abs(test.X - this.position.X)) test.X = horizontal;
            }

            if(hitbox.Top < wall.Bottom)  // Top
            {
                test.Y = wall.Bottom - this.normalized_hitbox.Top;
                vertical = test.Y;
            }
            if(hitbox.Bottom > wall.Top)  // Bottom
            {
                test.Y = wall.Top - this.normalized_hitbox.Bottom;
                if (Math.Abs(vertical - this.position.Y) < Math.Abs(test.Y - this.position.Y)) test.Y = vertical;
            }

            if (((test.X != this.position.X) && Math.Abs((this.position.X - test.X)) < Math.Abs(this.position.Y - test.Y)) || ((test.Y == this.position.Y)))
            {
                this.position.X = test.X;
                this.speed.X = 0;
            }
            else
            {
                this.position.Y = test.Y;
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
