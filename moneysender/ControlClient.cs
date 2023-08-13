using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace moneysender
{
    internal class ControlClient
    {
        private List<TextBlock> _textBlockClient = new List<TextBlock>();
        private Socket tcpClient;
        public string sms;
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
        public void ClientSend(string textBox)
        {
            try
            {
                if (tcpClient != null)
                {
                    string sendSms = textBox;
                    ChangeBalanceDec(sendSms);
                    byte[] data = Encoding.UTF8.GetBytes(sendSms);
                    // отправляем данные
                    tcpClient.Send(data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tcpClient.Close();
            }
        }
        private void ChangeBalanceDec(string count)
        {
            string balance = _textBlockClient[1].Text;
            int first = Convert.ToInt32(balance);
            int second = Convert.ToInt32(count);
            int inc = first - second;
            _textBlockClient[1].Text = inc.ToString();
        }
        public async void ReceiveClient()
        {
            byte[] data = new byte[512];
            if (tcpClient != null)
            {
                try
                {
                    // принимаем данные
                    int bytes = await tcpClient.ReceiveAsync(data);
                    sms = Encoding.UTF8.GetString(data, 0, bytes);
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
