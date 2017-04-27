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
    public class Player
    {

        public Building bTemplate;
        public bool collideTile;
        public bool collideBuilding;

        public Rectangle targetRectangle = new Rectangle();
        Texture2D box, boxbox;

        #region game stats
        public int wood, food, stone, currentUnits, currentBuildings, maxUnits, maxBuildings;
        #endregion        

        public bool gotTarget;

        public Player(Texture2D box, Texture2D boxbox)
        {
            wood = 50;
            food = 50;
            stone = 0;
            this.box = box;
            this.boxbox = boxbox;
        }
        public Building GetTemplate()
        {
            return bTemplate;
        }
        public double Distance(double x)
        {
            return x * x;
        }
        public void Update(GameTime gameTime, ScreenPlaying p)
        {
            if (p.gui.currentUnit != null)
                if (p.gui.currentUnit.Type == UnitType.Worker)
                {
                    if (Input.KeyClick(Keys.W))
                        bTemplate = new Building(BuildingType.TownCenter, p.buildingSheet96, Input.mPos, p.box, p.boxbox, true) { Color = new Color(230, 230, 230, 0.75f) };
                    if (Input.KeyClick(Keys.Q))
                        bTemplate = new Building(BuildingType.House, p.buildingSheet64, Input.mPos, p.box, p.boxbox, true) { Color = new Color(230, 230, 230, 0.75f) };
                    if (Input.KeyClick(Keys.E))
                        bTemplate = new Building(BuildingType.Storage, p.buildingSheet64, Input.mPos, p.box, p.boxbox, true) { Color = new Color(230, 230, 230, 0.75f) };
                    if (Input.KeyClick(Keys.R))
                        bTemplate = new Building(BuildingType.Barracks, p.buildingSheet96, Input.mPos, p.box, p.boxbox, true) { Color = new Color(230, 230, 230, 0.75f) };
                }
            if (Input.RightClick())
                bTemplate = null;

            if (bTemplate == null)
            {
                if (Input.LeftClick())
                {
                    targetRectangle = new Rectangle((int)Input.mPos.X, (int)Input.mPos.Y, 0, 0);
                }
                if (Input.LeftHold())
                {
                    float disX = 0;
                    float disY = 0;
                    if (Input.mPos.X > targetRectangle.X)
                        disX = Math.Abs(Input.mPos.X - targetRectangle.X);
                    if (Input.mPos.X < targetRectangle.X)
                    {
                        disX = Math.Abs(Input.mPos.X - targetRectangle.X);
                        disX = -disX;
                    }
                    if (Input.mPos.Y > targetRectangle.Y)
                        disY = Math.Abs(Input.mPos.Y - targetRectangle.Y);
                    if (Input.mPos.Y < targetRectangle.Y)
                    {
                        disY = Math.Abs(Input.mPos.Y - targetRectangle.Y);
                        disY = -disY;
                    }

                    var distance = Vector2.Distance(new Vector2(targetRectangle.X, targetRectangle.Y), Input.mPos);
                    targetRectangle = new Rectangle(targetRectangle.X, targetRectangle.Y, (int)disX, (int)disY);
                }
                if (Input.LeftRelease())
                {
                    if (Globals.screenGame.Contains(Input.mPos))
                    {
                        p.gui.ClearTargets();
                        gotTarget = false;
                    }
                    foreach (var u in p.eManager.units)
                        if (targetRectangle.Intersects(u.Rectangle) && Globals.screenGame.Contains(Input.mPos))
                        {
                            u.Targeted = true;
                            p.gui.currentUnit = u;
                            gotTarget = true;
                            p.sm.yes1Instance.Play();
                        }
                        else if ((Globals.screenGame.Contains(Input.mPos) && !targetRectangle.Intersects(u.Rectangle))) u.Targeted = false;
                    if (!gotTarget)
                        foreach (var b in p.eManager.buildings)
                        {
                            if (targetRectangle.Intersects(b.Rectangle) && Globals.screenGame.Contains(Input.mPos))
                            {
                                b.Targeted = true;
                                p.gui.currentBuilding = b;
                                gotTarget = true;
                            }
                            else if ((Globals.screenGame.Contains(Input.mPos) && !targetRectangle.Intersects(b.Rectangle))) b.Targeted = false;
                        }
                    if (!gotTarget)
                    {
                        var X = (Globals.RoundToUp(Input.mPos.X, 32) / 32) - 1;
                        var Y = (Globals.RoundToUp(Input.mPos.Y, 32) / 32) - 1;
                        var tile = p.tileManager.tiles[X, Y];
                        if (tile.Point == new Point((Globals.RoundToUp(Input.mPos.X, 32) / 32) - 1, (Globals.RoundToUp(Input.mPos.Y, 32) / 32) - 1) && tile.Type != TileType.Grass && Globals.screenGame.Contains(Input.mPos))
                        {
                            tile.targeted = true;
                            p.gui.currentTile = tile;
                        }
                        else if ((Globals.screenGame.Contains(Input.mPos) && !targetRectangle.Intersects(tile.Rectangle))) tile.targeted = false;
                    }
                    targetRectangle = new Rectangle();
                }
            }
            if (bTemplate != null)
            {
                bTemplate.Position = new Vector2(Globals.RoundToUp(Input.mPos.X, 32) - bTemplate.Size.X, Globals.RoundToUp(Input.mPos.Y, 32) - bTemplate.Size.Y);
                foreach (var b in p.eManager.list)
                {
                    if (b.Rectangle.Intersects(bTemplate.Rectangle))
                    {
                        collideBuilding = true;
                        break;
                    }
                    else
                        collideBuilding = false;
                }

                foreach (var t in p.tileManager.tiles)
                {
                    if (t.Rectangle.Intersects(bTemplate.Rectangle) && !t.Walkable)
                    {
                        collideTile = true;
                        break;
                    }
                    else
                        collideTile = false;
                }

                if (collideBuilding || collideTile)
                    bTemplate.Color = Color.Red;
                else if (!collideTile && !collideTile)
                    bTemplate.Color = Color.White;

                if (Input.LeftRelease() && !collideTile && !collideBuilding && Globals.screenGame.Contains(Input.mPos))
                {
                    BuildBuilding(p);
                }
            }

            currentUnits = p.eManager.units.Count;
            currentBuildings = p.eManager.buildings.Count;
        }
        public void UpdateBuildingAction(ScreenPlaying p)
        {
            var hh = p.eManager.buildings.Where(h => h.Type == BuildingType.House).Count();
            var tt = p.eManager.buildings.Where(h => h.Type == BuildingType.TownCenter).Count();
            maxUnits = (hh * 3) + (tt * 5);
        }

        public void BuildBuilding(ScreenPlaying p)
        {
            if (bTemplate.costWood <= wood && bTemplate.costStone <= stone)
            {
                var b = new Building(bTemplate.Type, bTemplate.Texture, bTemplate.Position, p.box, p.boxbox, false);
                p.eManager.AddBuilding(b, p);
                for (int i = 0; i < p.eManager.units.Count; i++)
                {
                    var u = p.eManager.units[i];
                    if (u.Targeted)
                        u.Build(b, p);
                }
                wood -= bTemplate.costWood;
                stone -= bTemplate.costStone;
                bTemplate = null;
            }
            else
            {
                if (bTemplate.costWood > wood)
                    Console.WriteLine("not enough wood");
                if (bTemplate.costStone > stone)
                    Console.WriteLine("not enough stone");
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (bTemplate != null)
            {
                bTemplate.Draw(sb);
                sb.Draw(box, bTemplate.Rectangle, new Color(50, 50, 50, 0.1f));
            }

            if (targetRectangle != null)
            {
                sb.Draw(box, targetRectangle, new Color(35, 35, 35, 0.25f));
                sb.Draw(boxbox, new Rectangle(targetRectangle.X - 1, targetRectangle.Y - 1, targetRectangle.Width + 2, targetRectangle.Height + 2), new Color(100, 100, 100, .25f));
            }
        }
    }
}
