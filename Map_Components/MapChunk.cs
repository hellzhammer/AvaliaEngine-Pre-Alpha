using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine_lib.Map_Components
{
    public class MapChunk
    {
        /// <summary>
        /// chunks actual id
        /// </summary>
        public readonly string Chunk_ID;
        public Vector2 Position { get; set; }

        /// <summary>
        /// chunk max x
        /// </summary>
        public int Size_X { get; set; }
        /// <summary>
        /// chunk max y
        /// </summary>
        public int Size_Y { get; set; }

        /// <summary>
        /// this chunks tiles
        /// </summary>
        public string[][] ChunkIDMap { get; set; }

        /// <summary>
        /// flag for whether or not the object is in camera view.
        /// </summary>
        public bool InRenderView { get; private set; }

        public List<string> ResourceIDList { get; set; }

        public Rectangle Rect { get; set; }

        public MapChunk(string[][] chunk, string chunkID)
        {
            this.Chunk_ID = chunkID;
            this.ChunkIDMap = chunk;
            this.Size_X = 32 * chunk[0].Length; // horizontal
            this.Size_Y = 32 * chunk.Length; // vertical

            Position = WorldMap.TerrainTileDictionary[chunk[0][0]].Position;
            Rect = new Rectangle(
               Position.ToPoint(),
               new Point(Size_X, Size_Y)
            );
        }

        /// <summary>
        /// check if a tile exists in this chunk.
        /// </summary>
        public bool Contains(Vector2 tile)
        {
            bool found = false;
            for (int i = 0; i < ChunkIDMap.Length; i++)
            {
                for (int j = 0; j < ChunkIDMap[0].Length; j++)
                {
                    if (WorldMap.TerrainTileDictionary[ChunkIDMap[i][j]].Position == tile)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// check if a tile exists in this chunk.
        /// </summary>
        public Tile TileAround(Vector2 coords)
        {
            Tile found = null;
            Rectangle coords_rect = new Rectangle(new Vector2(coords.X + 16, coords.Y + 16).ToPoint(), new Point(2, 2));
            for (int i = 0; i < ChunkIDMap.Length; i++)
            {
                for (int j = 0; j < ChunkIDMap[0].Length; j++)
                {
                    if (WorldMap.TerrainTileDictionary[ChunkIDMap[i][j]].Collision_Rect.Intersects(coords_rect))
                    {
                        found = WorldMap.TerrainTileDictionary[ChunkIDMap[i][j]];
                        break;
                    }
                }
                if (found != null)
                    break;
            }

            return found;
        }

        /// <summary>
        /// returns a single tile based on a position from the game world.
        /// </summary>
        public Tile GetTile(Vector2 tile)
        {
            Tile found = null;
            for (int i = 0; i < ChunkIDMap.Length; i++)
            {
                for (int j = 0; j < ChunkIDMap[0].Length; j++)
                {
                    if (WorldMap.TerrainTileDictionary[ChunkIDMap[i][j]].Position == tile)
                    {
                        found = WorldMap.TerrainTileDictionary[ChunkIDMap[i][j]];
                        break;
                    }
                }
                if (found != null)
                {
                    break;
                }
            }

            return found;
        }

        public void Update(GameTime gt)
        {
            InRenderView = Camera2D.Is_In_Render_View_RectIntersectsCheck(this.Position, this.Size_X, this.Size_Y);
            if (InRenderView)
            {
                // update the chunk tiles.
                for (int y = 0; y < ChunkIDMap.Length; y++)
                {
                    for (int x = 0; x < ChunkIDMap[y].Length; x++)
                    {
                        WorldMap.TerrainTileDictionary[ChunkIDMap[y][x]].Update(gt);
                    }
                }
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            if (this.InRenderView)
            {
                for (int y = 0; y < ChunkIDMap.Length; y++)
                {
                    for (int x = 0; x < ChunkIDMap[y].Length; x++)
                    {
                        WorldMap.TerrainTileDictionary[ChunkIDMap[y][x]].Draw(sprite);
                    }
                }
            }
        }
    }
}