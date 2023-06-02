using PoissonDiskLogic;

namespace NoiseGeneration
{
    public interface INoiseAlgorithm
    {
        public ITexture RenderNoise(Grid grid, bool cache);
    }
}