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
    /// Voronoi noise coloured by cell.
    /// </summary>
    public class VoronoiCell : IVoronoiGenerator
    {
        public ITexture RenderNoise(PointVoxelStore grid, bool cache)
        {
            float max = grid.Radius;
            ValueTexture texture = new (grid.ResolutionX, grid.ResolutionY, max, cache);
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    var point = grid.NearestDistance(x, y, 2).Item2;
                    float value = (point.X * 53 + point.Y * 177) % 21 / 21 * max;
                    texture.SetPixel(x, y, value);
                }
            }
            return texture;
        }
    }
}
