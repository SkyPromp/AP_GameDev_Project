using AP_GameDev_Project.Entities.Mobs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AP_GameDev_Project.Entities.Collectables
{
    internal abstract class ACollectables: ISpawnable
    {
        private Animate animation;
        private Vector2 position;
        public Vector2 Position { get { return position; } }
        private readonly Rectangle normalized_hitbox;
        public Rectangle GetHitbox
        {
            get
            {
                return new Rectangle((int)(position.X + normalized_hitbox.X), (int)(position.Y + normalized_hitbox.Y), normalized_hitbox.Width, normalized_hitbox.Height);
            }
        }

        public Vector2 GetCenter
        {
            get { return position + normalized_hitbox.Center.ToVector2(); }
        }

        public bool show_hitbox;
        private HitboxDrawer hitboxDrawer;

        public ACollectables(Vector2 position, Animate animation, Rectangle normalized_hitbox)
        {
            this.position = position;
            this.animation = animation;
            this.normalized_hitbox = normalized_hitbox;
            this.show_hitbox = false;
            this.hitboxDrawer = HitboxDrawer.getInstance;
        }

        public void Update(GameTime gameTime)
        {
            this.animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.animation.Draw(spriteBatch, this.position);

            if (show_hitbox)
            {
                spriteBatch.End();  // Required to draw the hitbox on top
                spriteBatch.Begin();
                this.hitboxDrawer.DrawHitbox(this.GetHitbox);
            }
        }

        public abstract void OnCollision(Player player);
    }
}
