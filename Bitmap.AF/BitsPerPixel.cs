using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public abstract class BitsPerPixel : IBitsPerPixel
    {
        protected uint width;
        protected uint height;

        protected abstract int NbrOfBytes { get; }
        protected byte[] InitializePixelArray { get => new byte[RowSize * height]; }

        public int Padding
        {
            get
            {
                var padding = 4 - NbrOfBytes % 4;
                return padding == 4 || height == 1 ? 0 : padding;
            }
        }

        public int RowSize { get => NbrOfBytes + Padding; }

        protected abstract int Index(int x, int y);

        public BitsPerPixel(uint width, uint height)
        {
            this.width = width;
            this.height = height;
        }

        public abstract void SetPixelArray(Image image, byte[][] pixelArray);
    }
}
