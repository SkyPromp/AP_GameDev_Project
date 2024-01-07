﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Entities.Mobs
{
    internal class Enemy2: AEntity
    {
        private bool is_attacking;
        private Vector2 target;
        private double max_target_cooldown;
        private double target_cooldown;

        public bool IsAttacking {  get { return this.is_attacking; } }

        public Enemy2(Vector2 position, ContentManager contentManager, float max_speed, int base_health, float speed_damping_factor = 0.8f) :
            base(position, max_speed, new Hitbox().AddChild(new Rectangle(15, 16, 33, 33)), 0f, 5f, contentManager.GetAnimations["ENEMY2_STANDSTILL"], contentManager.GetAnimations["ENEMY2_WALK"], base_health: base_health, speed_damping_factor: speed_damping_factor, hitbox_center: new Vector2(30.5f, 31.5f), damage: 2)
        {
            this.is_attacking = true;
            this.target = position;
            this.target_cooldown = 0;
            this.max_target_cooldown = 4;
        }

        public override void Update(GameTime gameTime, Vector2 player_center)
        {
            if (this.target_cooldown > 0) this.target_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            else if (this.target != player_center)
            {
                this.target_cooldown = this.max_target_cooldown;
                this.target = player_center;
            }

            if ((this.target - this.GetCenter).Length() > 1)
            {
                base.is_standing = false;
                base.SpeedUp(Vector2.Normalize(this.target - this.GetCenter));
                base.Update(gameTime, player_center);
            } else
            {
                base.is_standing = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Attack(Vector2 player_center)
        {
        }

        public override int DoDamage(int damage = 1)
        {
            if (damage < 0) health = 0;
            else base.DoDamage(damage);

            return base.health;
        }

        public override void Die(ContentManager contentManager)
        {
            contentManager.GetSoundEffects["EXPLOSION"].Play();
        }
    }
}
