using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NoiseGeneration;
using PoissonDiskLogic;

namespace PoissonDisc2UI
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Bitmap texture;

        public Bitmap Texture
        {
            get { return texture; }
            set { texture = value; RaisePropertyChanged(nameof(TextureDisplay)); }
        }

        public ImageSource TextureDisplay
        {
            get {
                if (texture == null)
                    return null;
                else
                    return Texture.ExtractImageSource(); }
        }
        public void Render(int width, int height, int radius, int seed)
        {
            var bmp = NoiseGenerator.Generate(width, height, new RenderParameters []{ new RenderParameters(GenerationType.Distance, radius, seed)});
            texture = bmp.GetBitmap();
            //texture = new Bitmap(width, height);
            //foreach (var point in sampler.Points)
            //{
            //    texture.SetPixel((int)point.X, (int)point.Y, System.Drawing.Color.Red);
            //}
            RaisePropertyChanged(nameof(TextureDisplay));
        }
    }
}
