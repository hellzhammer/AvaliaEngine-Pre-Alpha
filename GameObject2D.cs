using Engine_lib.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_lib
{
    public class GameObject2D
	{
		/// <summary>
		/// the objects texture name for streaming.
		/// </summary>
		public readonly string texture_name;
		
		/// <summary>
		/// the objects rect, for collision and render checks.
		/// </summary>
		public Rectangle Rect { get; protected set; }

		/// <summary>
		/// where in the n*n space the object is drawn.
		/// </summary>
		protected Vector2 Origin { get; set; }

		/// <summary>
		/// the objects world position.
		/// </summary>
        public Vector2 Position { get; set; }

        protected double next_update = 0;
        public float scale { get; set; }
        public bool is_mouse_over { get; protected set; }
        public string id { get; protected set; }
        public string object_name { get; set; }

        public float rotation { get; set; }

        public Action OnMouseOver { get; set; }
        public Action OnMouseExit { get; set; }
        public Action OnLeftClick { get; set; }
        public Action OnRightClick { get; set; }

        public bool In_Render_View { get; protected set; }

        public GameObject2D(string ID, string obj_name, string textureName, Vector2 position)
		{
			this.texture_name = textureName;

			this.rotation = 0;
			this.scale = 1;
			this.next_update = new Random().Next(1, 5) * 0.0667;
			this.In_Render_View = false;
			this.is_mouse_over = false;
			this.id = ID;
			this.object_name = obj_name;
			this.Position = position;
			this.Origin = new Vector2(32, 32);
		}

		public virtual void Update(GameTime gt) 
		{
            this.In_Render_View = Camera2D.Is_In_Render_View_BoundsCheck(this.Position);

            if(In_Render_View)
                Input._OnMouseOver(this);
        }

        public void SetMouseOver(bool change)
        {
            this.is_mouse_over = change;
        }

        public virtual void Draw(SpriteBatch batch) 
		{
			if (In_Render_View)
			{
				batch.Draw(TextureManager.Texture_Dictionary[this.texture_name], Position, null, Color.White, rotation, /*new Vector2(16, 16)*/ Vector2.Zero, scale, SpriteEffects.None, 1);
            }
		}
	}
}