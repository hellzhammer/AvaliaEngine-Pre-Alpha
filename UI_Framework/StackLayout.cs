using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.UI_Framework
{
    /// <summary>
    /// Basic stack layout implementation. 
    /// 
    /// there is no margins, or padding or anything like that yet. 
    /// 
    /// just a basic stack layout.
    /// </summary>
    public class StackLayout : Box
    {
        public WidgetOrientation Orientation { get; set; }
        public List<Widget> Children { get; set; }
        public StackLayout(string name, Vector2 pos, float width, float height, WidgetOrientation orientation = WidgetOrientation.Vertical) : base(name, pos, width, height)
        {
            Children = new List<Widget>();
            this.Orientation = orientation;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(bool simple_draw, SpriteBatch batch)
        {
            var viewport = Camera2D.main_camera.GetViewMatrix();

            if (!simple_draw)
                batch.Draw(background, Camera2D.ScreenToWorldSpace(Position, viewport), Color.White); // this works for menu, where below does not.
            else if (simple_draw)
                batch.Draw(background, Camera2D.ScreenToWorldSpace(Position, viewport), this.rect, Color.White, this.Rotation, Origin, 1 / Camera2D.main_camera.Zoom, SpriteEffects.None, 1);

            if (Children != null && Children.Count > 0)
            {
                foreach (var child in Children)
                {
                    child.Draw(simple_draw, batch);
                }
            }
        }

        public void AddChild(Widget widget)
        {
            // first reposition the new object, based on the last object in the list.
            if (Children.Count > 0)
            {
                // get last object
                var lst_obj = Children[Children.Count - 1];
                // set new position based on orientation
                if (Orientation == WidgetOrientation.Horizontal)
                {
                    widget.Position = new Vector2(lst_obj.Position.X + lst_obj.Width, lst_obj.Position.Y);
                }
                else // vertical
                {
                    widget.Position = new Vector2(lst_obj.Position.X, lst_obj.Position.Y + lst_obj.Height);
                }
            }
            else
            {
                // if no objects, set to box position.
                // -- this will be changed later to include padding, centering and more
                widget.Position = this.Position;
            }

            Children.Add(widget);

            // now we need to rebuild the background.
            float Max_width = 0;
            float Max_height = 0;

            // side to side/horizontal
            if (this.Orientation == WidgetOrientation.Horizontal)
            {
                foreach (var child in Children)
                {
                    // get the biggest widgets width
                    if (child.Height > Max_height)
                        Max_height = child.Height;

                    // get full height required.
                    Max_width += child.Width;
                }
            }
            // up/down/vertical
            else
            {
                foreach (var child in Children)
                {
                    // get the biggest widgets width
                    if (child.Width > Max_width)
                        Max_width = child.Width;

                    // get full height required.
                    Max_height += child.Height;
                }
            }

            // build the new background texture.
            this.background = this.create_background(
                    (int)Max_width,
                    (int)Max_height,
                    (c) => Color.Black
                );
        }
    }
}
