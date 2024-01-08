using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AP_GameDev_Project.Entities
{
    internal class Animate
    {
        private readonly double animation_length;
        private readonly int total_frames;
        private int current_frame;
        private double cooldown;
        private readonly Rectangle frame_size;
        private Texture2D spritemap;

        public Animate(double animation_length, int total_frames, Rectangle frame_size, Texture2D spritemap)
        {
            this.animation_length = animation_length;
            this.total_frames = total_frames;
            cooldown = animation_length / total_frames;
            current_frame = 0;
            this.frame_size = frame_size;
            this.spritemap = spritemap;
        }

        public int Update(GameTime gameTime)
        {
            cooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            if (cooldown <= 0)
            {
                cooldown = animation_length / total_frames;
                current_frame = (current_frame + 1) % total_frames;
            }

            return current_frame;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool flip_texture = false)
        {
            Rectangle drawRect = frame_size;  // Refactor this away by removing this.current_frame
            drawRect.X = drawRect.Width * current_frame;

            spriteBatch.Draw(
            texture: spritemap,
            position: position,
            sourceRectangle: drawRect,
            color: Color.White,
            rotation: 0,
            origin: Vector2.Zero,
            scale: 1.0f,
            effects: flip_texture ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
            layerDepth: 0
            ); ;
        }
    }
}
