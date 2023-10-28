using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.State_handlers
{
    internal class MapMakingStateHandler : IStateHandler
    {
        private MouseHandler mouseHandler;
        private bool is_init;
        public bool IsInit { get; }

        public MapMakingStateHandler() {
            this.mouseHandler = MouseHandler.getInstance;
        }

        public void Init()
        {
            this.is_init = true;
            mouseHandler.LeftClickHook = () => { Debug.WriteLine("MapMaker Left"); };
            mouseHandler.RightClickHook = () => { Debug.WriteLine("MapMaker Right"); };
        }

        public void Update(GameTime gameTime)
        {
            mouseHandler.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
