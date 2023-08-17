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
                    _textBlockClient[1].Text = ControlValue.ChangeBalanceDec(countSend, balance);
                    byte[] intBytes = BitConverter.GetBytes(countSend);
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
