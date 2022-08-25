using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public partial class Image
    {
        public class ImageBuilder
        {
            private readonly int bitsPerPixel = 3;
            
            private ushort width;
            private short height;
            private BitmapData bitmapData;

            private int Padding
            {
                get
                {
                    var padding = 4 - ((width * bitsPerPixel) % 4);
                    return padding == 4 || Math.Abs(height) == 1 ? 0 : padding;
                }
            }
            private int RowSize
            {
                get
                {
                    return width * bitsPerPixel + Padding;
                }
            }

            public Image Build()
            {
                Guard.Conforms(() => width != 0, "Width cannot be 0");
                Guard.Conforms(() => height != 0, "Height cannot be 0");
                Guard.Conforms(() => bitmapData.PixelArray.Length == (RowSize * Math.Abs(height)), "Bitmap pixel array not correct size");

                var image = new Image();
                image.OS21XHeader.Width = width;
                image.OS21XHeader.Height = height;

                image.Data = bitmapData;
                image.Header.FileSize = image.Header.OffsetPixelArray + image.Data.PixelArray.Length;

                return image;
            }

            public ImageBuilder WithWith(ushort width)
            {
                this.width = width;
                return this;
            }

            public ImageBuilder WithHeight(short height)
            {
                this.height = height;
                return this;
            }

            public ImageBuilder WithBitmapData(BitmapData bitmapData)
            {
                this.bitmapData = bitmapData;
                return this;
            }

            public ImageBuilder SetPixel(int x, int y, byte red, byte green, byte blue)
            {
                Guard.Conforms(() => x >= 0 && x < width, "X out of range");
                Guard.Conforms(() => y >= 0 && y < height, "Y out of range");
                Guard.Conforms(() => (RowSize * Math.Abs(height)) != 0, "Width or Height are not set");

                InitializeBitmapData();

                int realY = height - 1 - y;
                int index = RowSize * realY + x * bitsPerPixel;
                bitmapData.PixelArray[index+2] = red;
                bitmapData.PixelArray[index + 1] = green;
                bitmapData.PixelArray[index] = blue;

                return this;
            }

            private void InitializeBitmapData()
            {
                if (bitmapData != null)
                    return;

                bitmapData = new BitmapData();
                bitmapData.PixelArray = new byte[RowSize * Math.Abs(height)];
            }
        }
    }
}
