using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            UIElement[] elementsFirstWindow = new UIElement[] { CreateGame, JoinGame };
            UIElement[] elementsSecondWindow = new UIElement[] { LocalGame, WanGame };
            UIElement[] elementsThirdWindow = new UIElement[] { JoinGameLast, textBlockIPFriend, ClientIP, ClientPort };
            UIElement[] elementsLastWindow = new UIElement[] { IPForConnect, ConnectIP, Balance, YourBalance, Rubles, CountSend, howMuchMoneySend };
            _controlUI.AddRangeUIWindow(elementsFirstWindow,
                                        elementsSecondWindow,
                                        elementsThirdWindow,
                                        elementsLastWindow);
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
            Balance.Text = "500.54";
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
                Balance.Text = "500.11";
                ButtonSendClient.Visibility = Visibility.Visible;
                _controlClient.CreateClient(ClientIP.Text, Convert.ToInt32(ClientPort.Text));
            }
        }
        private void CountSend_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void ClientPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number;
            bool succes = int.TryParse(ClientPort.Text, out number);
            if (succes == false && ClientPort.Text != "")
            {
                Regex regex = new Regex(@"\D");
                ClientPort.Text = regex.Replace(ClientPort.Text, "");
            }
        }
        private void ButtonSendServer_Click(object sender, RoutedEventArgs e)
        {
            int SendValue = ControlValue.searchSendValue(CountSend.Text);
            int balance = ControlValue.getBalance(Balance.Text);
            if (SendValue < balance && SendValue != 0)
            {
                _controlServer.ServerSend(SendValue, balance);
            }
            else if (SendValue < balance)
            {
                MessageBox.Show("На вашем счету недостаточно средств");
            }
            else
            {
                MessageBox.Show("Введите кол-во средств например: \"12 руб. 34 коп.\" для перевода");
            }
        }
        private void ButtonSendClient_Click(object sender, RoutedEventArgs e)
        {
            int SendValue = ControlValue.searchSendValue(CountSend.Text);
            int balance = ControlValue.getBalance(Balance.Text);
            if (SendValue < balance && SendValue != 0)
            {
                _controlClient.ClientSend(SendValue, balance);
            }
            else if (SendValue < balance)
            {
                MessageBox.Show("На вашем счету недостаточно средств");
            }
            else
            {
                MessageBox.Show("Введите кол-во средств например: \"12 руб. 34 коп.\" для перевода");
            }
        }
        private void receiverServer()
        {
            int mySms = _controlServer.sms;
            Balance.Text = ControlValue.ChangeBalanceInc(mySms, Balance.Text);
            receivSmsServer.Invoke();
        }
        private void receiverClient()
        {
            int mySms = _controlClient.sms;
            Balance.Text = ControlValue.ChangeBalanceInc(mySms, Balance.Text);
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
