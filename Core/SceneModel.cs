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

        /// <summary>
        /// base draws ONLY entities available in the "to_draw" array.
        /// </summary>
        public virtual void Draw(SpriteBatch sprite)
        {
            if (Engine2D.current.entityManager != null 
                && Engine2D.current.entityManager.Entities != null 
                && Engine2D.current.entityManager.to_draw != null)
            {
                var items = Engine2D.current.entityManager.to_draw;
                for (int i = 0; i < items.Count; i++)
                {
                    Engine2D.current.entityManager.Entities[items[i]].Draw(sprite);
                }
            }
        }
    }
}
