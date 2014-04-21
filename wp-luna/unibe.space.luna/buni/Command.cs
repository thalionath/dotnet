using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unibe.space.luna.crc;

namespace unibe.space.luna.buni
{
    public enum Marker : byte
    {
        Buni                 = 0xA5, //!< Message sent from board computer to scientific payload.
        ScientificInstrument = 0xB6
    }

    public enum Flag : byte
    {
        TimeCode      = 0x01,
        UKS           = 0x02,
        StatusRequest = 0x03,
        DataRequest   = 0x04,
        TimeStamp     = 0x05
    };

    public class Command
    {
        public static UInt16 CRC_SEED = 0xFFFF;

        public Marker Marker { get; set; }

        public Flag Flag { get; set; }

        private byte[] payload_;

        public int Size
        { 
            get { return payload_.Length; }
        }

        public byte[] Payload
        { 
            get { return payload_; }
            set { payload_ = value; }
        }

        public Command( Flag flag = Flag.StatusRequest )
        : this(Marker.Buni, flag, new byte[0])
        {
        }

        public Command( Marker marker, Flag flag, byte[] payload )
        {
            Marker   = marker;
            Flag     = flag;
            payload_ = payload; // better to assign property or member?
        }

        private byte[] fieldsToBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)Marker);
                    writer.Write((byte)Flag);
                    
                    writer.Write(((UInt16)Size).ToBytes());

                    writer.Write(Payload);
                }

                return stream.ToArray();
            }
        }

        public byte[] ToBytes( UInt16 crc )
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(fieldsToBytes());
                    writer.Write(crc.ToBytes());
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

        public UInt16 ComputeCrc()
        {
            return fieldsToBytes().crc16(CRC_SEED);
        }

        public static Command CreateStatusRequest()
        {
            return new Command(Flag.StatusRequest);
        }
    }
}
