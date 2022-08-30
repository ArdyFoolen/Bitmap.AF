using Bitmap.AF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bitmap.AF.Image;

namespace BitmapTests
{
    public class ImageBuilderTests
    {
        private const int BytesPerPixel = 3;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Build_WidthZero_ShouldThrow()
        {
            // Arrange
            var builder = new ImageBuilder();
            TestDelegate action = () => builder.Build();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => action());

            Assert.NotNull(exception);
            Assert.AreEqual("Width cannot be 0", exception.Message);
        }

        [Test]
        public void Build_HeightZero_ShouldThrow()
        {
            // Arrange
            var builder = new ImageBuilder();
            builder.WithWith(8);
            TestDelegate action = () => builder.Build();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => action());

            Assert.NotNull(exception);
            Assert.AreEqual("Height cannot be 0", exception.Message);
        }

        [Test]
        public void Build_PixelArrayHeightNull_ShouldThrow()
        {
            // Arrange
            var builder = new ImageBuilder();
            builder.WithWith(8)
                .WithHeight(8);
            TestDelegate action = () => builder.Build();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => action());

            Assert.NotNull(exception);
            Assert.AreEqual("Bitmap pixel array not correct height", exception.Message);
        }

        [Test]
        public void Build_PixelArrayAnyWidthNull_ShouldThrow()
        {
            // Arrange
            var builder = new ImageBuilder();
            builder.WithWith(8)
                .WithHeight(8)
                .SetPixel(0, 0, 0, 0, 0);
            TestDelegate action = () => builder.Build();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => action());

            Assert.NotNull(exception);
            Assert.AreEqual("Bitmap pixel array not correct width", exception.Message);
        }

        [Test]
        public void Build_OS21XHeader_ShouldSucceed()
        {
            // Arrange
            uint height = 8;
            uint width = 8;

            var builder = new ImageBuilder();
            builder
                .UseOS21XHeader()
                .WithWith(width)
                .WithHeight(height);
            for (int y = 0; y < 8; y++)
                builder.SetPixel(0, y, 0, 0, 0);

            // Act
            var image = builder.Build();

            // Assert
            int padding = Padding(height, width);
            int fileSize = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length + (int)height * ((int)width * BytesPerPixel + padding);
            int offsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
            AssertBitmapFileHeader(image.Header, fileSize, offsetPixelArray);
            AssertBitmapOS21XHeader(image.InfoHeader, (ushort)height, (ushort)width);
            Assert.AreEqual(fileSize - offsetPixelArray, image.Data.PixelArray.Length);

            image.Save("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\SaveOS21X8x8Black.bmp");
        }

        [Test]
        public void Build_InfoHeader24bpp_ShouldSucceed()
        {
            // Arrange
            uint height = 8;
            uint width = 8;

            var builder = new ImageBuilder();
            builder
                .UseInfoHeader()
                .WithWith(width)
                .WithHeight(height);
            for (int y = 0; y < 8; y++)
                builder.SetPixel(0, y, 0, 0, 0);

            // Act
            var image = builder.Build();

            // Assert
            int padding = Padding(height, width);
            int fileSize = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length + (int)height * ((int)width * BytesPerPixel + padding);
            int offsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
            AssertBitmapFileHeader(image.Header, fileSize, offsetPixelArray);
            AssertBitmapInfoHeader(image.InfoHeader, height, width);
            Assert.AreEqual(fileSize - offsetPixelArray, image.Data.PixelArray.Length);

            image.Save("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\SaveInfo8x8Black.bmp");
        }

        [Test]
        public void Build_InfoHeader01Bpp_ShouldSucceed()
        {
            // Arrange
            uint height = 8;
            uint width = 8;

            Rectangle rect00 = new Rectangle(0, 0, 4, 4);
            Rectangle rect01 = new Rectangle(0, 4, 4, 4);
            Rectangle rect10 = new Rectangle(4, 0, 4, 4);
            Rectangle rect11 = new Rectangle(4, 4, 4, 4);

            var builder = new ImageBuilder();
            builder
                .UseColorTable()
                .UseInfoHeader()
                .WithWith(width)
                .WithHeight(height)
                .SetRectangle(rect00, 255, 0, 0)
                .SetRectangle(rect01, 255, 255, 255)
                .SetRectangle(rect10, 255, 255, 255)
                .SetRectangle(rect11, 255, 0, 0);

            // Act
            var image = builder.Build();

            // Assert
            var nbrOfBytes = (int)Math.Ceiling((decimal)width / 8);
            var prepadding = 4 - nbrOfBytes % 4;
            int padding = prepadding == 4 || height == 1 ? 0 : prepadding;
            int fileSize = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length + (int)height * ((int)Math.Ceiling((decimal)width / 8) + padding);
            int offsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
            AssertBitmapFileHeader(image.Header, fileSize, offsetPixelArray);
            AssertBitmapInfoHeader(image.InfoHeader, height, width, 1, 2);
            Assert.AreEqual(fileSize - offsetPixelArray, image.Data.PixelArray.Length);

            image.Save("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\SaveInfo8x8ColorTableBpp1RWWR.bmp");
        }

        [Test]
        public void Build_InfoHeader04Bpp_ShouldSucceed()
        {
            // Arrange
            uint height = 8;
            uint width = 8;

            Rectangle rect00 = new Rectangle(0, 0, 4, 4);
            Rectangle rect01 = new Rectangle(0, 4, 4, 4);
            Rectangle rect10 = new Rectangle(4, 0, 4, 4);
            Rectangle rect11 = new Rectangle(4, 4, 4, 4);

            var builder = new ImageBuilder();
            builder
                .UseColorTable()
                .UseInfoHeader()
                .WithWith(width)
                .WithHeight(height)
                .SetRectangle(rect00, 255, 0, 0)
                .SetRectangle(rect01, 0, 0, 255)
                .SetRectangle(rect10, 0, 255, 0)
                .SetRectangle(rect11, 255, 255, 255);

            // Act
            var image = builder.Build();

            // Assert
            var nbrOfBytes = (int)Math.Ceiling((decimal)width / 2);
            var prepadding = 4 - nbrOfBytes % 4;
            int padding = prepadding == 4 || height == 1 ? 0 : prepadding;
            int fileSize = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length + (int)height * ((int)Math.Ceiling((decimal)width / 2) + padding);
            int offsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
            AssertBitmapFileHeader(image.Header, fileSize, offsetPixelArray);
            AssertBitmapInfoHeader(image.InfoHeader, height, width, 4, 4);
            Assert.AreEqual(fileSize - offsetPixelArray, image.Data.PixelArray.Length);

            image.Save("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\SaveInfo8x8ColorTableBpp4RGBW.bmp");
        }

        [Test]
        public void Build_InfoHeader08Bpp_ShouldSucceed()
        {
            // Arrange
            uint height = 8;
            uint width = 8;

            var builder = new ImageBuilder();
            builder
                .UseColorTable()
                .UseInfoHeader()
                .WithWith(width)
                .WithHeight(height);

            for (byte r = 0; r < 64; r++)
                builder.SetPixel(r % 8, r / 8, (byte)(0xff - r * 3), 0, 0);

            // Act
            var image = builder.Build();

            // Assert
            var nbrOfBytes = (int)width;
            var prepadding = 4 - nbrOfBytes % 4;
            int padding = prepadding == 4 || height == 1 ? 0 : prepadding;
            int fileSize = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length + (int)height * (int)(width + padding);
            int offsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
            AssertBitmapFileHeader(image.Header, fileSize, offsetPixelArray);
            AssertBitmapInfoHeader(image.InfoHeader, height, width, 8, 64);
            Assert.AreEqual(fileSize - offsetPixelArray, image.Data.PixelArray.Length);

            image.Save("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\SaveInfo8x8ColorTableBpp8Reds.bmp");
        }

        [TestCase(-1)]
        [TestCase(8)]
        public void SetPixel_XOutOfRange_ShouldThrow(int x)
        {
            // Arrange
            var builder = new ImageBuilder();
            builder.WithWith(8)
                .WithHeight(8);
            TestDelegate action = () => builder.SetPixel(x, 0, 0, 0, 0);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => action());

            Assert.NotNull(exception);
            Assert.AreEqual("X out of range", exception.Message);
        }

        [TestCase(-1)]
        [TestCase(8)]
        public void SetPixel_YOutOfRange_ShouldThrow(int y)
        {
            // Arrange
            var builder = new ImageBuilder();
            builder.WithWith(8)
                .WithHeight(8);
            TestDelegate action = () => builder.SetPixel(0, y, 0, 0, 0);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => action());

            Assert.NotNull(exception);
            Assert.AreEqual("Y out of range", exception.Message);
        }

        private void AssertBitmapFileHeader(BitmapFileHeader header, int fileSize, int offsetPixelArray)
        {
            Assert.NotNull(header);
            Assert.AreEqual(header.HeaderField[0], 0x42);
            Assert.AreEqual(header.HeaderField[1], 0x4D);
            Assert.AreEqual(header.FileSize, fileSize);
            Assert.AreEqual(header.OffsetPixelArray, offsetPixelArray);
            Assert.AreEqual(header.HeaderSize, 14);
        }

        private void AssertBitmapOS21XHeader(IBitmapInfoHeader infoHeader, ushort height, ushort width)
        {
            OS21XBitmapHeader header = infoHeader as OS21XBitmapHeader;
            Assert.NotNull(header);
            Assert.AreEqual(header.HeaderSize, 12);
            Assert.AreEqual(header.Height, height);
            Assert.AreEqual(header.Width, width);
            Assert.AreEqual(header.NumberOfColorPlanes, 1);
            Assert.AreEqual(header.BitsPerPixel, 24);
        }

        private void AssertBitmapInfoHeader(IBitmapInfoHeader infoHeader, uint height, uint width, ushort bitspPerPixel = 24, uint colorsUsed = 0)
        {
            BitmapInfoHeader header = infoHeader as BitmapInfoHeader;
            Assert.NotNull(header);
            Assert.AreEqual(header.HeaderSize, 40);
            Assert.AreEqual(header.Height, height);
            Assert.AreEqual(header.Width, width);
            Assert.AreEqual(header.NumberOfColorPlanes, 1);
            Assert.AreEqual(header.BitsPerPixel, bitspPerPixel);
            Assert.AreEqual(header.Compression, 0);
            Assert.AreEqual(header.ImageSize, 0);
            Assert.AreEqual(header.HorizontalResolution, 2835);
            Assert.AreEqual(header.VerticalResolution, 2835);
            Assert.AreEqual(header.ColorsUsed, colorsUsed);
            Assert.AreEqual(header.ColorsImportant, 0);
        }

        private int Padding(uint height, uint width)
        {
            var padding = (int)(4 - ((width * BytesPerPixel) % 4));
            return padding == 4 || height == 1 ? 0 : padding;
        }
    }
}
