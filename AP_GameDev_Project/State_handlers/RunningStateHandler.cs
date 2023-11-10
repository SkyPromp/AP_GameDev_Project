using AP_GameDev_Project.Entities;
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
        private double debug_cooldown;
        private StateHandler stateHandler;

        public RunningStateHandler(Texture2D tilemap, Player player, List<AEntity> base_enemies)
        {
            this.current_room = new Room(tilemap, "Rooms\\BigRoom.room");
            this.mouseHandler = MouseHandler.getInstance;
            this.base_enemies = base_enemies;
            this.entities = new List<AEntity>();
            this.max_debug_cooldown = 0.3;
            this.debug_cooldown = 0;
            this.stateHandler = StateHandler.getInstance;

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
            this.HandleKeyboard();
            this.HandleCollision(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            current_room.Draw(spriteBatch);

            foreach (AEntity entity in this.entities) entity.Draw(spriteBatch);
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

                foreach (Bullet bullet in this.Player.Bullets)  // Remove hit bullet
                {
                    if (bullet.GetHitbox.Intersects(entity.GetHitbox))
                    {
                        int health = entity.DoDamage();

                        if (health <= 0) entities_new.Remove(entity);
                    }
                }
            }

            this.entities = entities_new;
        }

        private void HandleKeyboard()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                stateHandler.SetCurrentState(StateHandler.states_enum.START).Init();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F3) && this.debug_cooldown <= 0)
            {
                this.debug_cooldown = this.max_debug_cooldown;

                foreach(AEntity enemy in this.entities) enemy.do_draw_hitbox = !enemy.do_draw_hitbox;
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

            this.Player.SpeedUp(addSpeed / 3);
        }
    }
}
