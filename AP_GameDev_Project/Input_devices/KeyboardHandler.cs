using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Input_devices
{
    internal class KeyboardHandler : IInputHandler
    {
        // Singleton vars
        private volatile static KeyboardHandler instance;
        private static object syncRoot = new object();

        private KeyboardHandler() { }

        public static KeyboardHandler getInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null) instance = new KeyboardHandler();
                    }
                }

                return instance;
            }
        }

        public void Update()
        {

        }
    }
}
