using System;

namespace Bitmap.AF
{
    public class OS21XBitmapHeader
    {
        public uint HeaderSize { get; } = 12;
        public ushort Width { get; set; }
        /// <summary>
        /// Use negative height to store top to bottom
        /// </summary>
        public short Height { get; set; }
        public ushort NumberOfColorPlanes { get; } = 1;
        public ushort BitsPerPixel { get; set; } = 24;

        public byte[] ToBytes()
            => HeaderSize.ToBytes().Concat(Width.ToBytes(), Height.ToBytes(), NumberOfColorPlanes.ToBytes(), BitsPerPixel.ToBytes());
    }
}