using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using unibe.space.luna.buni;

namespace unibe.space.luna.test
{
    [TestClass]
    public class AcknowledgeTest
    {
        //static byte[] _test_bytes = new byte[] { 0xB6 };

        [TestMethod]
        public void FromBytesSetsMarkerTest()
        {
            Acknowledge ack = new Acknowledge();

            ack.FromBytes(new Byte[] { 0xAB, 0xCD, 0x00, 0x00, 0xCC, 0xCC });

            Assert.AreEqual(0xAB, (int)ack.Marker);
        }

        [TestMethod]
        public void FromBytesSetsStatusFlagsTest()
        {
            Acknowledge ack = new Acknowledge();

            ack.FromBytes(new Byte[] { 0xAB, 0xCD, 0x00, 0x00, 0xCC, 0xCC });

            Assert.AreEqual(0xCD, (int)ack.Status);
        }

        [TestMethod]
        public void FromBytesSetsSizeTest()
        {
            Acknowledge ack = new Acknowledge();

            ack.FromBytes(new Byte[] { 0xAB, 0xCD, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0xCC, 0xCC });

            Assert.AreEqual(0x0004, ack.Size);
        }

        [TestMethod]
        public void FromBytesSetsPayloadTest()
        {
            Acknowledge ack = new Acknowledge();

            ack.FromBytes(new Byte[] { 0xAB, 0xCD, 0x00, 0x04, 0x01, 0x02, 0x03, 0x04, 0xCC, 0xCC });

            CollectionAssert.AreEqual(new byte[] { 0x01, 0x02, 0x03, 0x04 }, ack.Payload);
        }

        [TestMethod]
        public void FromBytesSetsCrcTest()
        {
            Acknowledge ack = new Acknowledge();

            ack.FromBytes(new Byte[] { 0xAB, 0xCD, 0x00, 0x02, 0x01, 0x02, 0x03, 0x04 });

            Assert.AreEqual(0x0304, ack.CRC);
        }
    }
}
