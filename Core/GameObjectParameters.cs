using Microsoft.Xna.Framework;

namespace Engine_lib.Core
{
    public class GameObjectParameters
    {
        public string id { get; set; }
        public string texture_name;
        public string object_name { get; set; }

        public Vector2 Position;
        public float rotation { get; set; }
        public float scale { get; set; }

        public Rectangle Collision_Rect { get; protected set; }
        public Vector2 Origin { get; set; }

        public bool is_mouse_over { get; protected set; }
        public bool In_Render_View { get; protected set; }
    }
}
