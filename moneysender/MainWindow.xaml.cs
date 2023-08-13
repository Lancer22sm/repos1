using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace moneysender
{
    public partial class MainWindow : Window
    {
        private readonly ControlUI _controlUI = new();
        private readonly ControlServer _controlServer;
        private readonly ControlClient _controlClient;
        event Action receivSmsServer;
        event Action receivSmsClient;
        public MainWindow()
        {
            InitializeComponent();
            Action OnReceiveServer = receiverServer;
            _controlServer = new(OnReceiveServer);
            Action OnReceiveClient = receiverClient;
            _controlClient = new(OnReceiveClient);
            receivSmsServer += _controlServer.ReceiveServer;
            receivSmsClient += _controlClient.ReceiveClient;
            _controlUI.AddRangeUIFirstWindow(new UIElement[] { CreateGame, JoinGame });
            _controlUI.AddRangeUISecondWindow(new UIElement[] { LocalGame, WanGame });
            _controlUI.AddRangeUIThirdWindow(new UIElement[] { JoinGameLast, textBlockIPFriend, ClientIP, ClientPort });
            _controlUI.AddRangeUILastWindow(new UIElement[] { IPForConnect, ConnectIP, Balance, YourBalance, Rubles, CountSend, howMuchMoneySend });
            _controlServer.AddRangeTextBlock(new TextBlock[] { ConnectIP, Balance });
            _controlClient.AddRange(new TextBlock[] { ConnectIP, Balance });
        }
        private void CreateGame_Click(object sender, RoutedEventArgs e)
        {
            _controlUI.Hide(1);
            _controlUI.Show(2);
        }
        private void LocalGame_Click(object sender, RoutedEventArgs e)
        {
            _controlUI.Hide(2);
            _controlUI.Show(4);
            Balance.Text = "500";
            ButtonSendServer.Visibility = Visibility.Visible;
            IPAddress localAddres = _controlServer.SayIpLocal();
            _controlServer.CreateServer(localAddres);
        }

        private void WanGame_Click(object sender, RoutedEventArgs e)
        {
            _controlServer.SayIPWan();
        }

        private void JoinGame_Click(object sender, RoutedEventArgs e)
        {
            _controlUI.Hide(1);
            _controlUI.Show(3);
        }
        private void JoinGameLast_Click(object sender, RoutedEventArgs e)
        {
            if (ClientIP.Text != null && ClientPort.Text != null)
            {
                _controlUI.Hide(3);
                _controlUI.Show(4);
                Balance.Text = "500";
                ButtonSendClient.Visibility = Visibility.Visible;
                string ipClient = ClientIP.Text;
                int port = Convert.ToInt32(ClientPort.Text);
                _controlClient.CreateClient(ipClient, port);
            }
        }
        private void CountSend_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number;
            bool succes = int.TryParse(CountSend.Text, out number);
            if (succes == false && CountSend.Text != "")
            {
                string pattern = @"\D";
                string target = "";
                Regex regex = new Regex(pattern);
                string result = regex.Replace(CountSend.Text, target);
                CountSend.Text = result;
            }
        }
        private void ClientPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number;
            bool succes = int.TryParse(ClientPort.Text, out number);
            if (succes == false && ClientPort.Text != "")
            {
                string pattern = @"\D";
                string target = "";
                Regex regex = new Regex(pattern);
                string result = regex.Replace(ClientPort.Text, target);
                ClientPort.Text = result;
            }
        }
        private void ButtonSendServer_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(CountSend.Text) < Convert.ToInt32(Balance.Text))
            {
                _controlServer.ServerSend(CountSend.Text);
            }
        }
        private void ButtonSendClient_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(CountSend.Text) < Convert.ToInt32(Balance.Text))
            {
                _controlClient.ClientSend(CountSend.Text);
            }
        }
        private void receiverServer()
        {
            string mySms = _controlServer.sms;
            ChangeBalanceInc(mySms);
            receivSmsServer.Invoke();
        }
        private void ChangeBalanceInc(string sms)
        {
            int first = Convert.ToInt32(sms);
            string text = "";
            Dispatcher.Invoke(() => text = Balance.Text);
            int second = Convert.ToInt32(text);
            int inc = first + second;
            Dispatcher.Invoke(() => Balance.Text = inc.ToString());
        }
        private void receiverClient()
        {
            string mySms = _controlClient.sms;
            ChangeBalanceInc(mySms);
            receivSmsClient.Invoke();
        }
        private void XButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        private void LineButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Polygon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
