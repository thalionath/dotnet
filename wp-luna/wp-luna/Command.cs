using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wp.crc;

namespace wp.luna.buni
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

    [Flags]
    public enum Status
    {
        Error          = 0x01, //!< Bit is set if unknown command was received or command with error.
        ServiceRequest = 0x02, //!< Bit is set if SI has data packets (at least 1) to transmit to BUNI.
        Busy           = 0x04, //!< Bit is set if SI is busy can't correctly reply to command (receive UKS or send data packet).
        Reserved       = 0x08,
        Data           = 0x10  //!< Bit is set if SI sends data packet in this message.
    };

    public class Command
    {
        public static UInt16 crc_seed = 0xFFFF;

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
                // -1 for .net not allowing to specify endianess here
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)Marker);
                    writer.Write((byte)Flag);

                    // -1 for the programmer assuming little endian CPUs only
                    writer.Write((byte)(Size >> 8));
                    writer.Write((byte)(Size >> 0));

                    writer.Write(Payload);
                }

                return stream.ToArray();
            }
        }

        public byte[] ToBytes( UInt16 crc )
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // -1 for .net not allowing to specify endianess here
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(fieldsToBytes());

                    // -1 for the programmer assuming little endian CPUs only
                    writer.Write((byte)(crc >> 8));
                    writer.Write((byte)(crc >> 0));
                }

                return stream.ToArray();
            }
        }

        public byte[] ToBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // -1 for .net not allowing to specify endianess here
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    var bytes = fieldsToBytes();

                    UInt16 crc = ccitt16.compute(bytes, crc_seed);

                    writer.Write(bytes);

                    // -1 for the programmer assuming little endian CPUs only
                    writer.Write((byte)(crc >> 8));
                    writer.Write((byte)(crc >> 0));
                }

                return stream.ToArray();
            }
        }

        public UInt16 ComputeCrc()
        {
            return ccitt16.compute(fieldsToBytes(), crc_seed);
        }

        public static Command CreateStatusRequest()
        {
            return new Command(Flag.StatusRequest);
        }
    }
}
