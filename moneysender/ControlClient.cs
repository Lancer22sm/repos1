using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace moneysender
{
    internal class ControlClient
    {
        private List<TextBlock> _textBlockClient = new List<TextBlock>();
        private Socket tcpClient;
        public int sms;
        Action ClientReceive;

        public ControlClient(Action GiveSms)
        {
            ClientReceive = GiveSms;
        }

        public void AddRange(TextBlock[] textBlocks)
        {
            _textBlockClient.AddRange(textBlocks);

        }
        public async void CreateClient(string ipclient, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ipclient);
            IPEndPoint ipPoint = new IPEndPoint(localAddr, port);
            Console.WriteLine(ipPoint);
            tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await tcpClient.ConnectAsync(localAddr, port);
            ReceiveClient();
        }
        public void ClientSend(int countSend, int balance)
        {
            try
            {
                if (tcpClient != null)
                {
                    string sendSms = ChangeBalanceDec(countSend, balance);
                    int sendValue = Convert.ToInt32(sendSms);
                    byte[] intBytes = BitConverter.GetBytes(sendValue);
                    //byte[] data = Encoding.UTF8.GetBytes(sendSms);
                    // отправляем данные
                    tcpClient.Send(intBytes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tcpClient.Close();
            }
        }
        private string ChangeBalanceDec(int countSend, int balance)
        {
            int SendRub = getRub(countSend);
            int SendCop = getCop(countSend);
            int balanceRub = getRub(balance);
            int balanceCop = getCop(balance);
            if (SendCop > balanceCop)
            {
                SendCop = SendCop + 100;
                SendRub = SendRub - 1;
            }
            int Rubles = balanceRub - SendRub;
            int Cop = balanceCop - SendCop;
            _textBlockClient[1].Text = $"{Rubles}.{Cop}";
            string send = $"{SendRub}{SendCop}";
            return send;
        }
        private int getRub(int money)
        {
            Regex regexsearchValues = new Regex(@"\d");
            MatchCollection matchesNumber = regexsearchValues.Matches(money.ToString());
            int matchesNumberCount = matchesNumber.Count;
            string Rub = "";
            for (int i = 0; i < matchesNumberCount - 2; i++)
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
        public async void ReceiveClient()
        {
            byte[] data = new byte[512];
            if (tcpClient != null)
            {
                try
                {
                    // принимаем данные
                    await tcpClient.ReceiveAsync(data);
                    sms = BitConverter.ToInt32(data, 0);
                    ClientReceive();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    tcpClient.Close();
                }
            }
        }
    }
}
