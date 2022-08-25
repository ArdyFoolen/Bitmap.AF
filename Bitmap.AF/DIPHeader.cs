using System;

namespace Bitmap.AF
{
    public class DIPHeader
    {
        public byte[] HeaderField { get; } = new byte[2] { 0x42, 0x4D };
        public int FileSize { get; set; }
        public byte[] Reserved1 { get; } = new byte[2] { 0x00, 0x00 };
        public byte[] Reserved2 { get; } = new byte[2] { 0x00, 0x00 };
        public int StartingAddressPixelArray { get; set; }
    }
}