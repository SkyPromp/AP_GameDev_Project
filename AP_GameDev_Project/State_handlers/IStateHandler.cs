using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AP_GameDev_Project.State_handlers
{
    internal interface IStateHandler
    {
        public bool IsInit { get; }
        public void Init();
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);
    }
}
