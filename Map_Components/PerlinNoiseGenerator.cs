namespace Engine_lib.Map_Components
{
    public class PerlinNoiseGenerator
    {
        private int[] _permutation;
        private int[] _p;

        public PerlinNoiseGenerator()
        {
            _permutation = new int[]
            {
            151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,
            8,99,37,240,21,10,23,190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,
            35,11,32,57,177,33,88,237,149,56,87,174,20,125,136,171,168,68,175,74,165,71,
            134,139,48,27,166,77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,
            41,55,46,245,40,244,102,143,54,65,25,63,161,1,216,80,73,209,76,132,187,208,
            89,18,169,200,196,135,130,116,188,159,86,164,100,109,198,173,186,3,64,52,
            217,226,250,124,123,5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,
            16,58,17,182,189,28,42,223,183,170,213,119,248,152,2,44,154,163,70,221,153,
            101,155,167,43,172,9,129,22,39,253,19,98,108,110,79,113,224,232,178,185,
            112,104,218,246,97,228,251,34,242,193,238,210,144,12,191,179,162,241,81,
            51,145,235,249,14,239,107,49,192,214,31,181,199,106,157,184,84,204,176,115,
            121,50,45,127,4,150,254,138,236,205,93,222,114,67,29,24,72,243,141,128,195,
            78,66,215,61,156,180
            };
            _p = new int[512];
            for (int i = 0; i < 512; i++)
                _p[i] = _permutation[i % 256];
        }

        /// <summary>
        /// use this function
        /// 
        /// generates a perlin noise map for games. 
        /// 
        /// user can specify the number of output options should be available. 
        /// </summary>
        /// <param name="width"> number of tiles in maps x axis </param>
        /// <param name="height"> number of tiles in maps y axis </param>
        /// <param name="scale"> between 0.001 and 0.1 </param>
        /// <param name="offsetX"> map offset to generate differing terrain. x axis </param>
        /// <param name="offsetY"> map offset to generate differing terrain. y axis </param>
        /// <returns></returns>
        public int[][] GenerateNoiseMap(int width, int height, double scale, int offsetX, int offsetY, int map_value_count)
        {
            int[][] noiseMap = new int[height][];
            for (int y = 0; y < height; y++)
            {
                noiseMap[y] = new int[width];
                for (int x = 0; x < width; x++)
                {
                    // Apply the offset to x and y coordinates
                    double noiseValue = Generate((x + offsetX) * scale, (y + offsetY) * scale);
                    noiseMap[y][x] = MapToInt(noiseValue * 2.7, -1, 1, 0, map_value_count);
                }
            }
            return noiseMap;
        }

        private double Generate(double x, double y)
        {
            // Determine grid cell coordinates
            int x0 = (int)Math.Floor(x);
            int x1 = x0 + 1;
            int y0 = (int)Math.Floor(y);
            int y1 = y0 + 1;

            // Relative position in grid cell
            double sx = x - x0;
            double sy = y - y0;

            // Generate gradient vectors at the corners of the grid cell
            double[] g00 = GenerateGradient(x0, y0);
            double[] g10 = GenerateGradient(x1, y0);
            double[] g01 = GenerateGradient(x0, y1);
            double[] g11 = GenerateGradient(x1, y1);

            // Compute the dot product between the gradient vector and the distance vector
            double n00 = Dot(g00, sx, sy);
            double n10 = Dot(g10, sx - 1, sy);
            double n01 = Dot(g01, sx, sy - 1);
            double n11 = Dot(g11, sx - 1, sy - 1);

            // Compute the fade curves for sx and sy
            double u = Fade(sx);
            double v = Fade(sy);

            // Interpolate between the four corners
            double nx0 = Lerp(n00, n10, u);
            double nx1 = Lerp(n01, n11, u);
            double nxy = Lerp(nx0, nx1, v);

            // Return the noise value, which is in the range [-1, 1]
            return nxy;
        }

        private double[] GenerateGradient(int x, int y)
        {
            // Simple hash function to generate gradient direction
            int hash = Hash(x, y);
            double angle = hash * (2.0 * Math.PI / 256.0); // 256 unique gradients
            return new double[] { Math.Cos(angle), Math.Sin(angle) };
        }

        private float Grad(int hash, float x, float y)
        {
            int h = hash & 7;
            float u = h < 4 ? x : y;
            float v = h < 4 ? y : x;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        private int Hash(int x, int y)
        {
            // Simple hash function based on bit manipulation
            int hash = x;
            hash = hash ^ y * 374761393; // large prime number
            hash = (hash << 13) ^ hash;
            return (hash * (hash * hash * 15731 + 789221) + 1376312589) & 0xFF; // return a value between 0-255
        }

        private double Dot(double[] g, double x, double y)
        {
            return g[0] * x + g[1] * y;
        }

        private double Fade(double t)
        {
            // 6t^5 - 15t^4 + 10t^3
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private double Lerp(double a, double b, double t)
        {
            return a + t * (b - a);
        }

        private int MapToInt(double value, double minSrc, double maxSrc, int minDst, int maxDst)
        {
            // Map a value from one range to another, then clamp to the destination range
            int mappedValue = (int)((value - minSrc) / (maxSrc - minSrc) * (maxDst - minDst) + minDst);
            return Math.Max(minDst, Math.Min(maxDst, mappedValue)); // Clamp between minDst and maxDst
        }
    }
}
