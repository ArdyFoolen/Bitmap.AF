using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Bitmap.AF.Image;

namespace Bitmap.AF
{
    public partial class Image
    {
        public class ImageLoader
        {
            private Image Image = new Image();

            public Image Load(string filePath)
            {
                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        LoadHeader(fs);
                        LoadInfoHeader(fs);
                        if (Image.InfoHeader.ColorsUsed != 0)
                            LoadColorTable(fs);
                        LoadBitmapData(fs);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Image not loaded: {ex.Message}");
                }

                return Image;
            }

            private void LoadHeader(FileStream fs)
            {
                var bytes = new byte[14];
                int read = fs.Read(bytes, 0, bytes.Length);

                Guard.Conforms(() => read == bytes.Length, "Bitmap file Header invalid");
                Guard.Conforms(() => bytes[0] == 0x42 && bytes[1] == 0x4D, "Not a bitmap file");

                Image.Header.FileSize = BitConverter.ToInt32(bytes, 2);
                Image.Header.OffsetPixelArray = BitConverter.ToInt32(bytes, 10);
            }

            private void LoadInfoHeader(FileStream fs)
            {
                var bytes = new byte[40];
                int read = fs.Read(bytes, 0, 4);

                int infoSize = BitConverter.ToInt32(bytes, 0);
                read = fs.Read(bytes, 4, infoSize - 4);
                switch (infoSize)
                {
                    case 12:
                        Image.InfoHeader = LoadOS21XHeader(bytes);
                        break;
                    case 40:
                        Image.InfoHeader = LoadInfo(bytes);
                        break;
                    default:
                        throw new Exception("Info header not supported");
                }
            }

            private IBitmapInfoHeader LoadInfo(byte[] bytes)
            {
                var info = new BitmapInfoHeader();

                info.Width = (uint)BitConverter.ToInt32(bytes, 4);
                info.Height = (uint)BitConverter.ToInt32(bytes, 8);
                info.BitsPerPixel = (ushort)BitConverter.ToInt16(bytes, 14);
                info.ColorsUsed = (uint)BitConverter.ToInt16(bytes, 32);

                return info;
            }

            private IBitmapInfoHeader LoadOS21XHeader(byte[] bytes)
            {
                var info = new OS21XBitmapHeader();

                info.Width = (ushort)BitConverter.ToInt16(bytes, 4);
                info.Height = (ushort)BitConverter.ToInt16(bytes, 6);
                info.BitsPerPixel = (ushort)BitConverter.ToInt16(bytes, 10);

                return info;
            }

            private void LoadColorTable(FileStream fs)
            {
                Image.ColorTable = new byte[Image.InfoHeader.ColorsUsed * 4];
                int read = fs.Read(Image.ColorTable, 0, Image.ColorTable.Length);
            }

            private void LoadBitmapData(FileStream fs)
            {
                Image.Data.PixelArray = new byte[Image.Header.FileSize - Image.Header.OffsetPixelArray];
                int read = fs.Read(Image.Data.PixelArray, 0, Image.Data.PixelArray.Length);
            }
        }
    }
}
