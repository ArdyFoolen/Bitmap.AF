using Bitmap.AF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bitmap.AF.Image;

namespace BitmapTests.Helpers
{
    internal static class ImageRepresentation
    {
        private static bool[,] Ver1 = new bool[29, 29]
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

        public static Image CreateVer1()
        {
            ImageBuilder builder = new ImageBuilder();
            builder
                .UseColorTable()
                .WithWith(232)
                .WithHeight(232);

            return BuildFromImageRepresentaion(builder, Ver1);
        }

        private static Image BuildFromImageRepresentaion(ImageBuilder builder, bool[,] representation)
        {
            byte red, green, blue;
            for (int r = 0; r < 29; r++)
                for (int c = 0; c < 29; c++)
                {
                    Rectangle rectangle = new Rectangle(c * 8, r * 8, 8, 8);
                    if (representation[r, c])
                        red = green = blue = 0;
                    else
                        red = green = blue = 255;
                    builder.SetRectangle(rectangle, red, green, blue);
                }

            return builder.Build();
        }
    }
}
