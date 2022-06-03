using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace CSM.Framework.Converter
{
    /// <summary>
    /// Converts a base64 string to an image and back.
    /// </summary>
    public static class ImageConverter
    {
        /// <summary>
        /// Gets the image source for the given base64 string.
        /// </summary>
        /// <param name="b64string">The base64 string to convert to an image.</param>
        public static BitmapSource BitmapFromBase64(string b64string)
        {
            var bytes = Convert.FromBase64String(b64string);

            using (var stream = new MemoryStream(bytes))
            {
                return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        /// <summary>
        /// Gets the base64 string of the image file with the given path.
        /// </summary>
        /// <param name="path">Path of the image file.</param>
        public static string StringFromBitmap(string path)
        {
            byte[] imageArray = File.ReadAllBytes(path);
            return Convert.ToBase64String(imageArray);
        }
    }
}