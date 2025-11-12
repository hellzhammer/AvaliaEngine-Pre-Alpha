using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.Core
{
    public static class SceneManager
    {
        // ---- SCENE MANAGER -------------------
        public static SceneModel currentScene { get; private set; }

        /// <summary>
        /// change the current scene
        /// </summary>
        public static void NewScene(SceneModel newscene)
        {
            // invoke any onclose functions 
            if (newscene.OnSceneClose != null)
            {
                newscene.OnSceneClose.Invoke();
            }

            // clear the current scene
            if (currentScene != null)
                currentScene = null;

            // add new scene
            currentScene = newscene; // new scene instance
        }

        public static void ClearScene()
        {
            if (currentScene != null)
            {
                currentScene = null;
            }
        }

        public static void Update(GameTime gt)
        {
            if (currentScene != null)
            {
                currentScene.Update(gt);
            }
        }

        public static void Draw(SpriteBatch batch)
        {
            if (currentScene != null)
            {
                currentScene.Draw(batch);
            }
        }
    }
}