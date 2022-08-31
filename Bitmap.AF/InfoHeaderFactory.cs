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
        public IBitsPerPixel BitsPerPixel { get; private set; }
        internal bool UseColorTable { get; set; }

        internal InfoHeaderFactory(bool useColorTable)
        {
            UseColorTable = useColorTable;
        }

        public IBitmapInfoHeader Create(uint width, uint height, Dictionary<int, byte> colorTableIndex)
        {
            BitmapInfoHeader header = new BitmapInfoHeader();
            SetHeaderProperties(header, width, height, (uint)colorTableIndex.Count);
            CreateBitsPerPixel(header, colorTableIndex, width, height);

            return header;
        }

        private void CreateBitsPerPixel(BitmapInfoHeader header, Dictionary<int, byte> colorTableIndex, uint width, uint height)
        {
            switch (header.BitsPerPixel)
            {
                case 1:
                    BitsPerPixel = new BitsPerPixel01(colorTableIndex, width, height);
                    break;
                case 4:
                    BitsPerPixel = new BitsPerPixel04(colorTableIndex, width, height);
                    break;
                case 8:
                    BitsPerPixel = new BitsPerPixel08(colorTableIndex, width, height);
                    break;
                case 24:
                    BitsPerPixel = new BitsPerPixel24(width, height);
                    break;
            }
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
