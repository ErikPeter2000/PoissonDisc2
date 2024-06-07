using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGeneration.Textures
{
    /// <summary>
    /// Stores and manipulates voronoi noise data after generation.
    /// </summary>
    public interface ITexture
    {
        public int Width { get; }
        public int Height { get; }
        /// <summary>
        /// The number of channels in the texture.
        /// </summary>
        public int DefinedChannels { get; }
        /// <summary>
        /// The brightest texture value, used when normalising data before rendering.
        /// </summary>
        public float MaxDisplayValue { get; }
        public bool UseCache { get; }
        /// <summary>
        /// Get the bitmap representation of the texture.
        /// </summary>
        public Bitmap GetBitmap(bool ignoreCache = false);

        /// <summary>
        /// Set a pixel in the texture.
        /// </summary>
        /// <param name="data">RGBA normalised intensities.</param>
        public void SetPixel(int x, int y, float[] data);
        public float[,][] ValueData { get; set; }
    }
}
