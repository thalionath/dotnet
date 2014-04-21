using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unibe.space.luna.crc
{
    public static class Extensions
    {
        // This is the extension method. 
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined. 
        public static UInt16 crc16(this byte[] bytes, UInt16 seed = ccitt16.DEFAULT_SEED)
        {
            return ccitt16.compute(bytes);
        }
    }
}
