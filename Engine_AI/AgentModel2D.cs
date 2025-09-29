using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Engine_lib.Map_Components;
using System.Diagnostics;

namespace Engine_lib.Engine_AI
{
    public class AgentModel2D : GameObject2D
    {
        // movement parameters
        public Vector2 Movement_Order_Target { get; protected set; }
        protected Vector2 Combat_Order_Target { get; set; }
        protected Vector2 direction = Vector2.Zero;
        protected float speed = 60;
        protected float stop_distance = 32f;
        protected List<Vector2> waypoints = null;

        public AgentModel2D(string ID, string obj_name, string texture, Vector2 position) : base(ID, obj_name, texture, position)
        {

        }

        /// <summary>
        /// Uses pathfinding to get to specified destination. Requires the map instance in order to work properly. 
        /// </summary>
        public void SetDestinationPathing(Vector2 target)
        {
            waypoints = null;
            Movement_Order_Target = target;

            PathBuilder();
        }

        /// <summary>
        /// Moves the unit to the specified destination. No pathfinding. 
        /// </summary>
        public void SetDestination(Vector2 target)
        {
            waypoints = null;
            Movement_Order_Target = target;
            waypoints = new List<Vector2>() { target };
        }

        /// <summary>
        /// Logic to have unit face a point.
        /// </summary>
        protected void FaceTarget()
        {
            direction = (Movement_Order_Target - this.Position);
            direction.Normalize();

            rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X) + MathHelper.PiOver2;
        }

        /// <summary>
        /// Logic to move the unit to a point. 
        /// </summary>
        protected void MoveToTarget(GameTime gt)
        {
            this.FaceTarget();
            if (Vector2.Distance(Movement_Order_Target, Position) > stop_distance)
            {
                Position += direction * speed * (float)gt.ElapsedGameTime.TotalSeconds;
            }
        }

        public void DrawDebugPath(SpriteBatch sprite)
        {
            if (waypoints != null && waypoints.Count > 1)
            {
                for (int i = 2; i < waypoints.Count; i+=2)
                {
                    Engine2D.DrawLine(waypoints[i-2], waypoints[i], Color.Red, 6, sprite);
                    Engine2D.DrawCircle(sprite, waypoints[waypoints.Count-2], 92, 6, Color.Green, 6);
                }
            }
            else if (waypoints != null && waypoints.Count == 1)
            {
                Engine2D.DrawCircle(sprite, waypoints[0], 92, 6, Color.Green, 10);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }

        public override void Update(GameTime gt)
        {
            if (waypoints != null && waypoints.Count > 0)
            {
                float dist = Vector2.Distance(this.Position, waypoints[0]);
                if (dist <= 32)
                {
                    waypoints.RemoveAt(0);
                    if (waypoints.Count > 0)
                    {
                        waypoints.RemoveAt(0);
                    }
                    if (waypoints.Count > 0)
                    {
                        Movement_Order_Target = waypoints[0];
                    }
                    else
                    {
                        Movement_Order_Target = Vector2.Zero;
                        waypoints = null;
                    }
                }
                else
                {
                    MoveToTarget(gt);
                }
            }
            else
            {
                waypoints = null;
            }

            base.Update(gt);
        }

        protected void PathBuilder()
        {
            Vector2 end = Movement_Order_Target;

            // Check if target is blocked
            var targetTile = WorldMap.GetTileAround(end);
            if (targetTile == null || targetTile.Is_Obstacle || IsAdjacentToObstacle(end))
                return;

            var ti = WorldMap.GetTileAround(this.Position);
            if (ti == null)
                return;

            Vector2 start = ti.Position;

            var openSet = new SortedSet<(float fScore, Vector2 pos)>(
                Comparer<(float fScore, Vector2 pos)>.Create((a, b) =>
                {
                    int cmp = a.Item1.CompareTo(b.Item1); // fScore
                    if (cmp != 0) return cmp;
                    return a.Item2.GetHashCode().CompareTo(b.Item2.GetHashCode());
                }));

            var cameFrom = new Dictionary<Vector2, Vector2>();
            var gScore = new Dictionary<Vector2, float> { [start] = 0 };
            var fScore = new Dictionary<Vector2, float> { [start] = Heuristic(start, end) };

            var openSetLookup = new HashSet<Vector2> { start };
            openSet.Add((fScore[start], start));

            int failCounter = 0;
            int failLimit = 5000; // tweak depending on map size / performance

            while (openSet.Count > 0)
            {
                // Early bail-out if too many attempts
                failCounter++;
                if (failCounter > failLimit)
                {
                    Debug.WriteLine("Pathfinding aborted: too many iterations.");
                    return;
                }

                // Get lowest fScore node
                var current = openSet.Min.pos;
                openSet.Remove(openSet.Min);
                openSetLookup.Remove(current);

                // Goal check
                if (Vector2.Distance(current, end) < stop_distance)
                {
                    var path = ReconstructPath(cameFrom, current);
                    path.Insert(0, this.Position); // start with actual position
                    waypoints = path;
                    return;
                }

                // Neighbors
                foreach (var neighbor in GetNeighbors(current))
                {
                    var tile = WorldMap.GetTileAround(neighbor);
                    if (tile == null || tile.Is_Obstacle || IsAdjacentToObstacle(neighbor))
                        continue;

                    float tentativeG = gScore[current] + 32; // cost per step

                    if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeG;
                        fScore[neighbor] = tentativeG + Heuristic(neighbor, end);

                        if (!openSetLookup.Contains(neighbor))
                        {
                            openSet.Add((fScore[neighbor], neighbor));
                            openSetLookup.Add(neighbor);
                        }
                    }
                }
            }

            Debug.WriteLine("Pathfinding failed to find a path.");
        }


        private float Heuristic(Vector2 a, Vector2 b)
        {
            // Use Manhattan since movement is 4-way
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private IEnumerable<Vector2> GetNeighbors(Vector2 pos)
        {
            yield return new Vector2(pos.X + 32, pos.Y);
            yield return new Vector2(pos.X - 32, pos.Y);
            yield return new Vector2(pos.X, pos.Y + 32);
            yield return new Vector2(pos.X, pos.Y - 32);
        }

        // Function to check if a tile is adjacent to an obstacle
        private bool IsAdjacentToObstacle(Vector2 pos)
        {
            var checkPositions = new Vector2[]
            {
                new Vector2(pos.X, pos.Y - 32),  // Above
                new Vector2(pos.X, pos.Y + 32),  // Below
                new Vector2(pos.X - 32, pos.Y),  // Left
                new Vector2(pos.X + 32, pos.Y)   // Right
            };

            foreach (var checkPos in checkPositions)
            {
                var tile = WorldMap.GetTileAround(checkPos);
                if (tile != null)
                {
                    if (tile.Is_Obstacle)
                        return true;
                }
            }

            return false;
        }

        private List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
        {
            var path = new List<Vector2> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }
            return path;
        }

    }
}
