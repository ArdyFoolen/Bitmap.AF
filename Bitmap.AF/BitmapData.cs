using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public class BitmapData
    {
        /// <summary>
        /// Multiple of 4 bytes per row
        /// Positive Height means write bytes from bottom-to-top
        /// </summary>
        public byte[] PixelArray { get; set; }
    }
}
