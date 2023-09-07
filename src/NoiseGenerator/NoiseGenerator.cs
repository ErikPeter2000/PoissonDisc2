using PoissonDiskLogic;

namespace NoiseGeneration
{
    public static class NoiseGenerator
    {
        private static INoiseAlgorithm getAlgorithm(GenerationType type)
        {
            switch (type)
            {
                case GenerationType.Distance:
                    return new VoronoiDistance();
                case GenerationType.DistanceToEdge:
                    return new VoronoiDistanceOnEdge();
                case GenerationType.Cell:
                    return new VoronoiCell();
                case GenerationType.RawSample:
                    return new VoronoiRawSample();
                default:
                    return new VoronoiRawSample();
            }
        }
        public static ITexture Generate(int width, int height, RenderParameters[] parameters)
        {
            if (parameters.Length == 1)
                return generateValueTexture(width, height, parameters[0]);
            else
                return generateRGBATexture(width, height, parameters);
        }
        private static ITexture generateValueTexture(int width, int height, RenderParameters parameter)
        {
            PoissonSampler sampler = new PoissonSampler(width, height, parameter.Radius, parameter.Seed, parameter.RejectionSamples);
            sampler.GenerateSamples();
            var grid = sampler.InternalGrid;
            INoiseAlgorithm noiseAlgorithm = getAlgorithm(parameter.Generationtype);
            return noiseAlgorithm.RenderNoise(grid, true);
        }

        private static ITexture generateRGBATexture(int width, int height, RenderParameters[] parameters)
        {
            int channelCount = Math.Min(parameters.Length, 4);
            ITexture[] channels = new ITexture[channelCount];
            RGBTexture texture = new RGBTexture(width, height, parameters.Max(x => x.Radius), true, channelCount);
            for (int i = 0; i < channelCount; i++)
            {
                var p = parameters[i];
                PoissonSampler sampler = new PoissonSampler(width, height, p.Radius, p.Seed, p.RejectionSamples);
                sampler.GenerateSamples();
                var grid = sampler.InternalGrid;
                INoiseAlgorithm noiseAlgorithm = getAlgorithm(p.Generationtype);
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