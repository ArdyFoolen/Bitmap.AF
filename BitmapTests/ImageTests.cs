using Bitmap.AF;
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
            image.Save("C:\\Users\\HP-OMEN\\Documents\\Visual Studio 2019\\Git\\Bitmap.AF\\BitmapTests\\Images\\Save2x2BGRW.bmp");
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
    }
}