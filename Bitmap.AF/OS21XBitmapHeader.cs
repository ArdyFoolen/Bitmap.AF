using System;

namespace Bitmap.AF
{
    public class OS21XBitmapHeader : IBitmapInfoHeader
    {
        public int HeaderSize { get; } = 12;
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public ushort NumberOfColorPlanes { get; } = 1;
        public ushort BitsPerPixel { get; set; } = 24;

        public uint ColorsUsed
        {
            get
            {
                return 0;
            }
            set
            {
                if (value != 0) throw new ArgumentException();
            }
        }
        public byte[] ToBytes()
            => HeaderSize.ToBytes().Concat(Width.ToBytes(), Height.ToBytes(), NumberOfColorPlanes.ToBytes(), BitsPerPixel.ToBytes());
    }
}