using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public abstract class ColorTableIndex : BitsPerPixel
    {
        protected Dictionary<int, byte> colorTableIndex;

        public ColorTableIndex(Dictionary<int, byte> colorTableIndex, uint width, uint height) : base(width, height)
        {
            this.colorTableIndex = colorTableIndex;
        }

        protected abstract byte ShiftColor(int x, byte color);

        protected byte GetColor(byte[][] pixelArray, int x, int y)
        {
            byte red = pixelArray[y][x * 3 + 2];
            byte green = pixelArray[y][x * 3 + 1];
            byte blue = pixelArray[y][x * 3];

            var colorIndex = (red << 16) + (green << 8) + blue;
            if (!colorTableIndex.ContainsKey(colorIndex))
                throw new Exception();
            byte color = colorTableIndex[colorIndex];

            return ShiftColor(x, color);
        }
    }
}
