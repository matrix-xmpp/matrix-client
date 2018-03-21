namespace MatrixClient.Converters
{
    using MatrixClient.Images;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    public class ByteArrayToBitmapImageConverter : IValueConverter
    {
        static Dictionary<int, BitmapImage> avatarCache = new Dictionary<int, BitmapImage>();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            
            try
            {
                var imagesBytes = (byte[]) value;
                int hCode = imagesBytes.GetHashCode();

                if (avatarCache.ContainsKey(hCode))
                {
                    return avatarCache[hCode];
                }
                
                if (ImageUtils.GetImageFormat(imagesBytes) == Images.ImageFormat.Webp)
                {                    
                    // WebP images
                    using (WebP webp = new WebP())
                    {
                        var bmp = webp.Decode(imagesBytes);                        
                        var bitmapImage = BitmapToBitmapImage(bmp);
                        avatarCache.Add(hCode, bitmapImage);
                        return bitmapImage;
                    }                    
                }
                else
                {
                    // all other images
                    var bitmapImage = ConvertByteArrayToBitmapImage(imagesBytes);
                    avatarCache.Add(hCode, bitmapImage);
                    return bitmapImage;                    
                }
                
            }
            catch(Exception)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private BitmapImage ConvertByteArrayToBitmapImage(Byte[] bytes)
        {            
            MemoryStream stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();

            return image;            
        }

        private static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }        
    }
}
