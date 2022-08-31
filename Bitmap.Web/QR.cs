using static Bitmap.AF.Image;
using System.Drawing;

namespace Bitmap.Web
{
    public interface IQR
    {
        string CreateHtmlImage(string alt);
        string CreateHtmlImageWithColorTable(string alt);
    }

    public class QR : IQR
    {
        public string CreateHtmlImage(string alt)
        {
            var image = Create();
            return $"<img src=\"data:image/png;base64,{Convert.ToBase64String(image)}\" alt=\"{alt}\" />";
        }

        public string CreateHtmlImageWithColorTable(string alt)
        {
            var image = CreateWithColorTable();
            return $"<img src=\"data:image/png;base64,{Convert.ToBase64String(image)}\" alt=\"{alt}\" />";
        }

        private byte[] Create()
        {
            ImageBuilder builder = new ImageBuilder();
            builder
                .UseInfoHeader()
                .WithWith(232)
                .WithHeight(232);

            return BuildFromImageRepresentaion(builder);
        }

        private byte[] CreateWithColorTable()
        {
            ImageBuilder builder = new ImageBuilder();
            builder
                .UseColorTable()
                .WithWith(232)
                .WithHeight(232);

            return BuildFromImageRepresentaion(builder);
        }

        private byte[] BuildFromImageRepresentaion(ImageBuilder builder)
        {
            byte red, green, blue;
            for (int r = 0; r < 29; r++)
                for (int c = 0; c < 29; c++)
                {
                    Rectangle rectangle = new Rectangle(c * 8, r * 8, 8, 8);
                    if (imageRepresentation[r, c])
                        red = green = blue = 0;
                    else
                        red = green = blue = 255;
                    builder.SetRectangle(rectangle, red, green, blue);
                }

            var image = builder.Build();

            return image.ToBytes();
        }

        private bool[,] imageRepresentation = new bool[29, 29]
        {
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false,  true,  true,  true,  true,  true,  true,  true, false,  true,  true,  true,  true,  true, false,  true,  true,  true,  true,  true,  true,  true, false, false, false, false },
            { false, false, false, false,  true, false, false, false, false, false,  true, false,  true, false, false,  true, false, false,  true, false, false, false, false, false,  true, false, false, false, false },
            { false, false, false, false,  true, false,  true,  true,  true, false,  true, false,  true,  true, false,  true, false, false,  true, false,  true,  true,  true, false,  true, false, false, false, false },
            { false, false, false, false,  true, false,  true,  true,  true, false,  true, false,  true,  true,  true, false,  true, false,  true, false,  true,  true,  true, false,  true, false, false, false, false },
            { false, false, false, false,  true, false,  true,  true,  true, false,  true, false,  true,  true, false, false,  true, false,  true, false,  true,  true,  true, false,  true, false, false, false, false },
            { false, false, false, false,  true, false, false, false, false, false,  true, false, false, false, false,  true,  true, false,  true, false, false, false, false, false,  true, false, false, false, false },
            { false, false, false, false,  true,  true,  true,  true,  true,  true,  true, false,  true, false,  true, false,  true, false,  true,  true,  true,  true,  true,  true,  true, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false,  true,  true, false,  true,  true, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false,  true,  true, false,  true, false,  true,  true, false,  true,  true,  true,  true, false,  true, false,  true,  true,  true,  true,  true, false, false, false, false },
            { false, false, false, false, false, false, false, false,  true, false, false, false,  true, false, false,  true, false, false,  true, false, false,  true,  true, false, false, false, false, false, false },
            { false, false, false, false, false, false,  true,  true,  true, false,  true,  true,  true, false, false, false,  true, false, false, false,  true,  true,  true, false,  true, false, false, false, false },
            { false, false, false, false,  true, false, false,  true,  true, false, false, false, false,  true, false, false,  true, false,  true, false, false,  true,  true,  true, false, false, false, false, false },
            { false, false, false, false, false,  true,  true,  true, false, false,  true, false, false, false, false, false,  true, false,  true, false,  true, false,  true, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false,  true,  true, false, false,  true,  true, false,  true, false,  true, false, false, false, false, false, false, false },
            { false, false, false, false,  true,  true,  true,  true,  true,  true,  true, false,  true,  true, false,  true, false,  true,  true,  true, false, false,  true,  true,  true, false, false, false, false },
            { false, false, false, false,  true, false, false, false, false, false,  true, false, false,  true, false, false, false,  true, false,  true,  true,  true, false,  true,  true, false, false, false, false },
            { false, false, false, false,  true, false,  true,  true,  true, false,  true, false,  true,  true,  true,  true,  true,  true,  true,  true, false, false,  true, false,  true, false, false, false, false },
            { false, false, false, false,  true, false,  true,  true,  true, false,  true, false, false, false, false, false, false, false,  true, false, false, false,  true,  true, false, false, false, false, false },
            { false, false, false, false,  true, false,  true,  true,  true, false,  true, false,  true,  true, false, false,  true, false, false, false,  true, false, false, false,  true, false, false, false, false },
            { false, false, false, false,  true, false, false, false, false, false,  true, false,  true,  true,  true, false, false, false,  true, false, false, false,  true,  true,  true, false, false, false, false },
            { false, false, false, false,  true,  true,  true,  true,  true,  true,  true, false, false,  true,  true, false,  true, false,  true, false,  true, false,  true, false,  true, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false }
        };
    }
}
