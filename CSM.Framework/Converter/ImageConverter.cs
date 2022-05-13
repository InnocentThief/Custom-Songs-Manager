using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace CSM.Framework.Converter
{
    public static class ImageConverter
    {
        public static BitmapSource BitmapFromBase64(string b64string)
        {
            var bytes = Convert.FromBase64String(b64string);

            using (var stream = new MemoryStream(bytes))
            {
                return BitmapFrame.Create(stream,
                    BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }
    }
}