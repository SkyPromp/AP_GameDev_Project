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

        public enum mouseEnum: short
        {
            LEFT_CLICK=1,
            RIGHT_CLICK=2,
            MIDDLE_CLICK=4,
        }

        private short mouseActive;

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
                if (MouseHandler.instance == null)
                {
                    lock (MouseHandler.syncRoot)
                    {
                        if (MouseHandler.instance == null) MouseHandler.instance = new MouseHandler();
                    }
                }

                return MouseHandler.instance;
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
            this.mousePos = new Vector2(state.X, state.Y);
            this.mouseActive = 0;

            if (state.LeftButton == ButtonState.Pressed)
            {
                this.mouseActive |= (short) MouseHandler.mouseEnum.LEFT_CLICK;
                if (this.leftClickHook != null) this.leftClickHook();
            }

            if (state.RightButton == ButtonState.Pressed)
            {
                this.mouseActive |= (short) MouseHandler.mouseEnum.RIGHT_CLICK;
                if (this.rightClickHook != null) this.rightClickHook();
            }
        }
    }
}
