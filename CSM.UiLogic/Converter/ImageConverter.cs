using System.IO;
using System.Windows.Media.Imaging;

namespace CSM.UiLogic.Converter
{
   internal static class ImageConverter
    {
        public static BitmapSource BitmapFromBase64(string b64string)
        {
            var bytes = Convert.FromBase64String(b64string);

            using var stream = new MemoryStream(bytes);
            return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
        }

        public static string StringFromBitmap(string path)
        {
            byte[] imageArray = File.ReadAllBytes(path);
            return Convert.ToBase64String(imageArray);
        }
    }
}
