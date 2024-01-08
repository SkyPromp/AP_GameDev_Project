using AP_GameDev_Project.Entities.Mobs;
using AP_GameDev_Project.Utils;
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
        public Hitbox GetHitboxHitbox { get { return this.hitbox; } }
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
            get { return this.hitbox.GetHitbox.Center.ToVector2(); }
        }

        public bool show_hitbox;

        public ACollectables(Vector2 position, Animate animation, Hitbox hitbox)
        {
            this.position = position;
            this.animation = animation;
            this.hitbox = hitbox;
            this.hitbox.Position = position;
            this.show_hitbox = false;
        }

        public void Update(GameTime gameTime)
        {
            this.hitbox.Position = this.position;
            this.animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.animation.Draw(spriteBatch, this.position);

            //if (show_hitbox) this.hitboxDrawer.DrawHitbox(this.GetHitbox, spriteBatch);
            if (this.show_hitbox) { hitbox.Draw(spriteBatch); }
        }

        public virtual void OnCollision(Player player)
        {
            ContentManager.getInstance.GetSoundEffects["PICKUP"].Play();
        }
    }
}
