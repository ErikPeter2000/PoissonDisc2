using NoiseGeneration.Enums;
using NoiseGeneration.Textures;
using NoiseGeneration.VoronoiSamplers;
using PoissonDiskLogic;

namespace NoiseGeneration
{
    public static class NoiseGenerator
    {
        /// <summary>
        /// Returns an instance of the noise algorithm based on the <paramref name="generationType"/>.
        /// </summary>
        /// <param name="generationType"></param>
        /// <returns>A new instance of the voronoi generation algorithm</returns>
        public static IVoronoiGenerator GetGeneratorInstance(GeneratorType generationType)
        {
            return generationType switch
            {
                GeneratorType.Distance => new VoronoiDistance(),
                GeneratorType.DistanceToEdge => new VoronoiDistanceOnEdge(),
                GeneratorType.Cell => new VoronoiCell(),
                GeneratorType.RawSample => new VoronoiRawSample(),
                _ => new VoronoiRawSample(),
            };
        }
        /// <summary>
        /// Generates a voronoi texture based on the parameters.
        /// </summary>
        /// <param name="width">The texture width</param>
        /// <param name="height">The texture height</param>
        /// <param name="parameters">The set of parameters for each render pass</param>
        /// <returns></returns>
        public static ITexture GenerateTexture(int width, int height, RenderPassParameters[] parameters)
        {
            if (parameters.Length == 1)
                return generateValueTexture(width, height, parameters[0]);
            else
                return generateRGBATexture(width, height, parameters);
        }
        /// <summary>
        /// Generate a single channel texture based on the parameters.
        /// </summary>
        private static ITexture generateValueTexture(int width, int height, RenderPassParameters parameter)
        {
            PoissonSampler sampler = new PoissonSampler(width, height, parameter.Radius, parameter.Seed, parameter.RejectionSamples);
            sampler.GenerateSamples();
            var grid = sampler.InternalGrid;
            IVoronoiGenerator noiseAlgorithm = GetGeneratorInstance(parameter.GenerationType);
            return noiseAlgorithm.RenderNoise(grid, true);
        }

        /// <summary>
        /// Generate a 4-channel texture based on the parameters.
        /// </summary>
        private static ITexture generateRGBATexture(int width, int height, RenderPassParameters[] parameters)
        {
            int channelCount = Math.Min(parameters.Length, 4);
            ITexture[] channels = new ITexture[channelCount];
            RGBATexture texture = new RGBATexture(width, height, parameters.Max(x => x.Radius), true, channelCount);
            for (int i = 0; i < channelCount; i++)
            {
                var p = parameters[i];
                PoissonSampler sampler = new PoissonSampler(width, height, p.Radius, p.Seed, p.RejectionSamples);
                sampler.GenerateSamples();
                var grid = sampler.InternalGrid;
                IVoronoiGenerator noiseAlgorithm = GetGeneratorInstance(p.GenerationType);
                channels[i] = noiseAlgorithm.RenderNoise(grid, false);
            }
            float[] data = new float[channelCount];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int i = 0; i < channelCount; i++)
                    {
                        data[i] = channels[i].ValueData[x, y][0];
                    }
                    texture.SetPixel(x, y, data);
                }
            }
            return texture;
        }
    }
}