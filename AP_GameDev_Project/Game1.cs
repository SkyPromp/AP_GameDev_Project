using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace AP_GameDev_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RunningStateHandler runningStateHandler;

        private enum states
        {
            START,
            RUNNING,
            PAUSED,
            GAME_OVER
        }
        private states current_state;

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
            current_state = states.RUNNING;  // TODO Needs to start on START
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D tilemap = Content.Load<Texture2D>("gamedev_tilemap");
            this.runningStateHandler = new RunningStateHandler(tilemap);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch(this.current_state){
                case states.START:
                    throw new NotImplementedException();
                    break;
                case states.RUNNING:
                    runningStateHandler.Update(gameTime);
                    break;
                case states.PAUSED:
                    throw new NotImplementedException();
                    break;
                case states.GAME_OVER:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new Exception(string.Format("Invalid game state: {0}", this.current_state));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (this.current_state)
            {
                case states.START:
                    throw new NotImplementedException();
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
                    throw new Exception(string.Format("Invalid game state: {0}", this.current_state));
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}