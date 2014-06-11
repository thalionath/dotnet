using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace unibe.space.luna.buni
{
    public class DownLink
    {
        private SerialPort serial_port_ = new SerialPort();

        public DownLink()
        {
            serial_port_.BaudRate = 125000;
            serial_port_.Parity   = Parity.Odd;
            serial_port_.DataBits = 8;
            serial_port_.StopBits = StopBits.One;


            serial_port_.PortName = "COM17";
        }

        public void Open()
        {            
            serial_port_.Open();
        }

        public void Write(byte[] bytes)
        {
            serial_port_.Write(bytes, 0, bytes.Length);
        }

    }
}
