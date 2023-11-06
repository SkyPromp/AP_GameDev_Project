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
        private MouseHandler mouseHandler;

        public RunningStateHandler(Texture2D tilemap, Player player, List<AEntity> base_enemies)
        {
            this.current_room = new Room(tilemap, "Rooms\\BigRoom.room");
            this.player = player;
            this.mouseHandler = MouseHandler.getInstance;
            this.base_enemies = base_enemies;
            this.enemies = new List<AEntity>();

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
            this.mouseHandler.Update();
            this.HandleKeyboard();
            this.player.Update(gameTime);

            Rectangle player_hitbox = this.player.GetHitbox;
            Vector2 player_center = new Vector2(player_hitbox.X + player_hitbox.Width / 2 , player_hitbox.Y + player_hitbox.Height / 2);


            foreach(AEntity enemy in this.enemies)
            {
                enemy.Update(gameTime);
                enemy.Attack(player_center);  // Add condition
            }

            List<Bullet> player_bullets = new List<Bullet>(this.player.Bullets);
            List<List<Bullet>> enemy_bullets = new List<List<Bullet>>();
            List<List<Bullet>> enemy_removed_bullets = new List<List<Bullet>>();

            foreach (AEntity enemy in this.enemies) 
            { 
                enemy_bullets.Add(enemy.Bullets);
                enemy_removed_bullets.Add(new List<Bullet>());
            }

            List<AEntity> enemies_new = new List<AEntity>(this.enemies);

            foreach(Rectangle hitbox in this.current_room.GetHitboxes())
            {
                if (hitbox.Intersects(this.player.GetHitbox))
                {
                    this.player.HandleCollison(hitbox);
                }

                foreach(Bullet bullet in this.player.Bullets)
                {
                    Rectangle bullet_hitbox = bullet.GetHitbox;

                    if (hitbox.Intersects(bullet_hitbox))
                    {
                        player_bullets.Remove(bullet);
                    }
                }

                foreach (Enemy1 enemy in this.enemies)  // Abstract to AEntity
                {
                    if (hitbox.Intersects(enemy.GetHitbox))
                    {
                        enemy.HandleCollison(hitbox);
                    }

                    foreach (Bullet bullet in enemy_bullets[this.enemies.IndexOf(enemy)])
                    {
                        Rectangle bullet_hitbox = bullet.GetHitbox;

                        if (hitbox.Intersects(bullet_hitbox))
                        {
                            enemy_removed_bullets[enemy_bullets.Count - 1].Add(bullet);
                        }
                    }
                }
            }

            for(int enemy_index = 0; enemy_index < this.enemies.Count; enemy_index++)
            {
                List<Bullet> correct_bullets = new List<Bullet>(enemy_bullets[enemy_index]);

                for (int bullet_index = enemy_removed_bullets[enemy_index].Count - 1; bullet_index >= 0; bullet_index--)
                {
                    correct_bullets.Remove(enemy_removed_bullets[enemy_index][bullet_index]);
                }

                this.enemies[enemy_index].Bullets = correct_bullets;
            }

            foreach (Bullet bullet in this.player.Bullets)
            {
                Rectangle bullet_hitbox = bullet.GetHitbox;

                foreach (AEntity enemy in this.enemies)
                {
                    if (bullet_hitbox.Intersects(enemy.GetHitbox))
                    {
                        int health = enemy.DoDamage();
                        if (health <= 0)
                        {
                            enemies_new.Remove(enemy);
                        }
                    }
                }
            }

            this.enemies = enemies_new;
            this.player.Bullets = player_bullets;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            current_room.Draw(spriteBatch);
            player.Draw(spriteBatch);

            foreach (AEntity enemy in this.enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        private void HandleKeyboard()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Game1.current_state = Game1.States[Game1.states_enum.START];
                Game1.InitCurrentState();
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
