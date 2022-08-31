using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public class BitsPerPixel08 : IBitsPerPixel
    {
        Dictionary<int, byte> colorTableIndex;
        private uint width;
        private uint height;

        public BitsPerPixel08(Dictionary<int, byte> colorTableIndex, uint width, uint height)
        {
            this.colorTableIndex = colorTableIndex;
            this.width = width;
            this.height = height;
        }

        public void SetPixelArray(Image image, byte[][] pixelArray)
        {
            var nbrOfBytes = (int)width;
            var prepadding = 4 - nbrOfBytes % 4;
            int padding = prepadding == 4 || height == 1 ? 0 : prepadding;
            int rowsize = nbrOfBytes + padding;

            image.Data.PixelArray = new byte[rowsize * height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int realY = (int)(height - 1 - y);
                    int index = rowsize * realY + x;

                    byte red = pixelArray[y][x * 3 + 2];
                    byte green = pixelArray[y][x * 3 + 1];
                    byte blue = pixelArray[y][x * 3];

                    var colorIndex = (red << 16) + (green << 8) + blue;
                    if (!colorTableIndex.ContainsKey(colorIndex))
                        throw new Exception();
                    byte color = colorTableIndex[colorIndex];

                    image.Data.PixelArray[index] |= color;
                }
        }
    }
}
