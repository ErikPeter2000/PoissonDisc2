using NoiseGeneration.Textures;
using PoissonDiskLogic;

namespace NoiseGeneration.VoronoiSamplers
{
    /// <summary>
    /// Interface for voronoi noise generation algorithms
    /// </summary>
    public interface IVoronoiGenerator
    {
        /// <summary>
        /// Renders the voronoi noise to a texture.
        /// </summary>
        /// <param name="grid">List of samples to use when calculating.</param>
        /// <param name="useCache">Whether to use caching to store the texture bitmap.</param>
        /// <returns>A texture representing the voronoi data.</returns>
        public ITexture RenderNoise(PointVoxelStore grid, bool useCache = false);
    }
}