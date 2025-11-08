using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.UI_Framework.Interfaces
{
	public abstract class View : IView
    {
        public bool use_world_positioning = false;
        public bool Is_Active = true;
        public int Width, Height;
        public Vector2 Position { get; set; }
        public Box background { get; set; }

        public virtual void Initialize(Vector2 pos, float height, float width)
        {

        }

        public virtual void Draw(SpriteBatch sprite, Matrix view)
        {
            
        }

        public virtual void Update(GameTime gt = null)
        {
            
        }
    }
}