//using System;

//namespace Bitmap.AF
//{
//    public class OS22XBitmapHeader : IBitmapInfoHeader
//    {
//        public int HeaderSize { get; } = 16;
//        public int Width { get; set; }
//        /// <summary>
//        /// Use negative height to store top to bottom
//        /// </summary>
//        public int Height { get; set; }
//        public ushort NumberOfColorPlanes { get; } = 1;
//        public ushort BitsPerPixel { get; set; } = 24;

//        public byte[] ToBytes()
//            => HeaderSize.ToBytes().Concat(Width.ToBytes(), Height.ToBytes(), NumberOfColorPlanes.ToBytes(), BitsPerPixel.ToBytes());
//    }
//}