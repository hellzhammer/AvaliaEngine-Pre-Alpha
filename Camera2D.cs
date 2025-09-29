using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine_lib
{
    public class Camera2D
	{
        /// <summary>
        /// main singleton instance
        /// </summary>
        public static Camera2D main_camera { get; private set; }

        private Matrix CurrentViewMatrix { get; set; }

        /// <summary>
        /// allows player to use camera stand alone.
        /// </summary>
		public bool Controlled = false;		
		
        /// <summary>
        /// the cameras viewport
        /// </summary>
		public readonly Viewport _viewport;

        /// <summary>
        /// the current position of the camera
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// the current zoom level
        /// </summary>
        public float Zoom = 1.0f;

        public Vector2 Origin { get; private set; }

        public Camera2D()
		{
            _viewport = Engine2D.graphics.GraphicsDevice.Viewport;
            this.Position = Vector2.Zero;
            Origin = new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);
			main_camera = this;
        }

        public Camera2D(Vector2 _position)
        {
            _viewport = Engine2D.graphics.GraphicsDevice.Viewport;
            this.Position = _position;
            Origin = new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);
            main_camera = this;
        }

        public Camera2D(Viewport viewport, bool set_as_main)
		{
            _viewport = viewport;
            this.Position = Vector2.Zero;
            Origin = new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);
            if(set_as_main)
                main_camera = this;
        }

        public Camera2D(Vector2 _position, Viewport viewport, bool set_as_main)
        {
            _viewport = viewport;
            this.Position = _position;
            Origin = new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);
            if (set_as_main)
                main_camera = this;
        }

        public Camera2D(Viewport viewport)
		{
			_viewport = viewport;
			this.Position = Vector2.Zero;
			Origin = new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);
			main_camera = this;
		}

		private Matrix Get_View_Matrix()
		{
            return
                Matrix.CreateTranslation(new Vector3(-this.Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(0) *
                Matrix.CreateScale(Zoom, Zoom, 1) * 
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        /// <summary>
        /// returns the current view matrix
        /// </summary>
		public Matrix GetViewMatrix()
		{
			return CurrentViewMatrix;
		}

        /// <summary>
        /// gets the screen position in the worldspace
        /// </summary>
		public static Vector2 ScreenToWorldSpace(Vector2 screenPosition, Matrix cameraTransform)
        {
            // Invert the camera transform to go from screen -> world
            Matrix inverse = Matrix.Invert(cameraTransform);
            return Vector2.Transform(screenPosition, inverse);
        }

        /// <summary>
        /// checks if an object is within the cameras view
        /// </summary>
        public static bool Is_In_Render_View_RectIntersectsCheck(Vector2 position)
        {
            var cam_pos = Camera2D.main_camera.Position;

            var camrect = new Rectangle(cam_pos.ToPoint(), new Point(Engine2D.graphics.GraphicsDevice.DisplayMode.Width, Engine2D.graphics.GraphicsDevice.DisplayMode.Height));
            var itemrect = new Rectangle(position.ToPoint(), new Point(32, 32));
            return camrect.Intersects(itemrect);
        }

        /// <summary>
        /// checks if an object is within the cameras view
        /// </summary>
        public static bool Is_In_Render_View_RectIntersectsCheck(Vector2 position, int sizeX, int sizeY)
        {
            // Get the camera position
            var camPos = Camera2D.main_camera.Position;

            // Use the viewport dimensions instead of DisplayMode
            var viewport = Engine2D.graphics.GraphicsDevice.Viewport;
            var camRect = new Rectangle(camPos.ToPoint(), new Point(viewport.Width, viewport.Height));

            // Define the item's rectangle based on its position and size
            var itemRect = new Rectangle(position.ToPoint(), new Point(sizeX, sizeY));

            // Check if the item intersects with the camera's rectangle
            return itemRect.Intersects(camRect);
        }

        /// <summary>
        /// checks if an object is within the cameras view
        /// </summary>
        public static bool Is_In_Render_View_BoundsCheck(Vector2 position)
        {
            var cam_pos = Camera2D.main_camera.Position;
            var x_dist_max = cam_pos.X + Engine2D.graphics.GraphicsDevice.DisplayMode.Width;
            var y_dist_max = cam_pos.Y + Engine2D.graphics.GraphicsDevice.DisplayMode.Height;

            bool x_bool = false;
            bool y_bool = false;

            if (position.Y >= cam_pos.Y - 128 && position.Y <= y_dist_max + 128)
            {
                y_bool = true;
            }

            if (position.X >= cam_pos.X - 128 && position.X <= x_dist_max + 128)
            {
                x_bool = true;
            }

            if (x_bool && y_bool)
            {
                return true;
            }
            return false;
        }

        public float cam_speed = 10;
        public void Update()
		{
            CurrentViewMatrix = this.Get_View_Matrix();

            if (this.Controlled)
            {
                var cspeed = cam_speed;
                Vector2 move_to = Position;
                // movement
                if (Input.KeyHold(Keys.LeftShift))
                {
                    cspeed *= 2;
                }
                if (Input.KeyHold(Keys.Up) || Input.KeyHold(Keys.W))
                    move_to.Y -= cspeed;

                else if (Input.KeyHold(Keys.Down) || Input.KeyHold(Keys.S))
                    move_to.Y += cspeed;

                if (Input.KeyHold(Keys.Left) || Input.KeyHold(Keys.A))
                    move_to.X -= cspeed;

                else if (Input.KeyHold(Keys.Right) || Input.KeyHold(Keys.D))
                    move_to.X += cspeed;
                this.Position = move_to;
            }
        }
	}
}