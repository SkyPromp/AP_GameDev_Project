using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AP_GameDev_Project.Entities
{
    internal abstract class ACollectables
    {
        private Texture2D texture;
        private Vector2 position;
        public Vector2 Position { get { return this.position; } }
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

        public ACollectables(Vector2 position, Texture2D texture, Rectangle normalized_hitbox)
        {
            this.position = position;
            this.texture = texture;
            this.normalized_hitbox = normalized_hitbox;
        }

        public abstract void OnCollision(Player player);
    }
}
