using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine_lib.Map_Components
{
    public class WorldMap
    {
        public static Tile IsMouseOverTile { get; set; }

        public static Dictionary<string, Tile> TerrainTileDictionary { get; protected set; }
        public static MapChunk[][] World_Chunks { get; protected set; }

        public string MapName { get; protected set; }
        public int MapWidth { get; protected set; }
        public int MapHeight { get; protected set; }
        public int offset_x = 1000;
        public int offset_y = 1000;

        /// <summary>
        /// Ensure that 
        /// Grass, Mud, Water, Sand, Mountain tiles 
        /// are already added to your texture dictionary. this is a work in progress.
        /// </summary>
        public WorldMap(int x, int y, string mapname = "default")
        {
            MapWidth = x;
            MapHeight = y;

            TerrainTileDictionary = new Dictionary<string, Tile>();

            var world_id_map = BuildMap(offset_x, offset_y);

            World_Chunks = SplitIntoChunks(world_id_map, 32, 32);
        }

        public WorldMap(int x, int y, int offx, int offy, string mapname = "default")
        {
            MapWidth = x;
            MapHeight = y;
            offset_x = offx;
            offset_y = offy;

            TerrainTileDictionary = new Dictionary<string, Tile>();

            var world_id_map = BuildMap(offset_x, offset_y);

            World_Chunks = SplitIntoChunks(world_id_map, 32, 32);
        }

        /// <summary>
        /// builds the world using procedural generation, applying the settings provided by the player/developer
        /// </summary>
        protected string[][] BuildMap(int xoffset, int yoffset)
        {
            string[][] world_id_map = new string[MapHeight][];

            //var world = new Tile[MapHeight][];
            var perlin_gen = new PerlinNoiseGenerator();
            var perlin_map = perlin_gen.GenerateNoiseMap(MapWidth, MapHeight, scale: 0.0095f, xoffset, yoffset, 10);

            for (int i = 0; i < perlin_map.Length; i++)
            {
                world_id_map[i] = new string[MapWidth];

                for (int j = 0; j < perlin_map[i].Length; j++)
                {
                    var t = new Tile($"base_tile-{i}-{j}", $"base_tile-{i}-{j}", "base_map_texture", new Vector2(j * 32, i * 32), false);

                    if (t != null)
                    {
                        // add tile data to the dictionary
                        TerrainTileDictionary.Add(t.object_name, t);
                        // store tile id reference for later use.
                        world_id_map[i][j] = t.object_name;
                    }
                    else
                    {
                        throw new Exception("Terrain tile data should not be null. Error in map build method.");
                    }
                }
            }

            return world_id_map; //world;
        }

        public virtual void Update(GameTime gt)
        {
            for (int i = 0; i < World_Chunks.Length; i++)
            {
                for (int j = 0; j < World_Chunks[i].Length; j++)
                {
                    World_Chunks[i][j].Update(gt);
                }
            }
        }

        public virtual void Draw(SpriteBatch sprite)
        {
            for (int i = 0; i < World_Chunks.Length; i++)
            {
                for (int j = 0; j < World_Chunks[i].Length; j++)
                {
                    World_Chunks[i][j].Draw(sprite);
                }
            }
        }

        /// <summary>
        /// this function splits an existing world into parts. 
        /// this creates chunks making rendering much smoother. 
        /// </summary>
        private static MapChunk[][] SplitIntoChunks(string[][] tiles, int chunkWidth, int chunkHeight)
        {
            int count = 0;
            // Calculate the number of chunks horizontally and vertically
            int chunkColumns = tiles[0].Length / chunkWidth;
            int chunkRows = tiles.Length / chunkHeight;

            // Create the 2D array for MapChunks
            MapChunk[][] chunks = new MapChunk[chunkRows][];

            for (int row = 0; row < chunkRows; row++)
            {
                chunks[row] = new MapChunk[chunkColumns];
                for (int col = 0; col < chunkColumns; col++)
                {
                    // Create a chunk
                    string[][] chunkTiles = new string[chunkHeight][];

                    for (int y = 0; y < chunkHeight; y++)
                    {
                        chunkTiles[y] = new string[chunkWidth];
                        for (int x = 0; x < chunkWidth; x++)
                        {
                            // Copy tiles into the chunk
                            chunkTiles[y][x] = tiles[row * chunkHeight + y][col * chunkWidth + x];
                        }
                    }
                    // Initialize the MapChunk with the chunk tiles
                    chunks[row][col] = new MapChunk(chunkTiles, count.ToString());
                }
            }
            return chunks;
        }

        /// <summary>
        /// Gets a tile around a given position. if rectagles intersect.
        /// 
        /// returns null if no tile found.
        /// </summary>
        public static Tile GetTileAtPosition(Vector2 pos)
        {
            Tile rtn = null;

            MapChunk ch = null;
            var pos_rect = new Rectangle(pos.ToPoint(), new Point(8, 8));
            for (int i = 0; i < World_Chunks.Length; i++)
            {
                for (int j = 0; j < World_Chunks[i].Length; j++)
                {
                    var chunk_rect = World_Chunks[i][j].Rect;
                    if (chunk_rect.Intersects(pos_rect))
                    {
                        ch = World_Chunks[i][j];
                        break;
                    }
                }

                if (ch != null)
                    break;
            }

            if (ch == null)
                return null;

            var item = ch.TileAround(pos);
            if (item != null)
                rtn = item;

            return rtn;
        }
    }
}
