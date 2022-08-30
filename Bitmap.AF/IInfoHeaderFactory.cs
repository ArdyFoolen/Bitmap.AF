using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public interface IInfoHeaderFactory
    {
        IBitmapInfoHeader Create(uint width, uint height, uint colorsUsed);
    }
}
