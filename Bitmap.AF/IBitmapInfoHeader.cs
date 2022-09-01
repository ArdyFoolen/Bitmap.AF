using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public interface IBitmapInfoHeader
    {
        int HeaderSize { get; }
        ushort BitsPerPixel { get; }
        uint ColorsUsed { get; }
        byte[] ToBytes();
    }
}
