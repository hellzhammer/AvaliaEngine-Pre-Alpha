using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.UI_Framework
{
    public class Button : Widget
    {
        public string Content { get; set; }
        public Color font_color = Color.White;

        public Button(string name, string content, Vector2 pos, int width, int height)
        {
            this.contain_text = true;
            this._height = height;
            this._width = width;

            this.object_name = name;
            this.Position = pos;
            this.Content = content;

            Initialize();
        }

        public override void Update()
        {
            OnMouseOver(Input.MousePosition);
            OnClick();
        }

        private void OnClick()
        {
            if (Click != null)
            {
                if (this.is_mouse_over && Input.LeftMouseDown() || this.is_mouse_over && Input.RightMouseDown())
                {
                    this.Click.Invoke();
                }
            }
        }

		public override void Draw(bool simple_draw, SpriteBatch batch, Matrix viewport)
		{
			base.Draw(true, batch, viewport);
			if (!string.IsNullOrWhiteSpace(Content))
			{
				batch.DrawString(
					Engine2D.Game_Font,
					Content,
					Camera2D.ScreenToWorldSpace(new Vector2(Position.X + this.Width / 3 + 64, Position.Y + this.Height / 2), viewport),
					font_color,
					0,
					Origin,
					1.0f / Camera2D.main_camera.Zoom,
					SpriteEffects.None,
					0.5f
					);
			}
		}
    }
}
