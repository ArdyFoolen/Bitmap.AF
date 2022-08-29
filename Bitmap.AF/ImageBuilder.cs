using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public partial class Image
    {
        public class ImageBuilder
        {
            private IInfoHeaderFactory infoHeaderFactory = new OS21XHeaderFactory();
            //private IInfoHeaderFactory infoHeaderFactory = new InfoHeaderFactory();

            private uint width;
            private uint height;
            private BitmapData bitmapData;

            private bool usesColorTable = false;
            private Dictionary<int, byte> colorTableIndex = new Dictionary<int, byte>();
            private int BytesPerPixel { get => usesColorTable ? 1 : 3; }

            private int Padding
            {
                get
                {
                    var padding = (int)(4 - ((width * BytesPerPixel) % 4));
                    return padding == 4 || height == 1 ? 0 : padding;
                }
            }
            private int RowSize
            {
                get
                {
                    return (int)(width * BytesPerPixel + Padding);
                }
            }

            public Image Build()
            {
                Guard.Conforms(() => width != 0, "Width cannot be 0");
                Guard.Conforms(() => height != 0, "Height cannot be 0");
                Guard.Conforms(() => bitmapData.PixelArray.Length == (RowSize * Math.Abs(height)), "Bitmap pixel array not correct size");

                var image = new Image();

                image.InfoHeader = infoHeaderFactory.Create(width, height);
                image.Data = bitmapData;

                image.InfoHeader.BitsPerPixel = 24;
                image.InfoHeader.ColorsUsed = (uint)colorTableIndex.Count;
                if (usesColorTable)
                {
                    image.InfoHeader.BitsPerPixel = 8;
                    image.ColorTable = colorTableIndex.Keys.SelectMany(s => s.ToBytes()).ToArray();
                }

                image.Header.OffsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
                image.Header.FileSize = image.Header.OffsetPixelArray + image.Data.PixelArray.Length;

                return image;
            }

            public ImageBuilder UseColorTable()
            {
                Guard.Conforms(() => bitmapData == null, "Not allowed to use colors table");

                infoHeaderFactory = new InfoHeaderFactory();
                usesColorTable = true;

                return this;
            }

            public ImageBuilder WithWith(uint width)
            {
                this.width = width;
                return this;
            }

            public ImageBuilder WithHeight(uint height)
            {
                this.height = height;
                return this;
            }

            public ImageBuilder WithBitmapData(BitmapData bitmapData)
            {
                this.bitmapData = bitmapData;
                return this;
            }

            public ImageBuilder SetPixel(int x, int y, Color color)
                => SetPixel(x, y, color.Red, color.Green, color.Blue);

            public ImageBuilder SetPixel(int x, int y, byte red, byte green, byte blue)
            {
                Guard.Conforms(() => x >= 0 && x < width, "X out of range");
                Guard.Conforms(() => y >= 0 && y < height, "Y out of range");
                Guard.Conforms(() => (RowSize * height) != 0, "Width or Height are not set");

                InitializeBitmapData();

                int realY = (int)(height - 1 - y);
                int index = RowSize * realY + x * BytesPerPixel;
                SetPixel(red, green, blue, index);

                return this;
            }

            public ImageBuilder SetRectangle(Rectangle rectangle, Color color)
                => SetRectangle(rectangle, color.Red, color.Green, color.Blue);

            public ImageBuilder SetRectangle(Rectangle rectangle, byte red, byte green, byte blue)
            {
                for (int r = 0; r < rectangle.Height; r++)
                    for (int c = 0; c < rectangle.Width; c++)
                        SetPixel(rectangle.X + c, rectangle.Y + r, red, green, blue);

                return this;
            }

            private void SetPixel(byte red, byte green, byte blue, int index)
            {
                if (usesColorTable)
                {
                    //var colorIndex = (blue << 16) + (green << 8) + red;
                    var colorIndex = (red << 16) + (green << 8) + blue;
                    if (!colorTableIndex.ContainsKey(colorIndex))
                        colorTableIndex.Add(colorIndex, (byte)colorTableIndex.Count);
                    bitmapData.PixelArray[index] = colorTableIndex[colorIndex];
                }
                else
                {
                    bitmapData.PixelArray[index + 2] = red;
                    bitmapData.PixelArray[index + 1] = green;
                    bitmapData.PixelArray[index] = blue;
                }
            }

            private void InitializeBitmapData()
            {
                if (bitmapData != null)
                    return;

                bitmapData = new BitmapData();
                bitmapData.PixelArray = new byte[RowSize * height];
            }
        }
    }
}
