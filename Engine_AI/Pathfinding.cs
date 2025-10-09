/*using Engine_lib.Map_Components;
using Microsoft.Xna.Framework;

namespace Engine_lib.Engine_AI
{
    public static class Pathfinding
    {
        public static List<Vector2> FindPath(Tile startTile, Tile endTile, Tile[][] map)
        {
            List<Vector2> waypoints = new List<Vector2>();

            // Open set (tiles to be evaluated) using a list of positions
            var openSet = new List<Vector2> { startTile.Position };

            // Closed set (already evaluated tiles) using positions
            var closedSet = new HashSet<Vector2>();

            // Dictionary to track where we came from (for path reconstruction)
            var cameFrom = new Dictionary<Vector2, Vector2>();

            // G-cost: distance from start to this tile (using positions)
            var gScore = new Dictionary<Vector2, float>();
            gScore[startTile.Position] = 0;

            // F-cost: G-cost + estimated distance to end (heuristic)
            var fScore = new Dictionary<Vector2, float>();
            fScore[startTile.Position] = Heuristic(startTile.Position, endTile.Position);

            // Main loop
            while (openSet.Count > 0)
            {
                // Get tile in openSet with lowest F-cost
                Vector2 current = openSet.OrderBy(pos => fScore.ContainsKey(pos) ? fScore[pos] : float.MaxValue).First();

                // If we've reached the end tile, reconstruct the path
                if (current == endTile.Position)
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                // Evaluate neighbors
                foreach (var neighbor in GetNeighbors(current, map))
                {
                    if (closedSet.Contains(neighbor) || map[(int)neighbor.X / 32][(int)neighbor.Y / 32].Is_Obstacle)
                    {
                        continue; // Skip if already evaluated or an obstacle
                    }

                    float tentativeGScore = gScore[current] + 32; // Fixed distance between tiles (since they're 32x32)

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor); // Add the neighbor to open set if it's not already in
                    }
                    else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, float.MaxValue))
                    {
                        continue; // This is not a better path
                    }

                    // This path is the best so far
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, endTile.Position);
                }
            }

            // No path found
            return waypoints;
        }

        // Heuristic function (Manhattan distance for grid-based maps, adjusted for 32x32 tiles)
        private static float Heuristic(Vector2 a, Vector2 b)
        {
            return (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y)) / 32; // Divide by 32 to normalize to tile grid units
        }

        // Get neighbors (adjacent tiles, using grid positions)
        private static List<Vector2> GetNeighbors(Vector2 currentPos, Tile[][] map)
        {
            List<Vector2> neighbors = new List<Vector2>();
            int x = (int)(currentPos.X / 32); // Convert pixel position to grid index
            int y = (int)(currentPos.Y / 32);

            // Check each direction (up, down, left, right) and ensure within bounds
            if (x > 0) neighbors.Add(new Vector2((x - 1) * 32, y * 32)); // Left
            if (x < map.Length - 1) neighbors.Add(new Vector2((x + 1) * 32, y * 32)); // Right
            if (y > 0) neighbors.Add(new Vector2(x * 32, (y - 1) * 32)); // Up
            if (y < map[0].Length - 1) neighbors.Add(new Vector2(x * 32, (y + 1) * 32)); // Down

            return neighbors;
        }

        // Reconstruct the path from end tile to start tile
        private static List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
        {
            List<Vector2> path = new List<Vector2> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }
    }
}
*/