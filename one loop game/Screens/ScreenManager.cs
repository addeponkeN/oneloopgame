using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace one_loop_game
{
    class ScreenManager
    {
        ScreenMenu screenMenu;
        ScreenOptions screenOptions;
        ScreenPlaying screenPlaying;

        public ScreenManager()
        {
            screenMenu = new ScreenMenu();
            screenOptions = new ScreenOptions();
            screenPlaying = new ScreenPlaying();

        }
        public void Load(ContentManager content)
        {
            screenOptions.Load(content);
            screenPlaying.Load(content);
            screenMenu.Load(content, screenPlaying);
        }

        public void Update(GameTime gameTime, GraphicsDevice gd, GraphicsDeviceManager gdm)
        {
            switch (Globals.gameState)
            {
                case "menu":
                    screenMenu.Update(gameTime, screenPlaying);
                    break;
                case "options":
                    screenOptions.Update(gameTime, gdm);
                    break;
                case "playing":
                    screenPlaying.Update(gameTime, gd);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            switch (Globals.gameState)
            {
                case "menu":
                    screenMenu.Draw(spriteBatch, screenPlaying.cam, graphics);
                    break;
                case "options":
                    screenOptions.Draw(spriteBatch);
                    break;
                case "playing":
                    screenPlaying.Draw(spriteBatch, graphics);
                    break;
                case "exitGame":
                    Globals.exitGame = true;
                    break;
                default:
                    Globals.gameState = "menu";
                    break;
            }
        }
    }
}