using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unibe.space.luna.crc;

namespace unibe.space.luna.uks
{
    public enum Instrument : byte
    {
		LunaLasma   = 0x01,
		LunaNGMS    = 0x02,
		PhobosLasma = 0x0C			
	};

    public enum Model : byte
    {
        AllModels = 0x00,
        ElectronicModel1 = 0x01,
        ElectronicModel2 = 0x02,
        EQM1 = 0x05,
        EQM2 = 0x06,
        FS1 = 0x07,
        FS2 = 0x08,
        FM1 = 0x11,
        FM2 = 0x12
    };

    public enum Format : byte
    {
        PhobosGrunt = 0x00,
        Luna = 0x01
    };

    public class Command
    {
        public static UInt16 CRC_SEED = 0xFFFF;

        public Instrument Instrument { get; set; }
        public Model Model { get; set; }
        public Format Format { get; set; }
        public UInt16 CustomCommand { get; set; }
        public UInt16 SimpleCommand { get; set; }
        public UInt32 CommandArguments { get; set; }

        public byte Identifier
        {
            get
            {
                return (byte)((byte)Instrument << 4 | (byte)Model & 0xF);
            }
        }

        private byte[] fieldsToBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(Identifier);
                    writer.Write((byte)Format);
                    writer.Write(CustomCommand.ToBytes());
                    writer.Write(SimpleCommand.ToBytes());
                    writer.Write(CommandArguments.ToBytes());
                }

                return stream.ToArray();
            }
        }

        public byte[] ToBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    var bytes = fieldsToBytes();

                    writer.Write(bytes);
                    writer.Write(bytes.crc16(CRC_SEED).ToBytes());
                }

                return stream.ToArray();
            }
        }
    }
}
