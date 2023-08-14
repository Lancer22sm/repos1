using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace moneysender
{
    internal class ControlServer
    {

        private List<TextBlock> _textBlockServer = new List<TextBlock>();
        private Socket tcpServer;
        public int sms;
        Action ServerReceive;

        public ControlServer(Action GiveSms)
        {
            ServerReceive = GiveSms;
        }

        public void AddRangeTextBlock(TextBlock[] textBlocks)
        {
            _textBlockServer.AddRange(textBlocks);
        }
        public void SayIPWan()
        {
            string serviceUrl = "https://ipinfo.io/ip";
            IPAddress myIP = IPAddress.Parse(new System.Net.WebClient().DownloadString(serviceUrl));
            MessageBox.Show($"ваш внешний Ip: {myIP.ToString()}, пока что нет поддержки создания на нём сервера");
        }
        public IPAddress SayIpLocal()
        {
            string myIP = "";
            System.String host = Dns.GetHostName();
            IPAddress[] ip = Dns.GetHostByName(host).AddressList;
            foreach (IPAddress ipies in ip)
            {
                if (ipies.AddressFamily == AddressFamily.InterNetwork)
                {
                    myIP = ipies.ToString();
                    break;
                }
            }
            IPAddress localAddr = IPAddress.Parse(myIP);
            IPEndPoint ipPoint = new IPEndPoint(localAddr, 8888);
            _textBlockServer[0].Text = ipPoint.ToString();
            return localAddr;
        }
        public async void CreateServer(IPAddress Addres)
        {
            IPAddress MyAddres = IPAddress.Parse(Addres.ToString());
            IPEndPoint ipPoint = new IPEndPoint(MyAddres, 8888);
            Socket tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            tcpListener.Bind(ipPoint);
            tcpListener.Listen();    // запускаем сервер

            // получаем входящее подключение
            tcpServer = await tcpListener.AcceptAsync();
            ReceiveServer();
        }
        public void ServerSend(int countSend, int balance)
        {
            try
            {
                if (tcpServer != null)
                {
                    string sendSms = ChangeBalanceDec(countSend, balance);
                    int sendValue = Convert.ToInt32(sendSms);
                    byte[] intBytes = BitConverter.GetBytes(sendValue);
                    // отправляем данные
                    tcpServer.Send(intBytes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tcpServer.Close();
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
            _textBlockServer[1].Text = $"{Rubles}.{Cop}";
            string send = $"{SendRub}{SendCop}";
            return send;
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
        public async void ReceiveServer()
        {
            byte[] data = new byte[512];
            if (tcpServer != null)
            {
                try
                {
                    // принимаем данные
                    await tcpServer.ReceiveAsync(data);
                    sms = BitConverter.ToInt32(data, 0);
                    ServerReceive();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    tcpServer.Close();
                }
            }
        }
    }
}
