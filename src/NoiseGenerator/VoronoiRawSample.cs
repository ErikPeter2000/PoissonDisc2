using PoissonDiskLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGeneration
{
    public class VoronoiRawSample : INoiseAlgorithm
    {
        public ITexture RenderNoise(Grid grid, bool cache)
        {
            float max = grid.Radius;
            ValueTexture texture = new ValueTexture(grid.ResolutionX, grid.ResolutionY, max, cache);
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    float distance = grid.NearestDistance(x, y, 0).Item1;
                    texture.SetPixel(x, y, distance <= 0.49?max:0);
                }
            }
            return texture;
        }
    }
}
