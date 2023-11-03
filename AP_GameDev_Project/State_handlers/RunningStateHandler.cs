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
        private Player player;
        private List<AEntity> Enemies;
        private MouseHandler mouseHandler;

        public RunningStateHandler(Texture2D tilemap, Player player)
        {
            this.current_room = new Room(tilemap, "Rooms\\BigRoom.room");
            this.player = player;
            this.mouseHandler = MouseHandler.getInstance;
            this.Enemies = new List<AEntity>();
        }

        public void Init()
        {
            this.is_init = true;
            this.mouseHandler.LeftClickHook = () => {
                this.player.Attack(Vector2.Normalize(this.mouseHandler.MousePos - this.player.Position));
            };
        }

        public void Update(GameTime gameTime)
        {
            this.mouseHandler.Update();
            this.HandleKeyboard();
            this.player.Update(gameTime);

            foreach(AEntity enemy in this.Enemies)
            {
                enemy.Update(gameTime);
            }

            List<Bullet> bullets = new List<Bullet>(this.player.Bullets);

            foreach(Rectangle hitbox in this.current_room.GetHitboxes())
            {
                if (hitbox.Intersects(this.player.GetHitbox))
                {
                    Debug.WriteLine("Collision");
                    this.player.HandleCollison(hitbox);
                }

                foreach(Bullet bullet in this.player.Bullets)
                {
                    if (hitbox.Intersects(bullet.GetHitbox))
                    {
                        bullets.Remove(bullet);
                    }
                }
            }

            this.player.Bullets = bullets;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            current_room.Draw(spriteBatch);
            player.Draw(spriteBatch);

            foreach (AEntity enemy in this.Enemies)
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
