using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public class BitsPerPixel24 : IBitsPerPixel
    {
        private uint width;
        private uint height;

        public BitsPerPixel24(uint width, uint height)
        {
            this.width = width;
            this.height = height;
        }

        public void SetPixelArray(Image image, byte[][] pixelArray)
        {
            var prepadding = (int)(4 - ((width * 3) % 4));
            int padding = prepadding == 4 || height == 1 ? 0 : prepadding;
            int rowsize = (int)(width * 3 + padding);

            image.Data.PixelArray = new byte[rowsize * height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int realY = (int)(height - 1 - y);
                    int index = rowsize * realY + x * 3;
                    image.Data.PixelArray[index] = pixelArray[y][x * 3];
                    image.Data.PixelArray[index + 1] = pixelArray[y][x * 3 + 1];
                    image.Data.PixelArray[index + 2] = pixelArray[y][x * 3 + 2];
                }
        }
    }
}
