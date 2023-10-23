using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;

namespace AP_GameDev_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
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
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            current_state = states.START;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            /*switch(this.current_state){
                case states.START:
                    throw new NotImplementedException();
                    break;
                case states.RUNNING:
                    throw new NotImplementedException();
                    break;
                case states.PAUSED:
                    throw new NotImplementedException();
                    break;
                case states.GAME_OVER:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new Exception(string.Format("Invalid game state: {0}", this.current_state));
            }*/

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();



            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}