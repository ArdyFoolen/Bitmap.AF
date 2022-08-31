using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public interface IBitsPerPixel
    {
        void SetPixelArray(Image image, byte[][] pixelArray);
    }
}
