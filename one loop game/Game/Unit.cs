using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace one_loop_game
{
    public enum UnitType
    {
        Worker,
        Warrior,
        Archer
    }
    public enum UState
    {
        Idle,
        Walking,
        WalkingToGather,

        Gathering,
        Delivering,

        Building,
        WalkToBuilding,
    }

    public enum ResourceType
    {
        Wood,
        Food,
        Stone,

    }

    public class Unit : Entity
    {
        public UnitType Type;
        public UState State;
        public ResourceType RType;
        public bool Exist { get { return hp > 0; } }

        public AnimationSprite animation;

        public Point endPoint;
        public Bar hpBar;

        Tile tile;
        Building building;

        public Rectangle TargetBox { get { return new Rectangle((int)Position.X - 1, (int)Position.Y - 1, 34, 34); } }
        public Rectangle GatherRadius { get { return new Rectangle((int)Position.X - 2, (int)Position.Y - 2, (int)Size.X + 4, (int)Size.Y + 4); } }
        public Rectangle VisionRadius { get { return new Rectangle((int)Position.X - visionRange, (int)Position.Y - visionRange, (int)Size.X + (visionRange * 2), (int)Size.Y + (visionRange * 2)); } }

        List<Vector2> Waypoints { get; set; }
        public int StopAt = 1;
        public float DistanceToWaypoint { get { return Vector2.Distance(Waypoints.First(), Position); } }
        public bool AtWaypoint { get { return DistanceToWaypoint < StopAt; } }

        Texture2D box, boxbox;

        #region Gamestats
        public double hp, maxHp, damage;
        public int bagSpace, maxBagSpace, gatherSpeed, costWood, costFood, costStone;
        public int visionRange;
        #endregion
        #region aniamtion
        int up, down, left, right;
        #endregion

        public bool Targeted;

        public int animationTimer;
        public int AnimationSpeed = 15;
        public bool animate = true;
        public int updatePathTimer;

        public Unit(UnitType type, Texture2D texture, Vector2 spawnPosition, Texture2D box, Texture2D boxbox)
        {
            this.box = box;
            this.boxbox = boxbox;
            Type = type;
            Texture = texture;
            Position = spawnPosition;
            Waypoints = new List<Vector2>();
            animate = false;

            switch (Type)
            {
                case UnitType.Worker:
                    maxHp = 10;
                    hp = maxHp;
                    damage = 1;
                    Speed = 96f;
                    gatherSpeed = 1;
                    maxBagSpace = 10;
                    visionRange = 128;
                    costFood = 50;

                    up = 1;
                    down = 0;
                    left = 3;
                    right = 2;

                    break;
                case UnitType.Warrior:
                    maxHp = 25;
                    hp = maxHp;
                    damage = 6;
                    Speed = 96f;
                    visionRange = 256;
                    costFood = 75;
                    costWood = 50;

                    up = 5;
                    down = 4;
                    left = 7;
                    right = 6;

                    break;
                case UnitType.Archer:

                    break;
                default:
                    break;
            }
            animation = new AnimationSprite(Texture, true);
            hpBar = new Bar(box, Position, Size);
        }
        public override void Update(GameTime gameTime, ScreenPlaying playing)
        {
            hpBar.UpdatePosition(Position);
            hpBar.UpdatePercent(hp, maxHp, (int)Size.X);

            //animation.Animation(row, column, up, down, left, right, Direction, 15);
            //if (Targeted)
            //    Console.WriteLine(column);

            #region animation
            if (animate)
            {
                animationTimer--;
                if (Direction == new Vector2(0))
                    column = 0;
                if (Direction.Y > 0)
                    row = down;
                if (Direction.Y < 0)
                    row = up;
                if (Direction.X > 0)
                    row = right;
                if (Direction.X < 0)
                    row = left;
                if (animationTimer < 0)
                {
                    column++;
                    if (column > 3)
                        column = 0;
                    animationTimer = AnimationSpeed;
                }
            }
            else column = 0;
            #endregion






            if (State == UState.Walking || State == UState.WalkingToGather || State == UState.WalkToBuilding || State == UState.Delivering)
                animate = true;
            else animate = false;

            #region manual walk stuff
            if (Targeted)
            {
                if (Input.RightClick() && Globals.screenGame.Contains(Input.mPos) && playing.player.bTemplate == null)
                {
                    bool foundBuilding = false;
                    foreach (var b in playing.eManager.buildings)
                        if (b.Rectangle.Contains(Input.mPos))
                        {
                            if (!b.built)
                                Build(b, playing);
                            else if ((b.Type == BuildingType.TownCenter || b.Type == BuildingType.Storage) && bagSpace > 0)
                                Deliver(b, playing);
                            else Walk(playing);
                            foundBuilding = true;
                        }
                    if (!foundBuilding)
                    {
                        bool foundGather = false;
                        var fix = new Point((int)Globals.FixMpos(Input.mPos).X / 32, (int)Globals.FixMpos(Input.mPos).Y / 32);
                        var x = fix.X; var y = fix.Y;
                        if (playing.tileManager.tiles[x, y].Point == fix && !playing.tileManager.tiles[x, y].Walkable)
                        {
                            Gather(playing.tileManager.tiles[x, y], playing);
                            foundGather = true;
                        }
                        if (!foundGather)
                            Walk(playing);
                    }
                }
            }

            #endregion

            #region unit states

            switch (State)
            {
                case UState.Idle:

                    break;
                case UState.Walking:
                    if (Waypoints.Count < 1)
                        State = UState.Idle;
                    break;
                case UState.WalkingToGather:

                    if (GatherRadius.Intersects(tile.Rectangle))
                        Gather(tile, playing);

                    break;
                case UState.Gathering:
                    if (!GatherRadius.Intersects(tile.Rectangle))
                        WalkToGather(tile, playing);
                    else
                    {
                        remainingdelay -= Delta;
                        if (remainingdelay < 0 && tile.Value > 0)
                        {
                            switch (tile.Type)
                            {
                                case TileType.Tree:
                                    if (RType != ResourceType.Wood)
                                        bagSpace = 0;
                                    RType = ResourceType.Wood;
                                    bagSpace += gatherSpeed;
                                    tile.Value--;

                                    break;
                                case TileType.Stone:
                                    if (RType != ResourceType.Stone)
                                        bagSpace = 0;
                                    RType = ResourceType.Stone;
                                    bagSpace += gatherSpeed;
                                    tile.Value--;

                                    break;
                                case TileType.Bush:
                                    if (RType != ResourceType.Food)
                                        bagSpace = 0;
                                    RType = ResourceType.Food;
                                    bagSpace += gatherSpeed;
                                    tile.Value--;

                                    break;
                                default:
                                    break;
                            }
                            if (Globals.screenGame.Contains(Position))
                            {
                                var rnd = Globals.RandomOld(1);
                                switch (rnd)
                                {
                                    case 0:
                                        playing.sm.chop1Instance.Play();
                                        break;
                                    case 1:
                                        playing.sm.chop3Instance.Play();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            remainingdelay = delay2;
                        }
                        if (tile.Value < 1 && tile.Type != TileType.Grass)
                        {
                            bool foundNew = false;
                            var newTile = tile;
                            int maxY, maxX;
                            if (newTile.Point.X > 6 && newTile.Point.X < playing.tileManager.mapX - 6)
                                maxX = -5;
                            else maxX = 0;
                            if (newTile.Point.Y > 6 && newTile.Point.Y < playing.tileManager.mapY - 6)
                                maxY = -5;
                            else maxY = 0;
                            float shortest = float.MaxValue;
                            for (int y = newTile.Point.Y + maxY; y < newTile.Point.Y + 10; y++)
                            {
                                for (int x = newTile.Point.X + maxX; x < newTile.Point.X + 10; x++)
                                {
                                    var tempTile = playing.tileManager.tiles[x, y];
                                    if (newTile.Type == tempTile.Type && tempTile.Point != tile.Point)
                                    {
                                        var distance = Vector2.Distance(new Vector2(tempTile.Point.X, tempTile.Point.Y), new Vector2(Point.X, Point.Y));
                                        if (shortest > distance)
                                        {
                                            shortest = distance;
                                            newTile = tempTile;
                                            foundNew = true;
                                        }
                                    }
                                }
                            }
                            if (foundNew)
                            {
                                playing.CheckEverything();
                                tile = newTile;
                                if (tile.Value > 0)
                                    Gather(tile, playing);
                            }
                            else
                            {
                                float shortestBuilding = float.MaxValue;
                                var boop = playing.eManager.buildings.Where(g => (g.Type == BuildingType.TownCenter || g.Type == BuildingType.Storage) && g.built);
                                var boopC = boop.Count();
                                foreach (var b in boop)
                                {
                                    for (int i = 0; i < boopC; i++)
                                    {
                                        var distance = Vector2.Distance(new Vector2(b.Point.X, b.Point.Y), new Vector2(Point.X, Point.Y));
                                        if (shortestBuilding > distance)
                                        {
                                            building = b;
                                            shortestBuilding = distance;
                                        }
                                    }
                                }
                                Deliver(building, playing);
                            }
                        }
                    }

                    if (bagSpace >= maxBagSpace)
                    {
                        float shortest = float.MaxValue;
                        var boop = playing.eManager.buildings.Where(g => (g.Type == BuildingType.TownCenter || g.Type == BuildingType.Storage) && g.built);
                        var boopC = boop.Count();
                        foreach (var b in boop)
                        {
                            for (int i = 0; i < boopC; i++)
                            {
                                var distance = Vector2.Distance(new Vector2(b.Point.X, b.Point.Y), new Vector2(Point.X, Point.Y));
                                if (shortest > distance)
                                {
                                    building = b;
                                    shortest = distance;
                                }
                            }
                        }
                        Deliver(building, playing);
                    }

                    break;
                case UState.Delivering:
                    if (GatherRadius.Intersects(building.Rectangle))
                    {
                        switch (RType)
                        {
                            case ResourceType.Wood:
                                playing.player.wood += bagSpace;
                                break;
                            case ResourceType.Stone:
                                playing.player.stone += bagSpace;
                                break;
                            case ResourceType.Food:
                                playing.player.food += bagSpace;
                                break;
                            default:
                                break;
                        }
                        bagSpace = 0;
                        if (tile.Value > 0)
                        {
                            Gather(tile, playing);
                        }
                        else if (tile.Value < 1)
                        {
                            //playing.CheckEverything();
                            //var newTile = tile;
                            //int maxY, maxX;
                            //if (newTile.Point.X > 6 && newTile.Point.X < playing.tileManager.mapX - 6)
                            //    maxX = -5;
                            //else maxX = 0;
                            //if (newTile.Point.Y > 6 && newTile.Point.Y < playing.tileManager.mapY - 6)
                            //    maxY = -5;
                            //else maxY = 0;
                            //float shortest = float.MaxValue;
                            //for (int y = newTile.Point.Y + maxY; y < newTile.Point.Y + 10; y++)
                            //{
                            //    for (int x = newTile.Point.X + maxX; x < newTile.Point.X + 10; x++)
                            //    {
                            //        var tempTile = playing.tileManager.tiles[x, y];
                            //        if (newTile.Type == tempTile.Type && tempTile.Point != tile.Point)
                            //        {
                            //            var distance = Vector2.Distance(new Vector2(tempTile.Point.X, tempTile.Point.Y), new Vector2(newTile.Point.X, newTile.Point.Y));
                            //            if (shortest > distance)
                            //            {
                            //                shortest = distance;
                            //                newTile = tempTile;
                            //            }
                            //        }
                            //    }
                            //}
                            //tile = newTile;
                            //Gather(tile, playing);
                            Walker(tile.Position, playing);
                        }
                    }
                    break;

                case UState.Building:
                    if (!GatherRadius.Intersects(building.Rectangle))
                        WalkToBuilding(building, playing);
                    else
                    {
                        Waypoints.Clear();
                        remainingdelay -= Delta;
                        if (remainingdelay < 0)
                        {
                            building.buildProgress++;
                            building.hp += building.maxHp / building.buildTime;
                            remainingdelay = delay2;
                            if (building.buildProgress >= building.buildTime)
                            {
                                playing.player.UpdateBuildingAction(playing);
                                foreach (var b in playing.eManager.buildings)
                                    if (!b.built && VisionRadius.Intersects(b.Rectangle))
                                    {
                                        Build(b, playing);
                                        break;
                                    }
                                    else
                                        State = UState.Idle;
                            }
                            if (Globals.screenGame.Contains(Position))
                                playing.sm.hammer1Instance.Play();
                        }
                    }
                    break;
                case UState.WalkToBuilding:
                    if (GatherRadius.Intersects(building.Rectangle))
                        State = UState.Building;

                    break;
                default:

                    break;
            }

            #endregion

            if (Waypoints.Count < 1)
                Waypoints.Clear();
            if (Targeted)
                Color = new Color(200, 200, 200);
            else Color = Color.White;

            UpdateMovement(gameTime, playing);
            base.Update(gameTime, playing);
        }

        #region Enter State Methods
        public void EnterIdle()
        {
            State = UState.Idle;

        }
        public void Walk(ScreenPlaying p)
        {
            State = UState.Walking;
            WalkTo(Input.mPos, p);
        }
        public void Walker(Vector2 pos, ScreenPlaying p)
        {
            State = UState.Walking;
            WalkTo(Input.mPos, p);
        }
        public void Gather(Tile t, ScreenPlaying p)
        {
            tile = t;
            State = UState.Gathering;
        }
        public void WalkToGather(Tile t, ScreenPlaying p)
        {
            State = UState.WalkingToGather;
            WalkTo(t.Position, p);
        }
        public void Deliver(Building b, ScreenPlaying p)
        {
            State = UState.Delivering;
            building = b;
            WalkTo(b.CenterBox, p);
        }
        public void Build(Building b, ScreenPlaying p)
        {
            building = b;
            State = UState.Building;
        }
        public void WalkToBuilding(Building b, ScreenPlaying p)
        {
            State = UState.WalkToBuilding;
            WalkTo(b.CenterBox, p);
        }

        #endregion

        public void UpdateMovement(GameTime gameTime, ScreenPlaying p)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Waypoints.Count > 0)
            {
                foreach (var b in p.eManager.buildings)
                    CollideWith(b.Rectangle, p);
                if (AtWaypoint)
                {
                    Waypoints.RemoveAt(0);
                    //if (Waypoints.Count > 1)
                    //{
                    //RefreshPath(p);
                    //}
                }
                else
                {
                    var dir = -(Position - Waypoints.First());
                    dir.Normalize();
                    Direction = dir;
                    Position += (delta * Speed * Direction);
                }
            }
        }

        //not work----------
        public void AddWalkTo(Vector2 pos, ScreenPlaying p)
        {
            var startPoint = endPoint;
            //if (Waypoints.Count > 2)
            //Waypoints.RemoveRange(2, Waypoints.Count - 2);

            Position = new Vector2((int)Math.Round(Position.X), (int)Math.Round(Position.Y));
            endPoint = new Point((int)pos.X / 32, (int)pos.Y / 32);

            //p.finder.InitWalkNodes(p);
            List<Vector2> path = p.finder.FindPath(startPoint, endPoint);
            var waypoints = path;

            if (waypoints.Count > 0)
                Waypoints.InsertRange(0, waypoints);

            Position = new Vector2((int)Math.Round(Position.X), (int)Math.Round(Position.Y));
        }
        //------------
        public void CollideWith(Rectangle collideRectangle, ScreenPlaying playing)
        {
            collideRectangle = new Rectangle(collideRectangle.X + 2, collideRectangle.Y + 2, collideRectangle.Width - 4, collideRectangle.Height - 4);

            if (Rectangle.TouchTopOf(collideRectangle))
            {
                updatePathTimer++;
                Position = new Vector2(Position.X, collideRectangle.Y - Rectangle.Height - 2);
                if (updatePathTimer > 50)
                {
                    RefreshPath(playing);
                    //Console.WriteLine("top ref");

                    updatePathTimer = 0;
                }
            }
            if (Rectangle.TouchLeftOf(collideRectangle))
            {
                updatePathTimer++;
                Position = new Vector2(collideRectangle.X - Rectangle.Width - 2, Position.Y);
                if (updatePathTimer > 50)
                {
                    RefreshPath(playing);
                    //Console.WriteLine("left ref");
                    updatePathTimer = 0;
                }
            }
            if (Rectangle.TouchRightOf(collideRectangle))
            {
                updatePathTimer++;
                Position = new Vector2(collideRectangle.X + collideRectangle.Width + 2, Position.Y);
                if (updatePathTimer > 50)
                {
                    RefreshPath(playing);
                    //Console.WriteLine("right ref");
                    updatePathTimer = 0;
                }
            }
            if (Rectangle.TouchBottomOf(collideRectangle))
            {
                updatePathTimer++;
                Position = new Vector2(Position.X, collideRectangle.Y + collideRectangle.Height + 2);
                if (updatePathTimer > 50)
                {
                    RefreshPath(playing);
                    //Console.WriteLine("bot ref");
                    updatePathTimer = 0;
                }
            }

            //if (Position.X < 0) Position = new Vector2(0, Position.Y);
            //if (Position.X > Globals.screenX - Rectangle.Width) Position = new Vector2(Globals.screenX - Rectangle.Width, Position.Y);
            //if (Position.Y < 0) Position = new Vector2(Position.X, 0);
            //if (Position.Y > Globals.screenY - Rectangle.Height) Position = new Vector2(Position.X, Globals.screenY - Rectangle.Height);
        }
        public void WalkTo(Vector2 position, ScreenPlaying p)
        {
            if (Waypoints.Count > 1)
                Waypoints.RemoveRange(1, Waypoints.Count - 1);
            //if (Waypoints.Count > 0)
            //Waypoints.Clear();

            Position = new Vector2((int)Math.Round(Position.X), (int)Math.Round(Position.Y));

            endPoint = new Point((int)position.X / 32, (int)position.Y / 32);
            if (!p.tileManager.tiles[endPoint.X, endPoint.Y].Walkable)
                p.finder.InitWalkNodes(p);

            List<Vector2> path = p.finder.FindPath(new Point((int)Position.X / 32, (int)Position.Y / 32), endPoint);

            var waypoints = path;
            if (waypoints.Count > 0)
                Waypoints.AddRange(waypoints);

            Position = new Vector2((int)Math.Round(Position.X), (int)Math.Round(Position.Y));
        }
        public void RefreshPath(ScreenPlaying p)
        {
            if (Waypoints.Count > 1)
                Waypoints.Clear();

            Position = new Vector2((int)Math.Round(Position.X), (int)Math.Round(Position.Y));

            List<Vector2> path = p.finder.FindPath(Point, endPoint);

            var waypoints = path;

            if (waypoints.Count > 0)
                Waypoints.AddRange(waypoints);

            Position = new Vector2((int)Math.Round(Position.X), (int)Math.Round(Position.Y));
        }
        public override void Draw(SpriteBatch sb)
        {
            if (Targeted)
            {
                sb.Draw(boxbox, TargetBox, new Color(50, 50, 50));
                hpBar.Draw(sb);
                //foreach (var w in Waypoints)
                //{
                //    sb.Draw(box, new Vector2(w.X, w.Y), new Color(Color.Black, 0.25f));
                //}
            }
            sb.Draw(Texture, Rectangle, SourceRectangle, Color, 0f, Vector2.Zero, SpriteEffects.None, 0f);

            //if (Waypoints.Count > 0)
                //sb.Draw(box, new Rectangle(new Point((endPoint.X * 32) + 16, (endPoint.Y * 32) + 16), new Point(8, 8)), Color.Red);

            base.Draw(sb);
        }
    }
}
