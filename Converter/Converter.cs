using System.Runtime.CompilerServices;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Tubes3
{
    public class Converter
    {
        public static string ImageToAscii(string imagePath)
        {
            Image<Rgba32> image = null; 
            try{
                image = Image.Load<Rgba32>(imagePath);
            }catch(SystemException){
                return "";
            }

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

        public static string ImageToBin(string imagePath){
           Image<Rgba32> image = null; 
            try{
                image = Image.Load<Rgba32>(imagePath);
            }catch(SystemException){
                return "";
            }

            StringBuilder binaryStringBuilder = new StringBuilder();

            //ambil bagian tengah dari image
            int startX = Math.Max((image.Width - 30) / 2, 0);
            int startY = Math.Max(image.Height / 2, 0);
            int width = Math.Min(30, image.Width);

            //proses pixelnya 30x30
            //ngambil yang tengah-tengah aja
            for (int x = startX; x < startX + width; x++)
            {
                    //ambil pixel-pixelnya
                var pixel = image[x, startY];
                int grayValue = (int)((pixel.R + pixel.G + pixel.B) / 3.0);
                binaryStringBuilder.Append(grayValue > 128 ? '1' : '0');
            }

            //convert Ke binaryString
            return binaryStringBuilder.ToString(); 
        }
        public static string ImageToAsciiStraight(string imagePath){
           Image<Rgba32> image = null; 
            try{
                image = Image.Load<Rgba32>(imagePath);
            }catch(SystemException){
                return "";
            }

            StringBuilder binaryStringBuilder = new StringBuilder();

            //ambil bagian tengah dari image
            int startX = Math.Max((image.Width - 30) / 2, 0);
            int startY = Math.Max(image.Height / 2, 0);
            int width = Math.Min(30, image.Width);

            //proses pixelnya 30x30
            //ngambil yang tengah-tengah aja
            for (int x = startX; x < startX + width; x++)
            {
                    //ambil pixel-pixelnya
                var pixel = image[x, startY];
                int grayValue = (int)((pixel.R + pixel.G + pixel.B) / 3.0);
                binaryStringBuilder.Append(grayValue > 128 ? '1' : '0');
            }

            //convert Ke binaryString
            string binaryString = binaryStringBuilder.ToString();
            StringBuilder asciiStringBuilder = new StringBuilder();
            int i = 0;
            while(i < binaryString.Length){
                int start = i; 
                int end = i + 8; 
                if(end > binaryString.Length){
                    end = binaryString.Length;
                }
                string substring = binaryString.Substring(start, end-start); 
                byte b = Convert.ToByte(substring, 2);
                asciiStringBuilder.Append((char)b);
                i += 8;

                // i++; 
            }
            // Console.WriteLine(asciiStringBuilder.ToString() + " Length: " + asciiStringBuilder.ToString().Length);
            // Console.WriteLine(StringToBinary(asciiStringBuilder.ToString()) + " Length:" + StringToBinary(asciiStringBuilder.ToString()).Length);

            return asciiStringBuilder.ToString(); 
        }

        static string StringToBinary(string input)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            StringBuilder binary = new StringBuilder();

            foreach (byte b in bytes)
            {
                binary.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            return binary.ToString();
        }
    }
}
