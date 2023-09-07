using PoissonDiskLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGeneration
{
    public class VoronoiDistanceOnEdge : INoiseAlgorithm
    {
        public ITexture RenderNoise(Grid grid, bool cache)
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
