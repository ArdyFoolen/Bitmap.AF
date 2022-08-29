using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public class BitmapInfoHeader : IBitmapInfoHeader
    {
        public int HeaderSize { get; } = 40;
        public uint Width { get; set; }
        public uint Height { get; set; }
        public ushort NumberOfColorPlanes { get; } = 1;
        public ushort BitsPerPixel { get; set; } = 24;
        public uint Compression { get; } = 0;
        public uint ImageSize { get; } = 0;
        public uint HorizontalResolution { get; } = 2835; // 72 DPI × 39.3701 inches per metre yields 2834.6472
        public uint VerticalResolution { get; } = 2835;
        public uint ColorsUsed { get; set; } = 0;
        public uint ColorsImportant { get; } = 0;

        public byte[] ToBytes()
            => HeaderSize.ToBytes().Concat(Width.ToBytes(), Height.ToBytes(), NumberOfColorPlanes.ToBytes(), BitsPerPixel.ToBytes(),
                Compression.ToBytes(), ImageSize.ToBytes(), HorizontalResolution.ToBytes(), VerticalResolution.ToBytes(),
                ColorsUsed.ToBytes(), ColorsImportant.ToBytes());
    }
}
