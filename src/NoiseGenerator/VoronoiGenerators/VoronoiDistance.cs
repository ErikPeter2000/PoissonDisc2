using NoiseGeneration.Textures;
using PoissonDiskLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGeneration.VoronoiSamplers
{
    /// <summary>
    /// Voronoi noise coloured by distance.
    /// </summary>
    public class VoronoiDistance : IVoronoiGenerator
    {
        public ITexture RenderNoise(PointVoxelStore grid, bool cache)
        {
            float max = grid.Radius;
            ValueTexture texture = new ValueTexture(grid.ResolutionX, grid.ResolutionY, max, cache);
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    float distance = grid.NearestDistance(x, y, 2).Item1;
                    float value = distance;
                    texture.SetPixel(x, y, value);
                }
            }
            return texture;
        }
    }
}
