using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Bitmap.AF
{
    public class InfoHeaderFactory : IInfoHeaderFactory
    {
        internal bool UseColorTable { get; set; }

        internal InfoHeaderFactory(bool useColorTable)
        {
            UseColorTable = useColorTable;
        }

        public IBitmapInfoHeader Create(uint width, uint height, uint colorsUsed)
        {
            BitmapInfoHeader header;
            //if (!UseColorTable)
                header = new BitmapInfoHeader();
            //else
            //    throw new Exception();

            SetHeaderProperties(header, width, height, colorsUsed);

            return header;
        }

        private void SetHeaderProperties(BitmapInfoHeader header, uint width, uint height, uint colorsUsed)
        {
            header.Width = width;
            header.Height = height;
            header.ColorsUsed = colorsUsed;
            header.BitsPerPixel = CalculateBitsPerPixel(colorsUsed); 
        }

        private ushort CalculateBitsPerPixel(uint colorsUsed)
        {
            if (colorsUsed == 0)
                return 24;

            var bpp = (ushort)Math.Ceiling(Math.Log2(colorsUsed));
            if (bpp > 1)
            {
                if (bpp <= 4)
                    bpp = 4;
                else if (bpp <= 8)
                    bpp = 8;
            }

            return bpp;
        }
    }
}
