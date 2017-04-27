using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    public enum TileType
    {
        Grass,
        Water,
        Tree,
        Stone,
        Bush
    }
    public class Tile : AnimationSprite
    {
        public TileType Type { get; set; }

        public Rectangle ScanRectangle { get { return new Rectangle((int)Position.X - 32, (int)Position.Y - 32, (int)Size.X + 64, (int)Size.Y + 64); } }

        Texture2D box;

        //public bool Walkable { get { return Type == TileType.Grass; } }
        public bool Walkable { get; set; }

        public bool explored;
        public int Value;
        public bool targeted;
        public int id;

        public bool Visiblee()
        {
            return Globals.screenRectangle.Intersects(Rectangle);
        }

        //public void CheckBuildingsAndType(Building[] s)
        //{
        //    if (Type == TileType.Grass && s.Any(a => a.Rectangle.Intersects(Rectangle)))
        //        Walkable = false;
        //    else if (Type == TileType.Grass) Walkable = true;
        //    else Walkable = false;
        //}
        //public void CheckTileTypeWalkable()
        //{
        //    if (Type == TileType.Grass)
        //        Walkable = true;
        //    else Walkable = false;
        //}

        public bool BuildingInterect(Building[] b)
        {


            return true;
        }

        public Tile(TileType type, Texture2D texture, Vector2 position, Texture2D box) : base(texture, true)
        {
            Type = type;
            Texture = texture;
            Position = position;
            Visible = false;
            this.box = box;
            Color = new Color(Globals.Random(215, 225), Globals.Random(215, 225), Globals.Random(215, 225));

            switch (Type)
            {
                case TileType.Grass:
                    column = 0; row = 0;

                    break;
                case TileType.Water:
                    column = 2; row = 0;

                    break;
                case TileType.Tree:
                    column = 0; row = 1;
                    SourceSize = 32;
                    Value = 50;

                    break;
                case TileType.Stone:
                    Value = 200;
                    column = 1; row = 1;

                    break;
                case TileType.Bush:
                    Value = 100;
                    column = 2; row = 1;

                    break;
            }
        }

        public TileType ChangeTileTo(TileType type)
        {
            return type;
        }

        public void Update(GameTime gameTime, ScreenPlaying playing)
        {
            // fix also alter
            if (Value <= 0 && Type != TileType.Grass && Type != TileType.Water)
            {
                playing.tileManager.ChangeTileType(Position, TileType.Grass, playing);
                playing.CheckEverything();
                //playing.InitMapNodes();
                //Console.WriteLine(Point);
            }

            //if(new Point((int)Globals.FixMpos(Input.mPos).X / 32, (int)Globals.FixMpos(Input.mPos).Y / 32) == Point && Input.KeyClick(Keys.D))
                //Value -= 49;

            // fix l8r

            //switch (Type)
            //{
            //    case TileType.Grass: column = 0; row = 0; break;
            //    case TileType.Water: column = 2; row = 0; break;
            //    case TileType.Tree: column = 0; row = 1; break;
            //    case TileType.Stone: column = 1; row = 1; break;
            //    case TileType.Bush: column = 2; row = 1; break;
            //}
        }
        public void GetTile(TileType t)
        {
            switch (t)
            {
                case TileType.Grass:
                    column = 0; row = 0;
                    Value = 0;

                    break;
                case TileType.Water:
                    column = 2; row = 0;
                    Value = 0;

                    break;
                case TileType.Tree:
                    column = 0; row = 1;
                    SourceSize = 32;
                    Value = 100;

                    break;
                case TileType.Stone:
                    Value = 200;
                    column = 1; row = 1;

                    break;
                case TileType.Bush:
                    Value = 100;
                    column = 2; row = 1;

                    break;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            //if (targeted)
                //sb.Draw(box, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), Color.Black);
            base.Draw(sb);
        }
    }
}
