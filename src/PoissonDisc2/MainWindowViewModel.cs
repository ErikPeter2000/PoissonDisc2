using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public int _selectedPass = -1;
        public int SelectedPass { get { return _selectedPass; } set { _selectedPass = value; RaisePropertyChanged(nameof(CanDelete)); } }
        public bool CanAdd { get { return ParamterList.Count < 4; } }
        public bool CanDelete { get { return SelectedPass >= 0 && ParamterList.Count > 1; } }
        public ObservableCollection<RenderParameters> ParamterList { get; set; } = new ObservableCollection<RenderParameters>();
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

        public void Load()
        {
            ParamterList.Add(new RenderParameters(GenerationType.Distance, 20, 0));
            ParamterList.CollectionChanged += ParamterList_CollectionChanged;
        }

        private void ParamterList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(CanAdd));
            RaisePropertyChanged(nameof(CanDelete));
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
            var bmp = NoiseGenerator.Generate(width, height, ParamterList.ToArray());
            texture = bmp.GetBitmap();
            //texture = new Bitmap(width, height);
            //foreach (var point in sampler.Points)
            //{
            //    texture.SetPixel((int)point.X, (int)point.Y, System.Drawing.Color.Red);
            //}
            RaisePropertyChanged(nameof(TextureDisplay));
        }
        public void AddPass()
        {
            if (ParamterList.Count < 4)
            {
                ParamterList.Add(new RenderParameters(GenerationType.Cell, 80, 0));
            }
        }
        public void RemovePass()
        {
            if (ParamterList.Count > 1 && SelectedPass>=0)
            {
                ParamterList.RemoveAt(SelectedPass);
            }
        }
    }
}
