using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGeneration
{
    public interface ITexture
    {
        public int Width { get; }
        public int Height { get; }
        public int Channels { get; }
        public float MaxDisplayValue { get; }
        public bool Cache { get; }
        public Bitmap GetBitmap();
        public void SetPixel(int x, int y, float[] data);
        public float[,][] ValueData { get; set; }
    }
}
