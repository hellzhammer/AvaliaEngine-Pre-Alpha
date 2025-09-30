using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.Core
{
    public interface IScene
    {
        Action OnSceneClose { get; set; }
        void Update(GameTime gt);
        void Draw(SpriteBatch sprite);
    }

    public abstract class SceneModel : IScene
    {
        public Action OnSceneClose { get; set; }
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
