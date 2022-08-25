using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public static class Extensions
    {
        public static byte[] ToBytes(this int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return ToLittleEndianBytes(bytes);
        }
        public static byte[] ToBytes(this uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return ToLittleEndianBytes(bytes);
        }
        public static byte[] ToBytes(this short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return ToLittleEndianBytes(bytes);
        }
        public static byte[] ToBytes(this ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return ToLittleEndianBytes(bytes);
        }

        private static byte[] ToLittleEndianBytes(byte[] bytes)
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] Concat(this byte[] array, params byte[][] bytes)
        {
            var length = bytes.Sum(s => s.Length) + array.Length;
            var result = new byte[length];

            Array.Copy(array, 0, result, 0, array.Length);
            int index = array.Length;
            foreach (var byteArray in bytes)
            {
                Array.Copy(byteArray, 0, result, index, byteArray.Length);
                index += byteArray.Length;
            }

            return result;
        }
    }
}
