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
        protected float stop_distance = 0.2f;
        protected List<Vector2> waypoints = null;

        public AgentModel2D(string ID, string obj_name, string texture, Vector2 position) : base(ID, obj_name, texture, position)
        {

        }

        /// <summary>
        /// Uses pathfinding to get to specified destination. Requires the map instance in order to work properly. 
        /// </summary>
        public void SetDestination(Vector2 target, Tile[][] world_map)
        {
            waypoints = null;
            Movement_Order_Target = target;

            PathBuilder(world_map);
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
            /*if (waypoints != null)
            {
                for (int p = 0; p < waypoints.Count; p++)
                {
                    batch.Draw(this.object_sprite, waypoints[p], Color.White);
                }
            }*/
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

        /// <summary>
        /// this is a test function, change back to pathbuilder3 if this does not pan out. 
        /// 
        /// this function does not allow characters to stop or move within 1 tile of an obstacle.
        /// </summary>
        protected void PathBuilder(Tile[][] world_map)
        {
            bool found = false;
            Vector2 start = Vector2.Zero;
            Vector2 end = Movement_Order_Target;

            // Check if the target is an obstacle
            try
            {
                if (world_map[(int)Movement_Order_Target.Y / 32][(int)Movement_Order_Target.X / 32].Is_Obstacle)
                    return;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return;
            }

            // Find the starting tile
            for (int i = 0; i < world_map.Length; i++)
            {
                for (int j = 0; j < world_map[i].Length; j++)
                {
                    var tile = world_map[i][j];
                    if (!tile.Is_Obstacle)
                    {
                        this.Rect = new Rectangle(this.Position.ToPoint(), new Point(1, 1));
                        Rectangle tileRect = new Rectangle(new Vector2(tile.Position.X + 16, tile.Position.Y + 16).ToPoint(), new Point(32, 32));

                        if (this.Rect.Intersects(tileRect))
                        {
                            start = tile.Position;
                            found = true;
                            break;
                        }
                    }
                }
                if (found)
                {
                    break;
                }
            }

            // If starting tile not found, exit early
            if (!found)
                return;

            List<Vector2> been_to = new List<Vector2>();
            List<Vector2> path = new List<Vector2> { start };

            int fails = 0;
            bool complete = false;
            float epsilon = 0.1f; // Tolerance for floating-point comparison

            // Begin pathfinding
            do
            {
                if (path.Count > 0)
                {
                    Vector2 currentPos = path[path.Count - 1];

                    // Get the surrounding positions (forward, backward, left, right)
                    Vector2 forward = new Vector2(currentPos.X, currentPos.Y + 32);
                    Vector2 backward = new Vector2(currentPos.X, currentPos.Y - 32);
                    Vector2 left = new Vector2(currentPos.X - 32, currentPos.Y);
                    Vector2 right = new Vector2(currentPos.X + 32, currentPos.Y);

                    var directions = new (Vector2 position, float distance)[]
                    {
                        (forward, Vector2.Distance(forward, end)),
                        (backward, Vector2.Distance(backward, end)),
                        (left, Vector2.Distance(left, end)),
                        (right, Vector2.Distance(right, end))
                    };

                    // Sort directions by distance to the target
                    Array.Sort(directions, (a, b) => a.distance.CompareTo(b.distance));

                    bool moved = false;

                    foreach (var (nextPos, _) in directions)
                    {
                        // Ensure nextPos is within bounds
                        if (nextPos.X >= 0 && nextPos.Y >= 0 &&
                            nextPos.Y / 32 < world_map.Length && nextPos.X / 32 < world_map[0].Length)
                        {
                            Tile tile = world_map[(int)nextPos.Y / 32][(int)nextPos.X / 32];

                            // Check if the next tile is not an obstacle
                            // and if any neighboring tiles are obstacles, skip this tile
                            if (!tile.Is_Obstacle && !IsAdjacentToObstacle(nextPos, world_map) && !path.Contains(nextPos) && !been_to.Contains(nextPos))
                            {
                                path.Add(nextPos);
                                moved = true;
                                break;
                            }
                        }
                    }

                    // If no movement was made, backtrack
                    if (!moved)
                    {
                        if (path.Count > 1)
                        {
                            been_to.Add(path[path.Count - 1]);
                            path.RemoveAt(path.Count - 1);
                        }
                        fails++;
                    }
                    else
                    {
                        fails = 0; // Reset fails if a move was made
                    }

                    // Check if the path is complete (within tolerance of the destination)
                    if (Vector2.Distance(path[path.Count - 1], end) < epsilon)
                    {
                        complete = true;
                    }

                    // Stop if the pathfinding fails too much
                    if (fails == 100)
                        break;

                }

            } while (!complete);

            // If a path was found, set waypoints
            if (complete || path.Count > 0)
            {
                path.RemoveAt(0);
                path.Insert(0, this.Position); // Add the current position as the first waypoint
                waypoints = path;
            }

            return;
        }

        // Function to check if a tile is adjacent to an obstacle
        private bool IsAdjacentToObstacle(Vector2 pos, Tile[][] world_map)
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
                if (checkPos.X >= 0 && checkPos.Y >= 0 &&
                    checkPos.Y / 32 < world_map.Length && checkPos.X / 32 < world_map[0].Length)
                {
                    var tile = world_map[(int)checkPos.Y / 32][(int)checkPos.X / 32];
                    if (tile.Is_Obstacle)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// this is the new function to be used with the chunk system. not done yet. just copied and pasted the above 
        /// function in order to alter its flow to incorporate chunk tile checks to determine the parent chunks 
        /// of the tile being tested.
        /// </summary>
        protected void ChunkyPathBuilder(Tile[][] world_map)
        {
            bool found = false;
            Vector2 start = Vector2.Zero;
            Vector2 end = Movement_Order_Target;

            // Check if the target is an obstacle
            try
            {
                if (world_map[(int)Movement_Order_Target.Y / 32][(int)Movement_Order_Target.X / 32].Is_Obstacle)
                    return;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return;
            }

            // Find the starting tile
            for (int i = 0; i < world_map.Length; i++)
            {
                for (int j = 0; j < world_map[i].Length; j++)
                {
                    var tile = world_map[i][j];
                    if (!tile.Is_Obstacle)
                    {
                        this.Rect = new Rectangle(this.Position.ToPoint(), new Point(1, 1));
                        Rectangle tileRect = new Rectangle(new Vector2(tile.Position.X + 16, tile.Position.Y + 16).ToPoint(), new Point(32, 32));

                        if (this.Rect.Intersects(tileRect))
                        {
                            start = tile.Position;
                            found = true;
                            break;
                        }
                    }
                }
                if (found)
                {
                    break;
                }
            }

            // If starting tile not found, exit early
            if (!found)
                return;

            List<Vector2> been_to = new List<Vector2>();
            List<Vector2> path = new List<Vector2> { start };

            int fails = 0;
            bool complete = false;
            float epsilon = 0.1f; // Tolerance for floating-point comparison

            // Begin pathfinding
            do
            {
                if (path.Count > 0)
                {
                    Vector2 currentPos = path[path.Count - 1];

                    // Get the surrounding positions (forward, backward, left, right)
                    Vector2 forward = new Vector2(currentPos.X, currentPos.Y + 32);
                    Vector2 backward = new Vector2(currentPos.X, currentPos.Y - 32);
                    Vector2 left = new Vector2(currentPos.X - 32, currentPos.Y);
                    Vector2 right = new Vector2(currentPos.X + 32, currentPos.Y);

                    var directions = new (Vector2 position, float distance)[]
                    {
                        (forward, Vector2.Distance(forward, end)),
                        (backward, Vector2.Distance(backward, end)),
                        (left, Vector2.Distance(left, end)),
                        (right, Vector2.Distance(right, end))
                    };

                    // Sort directions by distance to the target
                    Array.Sort(directions, (a, b) => a.distance.CompareTo(b.distance));

                    bool moved = false;

                    foreach (var (nextPos, _) in directions)
                    {
                        // Ensure nextPos is within bounds
                        if (nextPos.X >= 0 && nextPos.Y >= 0 &&
                            nextPos.Y / 32 < world_map.Length && nextPos.X / 32 < world_map[0].Length)
                        {
                            Tile tile = world_map[(int)nextPos.Y / 32][(int)nextPos.X / 32];

                            // Check if the next tile is not an obstacle
                            // and if any neighboring tiles are obstacles, skip this tile
                            if (!tile.Is_Obstacle && !IsAdjacentToObstacle(nextPos, world_map) && !path.Contains(nextPos) && !been_to.Contains(nextPos))
                            {
                                path.Add(nextPos);
                                moved = true;
                                break;
                            }
                        }
                    }

                    // If no movement was made, backtrack
                    if (!moved)
                    {
                        if (path.Count > 1)
                        {
                            been_to.Add(path[path.Count - 1]);
                            path.RemoveAt(path.Count - 1);
                        }
                        fails++;
                    }
                    else
                    {
                        fails = 0; // Reset fails if a move was made
                    }

                    // Check if the path is complete (within tolerance of the destination)
                    if (Vector2.Distance(path[path.Count - 1], end) < epsilon)
                    {
                        complete = true;
                    }

                    // Stop if the pathfinding fails too much
                    if (fails == 100)
                        break;

                }

            } while (!complete);

            // If a path was found, set waypoints
            if (complete || path.Count > 0)
            {
                path.RemoveAt(0);
                path.Insert(0, this.Position); // Add the current position as the first waypoint
                waypoints = path;
            }

            return;
        }
    }
}
