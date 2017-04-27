using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace one_loop_game
{
    class ScreenOptions
    {
        Texture2D texture;
        Button back;
        Button res1;
        Button res2;
        Button res3;
        Button apply;
        Button fullscreen;

        Vector2 resTemp;
        bool isFs;

        List<Button> buttons = new List<Button>();
        SpriteFont font;

        public void Load(ContentManager content)
        {
            font = content.Load<SpriteFont>("buttonFont");
            texture = content.Load<Texture2D>("box");
            back = new Button(new Vector2(128, 32), new Vector2(Globals.screenCenter.X - (128 / 2), Globals.screenCenter.Y - (32 / 2) + 212), new Color(85, 85, 85), "back", Color.White, font);
            res1 = new Button(new Vector2(128, 32), new Vector2(Globals.screenCenter.X - (128 / 2), Globals.screenCenter.Y - (32 / 2) + 0), new Color(85, 85, 85), "720", Color.White, font);
            res2 = new Button(new Vector2(128, 32), new Vector2(Globals.screenCenter.X - (128 / 2), Globals.screenCenter.Y - (32 / 2) + 36), new Color(85, 85, 85), "1080", Color.White, font);
            res3 = new Button(new Vector2(128, 32), new Vector2(Globals.screenCenter.X - (128 / 2), Globals.screenCenter.Y - (32 / 2) + 72), new Color(85, 85, 85), "1440", Color.White, font);
            fullscreen = new Button(new Vector2(128, 32), new Vector2(Globals.screenCenter.X - (128 / 2), Globals.screenCenter.Y - (32 / 2) + 112), new Color(85, 85, 85), "fullscreen", Color.White, font);

            apply = new Button(new Vector2(128, 32), new Vector2(Globals.screenCenter.X - (128 / 2), Globals.screenCenter.Y - (32 / 2) + 180), new Color(85, 85, 85), "apply", Color.White, font);


            buttons.Add(res1);
            buttons.Add(res2);
            buttons.Add(res3);
            buttons.Add(apply);
            buttons.Add(fullscreen);


            buttons.Add(back);
            resTemp = new Vector2(Globals.screenX, Globals.screenY);

            foreach (Button b in buttons)
                b.Load(content);
        }
        public void Update(GameTime gameTime, GraphicsDeviceManager gdm)
        {
            Input.mPos = new Vector2(Input.m.X, Input.m.Y);
            if (Input.KeyClick(Keys.Escape) || (back.Rectangle.Contains(Input.mPos) && Input.LeftRelease()))
                Globals.gameState = "menu";

            if (Input.LeftClickInside(res1.Rectangle))
                resTemp = new Vector2(1280, 720);
            if (Input.LeftClickInside(res2.Rectangle))
                resTemp = new Vector2(1920, 1080);
            if (Input.LeftClickInside(res3.Rectangle))
                resTemp = new Vector2(2560, 1440);

            if (Input.LeftClickInside(fullscreen.Rectangle) && isFs)
                isFs = false;
            else if (Input.LeftClickInside(fullscreen.Rectangle) && !isFs)
                isFs = true;

            if (Input.LeftClickInside(apply.Rectangle))
            {
                Globals.screenX = (int)resTemp.X;
                Globals.screenY = (int)resTemp.Y;
                gdm.PreferredBackBufferWidth = Globals.screenX;
                gdm.PreferredBackBufferHeight = Globals.screenY;
                gdm.IsFullScreen = isFs;
                gdm.ApplyChanges();
            }

            foreach (Button button in buttons)
                    button.Update(gameTime);
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            foreach (Button button in buttons)
                button.Draw(sb);

            sb.DrawString(font, "" + isFs, new Vector2(Globals.screenCenter.X - (128 / 2) + 132 + 1, Globals.screenCenter.Y - (32 / 2) + 115 + 1), Color.Black);
            sb.DrawString(font, "" + isFs, new Vector2(Globals.screenCenter.X - (128 / 2) + 132, Globals.screenCenter.Y - (32 / 2) + 115), Color.White);

            sb.DrawString(font, "settings (change at own risk, desktop resolution recommended)", new Vector2(Globals.screenCenter.X - (128 / 2) + 1 - 25, Globals.screenCenter.Y - (32 / 2) - 20 + 1), Color.Black);
            sb.DrawString(font, "settings (change at own risk, desktop resolution recommended)", new Vector2(Globals.screenCenter.X - (128 / 2) - 25, Globals.screenCenter.Y - (32 / 2) - 20), Color.White);
            sb.End();
        }
    }
}
