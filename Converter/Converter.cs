using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Tubes3
{
    public class Converter
    {
        public static string ImageToAscii(string imagePath)
        {
            using Image<Rgba32> image = Image.Load<Rgba32>(imagePath);
            StringBuilder binaryStringBuilder = new StringBuilder();

            //ambil bagian tengah dari image
            int startX = Math.Max((image.Width - 30) / 2, 0);
            int startY = Math.Max((image.Height - 30) / 2, 0);
            int width = Math.Min(30, image.Width);
            int height = Math.Min(30, image.Height);

            //proses pixelnya 30x30
            //ngambil yang tengah-tengah aja
            for (int y = startY; y < startY + height; y++)
            {
                for (int x = startX; x < startX + width; x++)
                {
                    //ambil pixel-pixelnya
                    var pixel = image[x, y];
                    int grayValue = (int)((pixel.R + pixel.G + pixel.B) / 3.0);
                    binaryStringBuilder.Append(grayValue > 128 ? '1' : '0');
                }
            }

            //convert Ke binaryString
            string binaryString = binaryStringBuilder.ToString();

            //convert ke ASCII
            StringBuilder asciiStringBuilder = new StringBuilder();
            for (int i = 0; i < binaryString.Length; i += 8)
            {
                string byteString = binaryString.Substring(i, Math.Min(8, binaryString.Length - i));
                if (byteString.Length == 8)
                {
                    byte b = Convert.ToByte(byteString, 2);
                    asciiStringBuilder.Append((char)b);
                }
            }

            return asciiStringBuilder.ToString();
        }
    }
}
