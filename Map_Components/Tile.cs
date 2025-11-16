using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Engine_lib.Core;

namespace Engine_lib.Map_Components
{
    public class Tile : GameObject2D
    {
        /// <summary>
        /// determines if this tile can be walked on or not.
        /// </summary>
        public bool Is_Obstacle { get; protected set; }

        public Tile(string ID, string obj_name, string texture, Vector2 position, bool is_obstacle) : base(ID, obj_name, texture, position)
        {
            this.Is_Obstacle = is_obstacle;

            this.Collision_Rect = new Rectangle(
                this.Position.ToPoint(),
                new Point(
                    TextureManager.Texture_Dictionary[texture].Width,
                    TextureManager.Texture_Dictionary[texture].Height
                    )
                );
        }

        public override void Update(GameTime gt)
        {
            this.in_render_view = Camera2D.Is_In_Render_View_BoundsCheck(this.Position);

            if (this.in_render_view)
                Input._OnMouseOver(this);

            TrackMouseOver();
        }

        private void TrackMouseOver()
        {
            if (this.mouse_over)
                WorldMap.IsMouseOverTile = this;
        }

        public override void Draw(SpriteBatch batch)
        {
            if (this.in_render_view)
            {
                batch.Draw(TextureManager.Texture_Dictionary[texture_name], Position, Color.White);
            }
        }        
    }
}