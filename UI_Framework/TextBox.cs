using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.UI_Framework
{
    public class TextBox : TextWidget
	{
        // width / 12 = charcount
        private const int text_char_width = 9;
        private const int text_char_height = 18;
        private string display_string = string.Empty;

        public TextBox(string name, string content, Vector2 pos, int width, int height)
        {
			this.contain_text = true;
			this._height = height;
            if (this._height < text_char_height)
                this._height = text_char_height + text_char_width;

            this._width = width;

            this.object_name = name;
            this.Position = pos;
            this.Content = content;

            Initialize();
        }

        public override void Update()
        {
            display_string = string.Empty;
            int max = (int)_width - 18;
            
            for (int i = 0; i < Content.Length; i++)
            {
                var len = Engine2D.Game_Font.MeasureString(display_string + Content[i]);
                if (len.X < max)
                {
                    display_string += Content[i];
                }
                else
                    break;
            }
            Content = display_string;

            OnMouseOver(Input.MousePosition);
            HandleActivation();
        }

        private void HandleActivation()
        {
            if (this.is_mouse_over && Input.LeftMouseDown(MouseButton.Left))
            {
                active_text_input = this;
                if (this.OnActivated != null)
                    OnActivated.Invoke();
            }
            else if(!this.is_mouse_over && Input.LeftMouseDown(MouseButton.Left))
            {
                if (active_text_input == this)
                {
                    active_text_input = null;
                    if (this.OnDeactivated != null)
                        OnDeactivated.Invoke();
                }
            }
        }

        public override void Draw(bool simple_draw, SpriteBatch sprite, Matrix viewport)
        {
            base.Draw(simple_draw, sprite, viewport);
            if (!string.IsNullOrWhiteSpace(Content))
            {
                sprite.DrawString(
                    Engine2D.Game_Font,
                    display_string,
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
