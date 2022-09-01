using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bitmap.AF.Image;
using Color = Bitmap.AF.Color;

namespace BitmapTests
{
    public class ImageLoaderTests
    {
        private const int BytesPerPixel = 3;

        [Test]
        public void Load_OS21XHeader_ShouldSucceed()
        {
            // Arrange
            uint height = 8;
            uint width = 8;

            ImageLoader loader = new ImageLoader();

            // Act
            var image = loader.Load("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\SaveOS21X8x8Black.bmp");

            // Assert
            int padding = Padding(height, width);
            int fileSize = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length + (int)height * ((int)width * BytesPerPixel + padding);
            int offsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
            Helpers.AssertionHelper.AssertBitmapFileHeader(image.Header, fileSize, offsetPixelArray);
            Helpers.AssertionHelper.AssertBitmapOS21XHeader(image.InfoHeader, (ushort)height, (ushort)width);
            Assert.That(image.Data.PixelArray.Length, Is.EqualTo(fileSize - offsetPixelArray));
        }

        [Test]
        public void Load_OS21XHeader2_ShouldSucceed()
        {
            // Arrange
            uint height = 24;
            uint width = 24;

            ImageLoader loader = new ImageLoader();

            // Act
            var image = loader.Load("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\SaveOS21X8x8Colored.bmp");

            // Assert
            int padding = Padding(height, width);
            int fileSize = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length + (int)height * ((int)width * BytesPerPixel + padding);
            int offsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
            Helpers.AssertionHelper.AssertBitmapFileHeader(image.Header, fileSize, offsetPixelArray);
            Helpers.AssertionHelper.AssertBitmapOS21XHeader(image.InfoHeader, (ushort)height, (ushort)width);
            Assert.That(image.Data.PixelArray.Length, Is.EqualTo(fileSize - offsetPixelArray));
        }

        [Test]
        public void Load_InfoHeader24bpp_ShouldSucceed()
        {
            // Arrange
            uint height = 8;
            uint width = 8;

            ImageLoader loader = new ImageLoader();

            // Act
            var image = loader.Load("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\SaveInfo8x8Black.bmp");

            // Assert
            int padding = Padding(height, width);
            int fileSize = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length + (int)height * ((int)width * BytesPerPixel + padding);
            int offsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
            Helpers.AssertionHelper.AssertBitmapFileHeader(image.Header, fileSize, offsetPixelArray);
            Helpers.AssertionHelper.AssertBitmapInfoHeader(image.InfoHeader, height, width);
            Assert.That(image.Data.PixelArray.Length, Is.EqualTo(fileSize - offsetPixelArray));
        }

        [Test]
        public void Load_InfoHeader01Bpp_ShouldSucceed()
        {
            // Arrange
            uint height = 8;
            uint width = 8;

            ImageLoader loader = new ImageLoader();

            // Act
            var image = loader.Load("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\SaveInfo8x8ColorTableBpp1RWWR.bmp");

            // Assert
            var nbrOfBytes = (int)Math.Ceiling((decimal)width / 8);
            var prepadding = 4 - nbrOfBytes % 4;
            int padding = prepadding == 4 || height == 1 ? 0 : prepadding;
            int fileSize = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length + (int)height * (nbrOfBytes + padding);
            int offsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
            Helpers.AssertionHelper.AssertBitmapFileHeader(image.Header, fileSize, offsetPixelArray);
            Helpers.AssertionHelper.AssertBitmapInfoHeader(image.InfoHeader, height, width, 1, 2);
            Assert.That(image.Data.PixelArray.Length, Is.EqualTo(fileSize - offsetPixelArray));
        }

        [Test]
        public void Load_InfoHeader04Bpp_ShouldSucceed()
        {
            // Arrange
            uint height = 8;
            uint width = 8;

            ImageLoader loader = new ImageLoader();

            // Act
            var image = loader.Load("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\SaveInfo8x8ColorTableBpp4RGBW.bmp");

            // Assert
            var nbrOfBytes = (int)Math.Ceiling((decimal)width / 2);
            var prepadding = 4 - nbrOfBytes % 4;
            int padding = prepadding == 4 || height == 1 ? 0 : prepadding;
            int fileSize = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length + (int)height * (nbrOfBytes + padding);
            int offsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
            Helpers.AssertionHelper.AssertBitmapFileHeader(image.Header, fileSize, offsetPixelArray);
            Helpers.AssertionHelper.AssertBitmapInfoHeader(image.InfoHeader, height, width, 4, 4);
            Assert.That(image.Data.PixelArray.Length, Is.EqualTo(fileSize - offsetPixelArray));
        }

        [Test]
        public void Load_InfoHeader08Bpp_ShouldSucceed()
        {
            // Arrange
            uint height = 8;
            uint width = 8;

            ImageLoader loader = new ImageLoader();

            // Act
            var image = loader.Load("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\SaveInfo8x8ColorTableBpp8Reds.bmp");

            // Assert
            var nbrOfBytes = (int)width;
            var prepadding = 4 - nbrOfBytes % 4;
            int padding = prepadding == 4 || height == 1 ? 0 : prepadding;
            int fileSize = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length + (int)height * (int)(width + padding);
            int offsetPixelArray = image.Header.HeaderSize + image.InfoHeader.HeaderSize + image.ColorTable.Length;
            Helpers.AssertionHelper.AssertBitmapFileHeader(image.Header, fileSize, offsetPixelArray);
            Helpers.AssertionHelper.AssertBitmapInfoHeader(image.InfoHeader, height, width, 8, 64);
            Assert.That(image.Data.PixelArray.Length, Is.EqualTo(fileSize - offsetPixelArray));
        }

        private int Padding(uint height, uint width)
        {
            var padding = (int)(4 - ((width * BytesPerPixel) % 4));
            return padding == 4 || height == 1 ? 0 : padding;
        }
    }
}
