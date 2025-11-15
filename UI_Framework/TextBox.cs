using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
            if (string.IsNullOrEmpty(Content))
            {
                display_string = string.Empty;
                return;
            }

            float maxWidth = _width - 10;   // padding
            float maxHeight = _height - 10; // padding
            float lineHeight = Engine2D.Game_Font.LineSpacing;

            List<string> lines = new List<string>();
            string[] words = Content.Split(' ');

            string currentLine = "";
            float currentHeight = 0f;

            foreach (string word in words)
            {
                // Try adding normally
                string testLine = (currentLine.Length == 0) ? word : currentLine + " " + word;
                Vector2 size = Engine2D.Game_Font.MeasureString(testLine);

                if (size.X <= maxWidth)
                {
                    currentLine = testLine;
                }
                else
                {
                    // Word doesn't fit → finalize the current line
                    lines.Add(currentLine);
                    currentHeight += lineHeight;

                    // Height check
                    if (currentHeight + lineHeight > maxHeight)
                        break;

                    // Start a new line with the word
                    // Character wrap fallback for REALLY long words
                    if (Engine2D.Game_Font.MeasureString(word).X > maxWidth)
                    {
                        string chunk = "";
                        foreach (char c in word)
                        {
                            Vector2 chunkSize = Engine2D.Game_Font.MeasureString(chunk + c);
                            if (chunkSize.X > maxWidth)
                            {
                                lines.Add(chunk);
                                currentHeight += lineHeight;

                                if (currentHeight + lineHeight > maxHeight)
                                    break;

                                chunk = c.ToString();
                            }
                            else
                            {
                                chunk += c;
                            }
                        }

                        currentLine = chunk;
                    }
                    else
                    {
                        currentLine = word;
                    }
                }
            }

            // Add last line
            if (!string.IsNullOrEmpty(currentLine) && currentHeight + lineHeight <= maxHeight)
                lines.Add(currentLine);

            // Build final string
            display_string = string.Join("\n", lines);

            // Continue your logic
            OnMouseOver(Input.MousePosition);
            HandleActivation();
        }


        private void HandleActivation()
        {
            if (this.is_mouse_over && Input.LeftMouseDown())
            {
                active_text_input = this;
                if (this.OnActivated != null)
                    OnActivated.Invoke();
            }
            else if (!this.is_mouse_over && Input.LeftMouseDown())
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
                    1f
                    );
            }
        }
    }
}
