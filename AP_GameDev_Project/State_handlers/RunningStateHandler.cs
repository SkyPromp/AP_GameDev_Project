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
        private Player player;
        private List<AEntity> base_enemies;
        private List<AEntity> enemies;
        private List<AEntity> entities;
        private MouseHandler mouseHandler;
        private readonly double max_debug_cooldown;
        private double debug_cooldown;

        public RunningStateHandler(Texture2D tilemap, Player player, List<AEntity> base_enemies)
        {
            this.current_room = new Room(tilemap, "Rooms\\BigRoom.room");
            this.player = player;
            this.mouseHandler = MouseHandler.getInstance;
            this.base_enemies = base_enemies;
            this.enemies = new List<AEntity>();
            this.max_debug_cooldown = 0.3;
            this.debug_cooldown = 0;

            // TEST (REMOVE)
            this.enemies.Add(this.base_enemies[0]);
            // END TEST
        }

        public void Init()
        {
            this.is_init = true;
            this.mouseHandler.LeftClickHook = () => { this.player.Attack(); };
        }

        public void Update(GameTime gameTime)
        {
            if (this.debug_cooldown > 0) this.debug_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            this.mouseHandler.Update();
            this.HandleKeyboard();
            this.player.Update(gameTime);
            this.HandleCollision(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            current_room.Draw(spriteBatch);
            player.Draw(spriteBatch);

            foreach (AEntity enemy in this.enemies) enemy.Draw(spriteBatch);
        }

        private void HandleCollision(GameTime gameTime)
        {
            Vector2 player_center = this.player.GetCenter;

            List<Bullet> player_bullets = new List<Bullet>(this.player.Bullets);
            List<List<Bullet>> enemy_removed_bullets = new List<List<Bullet>>();

            foreach (AEntity enemy in this.enemies) enemy_removed_bullets.Add(new List<Bullet>());

            List<AEntity> enemies_new = new List<AEntity>(this.enemies);

            foreach (AEntity enemy in this.entities)
            {
                enemy.Update(gameTime);

                foreach (Rectangle hitbox in this.current_room.GetHitboxes())
                {
                    if (hitbox.Intersects(enemy.GetHitbox)) enemy.HandleCollison(hitbox);

                    foreach (Bullet bullet in enemy.Bullets)
                    {
                        if (hitbox.Intersects(bullet.GetHitbox)) enemy_removed_bullets[enemies.IndexOf(enemy)].Add(bullet);
                    }
                }

                enemy.Attack(player_center);  // Add condition

                int enemy_index = this.enemies.IndexOf(enemy);
                for (int bullet_index = enemy_removed_bullets[enemy_index].Count - 1; bullet_index >= 0; bullet_index--)
                {
                    this.enemies[enemy_index].Bullets.Remove(enemy_removed_bullets[enemy_index][bullet_index]);
                }

                foreach (Bullet bullet in this.player.Bullets)
                {
                    if (bullet.GetHitbox.Intersects(enemy.GetHitbox))
                    {
                        int health = enemy.DoDamage();

                        if (health <= 0) enemies_new.Remove(enemy);
                    }
                }
            }

            foreach (Rectangle hitbox in this.current_room.GetHitboxes())  // Use previous foreach to generalize AEntity
            {
                if (hitbox.Intersects(this.player.GetHitbox)) this.player.HandleCollison(hitbox);

                foreach (Bullet bullet in this.player.Bullets)
                {
                    if (hitbox.Intersects(bullet.GetHitbox)) player_bullets.Remove(bullet);
                }
            }

            this.enemies = enemies_new;
            this.player.Bullets = player_bullets;
        }

        private void HandleKeyboard()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Game1.current_state = Game1.States[Game1.states_enum.START];
                Game1.InitCurrentState();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F3) && this.debug_cooldown <= 0)
            {
                this.debug_cooldown = this.max_debug_cooldown;
                this.player.do_draw_hitbox = !this.player.do_draw_hitbox;

                foreach(AEntity enemy in this.enemies)
                {
                    enemy.do_draw_hitbox = !enemy.do_draw_hitbox;
                }
            }

            Vector2 addSpeed = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                addSpeed += new Vector2(0, -1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                addSpeed += new Vector2(0, 1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                addSpeed += new Vector2(-1, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                addSpeed += new Vector2(1, 0);
            }

            this.player.SpeedUp(addSpeed / 3);
        }
    }
}
