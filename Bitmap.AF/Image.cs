using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    // Link: https://en.wikipedia.org/wiki/BMP_file_format#:~:text=The%20BMP%20file%20format%2C%20also,and%20OS%2F2%20operating%20systems.
    public partial class Image
    {
        public BitmapFileHeader Header { get; } = new BitmapFileHeader();
        public OS21XBitmapHeader OS21XHeader { get; } = new OS21XBitmapHeader();
        public BitmapData Data { get; private set; }

        private Image() { }

        public void Save(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    var bytes = ToBytes();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Image not saved: {ex.Message}");
            }
        }

        public byte[] ToBytes()
            => Header.ToBytes().Concat(OS21XHeader.ToBytes(), Data.PixelArray);
    }
}
