using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public interface IInfoHeaderFactory
    {
        IBitsPerPixel BitsPerPixel { get; }
        IBitmapInfoHeader Create(uint width, uint height, Dictionary<int, byte> colorTableIndex);
    }
}
