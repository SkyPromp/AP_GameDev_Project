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
            get { return position + this.NormalizedHitbox.Center.ToVector2(); }
        }

        public bool show_hitbox;
        private HitboxDrawer hitboxDrawer;

        public ACollectables(Vector2 position, Animate animation, Hitbox hitbox)
        {
            this.position = position;
            this.animation = animation;
            this.hitbox = hitbox;
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

            //if (show_hitbox) this.hitboxDrawer.DrawHitbox(this.GetHitbox, spriteBatch);
            if (this.show_hitbox) { hitbox.DrawHitboxes(spriteBatch, this.position); }
        }

        public abstract void OnCollision(Player player);
    }
}
