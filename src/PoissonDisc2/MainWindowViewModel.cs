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
using NoiseGeneration.Enums;
using PoissonDisc2UI.Utilities;
using PoissonDiskLogic;

namespace PoissonDisc2UI
{
    public class MainWindowViewModel : SetPropertyUtils, INotifyPropertyChanged
    {
        public int _selectedPass = -1;
        public int SelectedPass { 
            get => _selectedPass; 
            set { Set(ref _selectedPass, value); } 
        }
        public bool CanAddPass => RenderPassList.Count < 4;
        public bool CanDeletePass => RenderPassList.Count > 1;
        public bool CanExport => DisplayImage != null;

        /// <summary>
        /// The list of render passes.
        /// </summary>
        public ObservableCollection<RenderPassParameters> RenderPassList { get; set; } = new ObservableCollection<RenderPassParameters>();

        private Bitmap? _displayImage;
        /// <summary>
        /// The texture to display.
        /// </summary>
        public Bitmap? DisplayImage
        {
            get => _displayImage;
            set { Set(ref _displayImage, value); 
                RaisePropertyChanged(nameof(CanExport));
                RaisePropertyChanged(nameof(TextureDisplay));
            }
        }

        public void OnLoad()
        {
            RenderPassList.Add(new RenderPassParameters(GeneratorType.Distance, 20, 0));
            RenderPassList.CollectionChanged += ParameterList_CollectionChanged;
        }

        private void ParameterList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(CanAddPass));
            RaisePropertyChanged(nameof(CanDeletePass));
        }

        public ImageSource? TextureDisplay => DisplayImage?.ExtractImageSource();

        /// <summary>
        /// Calculate the voronoi texture.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Render(int width, int height)
        {
            var bmp = NoiseGenerator.GenerateTexture(width, height, RenderPassList.ToArray());
            DisplayImage = bmp.GetBitmap();
        }

        /// <summary>
        /// Add an empty render pass.
        /// </summary>
        public void AddPass()
        {
            if (RenderPassList.Count < 4)
                RenderPassList.Add(new RenderPassParameters(GeneratorType.Cell, 20, 0));
        }

        /// <summary>
        /// Remove the currently selected render pass.
        /// </summary>
        public void RemovePass()
        {
            if (RenderPassList.Count > 1 && SelectedPass >= 0)
            {
                int temp = SelectedPass;
                RenderPassList.RemoveAt(SelectedPass);
                SelectedPass = Math.Min(temp, RenderPassList.Count - 1);
            }
        }

        public void ExportBitmap()
        {
            if (DisplayImage == null)
                return;
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = NoiseExporter.SaveFilter,
                Title = "Save an Image File"
            };
            if (dialog.ShowDialog() == true)
            {
                NoiseExporter.ExportBitmap(DisplayImage, dialog.FileName);
            }
        }
    }
}
