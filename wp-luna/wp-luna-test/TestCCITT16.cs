using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using wp.crc;

namespace wp.crc.test
{
    [TestClass]
    public class TestCCITT16
    {
        [TestMethod]
        public void TestCompute()
        {
            Assert.AreEqual(0x29B1, ccitt16.compute(new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 }, 0xFFFF));
        }
    }
}
