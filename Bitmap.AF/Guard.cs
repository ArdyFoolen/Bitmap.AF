using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap.AF
{
    public static class Guard
    {
        public static void Conforms(Func<bool> predicate, string error)
        {
            if (!predicate())
                throw new ArgumentException(error);
        }
    }
}
