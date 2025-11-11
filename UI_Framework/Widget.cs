using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.UI_Framework
{
    public enum WidgetOrientation
    {
        Vertical,
        Horizontal
    }

    public abstract class Widget : UI_Object
    {
        public bool is_mouse_over { get; protected set; }
        public Action Mouse_Over { get; set; }
        public Action Mouse_Exit { get; set; }
        public Action Click { get; set; }
        public Texture2D background { get; set; }
        public Color background_color = Color.Black;
        public Rectangle rect { get; protected set; }
        protected Vector2 Origin { get; set; }

        /// <summary>
        /// Call after Your Code has been run.
        /// </summary>
        public virtual void Initialize(GraphicsDevice device)
        {
            this.is_mouse_over = false;
            SetHeight(this._height);
            SetWidth(this._width);
            Set_Background(background_color, device);
            this.rect = new Rectangle(this.Position.ToPoint(), new Point((int)this._width, (int)this._height));
            this.Origin = new Vector2(0, 0);
        }

        public void Set_Background(Color color, GraphicsDevice device)
        {
            this.background_color = color;
            this.background = this.set_color(device, (int)this._width, (int)this._height, pixel => background_color);
        }

        protected void OnMouseOver(Vector2 mouse)
        {
            var mouse_rect = new Rectangle(mouse.ToPoint(), this.Origin.ToPoint());
            if (mouse_rect.Intersects(this.rect) && !this.is_mouse_over)
            {
                this.is_mouse_over = true;
                if (Mouse_Over != null)
                {
                    this.Mouse_Over.Invoke();
                }
            }
            else if (!mouse_rect.Intersects(this.rect) && this.is_mouse_over)
            {
                this.is_mouse_over = false;
                if (this.Mouse_Exit != null)
                {
                    Mouse_Exit.Invoke();
                }
            }
        }

		public virtual void Draw(bool simple_draw, SpriteBatch batch, Matrix viewport)
        {
            if (this.background != null)
            {
                if (!simple_draw)
                    batch.Draw(background, Camera2D.ScreenToWorldSpace(Position, viewport), Color.White); // this works for menu, where below does not.
                else if (simple_draw)
                    batch.Draw(background, Camera2D.ScreenToWorldSpace(Position, viewport), this.rect, Color.White, this.Rotation, Origin, 1 / Camera2D.main_camera.Zoom, SpriteEffects.None, 1);
            }
        }
    }
}
