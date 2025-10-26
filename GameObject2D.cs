using Engine_lib.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Engine_lib
{
    public class GameObject2D : EntityObject, IDisposable
	{
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
        public Rectangle Collision_Rect { get; protected set; }
        public Vector2 Origin { get; set; }

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
            this.in_render_view = false;
			this.mouse_over = false;
			
			this.Position = position;
			if (!string.IsNullOrEmpty(textureName))
			{
                this.Origin = new Vector2(TextureManager.Texture_Dictionary[textureName].Height/2, TextureManager.Texture_Dictionary[textureName].Width/2);
            }

            Engine2D.current.entityManager.AddEntity(this);
            Debug.WriteLine(Engine2D.current.entityManager.FindObjectsByType<GameObject2D>().Count);
        }

        public GameObject2D(string ID, string obj_name, string textureName, float _x, float _y)
        {
            this.id = ID;
            this.object_name = obj_name;
            this.texture_name = textureName;
            this.rotation = 0;
            this.scale = 1;
            this.in_render_view = false;
            this.mouse_over = false;

            this.Position = new Vector2(_x, _y);
            if (!string.IsNullOrEmpty(textureName))
            {
                this.Origin = new Vector2(TextureManager.Texture_Dictionary[textureName].Height / 2, TextureManager.Texture_Dictionary[textureName].Width / 2);
            }

            Engine2D.current.entityManager.AddEntity(this);
            Debug.WriteLine(Engine2D.current.entityManager.FindObjectsByType<GameObject2D>().Count);
        }

        public void SetInRenderView(bool update)
        {
            this.in_render_view = update;
        }

        public void SetMouseOver(bool change)
        {
            this.mouse_over = change;
        }

        public virtual void Draw(SpriteBatch batch) 
		{
			if (in_render_view)
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

        public virtual void Dispose()
        {
            Destroy(this);
        }

        public static void Dispose(GameObject2D to_dispose)
        {
            to_dispose.Dispose();
        }
    }
}