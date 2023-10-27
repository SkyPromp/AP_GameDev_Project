using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace AP_GameDev_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private StartStateHandler startStateHandler;
        private RunningStateHandler runningStateHandler;
        private MouseHandler mouseHandler;

        public enum states
        {
            START,
            RUNNING,
            PAUSED,
            GAME_OVER
        }
        public static states current_state;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = GlobalConstants.SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = GlobalConstants.SCREEN_HEIGHT;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            current_state = states.START;  // TODO Needs to start on START
            this.mouseHandler = new MouseHandler();
            this.mouseHandler.LeftClickHook = () => { Debug.WriteLine(this.mouseHandler.MousePos.ToString()); };
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D tilemap = Content.Load<Texture2D>("gamedev_tilemap");
            this.startStateHandler = new StartStateHandler();
            this.runningStateHandler = new RunningStateHandler(tilemap);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            this.mouseHandler.Update();
            Debug.WriteLine(Game1.current_state.ToString());

            switch(Game1.current_state){
                case states.START:
                    this.startStateHandler.Update(gameTime, mouseHandler);
                    break;
                case states.RUNNING:
                    this.runningStateHandler.Update(gameTime);
                    break;
                case states.PAUSED:
                    throw new NotImplementedException();
                    break;
                case states.GAME_OVER:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new Exception(string.Format("Invalid game state: {0}", Game1.current_state));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (Game1.current_state)
            {
                case states.START:
                    this.startStateHandler.Draw(_spriteBatch);
                    break;
                case states.RUNNING:
                    runningStateHandler.Draw(_spriteBatch);
                    break;
                case states.PAUSED:
                    throw new NotImplementedException();
                    break;
                case states.GAME_OVER:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new Exception(string.Format("Invalid game state: {0}", Game1.current_state));
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}