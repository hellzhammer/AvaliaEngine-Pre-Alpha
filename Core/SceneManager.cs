namespace Engine_lib.Core
{
    public class SceneManager
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
    }
}