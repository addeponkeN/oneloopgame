using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace one_loop_game
{
    class Globals
    {
        #region Random
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();
        public static int Random(int min, int max)
        {
            byte[] randomNumber = new byte[1];
            _generator.GetBytes(randomNumber);
            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);
            int range = max - min + 1;
            double randomValueInRange = Math.Floor(multiplier * range);
            return (int)(min + randomValueInRange);
        }
        public static int Random(int max)
        {
            byte[] randomNumber = new byte[1];
            _generator.GetBytes(randomNumber);
            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);
            int range = max - 0 + 1;
            double randomValueInRange = Math.Floor(multiplier * range);
            return (int)(0 + randomValueInRange);
        }
        public static Random rnd = new Random();
        public static int RandomOld(int min, int max)
        {
            return rnd.Next(min, max + 1);
        }
        public static int RandomOld(int max)
        {
            return rnd.Next(0, max + 1);
        }
        #endregion

        #region
        public static int RoundToUp(double input, int roundTo)
        {
            var i = Math.Round(input);
            var output = (((int)i / roundTo) + 1) * roundTo;
            return output;
        }
        public static int RoundToDown(double input, int roundTo)
        {
            var i = Math.Round(input);
            var output = (((int)i / roundTo) - 1) * roundTo;
            return output;
        }
        public static Vector2 FixMpos(Vector2 mpos)
        {
            var x = Math.Round(mpos.X);
            var y = Math.Round(mpos.Y);
            var xx = ((((int)x / 32) + 1) * 32);
            var yy = ((((int)y / 32) + 1) * 32);
            return new Vector2((xx) - 1, (yy) - 1);
        }
        public static Point mPosPoint(Vector2 mpos)
        {
            var x = Math.Round(mpos.X) / 32;
            var y = Math.Round(mpos.Y) / 32;
            var xx = ((((int)x / 32) + 1) * 32);
            var yy = ((((int)y / 32) + 1) * 32);
            return new Point((xx) - 1, (yy) - 1);
        }
        #endregion

        public static int screenX = 1280;
        public static int screenY = 720;
        public static Vector2 screenXY { get { return new Vector2(screenX, screenY); } }
        public static Vector2 screenCenter = new Vector2(screenX / 2, screenY / 2);
        public static Rectangle screenRectangle = new Rectangle(0, 0, screenX, screenY);
        public static Rectangle screenBoundBox = new Rectangle(screenRectangle.X - 32, screenRectangle.Y- 32, screenRectangle.Width + 64, screenRectangle.Height + 64);
        public static Rectangle screenGame { get { return new Rectangle(screenRectangle.X, screenRectangle.Y + 32, screenRectangle.Width, screenRectangle.Height - 192); } }

        public static Rectangle screenTop { get { return new Rectangle(screenRectangle.X, screenRectangle.Y, screenRectangle.Width, 16); } }
        public static Rectangle screenBot { get { return new Rectangle(screenRectangle.X, (screenRectangle.Y + screenY) - 16, screenRectangle.Width, 16); } }
        public static Rectangle screenLeft { get { return new Rectangle(screenRectangle.X, screenRectangle.Y, 16, screenY); } }
        public static Rectangle screenRight { get { return new Rectangle(screenRectangle.X + screenX - 16, screenRectangle.Y, 16, screenY); } }


        public static string gameState = "menu";

        public static bool debug;         // F1 toggle

        public static bool exitGame;

    }
}
