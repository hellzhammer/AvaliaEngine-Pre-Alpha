using Engine_lib.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Engine_lib
{
    public class GameObject2D : GameObjectParameters
	{               
        public Action OnMouseOver { get; set; }
        public Action OnMouseExit { get; set; }
        public Action OnLeftClick { get; set; }
        public Action OnRightClick { get; set; }        

        public GameObject2D(string ID, string obj_name, string textureName, Vector2 position)
		{
            this.id = ID;
            this.object_name = obj_name;
            this.texture_name = textureName;
            this.rotation = 0;
            this.scale = 1;
            this.In_Render_View = false;
			this.is_mouse_over = false;
			
			this.Position = position;
			if (!string.IsNullOrEmpty(textureName))
			{
                this.Origin = new Vector2(TextureManager.Texture_Dictionary[textureName].Height/2, TextureManager.Texture_Dictionary[textureName].Width/2);
            }

            Engine2D.current.entityManager.AddEntity(this);
            Debug.WriteLine(Engine2D.current.entityManager.FindObjectsByType<GameObject2D>().Count);
        }

        public void SetInRenderView(bool update)
        {
            this.In_Render_View = update;
        }

        public void SetMouseOver(bool change)
        {
            this.is_mouse_over = change;
        }

        public virtual void Draw(SpriteBatch batch) 
		{
			if (In_Render_View)
			{
				batch.Draw(
					TextureManager.Texture_Dictionary[this.texture_name], 
					Position, 
					null, 
					Color.White, 
					rotation, 
					this.Origin, 
					scale, 
					SpriteEffects.None, 
					1
					);
            }
		}

        public virtual void Update(GameTime gt) { }

        public static void Destroy(GameObject2D to_destroy)
        {
            Engine2D.current.entityManager.Entities.Remove(to_destroy.id);
        }
    }
}