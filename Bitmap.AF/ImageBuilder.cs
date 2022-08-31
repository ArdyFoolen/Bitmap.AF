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

            public Image Build()
            {
                Guard.Conforms(() => width != 0, "Width cannot be 0");
                Guard.Conforms(() => height != 0, "Height cannot be 0");
                Guard.Conforms(() => pixelArray != null && pixelArray.Length == height, "Bitmap pixel array not correct height");
                Guard.Conforms(() => pixelArray.All(a => a != null && a.Length == width * 3), "Bitmap pixel array not correct width");

                var image = new Image();

                SetColorTableIndex();

                image.InfoHeader = infoHeaderFactory.Create(width, height, colorTableIndex);
                image.ColorTable = colorTableIndex.Keys.SelectMany(s => s.ToBytes()).ToArray();

                infoHeaderFactory.BitsPerPixel.SetPixelArray(image, pixelArray);

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
