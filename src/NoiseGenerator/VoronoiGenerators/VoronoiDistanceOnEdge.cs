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
    /// Voronoi noise coloured by distance to the perpendicular bisector to the two nearest points.
    /// </summary>
    public class VoronoiDistanceOnEdge : IVoronoiGenerator
    {
        public ITexture RenderNoise(PointVoxelStore grid, bool cache)
        {
            float max = grid.Radius;
            ValueTexture texture = new ValueTexture(grid.ResolutionX, grid.ResolutionY, max, cache);
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {

                    //distance to bisector of both points


                    float distance = grid.GetDistanceToEdge(x, y, 2);
                    texture.SetPixel(x, y, distance);
                }
            }
            return texture;
        }
    }
}
