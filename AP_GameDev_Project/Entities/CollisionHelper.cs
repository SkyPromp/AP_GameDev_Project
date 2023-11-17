using Microsoft.Xna.Framework;
using System;


namespace AP_GameDev_Project.Entities
{
    internal class CollisionHelper
    {
        public void HandleHardCollison(ICollidable self, Rectangle other)
        {
            if (self.GetHitbox.Intersects(other)) this.HardCollide(self, other);
        }

        public void HandleHardCollison(ICollidable self, ICollidable other)
        {
            if (self.GetHitbox.Intersects(other.GetHitbox)) this.HardCollide(self, other.GetHitbox);
        }

        private void HardCollide(ICollidable self, Rectangle other)
        {
            Vector2 test = new Vector2(self.Position.X, self.Position.Y);

            Rectangle hitbox = self.GetHitbox;
            Rectangle normalized_hitbox = self.GetNormalizedHitbox;
            Vector2 self_position = self.Position;

            float horizontal = float.PositiveInfinity;
            float vertical = float.PositiveInfinity;

            if (hitbox.Left < other.Right)  // Left
            {
                test.X = other.Right - normalized_hitbox.Left;
                horizontal = test.X;
            }
            if ((hitbox.Right > other.Left))  // Right
            {
                test.X = other.Left - normalized_hitbox.Right;
                if (Math.Abs(horizontal - self_position.X) < Math.Abs(test.X - self_position.X)) test.X = horizontal;
            }

            if (hitbox.Top < other.Bottom)  // Top
            {
                test.Y = other.Bottom - normalized_hitbox.Top;
                vertical = test.Y;
            }
            if (hitbox.Bottom > other.Top)  // Bottom
            {
                test.Y = other.Top - normalized_hitbox.Bottom;
                if (Math.Abs(vertical - self_position.Y) < Math.Abs(test.Y - self_position.Y)) test.Y = vertical;
            }

            if (((test.X != self_position.X) && Math.Abs((self_position.X - test.X)) < Math.Abs(self_position.Y - test.Y)) || ((test.Y == self_position.Y)))
            {
                self.Position = new Vector2(test.X, self_position.Y);
                self.Speed *= new Vector2(0, 1);
            }
            else
            {
                self.Position = new Vector2(self_position.X, test.Y);
                self.Speed *= new Vector2(1, 0);
            }
        }
    }
}
