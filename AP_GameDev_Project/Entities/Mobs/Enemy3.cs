using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Entities.Mobs
{
    internal class Enemy3: AEntity
    {
        private bool is_attacking;
        private readonly Animate stand_no_spike;
        private readonly Animate stand_spike;
        private readonly Animate walk_no_spike;
        private readonly Animate walk_spike;
        public bool IsAttacking { get { return this.is_attacking; } }

        public Enemy3(Vector2 position, ContentManager contentManager, float max_speed, int base_health, float speed_damping_factor = 0.8f) :
            base(position, max_speed, new Hitbox().AddChild(new Rectangle(22, 10, 17, 43)), 0f, 1f, contentManager.GetAnimations["ENEMY3A_STANDSTILL"], contentManager.GetAnimations["ENEMY3A_WALK"], base_health: base_health, speed_damping_factor: speed_damping_factor, hitbox_center: new Vector2(30.5f, 31.5f), damage: 1)
        {
            this.is_attacking = false;
            this.stand_no_spike = contentManager.GetAnimations["ENEMY3B_STANDSTILL"];
            this.walk_no_spike = contentManager.GetAnimations["ENEMY3B_WALK"];
            this.stand_spike = contentManager.GetAnimations["ENEMY3A_STANDSTILL"];
            this.walk_spike = contentManager.GetAnimations["ENEMY3A_WALK"];
        }

        public override void Update(GameTime gameTime, Vector2 player_center)
        {
            Attack(player_center);
            base.SpeedUp(Vector2.Normalize(player_center - GetCenter));
            base.Update(gameTime, player_center);
            Debug.WriteLine(this.is_attacking);
        }

        public void DealDamage()
        {
            this.is_attacking = false;
            base.stand_animation = stand_no_spike;
            base.walk_animation = walk_no_spike;
            base.bullet_cooldown = base.bullet_max_cooldown;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Attack(Vector2 player_center)
        {
            if (base.bullet_cooldown <= 0)
            {
                this.is_attacking = true;
                base.stand_animation = stand_spike;
                base.walk_animation = walk_spike;
                base.bullet_cooldown = base.bullet_max_cooldown;
            }
        }
    }
}
