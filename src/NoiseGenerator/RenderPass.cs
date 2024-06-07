using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoiseGeneration.Enums;

namespace NoiseGeneration
{
    public class RenderPassParameters
    {
        public IEnumerable<GeneratorType> GenerationTypes { get; } = Enum.GetValues(typeof(GeneratorType)).Cast<GeneratorType>();
        public bool ShowInPreview { get; set; }
        public int Radius { 
            get; 
            set; }
        public int Seed { get; set; }
        public int RejectionSamples { get; set; }
        public GeneratorType GenerationType { get; set; }
        public RenderPassParameters(GeneratorType generationType, int radius, int seed, bool preview = true, int rejectionSamples = 15)
        {
            GenerationType = generationType;
            ShowInPreview = preview;
            Radius = radius;
            Seed = seed;
            RejectionSamples = rejectionSamples;
        }
    }
}
