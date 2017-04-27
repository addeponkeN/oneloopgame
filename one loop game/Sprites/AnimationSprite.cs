using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    public class AnimationSprite
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Point Point { get { return new Point((int)Position.X / 32, (int)Position.Y / 32); } }
        public float Speed { get; set; } = 50f;
        public Vector2 Direction { get; set; }
        public float Delta { get; set; }
        public Vector2 Size { get; set; } = new Vector2(32);
        public Color BaseColor { get; set; } = Color.White;
        public Color Color { get; set; } = Color.White;
        public bool Visible { get; set; } = true;
        public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); } }
        public Vector2 CenterBox { get { return new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2)); } }
        public bool SourceRec;
        public int SourceSize = 32;
        public Rectangle SourceRectangle { get { return new Rectangle(column * SourceSize, row * SourceSize, SourceSize, SourceSize); } }
        public Rectangle DefaultSourceRectangle { get { return new Rectangle(0 * 32, 0 * 32, 32, 32); } }

        public int row = 0;
        public int column = 0;

        public int animationTimer = 0;
        public int AnimationSpeed { get; set; }
        public bool animate;

        public int[] Test { get; set; }

        //private float remainingdelay = 1f;
        //private float delay = 1f;           

        public AnimationSprite(Texture2D texture, bool sRec) : base()
        {
            Texture = texture;
            SourceRec = sRec;
            //Animation();

            int[] walkUp = new int[3] { 1, 2, 3 };

        }
        public void UpdateAnimation(GameTime gameTime, int a)
        {
            if (animate)
            {
                animationTimer++;
                if (Direction == new Vector2(0))
                    row = 0;

                if (Direction.Y > 0)
                    column = 0;
                if (Direction.Y < 0)
                    column = 1;
                if (Direction.X > 0)
                    column = 2;
                if (Direction.X < 0)
                    column = 3;
                if (animationTimer > AnimationSpeed)
                {
                    row++;
                    if (row > 3)
                        row = 0;
                    animationTimer = 0;
                }
            }
            else row = 0;
        }

        public void Animation(int row, int column, int up, int down, int left, int right, Vector2 direction, int animationSpeed)
        {
            if (direction == new Vector2(0))
                column = 0;
            animationTimer--;
            if (direction.Y > 0)
            {
                row = down;
            }
            if (direction.Y < 0)
            {
                row = up;
            }
            if (direction.X > 0)
            {
                row = right;
            }
            if (direction.X < 0)
            {
                row = left;
            }
            if (animationTimer < 0)
            {
                column++;
                if (column > 3)
                    column = 0;
                animationTimer = animationSpeed;
            }
            //Console.WriteLine(animationTimer);

            //if (animate)
            //{
            //    animationTimer--;
            //    if (Direction == new Vector2(0))
            //        column = 0;
            //    if (Direction.Y > 0)
            //        row = 0;
            //    if (Direction.Y < 0)
            //        row = 1;
            //    if (Direction.X > 0)
            //        row = 2;
            //    if (Direction.X < 0)
            //        row = 3;
            //    if (animationTimer < 0)
            //    {
            //        column++;
            //        if (column > 3)
            //            column = 0;
            //        animationTimer = AnimationSpeed;
            //    }
            //}
            //else column = 0;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (SourceRec)
                sb.Draw(Texture, Rectangle, SourceRectangle, Color);
            else
                sb.Draw(Texture, Rectangle, Color);
        }
    }
}