using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace one_loop_game
{
    public class Entity
    {
        public EntityType eType;
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Point Point { get { return new Point((int)Position.X / 32, (int)Position.Y / 32); } }
        public float Speed { get; set; } = 96f;
        public Vector2 Direction { get; set; }
        public float Delta { get; set; }
        public Vector2 Size { get; set; } = new Vector2(32);
        public Vector2 SetSize(Vector2 size) { if (Size.X <= 0 && Size.Y <= 0) return new Vector2(Texture.Width, Texture.Height); return Size; }
        public Color BaseColor { get; set; } = Color.White;
        public Color Color { get; set; } = Color.White;
        public bool Visible { get; set; } = true;

        public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); } }
        public int row = 0;
        public int column = 0;
        public int SourceSize = 32;
        public Rectangle SourceRectangle { get { return new Rectangle(column * SourceSize, row * SourceSize, SourceSize, SourceSize); } }
        public Vector2 CenterBox { get { return new Vector2(Position.X + (SourceSize / 2), Position.Y + (SourceSize / 2)); } }

        public float remainingdelay = 1f;
        public float delay = 1f;
        public float delay2 = 2f;


        public virtual void Update(GameTime gameTime, ScreenPlaying playing)
        {
            Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Position += Direction * Speed * Delta;
        }
        public virtual void Draw(SpriteBatch sb)
        {
            //sb.Draw(Texture, Rectangle, SourceRectangle, Color);
        }

        //public Unit GetUnit()
        //{


        //}
    }
}