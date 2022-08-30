using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public class OS21XHeaderFactory : IInfoHeaderFactory
    {
        public IBitmapInfoHeader Create(uint width, uint height, uint colorsUsed)
        {
            var header = new OS21XBitmapHeader();

            header.Width = (ushort)width;
            header.Height = (ushort)height;

            return header;
        }
    }
}
