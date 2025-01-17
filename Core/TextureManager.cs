using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib.Core
{
    public class TextureManager
    {
        /// <summary>
        /// this is the main source for texture "streaming". not real, just what
        /// it is being called atm.
        /// </summary>
        public static Dictionary<string, Texture2D> Texture_Dictionary { get; protected set; }

        public TextureManager(Dictionary<string, Texture2D> textdict) 
        {
            Texture_Dictionary = textdict;
        }

        public bool Add_Texture(string text_name, Texture2D text)
        {
            Texture_Dictionary.Add(text_name, text);
            return Texture_Dictionary.ContainsKey(text_name);
        }

        public void Update_Texture(string text_name, Texture2D text)
        {
            Texture_Dictionary[text_name] = text;
        }

        public bool Delete_Texture(string id)
        {
            Texture_Dictionary.Remove(id);
            return Texture_Dictionary.ContainsKey(id);
        }
    }
}
