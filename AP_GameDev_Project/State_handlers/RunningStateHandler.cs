﻿using AP_GameDev_Project.Entities;
using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace AP_GameDev_Project.State_handlers
{
    internal class RunningStateHandler : IStateHandler
    {
        private Room current_room;
        private bool is_init;
        public bool IsInit { get { return this.is_init; } }
        private List<AEntity> base_enemies;
        private List<AEntity> entities;
        private Player Player { get { return (Player)this.entities[0]; } set { this.entities[0] = value; } }
        private MouseHandler mouseHandler;
        private readonly double max_debug_cooldown;
        public double Max_debug_cooldown { get { return this.max_debug_cooldown; } }
        private double debug_cooldown;
        public double Debug_cooldown { get { return this.debug_cooldown; } set { this.debug_cooldown = value; } }
        
        private RunningKeyboardEventHandler keyboardHandler;

        public RunningStateHandler(Texture2D tilemap, Player player, List<AEntity> base_enemies)
        {
            this.current_room = new Room(tilemap, "Rooms\\BigRoom.room");
            this.mouseHandler = MouseHandler.getInstance;
            this.base_enemies = base_enemies;
            this.entities = new List<AEntity>();
            this.max_debug_cooldown = 0.3;
            this.debug_cooldown = 0;
            this.keyboardHandler = new RunningKeyboardEventHandler(new RunningKeyboardHandler(), this);

            // TEST (REMOVE)
            this.entities.Add(player);
            foreach (AEntity enemy in base_enemies) this.entities.Add(enemy);
            // END TEST
        }

        public void Init()
        {
            this.is_init = true;
            this.mouseHandler.LeftClickHook = () => { this.Player.Attack(); };
        }

        public void Update(GameTime gameTime)
        {
            if (this.debug_cooldown > 0) this.debug_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            this.mouseHandler.Update();
            this.keyboardHandler.Update();
            //this.entities = this.keyboardHandler.HandleKeyboard(this, this.entities);
            this.HandleCollision(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            current_room.Draw(spriteBatch);

            foreach (AEntity entity in this.entities) entity.Draw(spriteBatch);
        }

        public void MovePlayer(Vector2 speed)
        {
            this.Player.SpeedUp(speed);
        }

        public void ToggleDebug()
        {
            foreach(AEntity entity in this.entities) entity.show_hitbox = !entity.show_hitbox;
        }

        private void HandleCollision(GameTime gameTime)
        {
            Vector2 player_center = this.Player.GetCenter;
            List<AEntity> entities_new = new List<AEntity>(this.entities);
            bool is_player = true;

            foreach (AEntity entity in this.entities)
            {
                entity.Update(gameTime);
                List<Bullet> entity_bullets = new List<Bullet>(entity.Bullets);

                foreach (Rectangle hitbox in this.current_room.GetHitboxes())
                {
                    if (hitbox.Intersects(entity.GetHitbox)) entity.HandleCollison(hitbox);

                    foreach (Bullet bullet in entity.Bullets)
                    {
                        if (hitbox.Intersects(bullet.GetHitbox)) entity_bullets.Remove(bullet);
                    }
                }

                entity.Bullets = entity_bullets;

                if (is_player)
                {
                    is_player = false;
                    continue;
                }

                entity.Attack(player_center);  // Add condition

                // Check if player hits enemies
                List<Bullet> player_bullets = new List<Bullet>(this.Player.Bullets);

                foreach (Bullet bullet in this.Player.Bullets)
                {
                    if (bullet.GetHitbox.Intersects(entity.GetHitbox))
                    {
                        int health = entity.DoDamage();
                        player_bullets.Remove(bullet);
                        if (health <= 0) entities_new.Remove(entity);
                    }
                }

                this.Player.Bullets = player_bullets;

                // Check if enemies hit player
                entity_bullets = new List<Bullet>(entity.Bullets);

                foreach (Bullet bullet in entity.Bullets)
                {
                    if (bullet.GetHitbox.Intersects(this.Player.GetHitbox))
                    {
                        int health = this.Player.DoDamage();
                        entity_bullets.Remove(bullet);
                        if (health <= 0) throw new NotImplementedException("The player has died, a game over screen has not been implemented yet.");
                    }
                }

                entity.Bullets = entity_bullets;
            }

            this.entities = entities_new;
        }
    }
}
