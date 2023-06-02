using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGeneration
{
    public class RGBTexture : ITexture
    {
        public int Channels => 3;

        public int Width { get; }

        public int Height { get; }

        public float MaxDisplayValue { get; }

        public bool Cache { get; private set; }

        public float[,][] ValueData { get;  set; }
        private Bitmap _cacheBitmap;

        private bool Alpha;
        public RGBTexture(int width, int height, float maxDisplayValue, bool cache, bool alpha = false)
        {
            Width = width;
            Height = height;
            MaxDisplayValue = maxDisplayValue;
            Cache = cache;
            ValueData = new float[Width, Height][];
            Alpha = alpha;
            if (Alpha)
                _cacheBitmap = new Bitmap(width, height, PixelFormat.Format64bppArgb);
            else
                _cacheBitmap = new Bitmap(width, height, PixelFormat.Format48bppRgb);
        }
        public void DisableCache()
        {
            Cache = false;
        }

        private Bitmap renderBitmap()
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
        /// Either returns a cached Bitmap or renders. If one is already cached, the PixelFormat is ignored.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public Bitmap GetBitmap()
        {
            if (Cache)
            {
                return _cacheBitmap;
            }
            else
            {
                return renderBitmap();
            }
        }
        public void SetPixel(int x, int y, float data)
        {
            SetPixel(x, y, data);
        }

        public void SetPixel(int x, int y, float[] data)
        {
            if (Cache)
            {
                int r = (int)MathF.Min(data[0]/MaxDisplayValue*255, 255);
                int g = (int)MathF.Min(data[1] / MaxDisplayValue * 255, 255);
                int b = (int)MathF.Min(data[2] / MaxDisplayValue * 255, 255);
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
