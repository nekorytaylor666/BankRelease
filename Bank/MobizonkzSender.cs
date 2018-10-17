using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace Bank
{
    class MobizonkzSender : ISmsSender
    {
        string api = "kz5e1bca16b305629357b61fe1e53112cb03b26f095eb83ebbeb79cef8f348bfac78fa";

        public void LogSms(string value, string phoneNumber)
        {
            string url = $"https://api.mobizon.kz/service/message/sendsmsmessage?recipient=7{phoneNumber}&text={value}!&apiKey={api}";
            using (var webClient = new WebClient())
            {
                var response = webClient.DownloadString(url);
            }
        }

        public bool ValidateSms(string phoneNumber)
        {
            string code_for_check = "";
            Random rnd = new Random();
            string code = rnd.Next(1000, 9999).ToString();
            string mess = "Код для активации: " + code;
            string url = $"https://api.mobizon.kz/service/message/sendsmsmessage?recipient=7{phoneNumber}&text={mess}!&apiKey={api}";

            using (var webClient = new WebClient())
            {
                var response = webClient.DownloadString(url);
            }
            int tr = 3;
            Console.Clear();
            while (true)
            {
                Console.Write("Введите код активаций: ");
                while (true)
                {
                    ConsoleKeyInfo ck = Console.ReadKey();
                    if (ck.Key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        return false;
                    }
                    else if (ck.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    else if (ck.Key == ConsoleKey.Backspace)
                    {
                        if (code_for_check.Length != 0)
                        {
                            code_for_check = code_for_check.Remove(code_for_check.Length - 1, 1);
                            Console.Write(" \b");
                        }
                        else
                        {
                            Console.Write(" ");
                        }

                    }
                    else if (code_for_check.Length >= 4)
                    {
                        Console.Write("\b \b");
                    }
                    else if ((ConsoleKey.NumPad0 <= ck.Key && ck.Key <= ConsoleKey.NumPad9) || (ConsoleKey.D0 <= ck.Key && ck.Key <= ConsoleKey.D9))
                    {
                        code_for_check = code_for_check + ck.KeyChar;
                    }
                    else
                    {
                        Console.Write("\b \b");
                    }
                }
                Console.Clear();
                if (code_for_check == code)
                {
                    return true;
                }
                if (tr <= 0)
                {
                    return false;
                }
                tr--;
                Console.WriteLine("Осталось " + tr + " попыток");
                code_for_check = "";
            }
            
        }
    }
}
