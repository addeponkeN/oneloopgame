using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    public enum ChunkType
    {
        Spawn,
        Tree,
        Bush,
        Stone
    }

    public class Chunk
    {
        public ChunkType type;
        public Vector2 Size = new Vector2(10 * 32);
        public Vector2 Position;

        public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); } }

    }
    public class ChunkGenerator
    {
        int levelWidth, levelHeight;
        BuildNode node;

        public List<Chunk> chunks = new List<Chunk>();

        public ChunkGenerator(ScreenPlaying playing)
        {
            levelWidth = playing.tileManager.mapX / 10;
            levelHeight = playing.tileManager.mapY / 10;
        }


        public void GenerateChunks()
        {
            node = new BuildNode();


            for (int y = 0; y < levelHeight; y++)
            {
                for (int x = 0; x < levelWidth; x++)
                {
                    Chunk chunk = new Chunk();
                    chunk.Position = new Vector2(x * (int)chunk.Size.X, y * (int)chunk.Size.X);

                    int num = Globals.Random(0, 3);
                    //Console.WriteLine(num);
                    switch (num)
                    {
                        case 0: chunk.type = ChunkType.Spawn; break;
                        case 1: chunk.type = ChunkType.Bush; break;
                        case 2: chunk.type = ChunkType.Stone; break;
                        case 3: chunk.type = ChunkType.Tree; break;
                    }
                    num = Globals.Random(0, 3);
                    chunks.Add(chunk);
                }
            }
            //Console.WriteLine(chunks.Count);
        }
        public void InsertNodeFullFill(ScreenPlaying p)
        {
            foreach (var chunk in chunks)
            {
                switch (chunk.type)
                {
                    case ChunkType.Spawn: node.FillWholeChunk(TileType.Grass, chunk.Position, p); break;
                    case ChunkType.Bush: node.FillWholeChunk(TileType.Bush, chunk.Position, p); break;
                    case ChunkType.Stone: node.FillWholeChunk(TileType.Stone, chunk.Position, p); break;
                    case ChunkType.Tree: node.FillWholeChunk(TileType.Tree, chunk.Position, p); break;
                        //node.TreeNode(10, p, chunk.Position); break;
                }
            }
        }
        public void InsertNode(ScreenPlaying p)
        {
            foreach (var chunk in chunks)
            {
                switch (chunk.type)
                {
                    case ChunkType.Spawn: node.Scattered(TileType.Tree, chunk.Position, 2, 3, p); break;
                    case ChunkType.Bush: node.Scattered(TileType.Bush, chunk.Position, 2, 1, p); break;
                    case ChunkType.Stone: node.Scattered(TileType.Stone, chunk.Position, 2, 1, p); break;
                    case ChunkType.Tree: node.Scattered(TileType.Tree, chunk.Position, 10, 1, p); break;
                        //node.TreeNode(10, p, Scattered.Position); break;
                }
            }
        }
        //public void InsertNode(ScreenPlaying p)
        //{
        //    int count = 0;
        //    foreach (var chunk in chunks)
        //    {
        //        count++;
        //        if (count > 7 && spawn == 1)
        //        {
        //            node.SpawnNode(p, chunk.Position);
        //        }
        //        else
        //        {
        //            switch (chunk.type)
        //            {
        //                case ChunkType.Spawn: if (spawn > 0) { node.SpawnNode(p, chunk.Position); spawn--; } else node.Scattered(ResourceType.Tree, chunk.Position, 2, 8, p); break;
        //                case ChunkType.Bush: if (bush > 0) { node.Scattered(ResourceType.Berrybush, chunk.Position, 4, 1, p); bush--; } else node.Scattered(ResourceType.Tree, chunk.Position, 2, 8, p); break;
        //                case ChunkType.Stone: if (stone > 0) { node.Scattered(ResourceType.Stone, chunk.Position, 5, 1, p); stone--; } else node.Scattered(ResourceType.Tree, chunk.Position, 2, 8, p); break;
        //                case ChunkType.Tree: node.Scattered(ResourceType.Tree, chunk.Position, 10, 1, p); break;
        //                    //node.TreeNode(10, p, chunk.Position); break;
        //            }
        //        }

        //    }
        //}
    }

    public enum NodeType
    {
        Spawn,
        Bush,
        Stone,
        Tree
    }
    public class Node
    {
        public Vector2 Position;
        public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, 32, 32); } }
        public int size;
    }
    public class BuildNode
    {
        public Vector2 GetRandomPosInRec(Rectangle rec)
        {
            return Vector2.One;
        }

        public void SpawnNode(ScreenPlaying p, Vector2 pos)
        {
            //p.buildingManager.AddBuilding(new Building(BuildingType.TownCenter, p.buildingTex, p.box, new Vector2(pos.X + 96, pos.Y + 96)) { hp = 50, progress = 100 }, p);
            //p.unitManager.AddUnit(new Unit(p.unitSheet, UnitType.Worker, p.box, new Vector2(pos.X + 96, pos.Y + 192), p));
            //p.unitManager.AddUnit(new Unit(p.unitSheet, UnitType.Worker, p.box, new Vector2(pos.X + 169, pos.Y + 192), p));
            //Scattered(ResourceType.Berrybush, pos, 2, 1, p);
        }
        public void FillWholeChunk(TileType type, Vector2 pos, ScreenPlaying p)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    var t = new Tile(type, p.tileSheet, new Vector2(pos.X + (32 * x), pos.Y + (32 * y)), p.box);
                    //p.tileManager.ReplaceTile(t, p.tileManager.GetTile(t.Point));
                    //p.tileManager.AddTile(new Tile(type, p.tileSheet, new Vector2(pos.X + (32 * x), pos.Y + (32 * y))));
                }
            }
        }
        public void Scattered(TileType type, Vector2 pos, int size, int scatterNess, ScreenPlaying p)
        {
            List<Vector2> poslist = new List<Vector2>();
            Vector2[,] empty = new Vector2[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int X = Globals.Random(scatterNess - (2 * scatterNess), scatterNess);
                    int Y = Globals.Random(scatterNess - (2 * scatterNess), scatterNess);
                    var tempPos = new Vector2(pos.X + (X + x) * 32, pos.Y + (Y + y) * 32);

                    var t = new Tile(type, p.tileSheet, tempPos, p.box);
                    //p.tileManager.ReplaceTile(t, p.tileManager.GetTile(new Point((int)pos.X / 32,(int)pos.Y / 32)));
                    empty[y, x] = tempPos;
                }
            }


        }
    }
}
