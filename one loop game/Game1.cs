using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace one_loop_game
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        ScreenManager gStateManager;

        FrameCounter frameCounter;

        bool showFps;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = Globals.screenX;
            graphics.PreferredBackBufferHeight = Globals.screenY;
            IsMouseVisible = true;
            graphics.IsFullScreen = false;
        }

        public void ExitGame()
        {
            if (Globals.exitGame)
                Exit();
        }

        protected override void Initialize()
        {
            gStateManager = new ScreenManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("buttonFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            frameCounter = new FrameCounter();
            gStateManager.Load(Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            ExitGame();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Globals.gameState = "menu";

            Input.Update(gameTime);

            if (Input.KeyClick(Keys.F1) && !Globals.debug)
                showFps = true;
            else if (Input.KeyClick(Keys.F1) && Globals.debug)
                showFps = false;


            //if (Input.KeyClick(Keys.F5))
            //{
            //    Globals.screenX = 1280;
            //    Globals.screenY = 720;
            //    graphics.PreferredBackBufferWidth = Globals.screenX;
            //    graphics.PreferredBackBufferHeight = Globals.screenY;
            //}
            //if (Input.KeyClick(Keys.F6))
            //{
            //    Globals.screenX = 1920;
            //    Globals.screenY = 1080;
            //    graphics.PreferredBackBufferWidth = Globals.screenX;
            //    graphics.PreferredBackBufferHeight = Globals.screenY;
            //}
            //if (Input.KeyClick(Keys.F8))
            //    graphics.ApplyChanges();

                gStateManager.Update(gameTime, graphics.GraphicsDevice, graphics);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(35, 35, 35));
            gStateManager.Draw(spriteBatch, graphics.GraphicsDevice);

            spriteBatch.Begin();
            #region DEBUG
            if (Globals.debug)
            {
                spriteBatch.DrawString(font, "debug mode", new Vector2(1, 53), Color.Black);
                spriteBatch.DrawString(font, "debug mode", new Vector2(0, 52), Color.WhiteSmoke);
            }
            if (showFps)
            {
                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                frameCounter.Update(deltaTime);
                var fps = string.Format("FPS: {0}", (int)frameCounter.AverageFramesPerSecond);
                spriteBatch.DrawString(font, fps, new Vector2(1, 33), Color.Black);
                spriteBatch.DrawString(font, fps, new Vector2(0, 32), Color.White);
            }
            #endregion
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
