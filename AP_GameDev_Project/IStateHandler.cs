using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AP_GameDev_Project
{
    internal interface IStateHandler
    {
        public void Update(GameTime gameTime, IInputHandler inputDevice);
        public void Draw(SpriteBatch spriteBatch);
    }
}
