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
                    throw new NotImplementedException();
                case GenerationType.Cell:
                    throw new NotImplementedException();
                case GenerationType.RawSample:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
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
            ITexture[] channels = new ITexture[3];
            RGBTexture texture = new RGBTexture(width, height, parameters.Max(x => x.Radius), false);
            for (int i = 0; i < 3; i++)
            {
                var p = parameters[i];
                PoissonSampler sampler = new PoissonSampler(width, height, p.Radius, p.Seed, p.RejectionSamples);
                sampler.GenerateSamples();
                var grid = sampler.InternalGrid;
                INoiseAlgorithm noiseAlgorithm = getAlgorithm(p.Generationtype);
                channels[i] = noiseAlgorithm.RenderNoise(grid, false);
            }
            float[] data = new float[3];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; x++)
                {
                    data[0] = channels[0].ValueData[x, y][0];
                    data[1] = channels[1].ValueData[x, y][0];
                    data[2] = channels[2].ValueData[x, y][0];
                    data[4] = channels[4].ValueData[x, y][4];
                    texture.SetPixel(x, y, data);
                }
            }
            return texture;
        }
    }
}