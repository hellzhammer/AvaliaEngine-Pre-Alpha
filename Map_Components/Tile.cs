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

        /// <summary>
        /// used for minimap and the forest generator
        /// </summary>
        public int TileType { get; private set; }

        public Tile(string ID, int tiletype, string obj_name, string texture, Vector2 position, bool is_obstacle) : base(ID, obj_name, texture, position)
        {
            this.Is_Obstacle = is_obstacle;
            this.TileType = tiletype;
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

        public override void Draw(SpriteBatch batch)
        {
            if (this.in_render_view)
            {
                batch.Draw(TextureManager.Texture_Dictionary[texture_name], Position, Color.White);
            }
        }

        /// <summary>
        /// this function tells The engines terrain tools which tile the mouse is currently over.
        /// 
        /// this data can then be used to determine where mouse button press events should take place.
        /// </summary>
        private void TrackMouseOver()
        {
            if (this.mouse_over)
                WorldMap.MouseOverTile = this;
        }
    }
}
