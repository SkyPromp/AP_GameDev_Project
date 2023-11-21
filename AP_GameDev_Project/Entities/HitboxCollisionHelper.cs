using AP_GameDev_Project.Entities.Mobs;
using Microsoft.Xna.Framework;
using System;


namespace AP_GameDev_Project.Entities
{
    internal class HitboxCollisionHelper
    {
        public void HandleHardCollison(AEntity entity, Rectangle other)
        {
            Rectangle self_hitbox = entity.GetHitboxHitbox.DoesCollideR(other);

            if (!self_hitbox.IsEmpty)
            {
                (Vector2 delta_pos, Vector2 factor_speed) = this.HardCollide(self_hitbox, other);
                entity.Position += delta_pos;
                entity.Speed *= factor_speed;
            }
        }

        public (Vector2, Vector2) HandleHardCollison(Hitbox self, Rectangle other)  // self.position += output[0]; self.speed *= output[1]
        {
            Rectangle self_hitbox = self.DoesCollideR(other);

            if (!self_hitbox.IsEmpty) return this.HardCollide(self_hitbox, other);

            return (Vector2.Zero, Vector2.One);
        }

        public (Vector2, Vector2) HandleHardCollison(Rectangle self, Rectangle other)  // self.position += output[0]; self.speed *= output[1]
        {
            if (self.Intersects(other)) return this.HardCollide(self, other);

            return (Vector2.Zero, Vector2.One);
        }

        private (Vector2, Vector2) HardCollide(Rectangle self, Rectangle other)  // self.position += output[0]; self.speed *= output[1]
        {
            Vector2 test = Vector2.Zero;

            if (self.Left < other.Right && self.Right > other.Left)
            {
                if (self.Center.X < other.Center.X)  // move to the left
                {
                    test.X = other.Left - self.Right;
                }
                else  // move to the right
                {
                    test.X = other.Right - self.Left;
                }
            }

            if (self.Top < other.Bottom && self.Bottom > other.Top)
            {
                if (self.Center.Y < other.Center.Y)  // move to the top
                {
                    test.Y = other.Top - self.Bottom;
                }
                else  // move to the bottom
                {
                    test.Y = other.Bottom - self.Top;
                }
            }

            if ((test.X != 0) && Math.Abs(test.X) < Math.Abs(test.Y) || (test.Y == 0))
            {
                return (new Vector2(test.X, 0), new Vector2(0, 1));
            }

            return (new Vector2(0, test.Y), new Vector2(1, 0));
        }
    }
}
