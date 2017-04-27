using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    public class TileManager
    {

        public List<Tile> list = new List<Tile>();
        public Tile[,] tiles;

        public int[,] currentMap;

        ChunkGenerator chunk;

        public int mapX, mapY;
        //int tileSize = 32;

        int smoothTimes;
        //int waterHeight = 1;

        int nextId;
        int timer;

        Rectangle villageSpawn;

        Texture2D TileSheet;
        SpriteFont font;

        public void GenerateMap()
        {
            int smooth = 0;
            float startFill = .3f;
            currentMap = MapGenerator.CreateMap(mapX, mapY, smooth, startFill);
            smoothTimes = smooth;
        }
        public TileManager(Texture2D tileSheet, SpriteFont font, int mapX, int mapY)
        {
            TileSheet = tileSheet;
            this.mapX = mapX;
            this.mapY = mapY;
            tiles = new Tile[mapX, mapY];
            this.font = font;
        }
        public void CheckAllBuildingsIfOnTileAndInitNodes(Building[] bs, ScreenPlaying p)
        {
            foreach (var b in bs)
            {
                var X = b.Point.X;
                var Y = b.Point.Y;
                var sizeX = (b.Size.X / 32);
                var sizeY = (b.Size.Y / 32);
                for (int y = 0; y < sizeY; y++)
                    for (int x = 0; x < sizeX; x++)
                    {
                        if (new Point(b.Point.X + x, b.Point.Y + y) == tiles[X + x, Y + y].Point)
                            tiles[X + x, Y + y].Walkable = false;
                        else if (tiles[X + x, Y + y].Type == TileType.Grass)
                            tiles[X + x, Y + y].Walkable = true;
                        else tiles[X + x, Y + y].Walkable = false;

                    }
                //p.finder.UpdatePathfind(new Point(X, Y), 5, p.tileManager);
                //p.finder.InitArea(new Point((int)sizeX, (int)sizeY), new Point(X, Y), tiles);
            }
            p.finder.InitWalkNodes(p);
            Console.WriteLine("Checked all buildings");
        }
        public void CheckABuildingIfOnTileAndInitNodes(Building b, ScreenPlaying p)
        {
            var X = b.Point.X;
            var Y = b.Point.Y;
            var sizeX = (b.Size.X / 32);
            var sizeY = (b.Size.Y / 32);
            for (int y = 0; y < sizeY; y++)
                for (int x = 0; x < sizeX; x++)
                {
                    if (new Point(b.Point.X + x, b.Point.Y + y) == tiles[X + x, Y + y].Point)
                        tiles[X + x, Y + y].Walkable = false;
                    else if (tiles[X + x, Y + y].Type == TileType.Grass)
                        tiles[X + x, Y + y].Walkable = true;
                    else tiles[X + x, Y + y].Walkable = false;
                }
            //p.finder.InitArea(new Point((int)sizeX, (int)sizeY), new Point(X, Y), tiles);
            //p.finder.UpdatePathfind(new Point(X, Y), 5, p.tileManager);
            p.finder.InitWalkNodes(p);

            Console.WriteLine("Checked one building");
        }
        public void CheckAllTileTypes(ScreenPlaying p)
        {
            for (int y = 0; y < mapY; y++)
                for (int x = 0; x < mapX; x++)
                    if (tiles[x, y].Type == TileType.Grass)
                        tiles[x, y].Walkable = true;
                    else tiles[x, y].Walkable = false;
            p.finder.InitWalkNodes(p);
        }
        public void CheckATileType(Point pos, ScreenPlaying p)
        {
            var x = pos.X / 32; var y = pos.Y / 32;
            //Console.WriteLine(pos);

            if (tiles[x, y] != null)
                if (tiles[x, y].Type != TileType.Grass)
                {
                    tiles[x, y].Walkable = false;
                }
                else
                {
                    tiles[x, y].Value = 0;
                    tiles[x, y].Walkable = true;
                }

            //Console.WriteLine(pos);
            //p.finder.InitWalkNodes(p);
        }

        public void AddTile(Tile t)
        {
            t.id = nextId;
            nextId++;
            list.Add(t);
        }
        //laggy

        public void GenerateRandomMap(ScreenPlaying p)
        {
            chunk = new ChunkGenerator(p);
            chunk.GenerateChunks();
            chunk.InsertNode(p);
        }
        public void ChangeTileType(Vector2 pos, TileType type, ScreenPlaying p)
        {
            var x = (int)pos.X / 32; var y = (int)pos.Y / 32;
            if (x >= 0 && x < mapX && y >= 0 && y < mapY)
                if (tiles[x, y] != null)
                    if (tiles[x, y].Type != type)
                    {
                        tiles[x, y].Type = type;
                        tiles[x, y].GetTile(tiles[x, y].Type);
                    }
            CheckATileType(new Point(x, y), p);
        }
        public void LoadMap(ScreenPlaying playing)
        {
            for (int y = 0; y < mapY; y++)
            {
                for (int x = 0; x < mapX; x++)
                {
                    AddTile(new Tile(TileType.Grass, TileSheet, new Vector2(x * 32, y * 32), playing.box));
                }
            }
        }
        public Tile GetTile(Point point)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var t = list[i];
                if (t.Type == TileType.Grass)
                    if (point == t.Point)
                        return t;
            }
            return null;
        }
        public void CheckIfTreeAboveAll()
        {
            for (int y = 0; y < mapY; y++)
            {
                for (int x = 0; x < mapX; x++)
                {
                    var t = tiles[x, y];
                    if (t.Point.Y > 1)
                    {
                        var a = tiles[x, y - 1];
                        if (t.Type == TileType.Tree && a.Type == TileType.Tree)
                        {
                            a.row = 2;
                            t.row = 1;
                        }
                        else if (t.Type != TileType.Tree && a.Type == TileType.Tree)
                            a.row = 1;
                    }
                }
            }
        }

        public void Update(GameTime gameTime, ScreenPlaying p)
        {
            timer++;
            //if (timer > 20)
            //{
                foreach (var t in tiles)
                {
                    t.Update(gameTime, p);

                    foreach (var u in p.eManager.units)
                        if (u.VisionRadius.Contains(t.Position))
                            t.explored = true;
                }
                timer = 0;
            //}

            //if (Input.KeyHold(Keys.D1))
            //    ChangeTileType(Globals.FixMpos(Input.mPos), TileType.Grass, p);
            //if (Input.KeyHold(Keys.D2))
            //    ChangeTileType(Globals.FixMpos(Input.mPos), TileType.Tree, p);
            //if (Input.KeyHold(Keys.D3))
            //    ChangeTileType(Globals.FixMpos(Input.mPos), TileType.Water, p);
            //if (Input.KeyHold(Keys.D4))
            //    ChangeTileType(Globals.FixMpos(Input.mPos), TileType.Stone, p);
            //if (Input.KeyHold(Keys.D5))
            //    ChangeTileType(Globals.FixMpos(Input.mPos), TileType.Bush, p);

        }
        public bool CheckIfBuildingTouch(Building[] b, Tile t)
        {
            //var tilesInSight = list.Where(y => y.Rectangle.Intersects(Globals.screenRectangle));

            bool test = b.Any(h => h.Rectangle.Intersects(t.Rectangle));
            test = false;

            return test;
        }
        public void DrawMenu(SpriteBatch sb)
        {
            foreach (var t in tiles)
                t.Draw(sb);
        }
        public void Draw(SpriteBatch sb, Camera cam)
        {
            foreach (var t in tiles)
            {
                if (t.explored)
                    if (t.Visiblee())
                        t.Draw(sb);
            }

        }
        public void DrawString(SpriteBatch sb)
        {
            foreach (var tile in tiles)
            {
                if (tile.Rectangle.Contains(Input.mPos))
                {
                    sb.DrawString(font, $"[type: {tile.Type}], [walkable: {tile.Walkable}], [pos: {tile.Point}]", new Vector2(1, Globals.screenY - 180), Color.Black);
                    sb.DrawString(font, $"[type: {tile.Type}], [walkable: {tile.Walkable}], [pos: {tile.Point}]", new Vector2(0, Globals.screenY - 179), Color.White);
                }
            }
        }
        public void PlaceTiles(int smooth, int waterheight, ScreenPlaying p, bool spawn)
        {
            int units = 1;
            int stones = 20;
            int bushes = 30;
            currentMap = MapGenerator.Smoother(currentMap, smooth);
            for (int y = 0; y < mapY; y++)
            {
                for (int x = 0; x < mapX; x++)
                {
                    var pos = new Vector2(x * 32, y * 32);

                    if (currentMap[x, y] >= 17)
                        tiles[x, y] = new Tile(TileType.Tree, TileSheet, pos, p.box);
                    if (currentMap[x, y] <= 16 && currentMap[x, y] > waterheight)
                        tiles[x, y] = new Tile(TileType.Grass, TileSheet, pos, p.box) { Walkable = true };
                    if (currentMap[x, y] <= waterheight)
                        tiles[x, y] = new Tile(TileType.Water, TileSheet, pos, p.box);
                    if (spawn)
                    {
                        if (currentMap[x, y] < 10 && currentMap[x, y] > 2 && units > 0 && pos.Y > ((mapY / 2) * 32) - 6 && pos.X > ((mapX / 2) * 32) - 6)
                        {
                            villageSpawn = new Rectangle((int)(pos.X) - 64, (int)(pos.Y) - 64, 256, 256);
                            p.eManager.AddUnit(new Unit(UnitType.Worker, p.unitSheet, new Vector2(pos.X + 32, pos.Y + 96), p.box, p.boxbox));
                            p.eManager.AddUnit(new Unit(UnitType.Worker, p.unitSheet, new Vector2(pos.X + 64, pos.Y + 96), p.box, p.boxbox));
                            p.eManager.AddBuildingNotInit(new Building(BuildingType.TownCenter, p.buildingSheet96, pos, p.box, p.boxbox, true));
                            ChangeTileType(new Vector2(pos.X - 64, pos.Y - 96), TileType.Stone, p);
                            ChangeTileType(new Vector2(pos.X + 64, pos.Y - 96), TileType.Bush, p);
                            ChangeTileType(new Vector2(pos.X + 32, pos.Y - 96), TileType.Bush, p);
                            units--;
                        }
                    }
                    //var rnd = Globals.Random(1, 6);
                    //if (!villageSpawn.Contains(pos))
                    //    switch (rnd)
                    //    {
                    //        case 1:
                    //            if (currentMap[x, y] < 50 && currentMap[x, y] > 2 && bushes > 1)
                    //            {
                    //                var nodeSize = Globals.Random(10, 20);
                    //                var posX = Globals.Random(mapX);
                    //                var posY = Globals.Random(mapY);
                    //                for (int i = 0; i < nodeSize; i++)
                    //                {
                    //                    var r = Globals.Random(2);
                    //                    ChangeTileType(new Vector2(pos.X + (r * (i * 32)), pos.Y + (r * (i * 32))), TileType.Bush, p);
                    //                }
                    //                bushes--;
                    //            }
                    //            break;
                    //        case 2:
                    //            if (currentMap[x, y] < 50 && currentMap[x, y] > 2 && stones > 1)
                    //            {
                    //                var nodeSize = Globals.Random(10, 20);
                    //                var posX = Globals.Random(mapX);
                    //                var posY = Globals.Random(mapY);
                    //                for (int i = 0; i < nodeSize; i++)
                    //                {
                    //                    var r = Globals.Random(2);
                    //                    ChangeTileType(new Vector2(pos.X + (r * (i * 32)), pos.Y + (r * (i * 32))), TileType.Stone, p);
                    //                }
                    //                stones--;
                    //            }
                    //            break;
                    //        default:
                    //            break;
                    //    }

                }
            }
            for (int i = 0; i < bushes; i++)
            {
                var posX = Globals.Random(mapX) * 32;
                var posY = Globals.Random(mapY) * 32;
                ChangeTileType(new Vector2(posX, posY), TileType.Bush, p);
            }
            for (int i = 0; i < bushes; i++)
            {
                var posX = Globals.Random(mapX) * 32;
                var posY = Globals.Random(mapY) * 32;
                ChangeTileType(new Vector2(posX, posY), TileType.Stone, p);
            }
            for (int y = villageSpawn.Y; y < villageSpawn.Y + villageSpawn.Height; y++)
            {
                for (int x = villageSpawn.X; x < villageSpawn.X + villageSpawn.Width; x++)
                {
                    ChangeTileType(new Vector2(x, y), TileType.Grass, p);
                }
            }

        }
        public void PlaceMenuTiles(int smooth, int waterheight, Texture2D box, bool generateVillage)
        {
            int stones = 10;
            int bushes = 8;
            currentMap = MapGenerator.Smoother(currentMap, smooth);
            for (int y = 0; y < mapY; y++)
            {
                for (int x = 0; x < mapX; x++)
                {
                    var pos = new Vector2(x * 32, y * 32);

                    if (currentMap[x, y] >= 10)
                        tiles[x, y] = new Tile(TileType.Tree, TileSheet, pos, box);
                    if (currentMap[x, y] <= 9 && currentMap[x, y] > waterheight)
                        tiles[x, y] = new Tile(TileType.Grass, TileSheet, pos, box);
                    if (currentMap[x, y] <= waterheight)
                        tiles[x, y] = new Tile(TileType.Water, TileSheet, pos, box);
                }
            }
        }
    }
}