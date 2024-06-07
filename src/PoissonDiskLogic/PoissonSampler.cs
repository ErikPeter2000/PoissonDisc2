using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PoissonDiskLogic
{
    /// <summary>
    /// Generate samples using Poisson disk sampling.
    /// </summary>
    public class PoissonSampler
    {
        public PointVoxelStore InternalGrid { get; set; }
        public bool IsRendered { get; private set; } = false;
        public int RejectionCount { get; }
        public float Radius { get; }
        public int Seed { get; }
        private Random random;
        public Vector2[] Points
        {
            get
            {
                if (InternalGrid != null)
                    return InternalGrid.InternalPoints;
                else
                    return Array.Empty<Vector2>();
            }
        }
        public PoissonSampler(int width, int height, int radius, int seed, int rejectionCount = 4)
        {
            Radius = radius;
            RejectionCount = rejectionCount;
            Seed = seed;
            random = new Random(seed);
            InternalGrid = new PointVoxelStore(width, height, radius);
        }
        public void GenerateSamples()
        {
            Vector2 firstSample = new Vector2(InternalGrid.ResolutionX / 2, InternalGrid.ResolutionY / 2);

            Queue<Vector2> activeSamples = new Queue<Vector2>();
            activeSamples.Enqueue(firstSample);
            var cw = new Stopwatch();
            cw.Start();
            while (activeSamples.Count > 0)
            {
                Vector2 currentSample = activeSamples.Dequeue();
                for (int i = 0; i < RejectionCount; i++)
                {
                    float angleToCentre = MathF.Atan2(currentSample.Y - firstSample.Y, currentSample.X - firstSample.X);
                    float angle = (float)(angleToCentre + (random.NextDouble() * 2 - 1) * Math.PI * 2);
                    float distance = (float)(random.NextDouble() * (2 * Radius) + Radius);
                    Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    Vector2 candidate = currentSample + direction * distance;
                    if (InternalGrid.IsValidSample(candidate, Radius))
                    {
                        InternalGrid.Add(candidate);
                        activeSamples.Enqueue(candidate);
                    }
                }
            }
        }
    }
}
