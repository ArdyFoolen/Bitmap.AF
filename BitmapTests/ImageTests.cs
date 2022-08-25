using Bitmap.AF;
using System.Drawing;
using static Bitmap.AF.Image;

namespace BitmapTests
{
    public class ImageTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SaveImage_ShouldSucceed()
        {
            // Arrange
            // Bottom to top, BGR format
            // Creates a 2x2 bitmap
            // _________________
            // |  Red  | White |
            // _________________
            // | Blue  | Green |
            // _________________
            byte[] pixelArray = new byte[16]
            {
                255, 0, 0,      // Blue
                0, 255, 0,      // Green
                0, 0,           // Padding of Row to be multiple of 4 bytes
                0, 0, 255,      // Red
                255, 255, 255,  // White
                0, 0            // Padding of Row to be multiple of 4 bytes
            };
            BitmapData bitmapData = new BitmapData();
            bitmapData.PixelArray = pixelArray;

            ImageBuilder builder = new ImageBuilder();
            builder
                .WithWith(2)
                .WithHeight(2)
                .WithBitmapData(bitmapData);

            // Act
            var image = builder.Build();

            // Assert
            image.Save("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\Save2x2RWBG.bmp");
        }

        [Test]
        public void SaveImage_SetPixel_ShouldSucceed()
        {
            // Arrange
            // Bottom to top, BGR format
            // Creates a 2x2 bitmap
            // _________________
            // |  Red  | Green |
            // _________________
            // | Blue  | White |
            // _________________
            ImageBuilder builder = new ImageBuilder();
            builder
                .WithWith(2)
                .WithHeight(2)
                .SetPixel(0, 0, 255, 0, 0)
                .SetPixel(0, 1, 0, 0, 255)
                .SetPixel(1, 0, 0, 255, 0)
                .SetPixel(1, 1, 255, 255, 255);

            // Act
            var image = builder.Build();

            // Assert
            image.Save("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\Save2x2RGBW.bmp");
        }

        [Test]
        public void SaveImage_SetRectangle_ShouldSucceed()
        {
            // Arrange
            // Bottom to top, BGR format
            // Creates a 2x2 bitmap
            // _________________
            // |  Red  | Green |
            // _________________
            // | Blue  | White |
            // _________________
            Rectangle rect00 = new Rectangle(0, 0, 4, 4);
            Rectangle rect01 = new Rectangle(0, 4, 4, 4);
            Rectangle rect10 = new Rectangle(4, 0, 4, 4);
            Rectangle rect11 = new Rectangle(4, 4, 4, 4);
            ImageBuilder builder = new ImageBuilder();
            builder
                .WithWith(8)
                .WithHeight(8)
                .SetRectangle(rect00, 255, 0, 0)
                .SetRectangle(rect01, 0, 0, 255)
                .SetRectangle(rect10, 0, 255, 0)
                .SetRectangle(rect11, 255, 255, 255);

            // Act
            var image = builder.Build();

            // Assert
            image.Save("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\Save8x8RGBW.bmp");
        }
    }
}