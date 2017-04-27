using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace one_loop_game
{
    class Properties
    {
        public bool Exist { get; set; } = true;
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 SpeedVector { get; set; }
        public float Speed { get; set; }
        public Vector2 Direction { get; set; }
        public float Rotation { get; set; }
        public float Delta { get; set; }
        public Vector2 Size { get; set; }
        public Color Color { get; set; }

        public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); } }
        public Vector2 CenterBox { get { return new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2)); } }
    }
}
