using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unibe.space.luna.buni
{
    public class UpLink
    {
        private SerialPort serial_port_ = new SerialPort();
        private Parser parser_ = new Parser();

        public string PortName
        {
            get { return serial_port_.PortName; }
            set { serial_port_.PortName = value; }
        }

        public UpLink()
        {
            serial_port_.BaudRate = 1000000;
            serial_port_.Parity   = Parity.Odd;
            serial_port_.DataBits = 8;
            serial_port_.StopBits = StopBits.One;

            serial_port_.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            parser_.Parsed += new AcknowledgeParsedEventHandler(AcknowledgeParsedHandler);
        }

        public void Open()
        {            
            serial_port_.Open();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("Data Received: ");

            // byte[] data = { 1, 2, 4, 8, 16, 32 };
            // string hex = BitConverter.ToString(data);

            // string indata = _serial_port.ReadExisting();
            while( serial_port_.BytesToRead > 0 )
            {
                byte rx = (byte)serial_port_.ReadByte();

                // Console.WriteLine(rx.ToString("x2"));

                if( ! parser_.feed(rx) )
                {
                    Console.WriteLine("Parser Error");
                    parser_.Reset();
                }

            }
    
        }

        private void AcknowledgeParsedHandler(object sender, AcknowledgedParsedEventArgs e)
        {
            Console.WriteLine(e.Acknowledge.Status.ToString("x"));
        }
    }
}
