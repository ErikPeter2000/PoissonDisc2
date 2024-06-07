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
    /// <summary>
    /// Defines a texture that stores a single value per pixel.
    /// </summary>
    public class ValueTexture : ITexture
    {
        public int DefinedChannels => 1;

        public int Width { get; }

        public int Height { get; }

        public float MaxDisplayValue { get; }

        public bool UseCache { get; private set; }

        public float[,][] ValueData { get; set; }
        private Bitmap _cacheBitmap;

        public ValueTexture(int width, int height, float maxDisplayValue, bool cache)
        {
            Width = width;
            Height = height;
            MaxDisplayValue = maxDisplayValue;
            UseCache = cache;
            ValueData = new float[Width, Height][];
            _cacheBitmap = new Bitmap(width, height);
        }
        public void DisableCache()
        {
            UseCache = false;
        }

        /// <summary>
        /// Draws the texture to a bitmap.
        /// </summary>
        private Bitmap renderTextureToBitmap()
        {
            _cacheBitmap = new Bitmap(Width, Height, _cacheBitmap.PixelFormat);
            var BoundsRect = new Rectangle(0, 0, _cacheBitmap.Width, _cacheBitmap.Height);
            BitmapData bmpData = _cacheBitmap.LockBits(BoundsRect, ImageLockMode.WriteOnly, _cacheBitmap.PixelFormat);
            IntPtr ptr = bmpData.Scan0;

            int byte_Count = bmpData.Stride * _cacheBitmap.Height;
            int bytesPerColor = bmpData.Stride / _cacheBitmap.Width;
            var rgbValues = new byte[byte_Count];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float value = ValueData[x, y][0];
                    int index = bmpData.Stride * y + x * bytesPerColor;
                    byte[] bytes = BitConverter.GetBytes((int)MathF.Min(value * MaxDisplayValue, MaxDisplayValue));
                    for (int i = 0; i < bytesPerColor; i++)
                    {
                        rgbValues[index + i] = bytes[i];
                    }
                }
            }
            Marshal.Copy(rgbValues, 0, ptr, byte_Count);
            _cacheBitmap.UnlockBits(bmpData);

            return _cacheBitmap;
        }

        /// <summary>
        /// Either returns a cached Bitmap or recalculates one from the texture. If one is already cached, the PixelFormat is ignored.
        /// </summary>
        /// <param name="ignoreCache">Whether to ignore any cached bitmap data. Instead, recalculate the bitmap.</param>
        public Bitmap GetBitmap(bool ignoreCache = false)
        {
            if (UseCache && !ignoreCache)
            {
                return _cacheBitmap;
            }
            else
            {
                return renderTextureToBitmap();
            }
        }

        /// <summary>
        /// Set a pixel in the texture.
        /// </summary>
        /// <param name="data">A normalised value representing the intensity of the pixel.</param>
        public void SetPixel(int x, int y, float data)
        {
            if (UseCache)
            {
                int value = (int)MathF.Min(data / MaxDisplayValue * 255, 255);
                _cacheBitmap.SetPixel(x, y, Color.FromArgb(value, value, value));
            }
            ValueData[x, y] = new float[] { data };
        }

        /// <param name="data">A normalised value representing the intensity of the pixel. Only uses the first index for value texture.</param>
        /// <inheritdoc/>
        public void SetPixel(int x, int y, float[] data)
        {
            SetPixel(x, y, data[0]);
        }
    }
}
