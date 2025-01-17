using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.Core
{
    public abstract class SceneModel
    {
        protected SceneModel()
        {
            
        }

        protected virtual void Initialize()
        {

        }

        public virtual void Update(GameTime gt)
        {

        }

        public virtual void Draw(SpriteBatch sprite)
        {

        }
    }
}
