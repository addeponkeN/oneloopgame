using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;

namespace one_loop_game
{
    class ScreenMenu
    {
        Button btnStart;
        Button btnOptions;
        Button btnExit;
        List<Button> buttons = new List<Button>();
        Texture2D texture;
        SpriteFont font;

        SoundManager sm;

        TileManager tileM;
        Texture2D tileSheet, blur, logo;

        bool goRight;

        public ScreenMenu()
        {

        }

        public void Load(ContentManager content, ScreenPlaying p)
        {
            tileSheet = content.Load<Texture2D>("tileSheet");
            blur = content.Load<Texture2D>("blurfilter");
            logo = content.Load<Texture2D>("logo2");
            sm = new SoundManager();
            tileM = new TileManager(tileSheet, font, 150, 40);
            sm.Load(content);
            texture = content.Load<Texture2D>("box");
            font = content.Load<SpriteFont>("buttonFont");

            btnStart = new Button(new Vector2(128, 32), new Vector2(Globals.screenCenter.X - (128 / 2), Globals.screenCenter.Y - (32 / 2) - 32), new Color(85, 85, 85), "start", Color.Black, font);
            buttons.Add(btnStart);
            btnOptions = new Button(new Vector2(128, 32), new Vector2(Globals.screenCenter.X - (128 / 2), Globals.screenCenter.Y - (32 / 2) + 16), new Color(85, 85, 85), "options", Color.Black, font);
            buttons.Add(btnOptions);
            btnExit = new Button(new Vector2(128, 32), new Vector2(Globals.screenCenter.X - (128 / 2), Globals.screenCenter.Y - (32 / 2) + 80), new Color(85, 85, 85), "exit", Color.Black, font);
            buttons.Add(btnExit);

            tileM.GenerateMap();
            tileM.PlaceTiles(50, 3, p, false);
            tileM.CheckAllTileTypes(p);
            tileM.CheckIfTreeAboveAll();

            foreach (Button b in buttons)
                b.Load(content);


            //SoundManager.PlayMusicLooped(sm.songForest);
            sm.songForestInstance.IsLooped = true;
            sm.songForestInstance.Play();
            p.cam.pos.X = Globals.screenX / 2;
            p.cam.pos.Y = Globals.screenY / 2;

        }
        public void Exit()
        {
            sm.songForestInstance.Stop();
        }
        public void Update(GameTime gameTime, ScreenPlaying p)
        {
            Input.mPos = new Vector2(Input.m.X, Input.m.Y);
            if (btnStart.Rectangle.Contains(Input.mPos) && Input.LeftRelease())
            {
                Exit();
                Globals.gameState = "playing";
            }

            if (btnOptions.Rectangle.Contains(Input.mPos) && Input.LeftRelease())
                Globals.gameState = "options";

            if (btnExit.Rectangle.Contains(Input.mPos) && Input.LeftRelease())
                Globals.gameState = "exitGame";

            tileM.Update(gameTime, p);

            if (p.cam.pos.X / 32 > (p.tileManager.mapX) - 50 && goRight)
                goRight = false;
            else if (p.cam.pos.X < Globals.screenX / 2 && !goRight)
                goRight = true;

            if (goRight)
                p.cam.pos.X += 0.4f;
            else p.cam.pos.X -= 0.4f;


                Console.WriteLine(p.cam.pos.X / 32 + "   " + (p.tileManager.mapX));

            foreach (Button b in buttons)
                b.Update(gameTime);

            buttons.RemoveAll(b => !b.Exist);
        }

        public void Draw(SpriteBatch spriteBatch, Camera cam, GraphicsDevice gd)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default,
                    RasterizerState.CullNone, null, cam.get_transformation(gd));

            tileM.DrawMenu(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin();
            //spriteBatch.Draw(blur, new Rectangle(0, 0, Globals.screenX, Globals.screenY), Color.White);
            spriteBatch.Draw(logo, new Vector2((Globals.screenX / 2) - (logo.Width / 2), Globals.screenY / 6), Color.White);


            foreach (Button b in buttons)
                b.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
