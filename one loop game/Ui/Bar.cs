using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    public class Bar
    {

        public Texture2D TextureBox { get; set; }

        public int OutlineSize { get; set; } = 1;
        public Color OutlineColor { get; set; } = Color.Black;
        public Rectangle OutlineRectangle
        {
            get
            {
                return new Rectangle(
                    (int)NewPosition.X - OutlineSize,
                    (int)NewPosition.Y - OutlineSize,
                    (int)Size.X + (OutlineSize * 2),
                    (int)Size.Y + (OutlineSize * 2));
            }
        }

        public Color HpMaxColor { get; set; } = Color.DarkRed;
        public Rectangle HpMaxRectangle
        {
            get
            {
                return new Rectangle(
                    (int)NewPosition.X,
                    (int)NewPosition.Y,
                    (int)Size.X,
                    (int)Size.Y);
            }
        }

        public Color HpColor { get; set; } = Color.ForestGreen;
        public Rectangle HpRectangle
        {
            get
            {
                return new Rectangle(
                    (int)NewPosition.X,
                    (int)NewPosition.Y,
                    (int)Percent,
                    (int)Size.Y);
            }
        }

        public Vector2 Size { get { return new Vector2(BarWidth, BarHeight); } }

        public double Min;
        public double MinMax;
        public int BarWidth;
        public int BarHeight = 4;

        public int distanceBetweenObjectY = 2;

        public Vector2 NewPosition { get { return new Vector2(Position.X, Position.Y - ((Size.Y * distanceBetweenObjectY) + OutlineSize)); } }
        public Vector2 Position;
        public double Percent;
        public double PercentText;

        public bool OutlineActive = true;
        public bool HpMaxActive = true;

        //{
        //    get { return (Min / MinMax) * BarWidth; }
        //}

        public Bar(Texture2D box, Vector2 position, Vector2 size)
        {
            TextureBox = box;
            Position = position;
            Position = new Vector2(position.X, position.Y - (Size.Y * distanceBetweenObjectY));
        }
        public Vector2 UpdatePosition(Vector2 position)
        {
            return Position = position;
        }
        public double UpdatePercent(double min, double minMax, int barSizeOrPercent)
        {
            BarWidth = barSizeOrPercent;
            Percent = (min / minMax) * barSizeOrPercent;
            PercentText = (min / minMax) * 100;
            return Percent;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (OutlineActive)
                spriteBatch.Draw(TextureBox, OutlineRectangle, OutlineColor);
            if (HpMaxActive)
                spriteBatch.Draw(TextureBox, HpMaxRectangle, HpMaxColor);
            spriteBatch.Draw(TextureBox, HpRectangle, HpColor);
        }
    }
}