using Microsoft.Xna.Framework.Input;
using System;
using System.Numerics;


namespace AP_GameDev_Project
{
    internal class MouseHandler//: IInputHandler
    {
        // Singleton vars
        private volatile static MouseHandler instance;
        private static object syncRoot = new object();

        private short mouseActive;
        public short MouseActive { get { return this.mouseActive; } }

        private Vector2 mousePos;
        public Vector2 MousePos { get { return this.mousePos; } }

        private Action leftClickHook;
        public Action LeftClickHook { set { this.leftClickHook = value; } }

        private Action rightClickHook;
        public Action RightClickHook { set { this.rightClickHook = value; } }

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
                this.mouseActive = 0;
                this.leftClickHook = null;
                this.rightClickHook = null;

        }

        public void Update()
        {
            MouseState state = Mouse.GetState();
            this.mousePos = new Vector2 (state.X, state.Y);
            this.mouseActive = 0;

            if (state.LeftButton == ButtonState.Pressed)
            {
                this.mouseActive |= 1;
                if(this.leftClickHook != null) this.leftClickHook();
            }

            if (state.RightButton == ButtonState.Pressed) {
                this.mouseActive |= 2;
                if (this.rightClickHook != null) this.rightClickHook();
            }
        }
    }
}
