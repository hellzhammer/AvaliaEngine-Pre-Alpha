using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.UI_Framework
{
    public abstract class UI_Object
    {
        public bool Active { get; protected set; }
        private bool ContainsText = false;
        public bool contain_text { get { return ContainsText; } protected set { ContainsText = value; } }
        public string object_name { get; set; }
        public Vector2 Position { get; set; }

        public float Rotation = 0.0f;
        protected float _width { get; set; }
        protected float _height { get; set; }
        public float Height { get { return _height; } }
		public float Width { get { return _width; } }

		private readonly float font_char_size = 14;

        public void SetActive(bool value)
        {
            this.Active = value;
        }

        public void SetWidth(float width)
        {
            if (this.font_char_size > this._width)
            {
				this._width = width + this.font_char_size;
			}
            else
            {
                this._width = width;
            }
        }

        public void SetHeight(float height)
        {
            if (this.font_char_size > this._height)
            {
				this._height = height + this.font_char_size;
			}
            else
            {
                this._height = height;

			}
        }

        public virtual void Update() { }

		protected Texture2D set_color(GraphicsDevice device, int width, int height, Func<int, Color> paint)
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
    }
}
