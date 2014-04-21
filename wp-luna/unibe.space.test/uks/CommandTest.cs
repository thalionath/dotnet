using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unibe.space.luna.uks
{
    [TestClass]
    public class CommandTest
    {
        [TestMethod]
        public void TestPropertyGetIdentifer()
        {
            Command command = new Command()
            {
                Instrument = Instrument.LunaLasma,
                Model = Model.ElectronicModel2
            };

            Assert.AreEqual(0x12, command.Identifier);
        }

        [TestMethod]
        public void TestToBytes()
        {
            Command command = new Command()
            {
                Instrument = Instrument.PhobosLasma,
                Model = Model.ElectronicModel2,
                Format = Format.Luna,
                CustomCommand = 0xDEAD,
                SimpleCommand = 0xBEEF,
                CommandArguments = 0xDEFEC800
            };

            CollectionAssert.AreEqual(new byte[] { 0xc2, 0x01, 0xde, 0xad, 0xbe, 0xef, 0xde, 0xfe, 0xc8, 0x00, 0x8d, 0x42 }, command.ToBytes());
        }
    }
}
