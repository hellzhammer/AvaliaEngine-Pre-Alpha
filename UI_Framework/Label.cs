using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.UI_Framework
{
    public class Label : Widget
    {
        public string Content { get; set; }
        public Color font_color = Color.White;
        public Label(string name, string content, Vector2 pos, int width, int height)
        {
			this.contain_text = true;
            this._height = height;
            this._width = width;

            this.object_name = name;
            this.Position = pos;
            this.Content = content;

            this.Initialize();
        }

		public override void Draw(bool simple_draw, SpriteBatch batch)
		{
			base.Draw(true, batch);
			if (!string.IsNullOrWhiteSpace(Content))
			{
				var viewport = Camera2D.main_camera.GetViewMatrix();
                batch.DrawString(
					Engine2D.Game_Font,
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
