using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Converter
{
    public class Converter
    {
        public string ImageToAscii(string imagePath)
        {
            using (Bitmap image = new Bitmap(imagePath))
            {
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
                        Color pixel = image.GetPixel(x, y);
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
                    string byteString = binaryString.Substring(
                        i,
                        Math.Min(8, binaryString.Length - i)
                    );
                    if (byteString.Length == 8)
                    {
                        byte b = Convert.ToByte(byteString, 2);
                        asciiStringBuilder.Append((char)b);
                    }
                }

                return asciiStringBuilder.ToString();
            }
        }

        //testing
        static void Main(string[] args)
        {
            Converter converter = new Converter();
            string imagePath = "test.jpg";
            string asciiPattern = converter.ImageToAscii(imagePath);
            Console.WriteLine(asciiPattern);
        }
    }
}
