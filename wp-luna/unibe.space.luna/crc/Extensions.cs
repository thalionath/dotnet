using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unibe.space.luna.crc
{
    public enum Endianess
    {
        Little,
        Big
    }

    public static class Extensions
    {
        public static byte[] ToBytes(this UInt16 value, Endianess endian = Endianess.Big)
        {
            if (BitConverter.IsLittleEndian && endian == Endianess.Little)
            {
                return new byte[] { 
                    (byte)(value >> 0),
                    (byte)(value >> 8)
                };
            }
            else
            {
                return new byte[] { 
                    (byte)(value >> 8),
                    (byte)(value >> 0)
                };
            }            
        }

        public static byte[] ToBytes(this UInt32 value, Endianess endian = Endianess.Big)
        {
            if (BitConverter.IsLittleEndian && endian == Endianess.Little)
            {
                return new byte[] { 
                    (byte)(value >>  0),
                    (byte)(value >>  8),
                    (byte)(value >> 16),
                    (byte)(value >> 24)
                };
            }
            else
            {
                return new byte[] { 
                    (byte)(value >> 24),
                    (byte)(value >> 16),
                    (byte)(value >>  8),
                    (byte)(value >>  0)
                };
            }
        }

        // This is the extension method. 
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined. 
        public static UInt16 crc16(this byte[] bytes, UInt16 seed = ccitt16.DEFAULT_SEED)
        {
            return ccitt16.compute(bytes);
        }
    }
}

