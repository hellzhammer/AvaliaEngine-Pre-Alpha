using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.Core
{
    /// <summary>
    /// this component contains all logic to create primitive shapes and lines. 
    /// also contains the logic to add and remove game components 
    /// to the engines underlying framework
    /// </summary>
    public class EngineCore : Game
    {
        public static Random random { get; set; }

        protected SpriteBatch spriteBatch { get; set; }
        public static GraphicsDeviceManager graphics { get; protected set; }

        public virtual void RemoveComponent(GameComponent toremove)
        {
            if (!Engine2D.current.Components.Contains(toremove))
                return;

            this.Components.Remove(toremove);
            toremove.Dispose();
        }

        public virtual void RemoveComponent(DrawableGameComponent toremove)
        {
            if (!Engine2D.current.Components.Contains(toremove))
                return;

            this.Components.Remove(toremove);
            toremove.Dispose();
        }

        public static bool RemoveGameComponent(GameComponent toremove)
        {
            bool rtn = false;

            if (!Engine2D.current.Components.Contains(toremove))
                return false;

            Engine2D.current.Components.Remove(toremove);
            rtn = Engine2D.current.Components.Contains(toremove);
            toremove.Dispose();

            return rtn;
        }

        public static bool RemoveDrawableGameComponent(DrawableGameComponent toremove)
        {
            bool rtn = false;

            if (!Engine2D.current.Components.Contains(toremove))
                return false;

            Engine2D.current.Components.Remove(toremove);
            rtn = Engine2D.current.Components.Contains(toremove);
            toremove.Dispose();

            return rtn;
        }

        /// <summary>
        /// draws a primitive square
        /// </summary>
        public static Texture2D CreateSquare(GraphicsDevice device, int width, int height, Func<int, Color> paint)
        {
            Texture2D texture = new Texture2D(device, width, height);
            Color[] data = new Color[width * height];

            for (int pixel = 0; pixel < data.Count(); pixel++)
            {
                data[pixel] = paint(pixel);
            }

            texture.SetData(data);
            return texture;
        }

        /// <summary>
        /// draws a primitive square with outline
        /// </summary>
        public static Texture2D CreateOutlinedSquare(GraphicsDevice device, int width, int height, Func<int, Color> paint)
        {
            int dist = 10;
            Texture2D texture = new Texture2D(device, width, height);

            List<Color> data = new List<Color>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y < dist || y > height - dist || x < dist || x > width - dist)
                    {
                        data.Add(paint(x));
                    }

                    else
                    {
                        data.Add(Color.Transparent);
                    }
                }
            }

            texture.SetData(data.ToArray());
            return texture;
        }

        /// <summary>
        /// draws a primitive circle.
        /// </summary>
        public static Texture2D CreateCircle(GraphicsDevice device, float radius, int width, int height, Color paint)
        {
            Texture2D texture = new Texture2D(device, width, height);
            List<Color> data = new List<Color>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var dist = Vector2.Distance(new Vector2(x, y), new Vector2(width / 2, height / 2));
                    if (dist <= radius)
                    {
                        data.Add(paint);
                    }
                    else
                    {
                        data.Add(Color.Transparent);
                    }
                }
            }

            texture.SetData(data.ToArray());
            return texture;
        }

        /// <summary>
        /// draws a primitive circle.
        /// </summary>
        public static void DrawCircle(SpriteBatch spriteBatch, Vector2 center, float radius, int segments, Color color, float thickness)
        {
            float angleStep = MathHelper.TwoPi / segments;

            // Create a texture for a 1x1 pixel (for line drawing)
            Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            Vector2 startPoint = center + new Vector2(radius, 0);

            for (int i = 1; i <= segments; i++)
            {
                float angle = i * angleStep;
                Vector2 endPoint = center + new Vector2(
                    radius * (float)Math.Cos(angle),
                    radius * (float)Math.Sin(angle)
                );

                // Pass the thickness to the DrawLine method
                DrawLine(spriteBatch, pixel, startPoint, endPoint, color, thickness);

                startPoint = endPoint;
            }
        }

        /// <summary>
        /// draws lines between 2 points. global.
        /// </summary>
        public static void DrawLine(Vector2 start, Vector2 end, Color color, float thickness, SpriteBatch sprite)
        {
            // Calculate the distance and angle between the points
            float distance = Vector2.Distance(start, end);
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);

            // Draw the line
            sprite.Draw(
                Engine2D.CreateCircle(Engine2D.graphics.GraphicsDevice, 1, 1, 1, Color.DarkGray),
                start,
                null,
                color,
                angle,
                Vector2.Zero,
                new Vector2(distance, thickness),
                SpriteEffects.None,
                0
            );
        }

        /// <summary>
        /// uses rendering engine to draw a line from point a to b;
        /// </summary>
        public static void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end, Color color, float thickness)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            // Scale the rectangle height by the thickness value
            spriteBatch.Draw(
                texture,
                new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), (int)thickness),
                null,
                color,
                angle,
                Vector2.Zero,
                SpriteEffects.None,
                0
            );
        }
    }
}