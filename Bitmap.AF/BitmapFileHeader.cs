using System;

namespace Bitmap.AF
{
    public class BitmapFileHeader
    {
        //        The header field used to identify the BMP and DIB file is 0x42 0x4D in hexadecimal, same as BM in ASCII.The following entries are possible:
        //BM 0x42 0x4D
        //Windows 3.1x, 95, NT, ... etc.
        //BA 0x42 0x41
        //OS/2 struct bitmap array
        //CI
        //OS/2 struct color icon
        //CP
        //OS/2 const color pointer
        //IC
        //OS/2 struct icon
        //PT
        //OS/2 pointer
        public byte[] HeaderField { get; } = new byte[2] { 0x42, 0x4D };
        public int FileSize { get; set; }
        public byte[] Reserved1 { get; } = new byte[2] { 0x00, 0x00 };
        public byte[] Reserved2 { get; } = new byte[2] { 0x00, 0x00 };
        public int OffsetPixelArray { get; set; }

        public int HeaderSize { get; } = 14;

        public byte[] ToBytes()
            => HeaderField.Concat(FileSize.ToBytes(), Reserved1, Reserved2, OffsetPixelArray.ToBytes());
    }
}