using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.UI_Framework
{
    public class Box : Widget
    {
        public WidgetOrientation Orientation = WidgetOrientation.Vertical;
        public int padding = 0;

        public Box(string name, Vector2 pos, float width, float height, GraphicsDevice device)
        {
            this._height = height;
            this._width = width;

            this.object_name = name;
            this.Position = pos;
            this.Initialize(device);
        }
    }
}
