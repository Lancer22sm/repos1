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
            int SendValue = searchSendValue();
            int balance = getBalance();
            if (SendValue < balance && SendValue != 0)
            {
                _controlServer.ServerSend(SendValue, balance);
            }
            else
            {
                MessageBox.Show("Введите кол-во средств (12 руб. 34 коп.) для перевода");
            }
        }
        private int getBalance()
        {
            Regex regexsearchValues = new Regex(@"\d{1,4}\s*\d{1,2}");
            MatchCollection matches = regexsearchValues.Matches(Balance.Text);
            string searchValueRub = matches[0].Value.ToString();
            int searchValueCop = Convert.ToInt32(matches[1].Value.ToString());
            string changeValue2;
            if (searchValueCop < 10) { changeValue2 = "0" + searchValueCop; } else { changeValue2 = searchValueCop.ToString(); }
            int balanceValue = Convert.ToInt32(searchValueRub + changeValue2);
            return balanceValue;
        }
        private int searchSendValue()
        {
            int SendValue = 0;
            Regex regexsearchValues = new Regex(@"\d{1,4}\s*\d{1,2}");
            MatchCollection matches = regexsearchValues.Matches(CountSend.Text);
            if (matches.Count == 2)
            {
                string searchValueRub = matches[0].Value.ToString();
                int searchValueCop = Convert.ToInt32(matches[1].Value.ToString());
                string changeValue2;
                if (searchValueCop < 10) { changeValue2 = "0" + searchValueCop; } else { changeValue2 = searchValueCop.ToString(); }
                SendValue = Convert.ToInt32($"{searchValueRub}{changeValue2}");
            }
            return SendValue;
        }
        private void ButtonSendClient_Click(object sender, RoutedEventArgs e)
        {
            int SendValue = searchSendValue();
            int balance = getBalance();
            if (SendValue < balance && SendValue != 0)
            {
                _controlClient.ClientSend(SendValue, balance);
            }
            else
            {
                MessageBox.Show("Введите кол-во средств (12 руб. 34 коп.) для перевода");
            }
        }
        private void receiverServer()
        {
            int mySms = _controlServer.sms;
            ChangeBalanceInc(mySms);
            receivSmsServer.Invoke();
        }
        private void receiverClient()
        {
            int mySms = _controlClient.sms;
            ChangeBalanceInc(mySms);
            receivSmsClient.Invoke();
        }
        private void ChangeBalanceInc(int money)
        {
            int ReceivRub = getRub(money);
            int ReceivCop = getCop(money);
            int balance = getBalance();
            int balanceRub = getRub(balance);
            int balanceCop = getCop(balance);
            int FullRub;
            int FullCop;
            if (ReceivCop + balanceCop > 99)
            {
                FullCop = ReceivCop + balanceCop - 100;
                FullRub = ReceivRub + balanceRub + 1;
            }
            else
            {
                FullCop = ReceivCop + balanceCop;
                FullRub = ReceivRub + balanceRub;
            }
            Balance.Text = $"{FullRub}.{FullCop}";
        }
        private int getRub(int money)
        {
            Regex regexsearchValues = new Regex(@"\d");
            MatchCollection matchesNumber = regexsearchValues.Matches(money.ToString());
            int matchesNumberCount = matchesNumber.Count - 2;
            string Rub = "";
            for (int i = 0; i < matchesNumberCount; i++)
            {
                Rub += matchesNumber[i].Value.ToString();
            }
            int FullRub = Convert.ToInt32(Rub);
            return FullRub;
        }
        private int getCop(int money)
        {
            Regex regexsearchValues = new Regex(@"\d");
            MatchCollection matchesNumber = regexsearchValues.Matches(money.ToString());
            int matchesNumberCount = matchesNumber.Count;
            string SendCop = matchesNumber[matchesNumberCount - 2].Value.ToString() + matchesNumber[matchesNumberCount - 1].Value.ToString();
            int FullCop = Convert.ToInt32(SendCop);
            return FullCop;
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
