namespace MatrixClient.Converters
{
    using System;
    using System.IO;
    using System.Windows.Data;
    using System.Drawing;

    using System.Collections.Generic;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Drawing.Imaging;
    using System.Text.RegularExpressions;
    using System.Linq;

    /// <summary>
    /// Converter which creates a Default Avatar based on the initials of a name
    /// </summary>
    public class NameToAvatarConverter : IValueConverter
    {
        static Dictionary<int, byte[]> avatarCache = new Dictionary<int, byte[]>();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string name = (string)value;

            int hCode = name.GetHashCode();
            if (avatarCache.ContainsKey(hCode))
            {
                return avatarCache[hCode];
            }
            
            try
            {
                var avatarBytes = GenerateAvatar(name);
                avatarCache.Add(hCode, avatarBytes);
                return avatarBytes;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }      

        private static List<string> BackgroundColours = new List<string>
        {
            "#EEAD0E",
            "#8BBF61",
            "#DC143C",
            "#CD6889",
            "#8B8386",
            "#800080",
            "#9932CC",
            "#009ACD",
            "#00CED1",
            "#03A89E",
            "#00C78C",
            "#00CD66",
            "#66CD00",
            "#EEB422",
            "#FF8C00",
            "#EE4000",
            "#388E8E",
            "#8E8E38",
            "#7171C6",
        };

        private static byte[] GenerateAvatar(string name)
        {
            int imageSize = 160;
            int fontSize = 72;
                        
            var avatarString = ExtractInitialsFromName(name);

            var bmp = new Bitmap(imageSize, imageSize);
            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            var font = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(bmp);

            
            // simple algoprithm to choose a background color and make sure it stays the same, based on the name param
            var bgColorIndex = avatarString
                                    .ToCharArray()
                                    .ToList()
                                    .Sum(c => System.Convert.ToInt32(c)) % BackgroundColours.Count;
            
            var bgColour = BackgroundColours[bgColorIndex];
            graphics.Clear((Color)new ColorConverter().ConvertFromString(bgColour));
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.DrawString(avatarString, font, new SolidBrush(Color.WhiteSmoke), new RectangleF(0, 0, imageSize, imageSize), sf);

            graphics.Flush();

            var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);

            return ms.ToArray();
        }

        // see: https://stackoverflow.com/questions/10820273/regex-to-extract-initials-from-name
        /// <summary>
        /// Given a person's first and last name, we'll make our best guess to extract up to two initials, hopefully
        /// representing their first and last name, skipping any middle initials, Jr/Sr/III suffixes, etc. The letters 
        /// will be returned together in ALL CAPS, e.g. "TW". 
        /// 
        /// The way it parses names for many common styles:
        /// 
        /// Mason Zhwiti                -> MZ
        /// mason lowercase zhwiti      -> MZ
        /// Mason G Zhwiti              -> MZ
        /// Mason G. Zhwiti             -> MZ
        /// John Queue Public           -> JP
        /// John Q. Public, Jr.         -> JP
        /// John Q Public Jr.           -> JP
        /// Thurston Howell III         -> TH
        /// Thurston Howell, III        -> TH
        /// Malcolm X                   -> MX
        /// A Ron                       -> AR
        /// A A Ron                     -> AR
        /// Madonna                     -> M
        /// Chris O'Donnell             -> CO
        /// Malcolm McDowell            -> MM
        /// Robert "Rocky" Balboa, Sr.  -> RB
        /// 1Bobby 2Tables              -> BT
        /// Éric Ígor                   -> ÉÍ
        /// 행운의 복숭아                 -> 행복
        /// 
        /// </summary>
        /// <param name="name">The full name of a person.</param>
        /// <returns>One to two uppercase initials, without punctuation.</returns>
        private static string ExtractInitialsFromName(string name)
        {
            // first remove all: punctuation, separator chars, control chars, and numbers (unicode style regexes)
            string initials = Regex.Replace(name, @"[\p{P}\p{S}\p{C}\p{N}]+", "");

            // Replacing all possible whitespace/separator characters (unicode style), with a single, regular ascii space.
            initials = Regex.Replace(initials, @"\p{Z}+", " ");

            // Remove all Sr, Jr, I, II, III, IV, V, VI, VII, VIII, IX at the end of names
            initials = Regex.Replace(initials.Trim(), @"\s+(?:[JS]R|I{1,3}|I[VX]|VI{0,3})$", "", RegexOptions.IgnoreCase);

            // Extract up to 2 initials from the remaining cleaned name.
            initials = Regex.Replace(initials, @"^(\p{L})[^\s]*(?:\s+(?:\p{L}+\s+(?=\p{L}))?(?:(\p{L})\p{L}*)?)?$", "$1$2").Trim();

            if (initials.Length > 2)
            {
                // Worst case scenario, everything failed, just grab the first two letters of what we have left.
                initials = initials.Substring(0, 2);
            }

            return initials.ToUpperInvariant();
        }
    }
}
