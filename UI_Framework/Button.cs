using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.UI_Framework
{
    public class Button : Widget
    {
        public string Content { get; set; }
        public Color font_color = Color.White;

        public Button(string name, string content, Vector2 pos, int width, int height, GraphicsDevice device)
        {
            this.contain_text = true;
            this._height = height;
            this._width = width;

            this.name = name;
            this.Position = pos;
            this.Content = content;

            Initialize(device);
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
                if (this.is_mouse_over && Input.LeftMouseDown(MouseButton.Left))
                {
                    this.Click.Invoke();
                }
            }
        }

		public override void Draw(SpriteBatch batch, SpriteFont font)
		{
			base.Draw(batch, font);
			if (!string.IsNullOrWhiteSpace(Content))
			{
				batch.DrawString(
					font,
					Content,
					new Vector2(Position.X + 5, Position.Y + 5),
					font_color,
					0,
					Origin,
					1.0f / Camera2D.main_camera.Zoom,
					SpriteEffects.None,
					0.5f
					);
			}
		}

		public override void Draw(bool simple_draw, SpriteBatch batch, Matrix viewport, SpriteFont font)
		{
			base.Draw(true, batch, viewport);
			if (!string.IsNullOrWhiteSpace(Content))
			{
				batch.DrawString(
					font,
					Content,
					Camera2D.ScreenToWorldSpace(new Vector2(Position.X + 5, Position.Y + 5), viewport),
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
