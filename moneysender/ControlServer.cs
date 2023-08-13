using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace moneysender
{
    internal class ControlServer
    {

        private List<TextBlock> _textBlockServer = new List<TextBlock>();
        private Socket tcpServer;
        public string sms;
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
        public void ServerSend(string textBox)
        {
            try
            {
                if (tcpServer != null)
                {
                    string sendSms = textBox;
                    ChangeBalanceDec(sendSms);
                    byte[] data = Encoding.UTF8.GetBytes(sendSms);
                    // отправляем данные
                    tcpServer.Send(data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tcpServer.Close();
            }
        }
        private void ChangeBalanceDec(string count)
        {
            string balance = _textBlockServer[1].Text;
            int first = Convert.ToInt32(balance);
            int second = Convert.ToInt32(count);
            int inc = first - second;
            _textBlockServer[1].Text = inc.ToString();
        }
        public async void ReceiveServer()
        {
            byte[] data = new byte[512];
            if (tcpServer != null)
            {
                try
                {
                    // принимаем данные
                    int bytes = await tcpServer.ReceiveAsync(data);
                    sms = Encoding.UTF8.GetString(data, 0, bytes);
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
