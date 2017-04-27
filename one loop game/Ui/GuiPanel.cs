using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    public class GuiPanel : Sprite
    {

        public GuiPanel(Texture2D texture) : base(texture)
        {

        }
        public GuiPanel(Texture2D texture, Vector2 position, Vector2 size) : base(texture)
        {
            Position = position;
            Size = size;
        }
    }
}
