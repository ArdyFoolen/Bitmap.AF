using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public class BitsPerPixel04 : ColorTableIndex
    {
        protected override int NbrOfBytes { get => (int)Math.Ceiling((decimal)width / 2); }

        public BitsPerPixel04(Dictionary<int, byte> colorTableIndex, uint width, uint height) : base(colorTableIndex, width, height) { }

        protected override int Index(int x, int y)
        {
            int realY = (int)(height - 1 - y);
            return RowSize * realY + x / 2;
        }

        protected override byte ShiftColor(int x, byte color)
            => (byte)(color << (x % 2 * 4));

        public override void SetPixelArray(Image image, byte[][] pixelArray)
        {
            image.Data.PixelArray = InitializePixelArray;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int index = Index(x, y);
                    image.Data.PixelArray[index] |= GetColor(pixelArray, x, y);
                }
        }
    }
}
