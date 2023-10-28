using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace AP_GameDev_Project.State_handlers
{
    internal class RunningStateHandler : IStateHandler
    {
        Room current_room;
        private bool is_init;
        public bool IsInit { get; }

        public RunningStateHandler(Texture2D tilemap)
        {
            current_room = new Room(tilemap, "Rooms\\room1.room");
        }

        public void Init()
        {
            this.is_init = true;
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            current_room.Draw(spriteBatch);
        }
    }
}
