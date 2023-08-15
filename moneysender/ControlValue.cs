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
        public static int searchSendValue(string countSend)
        {
            int SendValue = 0;
            Regex regexsearchValues = new Regex(@"\d{1,4}\s*\d{1,2}");
            MatchCollection matches = regexsearchValues.Matches(countSend);
            if (matches.Count == 2)
            {
                string searchValueRub = matches[0].Value.ToString();
                int searchValueCop = Convert.ToInt32(matches[1].Value.ToString());
                string changeValue2;
                if (searchValueCop < 10) { changeValue2 = "0" + searchValueCop; } else { changeValue2 = searchValueCop.ToString(); }
                SendValue = Convert.ToInt32($"{searchValueRub}{changeValue2}");
            }
            else if (matches.Count == 1)
            {
                string searchValueRub = matches[0].Value.ToString();
                string changeValue2 = "00";
                SendValue = Convert.ToInt32($"{searchValueRub}{changeValue2}");
            }
            return SendValue;
        }
        public static int getBalance(string balance)
        {
            Regex regexsearchValues = new Regex(@"\d{1,4}\s*\d{1,2}");
            MatchCollection matches = regexsearchValues.Matches(balance);
            string searchValueRub = matches[0].Value.ToString();
            int searchValueCop = Convert.ToInt32(matches[1].Value.ToString());
            string changeValue2;
            if (searchValueCop < 10) { changeValue2 = "0" + searchValueCop; } else { changeValue2 = searchValueCop.ToString(); }
            int balanceValue = Convert.ToInt32(searchValueRub + changeValue2);
            return balanceValue;
        }
        private static int getRub(int money)
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
        private static int getCop(int money)
        {
            Regex regexsearchValues = new Regex(@"\d");
            MatchCollection matchesNumber = regexsearchValues.Matches(money.ToString());
            int matchesNumberCount = matchesNumber.Count;
            string SendCop = matchesNumber[matchesNumberCount - 2].Value.ToString() + matchesNumber[matchesNumberCount - 1].Value.ToString();
            int FullCop = Convert.ToInt32(SendCop);
            return FullCop;
        }
        public static string ChangeBalanceInc(int money, string StrBalance)
        {
            int ReceivRub = getRub(money);
            int ReceivCop = getCop(money);
            int balance = getBalance(StrBalance);
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
            if (FullCop < 10)
            {
                string plusNull = $"0{FullCop}";
                return $"{FullRub}.{plusNull}";
            }
            return $"{FullRub}.{FullCop}";
        }
        public static string SayCountSend(int countSend)
        {
            int SendRub = getRub(countSend);
            int SendCop = getCop(countSend);
            if (SendCop < 10)
            {
                string plusNull = $"0{SendCop}";
                return $"{SendRub}.{plusNull}";
            }
            return $"{SendRub}{SendCop}";
        }
        public static string ChangeBalanceDec(int countSend, int balance)
        {
            int SendRub = getRub(countSend);
            int SendCop = getCop(countSend);
            int balanceRub = getRub(balance);
            int balanceCop = getCop(balance);
            if (SendCop > balanceCop)
            {
                balanceCop = balanceCop + 100;
                balanceRub = balanceRub - 1;
            }
            int Rubles = balanceRub - SendRub;
            int Cop = balanceCop - SendCop;
            if (Cop < 10)
            {
                string plusNull = $"0{Cop}";
                return $"{Rubles}.{plusNull}";
            }
            return $"{Rubles}.{Cop}";
        }
    }
}