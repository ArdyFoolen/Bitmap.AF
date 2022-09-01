using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public class BitsPerPixel24 : BitsPerPixel
    {
        protected override int NbrOfBytes { get => (int)(width * 3); }

        public BitsPerPixel24(uint width, uint height) : base(width, height) { }

        protected override int Index(int x, int y)
        {
            int realY = (int)(height - 1 - y);
            return RowSize * realY + x * 3;
        }

        public override void SetPixelArray(Image image, byte[][] pixelArray)
        {
            image.Data.PixelArray = InitializePixelArray;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int index = Index(x, y);

                    image.Data.PixelArray[index] = pixelArray[y][x * 3];
                    image.Data.PixelArray[index + 1] = pixelArray[y][x * 3 + 1];
                    image.Data.PixelArray[index + 2] = pixelArray[y][x * 3 + 2];
                }
        }
    }
}
