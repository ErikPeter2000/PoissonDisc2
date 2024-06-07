using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGeneration
{
    public static class NoiseExporter
    {
        public static void ExportBitmap(Bitmap bitmap, string path)
        {
            string type = Path.GetExtension(path);
            ImageFormat format = parseImageFormat(type);
            bitmap.Save(path, format);
        }
        public static readonly string SaveFilter = "Bitmap Image|*.bmp|Gif Image|*.gif|JPEG Image|*.jpg;*.jpeg|PNG Image|*.png|TIFF Image|*.tiff|Exif Image|*.exif|Windows Metafile|*.wmf|Enhanced Metafile|*.emf|Icon Image|*.ico";
        private static readonly ImageFormat default_format = ImageFormat.Png;
        private static ImageFormat parseImageFormat(string type)
        {
            return type switch
            {
                ".bmp" => ImageFormat.Bmp,
                ".gif" => ImageFormat.Gif,
                ".jpg" => ImageFormat.Jpeg,
                ".jpeg" => ImageFormat.Jpeg,
                ".png" => ImageFormat.Png,
                ".tiff" => ImageFormat.Tiff,
                ".exif" => ImageFormat.Exif,
                ".wmf" => ImageFormat.Wmf,
                ".emf" => ImageFormat.Emf,
                ".ico" => ImageFormat.Icon,
                _ => default_format,
            };
        }
    }
}
