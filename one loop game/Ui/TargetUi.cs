using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    public enum TBType
    {
        None,

        TownCenter,
        House,
        Storage,
        Barracks,



        Worker,


    }
    public class TargetButton : Sprite
    {
        public TBType Type;
        Button button;

        public Unit unit;
        public Building building;

        public bool locked;
        public int cooldown;

        public TargetButton(Texture2D texture, Vector2 position, int row, int column, int sourceSize, TBType type) : base(texture)
        {
            this.row = row;
            this.column = column;
            SourceSize = sourceSize;
            Position = position;
            Type = type;
            Size = new Vector2(48);
            button = new Button(Size, Position, Color.White, Texture, Color.White, Size);
            button.ifOutline = false;
        }
        public void Update(GameTime gt)
        {
            button.Update(gt);
        }

        public void DrawTB(SpriteBatch sb, TBType type)
        {
            if (Type == type)
                sb.Draw(button.textureInside, Rectangle, SourceRectangle, Color);
        }
    }
}
