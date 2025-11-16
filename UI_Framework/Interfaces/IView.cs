using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine_lib.UI_Framework.Interfaces
{
    public interface IView
    {
        Vector2 Position { get; set; }
        Box background { get; set; }

        void Initialize(Vector2 pos, float height, float width);
        void Draw(SpriteBatch sprite);
        void Update(GameTime gt);
    }
}
