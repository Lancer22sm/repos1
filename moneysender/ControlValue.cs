using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Input;

namespace moneysender
{
    public static class ControlValue
    {
        public static int searchSendValue(string countSendRub, string countSendCop)
        {
            int SendRub = Convert.ToInt32(countSendRub) * 100;
            int SendValue = SendRub + Convert.ToInt32(countSendCop);
            return SendValue;
        }
        public static int getBalance(string balance)
        {
            double DoubleBalance = Convert.ToDouble(balance);
            int balanceValue = Convert.ToInt32(DoubleBalance * 100);
            return balanceValue;
        }
        public static string ChangeBalanceInc(int money, int balance)
        {
            double result = (Convert.ToDouble(money) + Convert.ToDouble(balance)) / 100;
            return result.ToString();
        }
        public static string ChangeBalanceDec(int countSend, int balance)
        {
            double result = (Convert.ToDouble(balance) - Convert.ToDouble(countSend)) / 100;
            return result.ToString();
        }
        public static string CheckInt(string checkString)
        {
            int number;
            bool succes = int.TryParse(checkString, out number);
            if (succes == false && checkString != "")
            {
                Regex regex = new Regex(@"\D");
                checkString = regex.Replace(checkString, "");
                return checkString ;
            }
            return checkString;
        }
    }
}