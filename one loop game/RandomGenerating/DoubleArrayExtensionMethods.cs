using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    public static class DoubleArrayExtensionMethods
    {
        static Random _rnd = new Random();

        /// <summary>
        /// Gets the coordinates of all the existing neigbors of a given tile
        /// </summary>
        /// <typeparam name="T">The type of the doublearray contents</typeparam>
        /// <param name="doubleArray">The doublearray to work on</param>
        /// <param name="x">The x-coordinate of the cell to get neigbors for</param>
        /// <param name="y">The y-coordinate of the cell to get neighbors for</param>
        /// <returns>An enumeration of the neigbors' coordinates</returns>
        public static IEnumerable<Point> GetNeighborCoordinates<T>(this T[,] doubleArray, int x, int y, bool onlyXYaxis = false)
        {
            //for the column on the left to the column on the right of the tile
            for (int deltaX = -1; deltaX <= 1; deltaX++)
            {

                //for the row above to the row below the tile
                for (int deltaY = -1; deltaY <= 1; deltaY++)
                {
                    //if we are only looking at the cells directly above/below and beside the cell
                    //we skip diagonal neighbors
                    if (onlyXYaxis && deltaY * deltaX != 0) { continue; }

                    //the potential neighbor's coordinates
                    int neighborX = x + deltaX;
                    int neighborY = y + deltaY;

                    //if the coordinate is within the map
                    if (doubleArray.ContainsCoordinate(neighborX, neighborY))
                    {
                        //and we aren't looking at the tile itself
                        if (!(x == neighborX && y == neighborY))
                        {
                            yield return new Point(neighborX, neighborY);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the contents of the neighboring positions in the array
        /// </summary>
        /// <typeparam name="T">The type of the doublearray</typeparam>
        /// <param name="doubleArray">The doublearray to work on</param>
        /// <param name="x">The x-coordinate of the cell to get neigbors for</param>
        /// <param name="y">The y-coordinate of the cell to get neighbors for</param>
        /// <returns>An enumeration of the neighbors' contents</returns>
        public static IEnumerable<T> GetNeighborContents<T>(this T[,] doubleArray, int x, int y, bool onlyXYaxis = false)
        {
            foreach (var position in doubleArray.GetNeighborCoordinates(x, y, onlyXYaxis))
            {
                yield return doubleArray[position.X, position.Y];
            }
        }


        /// <summary>
        /// Determines whether a given coordinate set (given by a Point) is within a doublearray
        /// </summary>
        /// <typeparam name="T">The type of the doublearray</typeparam>
        /// <param name="doubleArray">The doublearray to work on</param>
        /// <param name="coordinates">The coordinates of the cell to check for</param>
        /// <returns>Whether the coordinates are within the double array</returns>
        public static bool ContainsCoordinate<T>(this T[,] doubleArray, Point coordinates)
        {
            return coordinates.X >= 0 && coordinates.X < doubleArray.GetLength(0) && coordinates.Y >= 0 && coordinates.Y < doubleArray.GetLength(1);
        }


        /// <summary>
        /// Determines whether a given coordinate is within a doublearray
        /// </summary>
        /// <typeparam name="T">The type of the doublearray</typeparam>
        /// <param name="doubleArray">The doublearray to work on</param>
        /// <param name="x">The x-coordinate of the cell to check for</param>
        /// <param name="y">The y-coordinate of the cell to check for</param>
        /// <returns>Whether the coordinate is within the double array</returns>
        public static bool ContainsCoordinate<T>(this T[,] doubleArray, int x, int y)
        {
            return x >= 0 && x < doubleArray.GetLength(0) && y >= 0 && y < doubleArray.GetLength(1);
        }


        /// <summary>
        /// Gets the positions of the bordercells in the doublearray
        /// </summary>
        /// <typeparam name="T">The type of the doublearray</typeparam>
        /// <param name="doubleArray">The doublearray to work on</param>
        /// <param name="borderWidth">How many cells in from the border to use (default 1)</param>
        /// <returns>An enumeration of the positions of the bordercells in the doublearray</returns>
        public static IEnumerable<Point> GetBorderCellPositions<T>(this T[,] doubleArray, int borderWidth = 1)
        {
            int width = doubleArray.GetLength(0);
            int height = doubleArray.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x < borderWidth || y < borderWidth || x > width - borderWidth - 1 || y > height - borderWidth - 1)
                    {
                        yield return new Point(x, y);
                    }
                }
            }
        }


        /// <summary>
        /// Gets the coordinate of a random cell in the doublearray
        /// </summary>
        /// <typeparam name="T">The type of the contents of the doublearray</typeparam>
        /// <param name="doubleArray">The doublearray to work on</param>
        /// <returns>The coordinates of a random cell within the doublearray</returns>
        public static Point GetRandomCellPosition<T>(this T[,] doubleArray)
        {
            return new Point(_rnd.Next(doubleArray.GetLength(0)), _rnd.Next(doubleArray.GetLength(1)));
        }

        /// <summary>
        /// Gets the coordinates of all cells in the doublearray
        /// </summary>
        /// <typeparam name="T">The type of the contents of the doublearray</typeparam>
        /// <param name="doubleArray">The doublearray to work on</param>
        /// <returns>The coordinates of a random cell within the doublearray</returns>
        public static IEnumerable<Point> GetAllPositions<T>(this T[,] doubleArray)
        {
            int width = doubleArray.GetLength(0);
            int height = doubleArray.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    yield return new Point(x, y);
                }
            }
        }




        /// <summary>
        /// This method fills from a tile and out based on criteria in the Func parameter.
        /// </summary>
        /// <typeparam name="T">The type of the doublearray to work on</typeparam>
        /// <param name="doubleArray">The doublearray to work on</param>
        /// <param name="startingPosition">Where this round of fills should start from</param>
        /// <param name="checkAndChangeTile">The function which is passed the tile
        /// and can change the tile any which way it want to (e.g. set it to 'filled') 
        /// and returns whether to let the flooding continue on from this tile</param>
        /// <param name="canMoveDiagonally">Whether to try moving diagonally (default is false)</param>
        /// <param name="iterations">How many iterations to perform (default is int.MaxValue)</param>
        public static void FloodFill<T>(this T[,] doubleArray, Point startingPosition, Func<T[,], Point, bool> checkAndChangeTile, bool canMoveDiagonally = false, int iterations = int.MaxValue)
        {
            if (checkAndChangeTile(doubleArray, startingPosition))
            {
                foreach (var neighbor in doubleArray.GetNeighborCoordinates<T>(startingPosition.X, startingPosition.Y, !canMoveDiagonally))
                {
                    doubleArray.FloodFill(neighbor, checkAndChangeTile, canMoveDiagonally, iterations);
                }
            }
        }


        //sample implementation of the checkAndChangeTile
        private static bool F<T>(T[,] array, Point p)
        {
            if (!array[p.X, p.Y].Equals(default(T)))
            {
                array[p.X, p.Y] = default(T);
                return true;
            }
            return false;
        }
    }
}
