using MFireProtocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ServerTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MFireConnection con;

        public MainWindow()
        {
            InitializeComponent();
            con = new MFireConnection();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            con.Connect("localhost");
            WriteLog("Connected to server.");
            con.MFireCmdReceived += Con_MFireCmdReceived;
            con.SendResetSimulation();
            WriteLog("Reset simulation");
            configureMfire();
            WriteLog("Sent configuration");
        }

        private void configureMfire()
        {
            con.SendMFireCmd(new MFCConfigureMFire()
            {
               StartJunction = 1 
            });
            con.SendUpdateJunction(1, new MFJunction()
            {
                Number = 1,
                Temperature = 70
            });
            con.SendUpdateJunction(2, new MFJunction()
            {
                Number = 2,
                Temperature = 70
            });
            con.SendUpdateAirway(1, new MFAirway()
            {
                Length = 100,
                EndJunction = 2,
                StartJunction = 1,
                Number = 1,
                Perimeter = 10,
                CrossSectionalArea = 10,
                FlowRate = 1000
            });
            con.SendUpdateFan(1, new MFFan()
            {
                AirwayNo = 1,
                Number = 1
            });
        }

        private void Con_MFireCmdReceived(MFireConnection obj)
        {
            MFireCmd cmd = obj.DequeueReceivedCmd();

            while (cmd != null)
            {
                WriteLog($"Cmd Received({ DateTime.Now.TimeOfDay}): { cmd.ToString()}");
                cmd = obj.DequeueReceivedCmd();
            }
        }

        private void WriteLog(string text)
        {
            Dispatcher.Invoke(() =>
            {
                log.Text += $"{text}{Environment.NewLine}";
            });
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                con.SendRunSimulation();
                await Task.Delay(10);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            con.SendResetSimulation();
            WriteLog("Reset simulation");
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var r = await con.SendMFireCmdWithResult<MFRGetOutputUnits>(new MFCGetOutputUnits());
            Console.WriteLine(r);

            //con.SendMFireCmd(new MFCLoadConfigFile()
            //{
            //    FileName = @"C:\work\Git Repos\AMS_Monitor\MFIRE\ExampleFiles\example1.txt"
            //});
        }
    }
}
