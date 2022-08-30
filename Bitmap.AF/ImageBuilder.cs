using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Bitmap.AF
{
    public partial class Image
    {
        public class ImageBuilder
        {
            private IInfoHeaderFactory infoHeaderFactory = new OS21XHeaderFactory();

            private uint width;
            private uint height;
            private byte[][] pixelArray;

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
                Guard.Conforms(() => pixelArray != null && pixelArray.Length == height, "Bitmap pixel array not correct height");
                Guard.Conforms(() => pixelArray.All(a => a != null && a.Length == width * 3), "Bitmap pixel array not correct width");

                var image = new Image();

                SetColorTableIndex();

                image.InfoHeader = infoHeaderFactory.Create(width, height, (uint)colorTableIndex.Count);
                image.ColorTable = colorTableIndex.Keys.SelectMany(s => s.ToBytes()).ToArray();

                if (usesColorTable)
                    SetPixelArrayColorTable(image);
                else
                    SetPixelArrayBPP24(image);

                image.Header.OffsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
                image.Header.FileSize = image.Header.OffsetPixelArray + image.Data.PixelArray.Length;

                return image;
            }

            public ImageBuilder UseInfoHeader()
            {
                if (infoHeaderFactory is InfoHeaderFactory)
                {
                    var factory = infoHeaderFactory as InfoHeaderFactory;
                    factory.UseColorTable = usesColorTable;
                    return this;
                }

                infoHeaderFactory = new InfoHeaderFactory(usesColorTable);

                return this;
            }

            public ImageBuilder UseOS21XHeader()
            {
                if (infoHeaderFactory is OS21XHeaderFactory)
                    return this;

                infoHeaderFactory = new OS21XHeaderFactory();

                return this;
            }

            public ImageBuilder UseColorTable()
            {
                usesColorTable = true;

                return UseInfoHeader();
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

            public ImageBuilder SetPixel(int x, int y, Color color)
                => SetPixel(x, y, color.Red, color.Green, color.Blue);

            public ImageBuilder SetPixel(int x, int y, byte red, byte green, byte blue)
            {
                Guard.Conforms(() => x >= 0 && x < width, "X out of range");
                Guard.Conforms(() => y >= 0 && y < height, "Y out of range");

                InitializeBitmapData();

                if (pixelArray[y] == null)
                    pixelArray[y] = new byte[width * 3];
                pixelArray[y][x * 3 + 2] = red;
                pixelArray[y][x * 3 + 1] = green;
                pixelArray[y][x * 3] = blue;

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

            private void InitializeBitmapData()
            {
                if (pixelArray != null)
                    return;

                pixelArray = new byte[height][];
            }

            private void SetPixelArrayColorTable(Image image)
            {
                switch (image.InfoHeader.BitsPerPixel)
                {
                    case 1:
                        SetPixelArrayBPP01(image);
                        break;
                    case 4:
                        SetPixelArrayBPP04(image);
                        break;
                    case 8:
                        SetPixelArrayBPP08(image);
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(image.InfoHeader.BitsPerPixel));
                }
            }

            private void SetPixelArrayBPP01(Image image)
            {
                var nbrOfBytes = (int)Math.Ceiling((decimal)width / 8);
                var prepadding = 4 - nbrOfBytes % 4;
                int padding = prepadding == 4 || height == 1 ? 0 : prepadding;
                int rowsize = nbrOfBytes + padding;

                image.Data.PixelArray = new byte[rowsize * height];
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        int realY = (int)(height - 1 - y);
                        int index = rowsize * realY + x / 8;

                        byte red = pixelArray[y][x * 3 + 2];
                        byte green = pixelArray[y][x * 3 + 1];
                        byte blue = pixelArray[y][x * 3];

                        var colorIndex = (red << 16) + (green << 8) + blue;
                        if (!colorTableIndex.ContainsKey(colorIndex))
                            throw new Exception();
                        byte color = colorTableIndex[colorIndex];
                        color = (byte)(color << (7 - x % 8));

                        image.Data.PixelArray[index] |= color;
                    }
            }

            private void SetPixelArrayBPP04(Image image)
            {
                var nbrOfBytes = (int)Math.Ceiling((decimal)width / 2);
                var prepadding = 4 - nbrOfBytes % 4;
                int padding = prepadding == 4 || height == 1 ? 0 : prepadding;
                int rowsize = nbrOfBytes + padding;

                image.Data.PixelArray = new byte[rowsize * height];
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        int realY = (int)(height - 1 - y);
                        int index = rowsize * realY + x / 2;

                        byte red = pixelArray[y][x * 3 + 2];
                        byte green = pixelArray[y][x * 3 + 1];
                        byte blue = pixelArray[y][x * 3];

                        var colorIndex = (red << 16) + (green << 8) + blue;
                        if (!colorTableIndex.ContainsKey(colorIndex))
                            throw new Exception();
                        byte color = colorTableIndex[colorIndex];
                        color = (byte)(color << (x % 2 * 4));

                        image.Data.PixelArray[index] |= color;
                    }
            }

            private void SetPixelArrayBPP08(Image image)
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

            private void SetPixelArrayBPP24(Image image)
            {
                var prepadding = (int)(4 - ((width * BytesPerPixel) % 4));
                int padding = prepadding == 4 || height == 1 ? 0 : prepadding;
                int rowsize = (int)(width * BytesPerPixel + padding);

                image.Data.PixelArray = new byte[rowsize * height];
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        int realY = (int)(height - 1 - y);
                        int index = RowSize * realY + x * BytesPerPixel;
                        image.Data.PixelArray[index] = pixelArray[y][x];
                        image.Data.PixelArray[index + 1] = pixelArray[y][x + 1];
                        image.Data.PixelArray[index + 2] = pixelArray[y][x + 2];
                    }
            }

            private void SetColorTableIndex()
            {
                if (!usesColorTable)
                    return;

                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        var colorIndex = (pixelArray[y][x * 3 + 2] << 16) + (pixelArray[y][x * 3 + 1] << 8) + pixelArray[y][x * 3];
                        if (!colorTableIndex.ContainsKey(colorIndex))
                            colorTableIndex.Add(colorIndex, (byte)colorTableIndex.Count);
                    }
            }
        }
    }
}
