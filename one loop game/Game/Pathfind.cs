using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    public class PathfindNode
    {
        public Point Position;

        public bool Walkable;

        public PathfindNode[] Neighbors;

        public PathfindNode Parent;

        public bool isEnd;

        public bool InOpenList;

        public bool InClosedList;

        public float DistanceToGoal;

        public float DistanceTraveled;
    }

    public class Pathfind
    {
        public PathfindNode[,] searchNodes;

        private int levelWidth;

        private int levelHeight;

        private List<PathfindNode> openList = new List<PathfindNode>();

        private List<PathfindNode> closedList = new List<PathfindNode>();


        public Pathfind(ScreenPlaying playing)
        {
            levelWidth = playing.tileManager.mapX;
            levelHeight = playing.tileManager.mapY;
            //searchNodes = new PathfindNode[levelWidth, levelHeight];
        }

        private float Heuristic(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) +
                   Math.Abs(point1.Y - point2.Y);
        }

        public void InitWalkNodes(ScreenPlaying p)
        {
            searchNodes = new PathfindNode[levelWidth, levelHeight];

            //foreach (var tile in playing.tileManager.tiles)
            for (int y = 0; y < levelHeight; y++)
            {
                for (int x = 0; x < levelWidth; x++)
                {
                    PathfindNode node = new PathfindNode();
                    //node.Position = new Point(tile.Point.X, tile.Point.Y);
                    node.Position = new Point(x, y);
                    foreach (var u in p.eManager.units)
                        if (node.Position == u.endPoint)
                            node.isEnd = true;

                    //check if tile walkable && check if object is on tile
                    //Console.WriteLine(x + "  " + y);
                    node.Walkable = p.tileManager.tiles[x, y].Walkable;
                    //if (map[x, y].Walkable)
                    //{
                    //    node.Walkable = true;
                    //}

                    if (node.Walkable == true || node.isEnd)
                    {
                        node.Neighbors = new PathfindNode[4];
                        searchNodes[x, y] = node;
                    }
                }
            }

            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    PathfindNode node = searchNodes[x, y];

                    if (node == null || node.Walkable == false)
                    {
                        continue;
                    }

                    Point[] neighbors = new Point[]
                    {
                        new Point (x, y - 1),
                        new Point (x, y + 1),
                        new Point (x - 1, y),
                        new Point (x + 1, y),

                        //new Point (x + 1, y + 1),
                        //new Point (x + 1, y - 1),
                        //new Point (x - 1, y + 1),
                        //new Point (x + 1, y - 1),
                    };

                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        Point position = neighbors[i];

                        if (position.X < 0 || position.X > levelWidth - 1 ||
                            position.Y < 0 || position.Y > levelHeight - 1)
                        {
                            continue;
                        }

                        PathfindNode neighbor = searchNodes[position.X, position.Y];

                        if (neighbor == null || neighbor.Walkable == false)
                        {
                            continue;
                        }

                        node.Neighbors[i] = neighbor;
                    }
                }
            }
        }
        public void InitArea(Point size, Point pos, Tile[,] map)
        {
            // pos = building position
            var sizeX = size.X; // building sizeX
            var sizeY = size.Y; // building sizeY

            for (int y = 0; y < sizeY ; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    var X = pos.X + x;
                    var Y = pos.Y + y;

                    PathfindNode node = new PathfindNode();
                    node.Position = new Point(X, Y);                    
                    node.Walkable = map[X, Y].Walkable;

                    if (node.Walkable)
                    {
                        node.Neighbors = new PathfindNode[4];
                        searchNodes[X, Y] = node;
                    }
                }
            }

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    var X = pos.X + x;
                    var Y = pos.Y + y;
                    PathfindNode node = searchNodes[X, Y];

                    if (node == null || node.Walkable == false)
                    {
                        continue;
                    }

                    Point[] neighbors = new Point[]
                    {
                        new Point (x, y - 1),
                        new Point (x, y + 1),
                        new Point (x - 1, y),
                        new Point (x + 1, y),
                    };

                    for (int i = 0; i < neighbors.Length; i++)
                    {

                        Point position = neighbors[i];

                        if (position.X < 0 || position.X > levelWidth - 1 ||
                            position.Y < 0 || position.Y > levelHeight - 1)
                        {
                            continue;
                        }

                        PathfindNode neighbor = searchNodes[position.X, position.Y];

                        if (neighbor == null || neighbor.Walkable == false)
                        {
                            continue;
                        }
                        node.Neighbors[i] = neighbor;
                    }
                }
            }
        }
        public void InitATile(Point pos, Tile[,] map, EntityManager e)
        {
            // tile size / 32
            var sizeX = 1;
            var sizeY = 1;

            var X = pos.X;
            var Y = pos.Y;

            var tempNode = new PathfindNode[levelWidth, levelHeight];

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    X = pos.X + x;
                    Y = pos.Y + y;

                    PathfindNode node = new PathfindNode();
                    node.Position = new Point(X, Y);

                    foreach (var u in e.units)
                    {
                        if (node.Position == u.endPoint)
                        {
                            node.isEnd = true;
                        }
                        //Console.WriteLine(u.endPoint + "  " + node.Position);
                    }

                    if (map[X, Y].Walkable)
                    {
                        node.Walkable = true;
                    }
                    //Console.WriteLine($"X: {X},  Y: {Y}  nodeW: {node.Walkable}  tileW: {map[X, Y].Walkable}");

                    if (node.Walkable || node.isEnd)
                    {
                        node.Neighbors = new PathfindNode[5];
                        tempNode[X, Y] = node;
                        searchNodes[X, Y] = tempNode[X, Y];
                    }
                }
            }

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    var xx = pos.X + x;
                    var yy = pos.Y + y;
                    PathfindNode node = searchNodes[xx, yy];

                    if (node == null || node.Walkable == false)
                    {
                        continue;
                    }

                    Point[] neighbors = new Point[]
                    {
                        new Point (X, Y),
                        new Point (X, Y - 1),
                        new Point (X, Y + 1),
                        new Point (X - 1, Y),
                        new Point (X + 1, Y),
                    };

                    for (int i = 0; i < neighbors.Length; i++)
                    {

                        Point position = neighbors[i];

                        if (position.X < 0 || position.X > sizeX - 1 ||
                            position.Y < 0 || position.Y > sizeY - 1)
                        {
                            continue;
                        }

                        PathfindNode neighbor = searchNodes[position.X, position.Y];

                        if (neighbor == null || neighbor.Walkable == false)
                        {
                            continue;
                        }
                        node.Neighbors[i] = neighbor;
                    }
                }
            }
        }
        public void UpdatePathfind(Point location, int radius, TileManager p)
        {
            /*
				Update the nodes in a radius. If the node was skipped from the 
				last node init, it will create that node.
			*/
            for (var x = location.X - radius; x < location.X + radius; x++)
            {
                for (var y = location.Y - radius; y < location.Y + radius; y++)
                {
                    var isInCircle = (float)((x - location.X) * (x - location.X) + (y - location.Y) * (y - location.Y)) < radius + radius;
                    if ((x >= 0 && x < p.mapX - 1) && (y >= 0 && y < p.mapY - 1) && isInCircle)
                    {
                        var node = new PathfindNode();
                        var entry = p.tiles[x, y];
                        node.Position = new Point(x, y);
                        node.Walkable = entry.Walkable;

                        if (searchNodes[x, y] == null)
                        {
                            if (node.Walkable)
                            {
                                node.Neighbors = new PathfindNode[4];
                                searchNodes[x, y] = node;
                            }
                        }
                    }
                }
            }
            // Refresh the nodes turning them on or off based on the tile pathfind state
            for (var x = location.X - radius; x < location.X + radius; x++)
            {
                for (var y = location.Y - radius; y < location.Y + radius; y++)
                {
                    var isInCircle = (float)((x - location.X) * (x - location.X) + (y - location.Y) * (y - location.Y)) < radius + radius;
                    if ((x >= 0 && x < 32 - 1) && (y >= 0 && y < 32 - 1) && isInCircle)
                    {
                        var node = searchNodes[x, y];

                        if (node == null || !node.Walkable)
                        {
                            continue;
                        }
                        var neighbors = new[]
                        {
                            new Point(x, y - 1), // The node above current node
							new Point(x, y + 1), // The node below the current node
							new Point(x - 1, y), // The node left of the current node
							new Point(x + 1, y), // The node right of the current node
						};
                        // We loop though each of the possible neighbors
                        for (var i = 0; i < neighbors.Length; i++)
                        {
                            var position = neighbors[i];
                            // We need to make sure this neighbor is part of the area
                            if (position.X < 0 || position.X > p.mapX - 1 ||
                               position.Y < 0 || position.Y > p.mapY - 1)
                            {
                                continue;
                            }

                            var neighbor = searchNodes[position.X, position.Y];

                            if (neighbor == null || !neighbor.Walkable)
                                continue;

                            node.Neighbors[i] = neighbor;
                        }
                    }
                }
            }
        }

        private void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    PathfindNode node = searchNodes[x, y];

                    if (node == null)
                    {
                        continue;
                    }

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanceTraveled = float.MaxValue;
                    node.DistanceToGoal = float.MaxValue;
                }
            }
        }

        private List<Vector2> FindFinalPath(PathfindNode startNode, PathfindNode endNode)
        {
            closedList.Add(endNode);

            PathfindNode parentTile = endNode.Parent;
            if (parentTile == null)
            return new List<Vector2>();

            while (parentTile != startNode)
            {
                closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }

            List<Vector2> finalPath = new List<Vector2>();

            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Vector2(closedList[i].Position.X * 32,
                                          closedList[i].Position.Y * 32));
            }

            return finalPath;
        }

        private PathfindNode FindBestNode()
        {
            PathfindNode currentTile = openList[0];

            float smallestDistanceToGoal = float.MaxValue;

            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.DistanceToGoal;
                }
            }
            return currentTile;
        }

        public List<Vector2> FindPath(Point startPoint, Point endPoint)
        {
            if (startPoint == endPoint)
            {
                return new List<Vector2>();
            }

            ResetSearchNodes();

            Vector2 norm = new Vector2(startPoint.X, startPoint.Y);  //normalize to avoid decimals
            //norm.Normalize();
            startPoint = new Point((int)Math.Round(norm.X), (int)Math.Round(norm.Y));
            PathfindNode startNode = searchNodes[startPoint.X, startPoint.Y];
            PathfindNode endNode = searchNodes[endPoint.X, endPoint.Y];
            //endNode.isEnd = true;


            if (startNode == null)
            {
                Console.WriteLine("STARTNODE NULL(unit(s) blocked) - crash");
                return new List<Vector2>();
            }

            startNode.InOpenList = true;

            startNode.DistanceToGoal = Heuristic(startPoint, endPoint);
            startNode.DistanceTraveled = 0;

            openList.Add(startNode);

            PathfindNode nearestNode = startNode;  // preparing nearestNode 

            while (openList.Count > 0)
            {
                PathfindNode currentNode = FindBestNode();

                if (currentNode == null)
                {
                    break;
                }

                if (currentNode == endNode)
                {
                    return FindFinalPath(startNode, endNode);
                }

                for (int i = 0; i < currentNode.Neighbors.Length; i++)
                {
                    PathfindNode neighbor = currentNode.Neighbors[i];

                    if (neighbor == null)
                    {
                        continue;
                    }

                    float distanceTraveled = currentNode.DistanceTraveled + 1;

                    float heuristic = Heuristic(neighbor.Position, endPoint);

                    if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {
                        neighbor.DistanceTraveled = distanceTraveled;
                        neighbor.DistanceToGoal = distanceTraveled + heuristic;
                        neighbor.Parent = currentNode;
                        neighbor.InOpenList = true;
                        openList.Add(neighbor);
                    }
                    else if (neighbor.InOpenList || neighbor.InClosedList)
                    {
                        if (neighbor.DistanceTraveled > distanceTraveled)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;
                            neighbor.Parent = currentNode;

                        }
                    }
                    // if it could not find destination(endNode/endPoint), caluclate nearest node
                    if (endNode != null)
                        if (Heuristic(neighbor.Position, endNode.Position) < (Heuristic(nearestNode.Position, endNode.Position)))
                            nearestNode = neighbor;

                }
                openList.Remove(currentNode);
                currentNode.InClosedList = true;

            }

            // could not find endnode, return nearest node
            endNode = nearestNode;

            // if endNode somehow null, return null list
            if (endNode == null)
            {
                Console.WriteLine("null");
                return new List<Vector2>();
            }

            //Console.WriteLine("could not find endnode, return nearest node");
            //return new List<Vector2>();
            return FindFinalPath(startNode, endNode);
        }
    }
}
