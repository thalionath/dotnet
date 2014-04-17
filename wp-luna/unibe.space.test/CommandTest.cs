using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using unibe.space.luna.buni;

namespace unibe.space.luna.buni.test
{
    [TestClass]
    public class CommandTest
    {
     
        [TestMethod]
        public void TestSizePropertyIsEqualToPayloadLength()
        {
            byte[] payload = new byte[42];

            Command command = new Command()
            {
                Payload = payload
            };

            Assert.AreEqual(payload.Length, command.Size);
        }

        [TestMethod]
        public void TestToBytes()
        {
            byte[] payload = new byte[] {0x01, 0x01, 0x02, 0x03, 0x05};

            Command command = new Command()
            {
                Marker  = Marker.Buni,
                Flag    = Flag.StatusRequest,
                Payload = payload
            };    

            byte[] bytes = new byte[]
            {
                0xA5,
                0x03,
                0x00, 0x05,
                0x01, 0x01, 0x02, 0x03, 0x05,
                0x23, 0x03
            };
            
            CollectionAssert.AreEqual(bytes, command.ToBytes(0x2303));
        }

        [TestMethod]
        public void TestCreateStatusRequest()
        {
            Command command = Command.CreateStatusRequest();

            CollectionAssert.AreEqual(new byte[] { 0xA5, 0x03, 0x00, 0x00, 0x8B, 0xA3 }, command.ToBytes());
        }

        [TestMethod]
        public void TestComputeCrc()
        {
            Assert.AreEqual(0x8BA3, Command.CreateStatusRequest().ComputeCrc());
        }
    }
}
