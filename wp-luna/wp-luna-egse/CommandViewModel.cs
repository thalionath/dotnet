using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace wp.luna.buni
{
    class CommandViewModel : INotifyPropertyChanged
    {
        private Command _command = new Command();

        public Marker Marker
        {
            get { return _command.Marker; }
            set
            { 
                _command.Marker = value;

                RaisePropertyChanged();
                RaisePropertyChanged("CRC");
            }
        }

        public Flag Flag
        {
            get { return _command.Flag; }
            set
            { 
                _command.Flag = value;

                RaisePropertyChanged();
                RaisePropertyChanged("CanHavePayload");
                RaisePropertyChanged("CRC");
            }
        }

        public bool CanHavePayload
        {
            get
            { 
                return Flag == buni.Flag.TimeCode 
                    || Flag == buni.Flag.UKS;
            }
        }

        public UInt16 CRC
        {
            get { return _command.ComputeCrc(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if( PropertyChanged != null )
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
