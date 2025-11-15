using Microsoft.Xna.Framework;

namespace Engine_lib.UI_Framework
{
    public abstract class TextWidget : Widget
    {
        public bool numeric_input = false;

        public string Content { get; set; }
        public Color font_color = Color.White;

        public Action OnActivated { get; set; }
        public Action OnDeactivated { get; set; }

        public static TextWidget active_text_input { get; set; }
    }
}
