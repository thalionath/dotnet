using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace unibe.space.luna.egse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private buni.CommandViewModel _command_view_model = new buni.CommandViewModel();
        private buni.DownLink _down_link = new buni.DownLink();
        private buni.UpLink   _up_link   = new buni.UpLink();

        public MainWindow()
        {
            InitializeComponent();

            _up_link.PortName = "COM18";

            DataContext = _command_view_model;            
        }

        private void SendStatusRequest_Click(object sender, RoutedEventArgs e)
        {            
            _down_link.Write(buni.Command.CreateStatusRequest().ToBytes());
        }

        private void OpenPorts_Click(object sender, RoutedEventArgs e)
        {
            _down_link.Open();
            _up_link.Open();
        }

    }
}
