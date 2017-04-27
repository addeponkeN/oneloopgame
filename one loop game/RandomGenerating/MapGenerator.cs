using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace one_loop_game
{
    public static class MapGenerator
    {
        //public static List<Tile> CreateMapList(int width, int height, int smoothing = 4, float startFill = 0.5f)
        //{
        //    int[,] map = new int[width, height];

        //    int cell = width * height;
        //    int firstCell = (int)(cell * startFill);

        //    for (int i = 0; i < firstCell; i++)
        //    {
        //        Point p = map.GetRandomCellPosition();
        //        map[p.X, p.Y] = Globals.Random(100);
        //    }
        //    return map.OfType<Tile>().ToList();
        //}

        public static int[,] CreateMap(int width, int height, int smoothing = 4, float startFill = .05f)
        {
            int[,] map = new int[width, height];

            int cell = width * height;
            int firstCell = (int)(cell * startFill);

            for (int i = 0; i < firstCell; i++)
            {
                Point p = map.GetRandomCellPosition();
                map[p.X, p.Y] = Globals.Random(100);
            }
            return map;
        }

        //public static List<Tile> SmootherList(List<Tile> map, int times)
        //{
        //    for (int i = 0; i < times; i++)
        //    {
        //        var destList = map.Select(b => new Tile(b.Type, b.Texture, b.Position)).ToList();                
        //    }
        //    return new List<Tile>();
        //}

        public static int[,] Smoother(int[,] map, int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                int[,] destMap = (int[,])map.Clone();
                map.GetAllPositions().ToList().ForEach(pos => TileSmooth(map, destMap, pos.X, pos.Y));
                map = destMap;
            }
            return map;
        }

        public static void TileSmooth(int[,] sMap, int[,] destMap, int x, int y)
        {
            float tileSum = 0;
            float neighbors = 0;

            foreach (var n in sMap.GetNeighborContents(x, y))
            {
                neighbors++;
                tileSum += n;
            }

            float averageForNeighbors = tileSum / neighbors;

            float diff = averageForNeighbors - sMap[x, y];

            float randomPct = Math.Abs(diff * .1f) * (Globals.Random(6) - 2);

            destMap[x, y] = (int)MathHelper.Clamp((sMap[x, y] + diff * .2f + randomPct), 0, 255);
        }
    }
}