using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace AP_GameDev_Project
{
    internal class RunningStateHandler//: IStateHandler
    {
        Room current_room;
        public RunningStateHandler(Texture2D tilemap) { 
            this.current_room = new Room(tilemap, "Rooms\\room1.room");
        }

        public void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.current_room.Draw(spriteBatch);
        }
    }
}
