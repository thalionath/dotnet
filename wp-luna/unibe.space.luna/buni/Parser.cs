using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unibe.space.luna.buni
{
    public delegate void AcknowledgeParsedEventHandler(object sender, AcknowledgedParsedEventArgs e);

    public class Parser
    {
        enum State
        {
            Marker,
            Status,
            Size,
            Payload,
            CRC
        };

        private State state_;
        private uint counter_;
        private crc.ccitt16 crc_;

        private Status status_;
        private UInt16 size_;
        private byte[] payload_;
        private UInt16 crc_received_;

        public event AcknowledgeParsedEventHandler Parsed;

        public Parser()
        {
            Reset();
        }
        
        public void Reset()
        {
            state_ = State.Marker;
            crc_ = Acknowledge.CRC_SEED;
            counter_ = 0;
        }

        public bool feed(byte value)
        {
            switch(state_)
            {
                case State.Marker:
                { 
                    counter_ = 0;

                    if (value == (byte)Marker.ScientificInstrument)
                    {
                        crc_.addByte(value);
                        state_ = State.Status;  
                    }
                    else
                    {
                        return false;
                    }
                } break;

                case State.Status:
                {
                    status_ = (Status)value;
                    crc_.addByte(value);
                    state_ = State.Size;
                } break;

                case State.Size:
                {                     
                    crc_.addByte(value);

                    size_ <<= 8;
                    size_  |= value;

                    counter_ += 1;

                    if (counter_ == 2)
                    {
                        if (Acknowledge.PAYLOAD_SIZE == size_)
                        {
                            counter_ = 0;
                            payload_ = new byte[size_];
                            state_   = State.Payload;
                        }
                        else if (0 == size_)
                        {
                            counter_ = 0;
                            payload_ = new byte[size_];
                            state_   = State.CRC;
                        }
                        else
                        {
                            return false;
                        }
                    }
                } break;

                case State.Payload:
                {
                    crc_.addByte(value);

                    payload_[counter_] = value;

                    counter_ += 1;

                    if( counter_ >= payload_.Length )
                    {
                        counter_ = 0;
                        state_   = State.CRC;
                    }
                } break;

                case State.CRC:
                {                    
                    crc_received_ <<= 8;
                    crc_received_  |= value;

                    counter_ += 1;

                    if (counter_ == 2)
                    {
                        if (crc_ == crc_received_)
                        {
                            // Console.WriteLine(_crc_received.ToString("x4") + " " + crc_.ToString("x4"));

                            /*
                            Acknowledge acknowledge = new Acknowledge()
                            {
                                Marker  = Marker.ScientificInstrument,
                                Status  = (Status)status_,
                                Size    = (UInt16)payload_.Length,
                                Payload = payload_,
                                CRC     = crc_received_
                            };
                             */

                            AcknowledgedParsedEventArgs arguments = new AcknowledgedParsedEventArgs()
                            {
                                Acknowledge = new Acknowledge()
                                {
                                    Marker  = Marker.ScientificInstrument,
                                    Status  = status_,
                                    Size    = (UInt16)payload_.Length,
                                    Payload = payload_,
                                    CRC     = crc_received_
                                }
                            };

                            RaiseParsed(arguments);

                            Reset();
                        }
                        else
                        {
                            return false;
                        }
                    }
                } break;

                default: return false;
            }

            return true;
        }

        private void RaiseParsed(AcknowledgedParsedEventArgs e)
        {
            if( Parsed != null )
            {
                Parsed(this, e);
            }
        }
    }

    public class AcknowledgedParsedEventArgs : EventArgs
    {
        public Acknowledge Acknowledge { get; set; }
    }
}
