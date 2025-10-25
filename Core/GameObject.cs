using Microsoft.Xna.Framework;

namespace Engine_lib.Core
{
    public class GameObject
    {
        public string id { get; set; }
        public string texture_name;
        public string object_name { get; set; }

        protected float X;
        protected float Y;
        public Vector2 Position
        {
            get 
            { 
                return new Vector2(X, Y); 
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public float rotation { get; set; }
        public float scale { get; set; }

        public Rectangle Collision_Rect { get; protected set; }
        public Vector2 Origin { get; set; }

        public bool mouse_over { get; protected set; }
        public bool in_render_view { get; protected set; }
    }
}
