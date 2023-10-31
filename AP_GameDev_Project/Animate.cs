using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project
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
            this.cooldown = animation_length / total_frames;
            this.current_frame = 0;
            this.frame_size = frame_size;
        }

        public int Update(GameTime gameTime)
        {
            this.cooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            if (this.cooldown <= 0)
            {
                this.cooldown = animation_length / total_frames;
                this.current_frame = (this.current_frame + 1) % this.total_frames;
            }

            return this.current_frame;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Rectangle drawRect = this.frame_size;  // Refactor this away by removing this.current_frame
            drawRect.X = drawRect.Width * this.current_frame;

            spriteBatch.Draw(
            texture: this.spritemap,
            position: position,
            sourceRectangle: drawRect,
            color: Color.White
            );
        }
    }
}
