using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public class InfoHeaderFactory : IInfoHeaderFactory
    {
        public IBitmapInfoHeader Create(uint width, uint height)
        {
            var header = new BitmapInfoHeader();

            header.Width = width;
            header.Height = height;

            return header;
        }
    }
}
