using Microsoft.Xna.Framework;


namespace AP_GameDev_Project.Entities.Mobs
{
    internal abstract class AEnemy : AEntity
    {
        protected Vector2 target;

        public AEnemy(Vector2 position, float max_speed, Hitbox hitbox, float bullet_speed, double bullet_max_cooldown, Animate stand_animation, Animate walk_animation = null, int base_health = 5, float speed_damping_factor = 0.95f, Vector2 hitbox_center = default, int damage = 1) :
            base(position, max_speed, hitbox, bullet_speed, bullet_max_cooldown, stand_animation, walk_animation, base_health, speed_damping_factor, hitbox_center, damage)
        {
        }
        public override void Update(GameTime gameTime, Vector2 player_center)
        {
            Attack(player_center);

            if ((this.target - this.GetCenter).Length() > 3)
            {
                base.is_standing = false;
                base.SpeedUp(Vector2.Normalize(this.target - this.GetCenter));
            }
            else
            {
                base.is_standing = true;
            }

            base.Update(gameTime, this.target);
        }

        public abstract override void Attack(Vector2 player_center);

        public virtual void CollideWithPlayer(AEntity player)
        {
        }
    }
}
