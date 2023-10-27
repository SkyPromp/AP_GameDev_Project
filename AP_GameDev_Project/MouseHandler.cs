using Microsoft.Xna.Framework.Input;
using System;
using System.Numerics;


namespace AP_GameDev_Project
{
    internal class MouseHandler: IInputHandler
    {
        private short mouseActive;
        public short MouseActive { get { return mouseActive; } }
        private Vector2 mousePos;
        public Vector2 MousePos { get { return mousePos; } }

        private Action leftClickHook;
        public Action LeftClickHook { set { leftClickHook = value; } }
        private Action rightClickHook;
        public Action RightClickHook { set { rightClickHook = value; } }

        public MouseHandler() { 
            // Make into a singleton
            mouseActive = 0;
            leftClickHook = null;
            rightClickHook = null;
        }

        public void Update()
        {
            MouseState state = Mouse.GetState();
            this.mousePos = new Vector2 (state.X, state.Y);
            mouseActive = 0;

            if (state.LeftButton == ButtonState.Pressed)
            {
                mouseActive |= 1;
                if(leftClickHook != null) leftClickHook();
            }

            if (state.RightButton == ButtonState.Pressed) {
                mouseActive |= 2;
                if (rightClickHook != null) rightClickHook();
            }
        }
    }
}
