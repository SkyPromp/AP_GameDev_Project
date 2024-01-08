using AP_GameDev_Project.Entities.Mobs;
using AP_GameDev_Project.State_handlers;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace AP_GameDev_Project.Entities
{
    internal class HitboxCollisionHelper
    {
        public void HandleHardCollison(AEntity entity, AEntity other)  // Returns if other should die
        {
            (Rectangle self_hitbox, Rectangle other_hitbox) = entity.GetHitboxHitbox.DoesCollideR(other.GetHitboxHitbox);

            if (!self_hitbox.IsEmpty)
            {
                if (entity is Player && other is AEnemy)
                {
                    ((AEnemy)other).CollideWithPlayer(entity);
                }
                else
                {
                (Vector2 delta_pos, Vector2 factor_speed) = this.HardCollide(self_hitbox, other_hitbox);

                if (delta_pos.X == 0) entity.Position = new Vector2(entity.Position.X, (float)Math.Floor(entity.Position.Y));
                else entity.Position = new Vector2((float)Math.Floor(entity.Position.X), entity.Position.Y);

                entity.Position += delta_pos;
                entity.Speed *= factor_speed;
                }
            }
        }

        public void HandleHardCollison(AEntity entity, Rectangle other)
        {
            Rectangle self_hitbox = entity.GetHitboxHitbox.DoesCollideR(other);

            if (!self_hitbox.IsEmpty)
            {
                (Vector2 delta_pos, Vector2 factor_speed) = this.HardCollide(self_hitbox, other);

                if (delta_pos.X == 0) entity.Position = new Vector2(entity.Position.X, (float) Math.Floor(entity.Position.Y));
                else entity.Position = new Vector2((float) Math.Floor(entity.Position.X), entity.Position.Y);

                entity.Position += delta_pos;
                entity.Speed *= factor_speed;
            }
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
