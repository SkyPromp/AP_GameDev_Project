using Microsoft.Xna.Framework.Input;
using System;
using System.Numerics;


namespace AP_GameDev_Project.Input_devices
{
    internal class MouseHandler//: IInputHandler
    {
        // Singleton vars
        private volatile static MouseHandler instance;
        private static object syncRoot = new object();

        private short mouseActive;
        public short MouseActive { get { return mouseActive; } }

        private Vector2 mousePos;
        public Vector2 MousePos { get { return mousePos; } }

        private Action leftClickHook;
        public Action LeftClickHook { set { leftClickHook = value; } }

        private Action rightClickHook;
        public Action RightClickHook { set { rightClickHook = value; } }

        private MouseHandler() { }

        public static MouseHandler getInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null) instance = new MouseHandler();
                    }
                }

                return instance;
            }
        }

        public void Init()
        {
            mouseActive = 0;
            leftClickHook = null;
            rightClickHook = null;

        }

        public void Update()
        {
            MouseState state = Mouse.GetState();
            mousePos = new Vector2(state.X, state.Y);
            mouseActive = 0;

            if (state.LeftButton == ButtonState.Pressed)
            {
                mouseActive |= 1;
                if (leftClickHook != null) leftClickHook();
            }

            if (state.RightButton == ButtonState.Pressed)
            {
                mouseActive |= 2;
                if (rightClickHook != null) rightClickHook();
            }
        }
    }
}
