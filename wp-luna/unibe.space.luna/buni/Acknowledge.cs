using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unibe.space.luna.buni
{
    [Flags]
    public enum Status
    {
        Error          = 0x01, //!< Bit is set if unknown command was received or command with error.
        ServiceRequest = 0x02, //!< Bit is set if SI has data packets (at least 1) to transmit to BUNI.
        Busy           = 0x04, //!< Bit is set if SI is busy can't correctly reply to command (receive UKS or send data packet).
        Reserved       = 0x08,
        Data           = 0x10  //!< Bit is set if SI sends data packet in this message.
    };

    public class Acknowledge
    {
        public const UInt16 CRC_SEED = 0xFFFF;
        public const int HEADER_SIZE = 4;
        public const uint PAYLOAD_SIZE = 496;

        public Marker Marker  { get; set; }
        public Status Status  { get; set; }
        public UInt16 Size    { get; set; }
        public byte[] Payload { get; set; }
        public UInt16 CRC     { get; set; }

        public void FromBytes(byte[] bytes)
        {
            Marker  = (Marker)bytes[0];
            Status  = (Status)bytes[1];
            Size    = (UInt16)(bytes[2] << 8 | bytes[3] << 0);
            Payload = (Size > 0) ? new ArraySegment<byte>(bytes, HEADER_SIZE, Size).ToArray() : new byte[0];
            CRC = (UInt16)(bytes[HEADER_SIZE + Size + 0] << 8 | bytes[HEADER_SIZE + Size + 1] << 0);
        }

    }
}
