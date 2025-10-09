/*using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine_lib.Map_Components
{
    public class WorldMap
    {
        public static Tile MouseOverTile { get; set; }

        public static Dictionary<string, Tile> TerrainTileDictionary { get; protected set; }
        public static Dictionary<string, ResourceModel> ResourceTileDictionary { get; protected set; }

        public static MapChunk[][] World_Chunks { get; protected set; }
        public string MapName { get; protected set; }
        public int MapWidth { get; protected set; }
        public int MapHeight { get; protected set; }

        public WorldMap(int x, int y, string mapname = "default")
        {
            MapWidth = x;
            MapHeight = y;

            ResourceTileDictionary = new Dictionary<string, ResourceModel>();
            TerrainTileDictionary = new Dictionary<string, Tile>();

            var world_id_map = BuildMap(100, 2488);

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
                    var t = NextTemperateTile(perlin_map[i][j], i, j);

                    if (t.tile != null)
                    {
                        // add tile data to the dictionary
                        TerrainTileDictionary.Add(t.tile.object_name, t.tile);
                        // store tile id reference for later use.
                        world_id_map[i][j] = t.tile.object_name;

                        // once the perlin value is used we can simply overwrite in order to save compute/memory.
                        perlin_map[i][j] = t.tile_weight; // set the weight for pathfinding.
                    }
                    else
                    {
                        throw new Exception("Terrain tile data should not be null. Error in map build method.");
                    }
                }
            }

            return world_id_map; //world;
        }

        protected (Tile tile, int tile_weight) NextTemperateTile(int val, int i, int j)
        {
            Tile t = null;
            int tile_weight = 0; // can walk
            // this section builds the starting regions for the various races of the world. races towns may only be spawned in these regions. 
            // dungeons and POI's will be auto generated immediately for exploration and looting purposes.
            // the sand does not generate resources at this time. that could change?
            if (val == 0)
            {
                t = new Tile("Grass", 3, "Tile-" + j + "-" + i, "Grass", new Vector2(j * 32, i * 32), false);
            }
            else if (val == 1)
            {
                t = new Tile("Sand", 1, "Tile-" + j + "-" + i, "Sand", new Vector2(j * 32, i * 32), false);
            }

            // seperate the rest of world with water. -- no resources spawn in these areas.
            else if (val == 2)
            {
                t = new Tile("Water", 0, "Tile-" + j + "-" + i, "Water", new Vector2(j * 32, i * 32), true);
                tile_weight = 1; // cannot walk;
            }

            // build the main landscape. - the regions the various races tribes will eventually expand into.
            // no towns or dungeons will be auto spawned. they are going to be generated as the game progresses. 
            // this is the main terrain. simple grassy fields with scattered resources.
            else if (val == 3 || val == 4 || val == 5)
            {
                t = new Tile("Sand", 1, "Tile-" + j + "-" + i, "Sand", new Vector2(j * 32, i * 32), false);
            }

            // create the denser forested areas. these areas will have more wood and food, but be harder to traverse and more dangerous.
            // these areas are less travelled by any cerature. staying rich and reserved for generations.
            else if (val == 6 || val == 7 || val == 8)
            {
                t = new Tile("Grass", 3, "Tile-" + j + "-" + i, "Grass", new Vector2(j * 32, i * 32), false);
            }
            else if (val == 9)
            {
                t = new Tile("Mud", 2, "Tile-" + j + "-" + i, "Mud", new Vector2(j * 32, i * 32), false);
            }

            // then create the mountainous regions. whenever there is a number unassociated with the biome -- work around for complex code lol.
            else
            {
                t = new Tile("Mountain", 4, "Tile-" + j + "-" + i, "Mountain", new Vector2(j * 32, i * 32), true);
                tile_weight = 1; // cannot walk;
            }

            return (tile: t, tile_weight: tile_weight);
        }

        protected Tile NextWinterTile(int val, int i, int j)
        {
            Tile t = null;

            // create moutains
            if (val == 0)
            {
                t = new Tile("Mountain", 4, "Tile-" + j + "-" + i, "Mountain", new Vector2(j * 32, i * 32), true);
            }

            // create vegetation mud
            else if (val == 1)
            {
                t = new Tile("Mud", 2, "Tile-" + j + "-" + i, "Mud", new Vector2(j * 32, i * 32), false);
            }

            // create snowy regions
            else if (val == 2)
            {
                t = new Tile("Snow", 6, "Tile-" + j + "-" + i, "Snow", new Vector2(j * 32, i * 32), false);
            }

            // create icy regions -- shores
            else if (val == 3)
            {
                t = new Tile("Ice", 5, "Tile-" + j + "-" + i, "Ice", new Vector2(j * 32, i * 32), true);
            }

            // create water -- split up world a bit
            else if (val == 4)
            {
                t = new Tile("Water", 0, "Tile-" + j + "-" + i, "Water", new Vector2(j * 32, i * 32), true);
            }

            // create icy areas -- shores
            else if (val == 5)
            {
                t = new Tile("Ice", 5, "Tile-" + j + "-" + i, "Ice", new Vector2(j * 32, i * 32), true);
            }

            // create snowy areas
            else if (val == 6)
            {
                t = new Tile("Snow", 6, "Tile-" + j + "-" + i, "Snow", new Vector2(j * 32, i * 32), false);
            }
            else if (val == 7)
            {
                t = new Tile("Snow", 6, "Tile-" + j + "-" + i, "Snow", new Vector2(j * 32, i * 32), false);
            }
            else if (val == 8)
            {
               t = new Tile("Snow", 6, "Tile-" + j + "-" + i, "Snow", new Vector2(j * 32, i * 32), false);
            }

            // create mud for vegetation
            else if (val == 9)
            {
                t = new Tile("Mud", 2, "Tile-" + j + "-" + i, "Mud", new Vector2(j * 32, i * 32), false);
            }

            //then create mountains if any extra
            else
            {
                t = new Tile("Mountain", 4, "Tile-" + j + "-" + i, "Mountain", new Vector2(j * 32, i * 32), true);
            }

            return t;
        }

        protected Tile NextDesertTile(int val, int i, int j)
        {
            Tile t = null;


            // create the mountains
            if (val == 0)
            {
                t = new Tile("Mountain", 4, "Tile-" + j + "-" + i, "Mountain", new Vector2(j * 32, i * 32), true);
            }

            // create the sand
            else if (val == 1)
            {
                t = new Tile("Sand", 1, "Tile-" + j + "-" + i, "Sand", new Vector2(j * 32, i * 32), false);
            }
            else if (val == 2)
            {
                t = new Tile("Sand", 1, "Tile-" + j + "-" + i, "Sand", new Vector2(j * 32, i * 32), false);
            }
            else if (val == 3)
            {
                t = new Tile("Sand", 1, "Tile-" + j + "-" + i, "Sand", new Vector2(j * 32, i * 32), false);
            }
            else if (val == 4)
            {
                t = new Tile("Sand", 1, "Tile-" + j + "-" + i, "Sand", new Vector2(j * 32, i * 32), false);
            }            
            else if (val == 5)
            {
                t = new Tile("Sand", 1, "Tile-" + j + "-" + i, "Sand", new Vector2(j * 32, i * 32), false);
            }
            else if (val == 6)
            {
                t = new Tile("Sand", 1, "Tile-" + j + "-" + i, "Sand", new Vector2(j * 32, i * 32), false);
            }
            else if (val == 7)
            {
                t = new Tile("Sand", 1, "Tile-" + j + "-" + i, "Sand", new Vector2(j * 32, i * 32), false);
            }

            // create the oasis
            else if (val == 8)
            {
                t = new Tile("Mud", 2, "Tile-" + j + "-" + i, "Mud", new Vector2(j * 32, i * 32), false);
            }

            // create water for oasis and any extra should be mountains.
            else if (val == 9)
            {
                t = new Tile("Water", 0, "Tile-" + j + "-" + i, "Water", new Vector2(j * 32, i * 32), true);
            }
            else if (val == 10)
            {
                t = new Tile("Water", 0, "Tile-" + j + "-" + i, "Water", new Vector2(j * 32, i * 32), true);
            }
            else
            {
                t = new Tile("Mountain", 4, "Tile-" + j + "-" + i, "Mountain", new Vector2(j * 32, i * 32), true);
            }

            return t;
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
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Tile GetTileAround(Vector2 pos)
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
*/