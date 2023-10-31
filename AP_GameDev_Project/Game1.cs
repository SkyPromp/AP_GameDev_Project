using AP_GameDev_Project.State_handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace AP_GameDev_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;
        private StartStateHandler startStateHandler;
        private RunningStateHandler runningStateHandler;
        private MapMakingStateHandler mapMakingStateHandler;

        public enum states
        {
            START,
            RUNNING,
            MAPMAKING,
            PAUSED,
            GAME_OVER
        }
        public static states current_state;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = GlobalConstants.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = GlobalConstants.SCREEN_HEIGHT;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            current_state = states.START;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D tilemap = Content.Load<Texture2D>("gamedev_tilemap");
            SpriteFont font = Content.Load<SpriteFont>("Font");
            this.startStateHandler = new StartStateHandler();
            this.runningStateHandler = new RunningStateHandler(tilemap);
            this.mapMakingStateHandler = new MapMakingStateHandler(GraphicsDevice, tilemap, font);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch(Game1.current_state){
                case states.START:
                    if (!this.startStateHandler.IsInit) this.startStateHandler.Init();

                    this.startStateHandler.Update(gameTime);
                    break;
                case states.RUNNING:
                    if (!this.runningStateHandler.IsInit) this.runningStateHandler.Init();

                    this.runningStateHandler.Update(gameTime);
                    break;
                case states.MAPMAKING:
                    if (!this.mapMakingStateHandler.IsInit) this.mapMakingStateHandler.Init();
                    
                    this.mapMakingStateHandler.Update(gameTime);
                    break;
                case states.PAUSED:
                    throw new NotImplementedException();
                    break;
                case states.GAME_OVER:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Invalid game state: {0}", Game1.current_state));
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
                case states.MAPMAKING:
                    mapMakingStateHandler.Draw(_spriteBatch);
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
