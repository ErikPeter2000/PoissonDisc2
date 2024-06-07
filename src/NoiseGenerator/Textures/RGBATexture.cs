using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGeneration.Textures
{
    // Maybe define separate implementations of ITexture for RGB and RGBA textures
    /// <summary>
    /// Texture that stores RGB/RGBA values.
    /// </summary>
    public class RGBATexture : ITexture
    {
        public int DefinedChannels { get; } = 3;

        public int Width { get; }

        public int Height { get; }

        public float MaxDisplayValue { get; }

        public bool UseCache { get; private set; }

        public float[,][] ValueData { get; set; }
        private Bitmap _cacheBitmap;

        private readonly bool Alpha;
        public RGBATexture(int width, int height, float maxDisplayValue, bool cache, int channels)
        {
            Width = width;
            Height = height;
            MaxDisplayValue = maxDisplayValue;
            UseCache = cache;
            ValueData = new float[Width, Height][];
            DefinedChannels = channels;
            if (DefinedChannels == 4)
            {
                Alpha = true;
                _cacheBitmap = new Bitmap(width, height, PixelFormat.Format64bppArgb);
            }
            else
                _cacheBitmap = new Bitmap(width, height, PixelFormat.Format48bppRgb);
        }
        public void DisableCache() => UseCache = false;

        private Bitmap renderBitmap()
        {
            _cacheBitmap = new Bitmap(Width, Height, _cacheBitmap.PixelFormat);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    SetPixel(x, y, ValueData[x, y]);
                }
            }

            return _cacheBitmap;
        }

        /// <summary>
        /// Either returns a cached Bitmap or renders. If one is already cached, the PixelFormat is ignored.
        /// </summary>
        /// <param name="format"></param>
        public Bitmap GetBitmap(bool ignoreCache) =>UseCache && !ignoreCache ? _cacheBitmap : renderBitmap();

        public void SetPixel(int x, int y, float[] data)
        {
            if (UseCache)
            {
                int r = (int)MathF.Min(data[0] / MaxDisplayValue * 255, 255);
                int g = (int)MathF.Min(data[1] / MaxDisplayValue * 255, 255);
                int b = 0;
                if (DefinedChannels > 2)
                    b = (int)MathF.Min(data[2] / MaxDisplayValue * 255, 255);
                if (Alpha)
                {
                    int a = (int)MathF.Min(data[3] / MaxDisplayValue * 255, 255);
                    _cacheBitmap.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
                else
                {
                    _cacheBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            ValueData[x, y] = data;
        }
    }
}
