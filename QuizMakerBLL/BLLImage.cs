using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QuizMakerBLL
{
    public class BLLImage
    {
        public byte[] ImageToByteArray(BitmapSource image)
        {
            using (var ms = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        public BitmapImage ByteArrayToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            using (var ms = new MemoryStream(imageData))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // fondamentale
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze(); // opzionale ma consigliato (thread-safe)
                return image;
            }
        }
    }
}
