using Bitmap.AF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapTests.Helpers
{
    internal class AssertionHelper
    {
        public static void AssertBitmapFileHeader(BitmapFileHeader header, int fileSize, int offsetPixelArray)
        {
            Assert.NotNull(header);
            Assert.That(0x42, Is.EqualTo(header.HeaderField[0]));
            Assert.That(0x4D, Is.EqualTo(header.HeaderField[1]));
            Assert.That(fileSize, Is.EqualTo(header.FileSize));
            Assert.That(offsetPixelArray, Is.EqualTo(header.OffsetPixelArray));
            Assert.That(14, Is.EqualTo(header.HeaderSize));
        }

        public static void AssertBitmapOS21XHeader(IBitmapInfoHeader infoHeader, ushort height, ushort width)
        {
            OS21XBitmapHeader header = infoHeader as OS21XBitmapHeader;
            Assert.NotNull(header);
            Assert.That(12, Is.EqualTo(header.HeaderSize));
            Assert.That(height, Is.EqualTo(header.Height));
            Assert.That(width, Is.EqualTo(header.Width));
            Assert.That(1, Is.EqualTo(header.NumberOfColorPlanes));
            Assert.That(24, Is.EqualTo(header.BitsPerPixel));
        }

        public static void AssertBitmapInfoHeader(IBitmapInfoHeader infoHeader, uint height, uint width, ushort bitspPerPixel = 24, uint colorsUsed = 0)
        {
            BitmapInfoHeader header = infoHeader as BitmapInfoHeader;
            Assert.NotNull(header);
            Assert.That(40, Is.EqualTo(header.HeaderSize));
            Assert.That(height, Is.EqualTo(header.Height));
            Assert.That(width, Is.EqualTo(header.Width));
            Assert.That(1, Is.EqualTo(header.NumberOfColorPlanes));
            Assert.That(bitspPerPixel, Is.EqualTo(header.BitsPerPixel));
            Assert.That(0, Is.EqualTo(header.Compression));
            Assert.That(0, Is.EqualTo(header.ImageSize));
            Assert.That(2835, Is.EqualTo(header.HorizontalResolution));
            Assert.That(2835, Is.EqualTo(header.VerticalResolution));
            Assert.That(colorsUsed, Is.EqualTo(header.ColorsUsed));
            Assert.That(0, Is.EqualTo(header.ColorsImportant));
        }
    }
}
