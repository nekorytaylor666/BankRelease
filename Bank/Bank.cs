using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Bank
{
    public class Bank
    {
        public List<Client> clients = new List<Client>();
        public Client LogClient { get; set; }
        private MobizonkzSender smsSender;
        public Bank()
        {
            smsSender = new MobizonkzSender();
            String str;
            using (FileStream stream = new FileStream("LoginAndPasswords.txt", FileMode.OpenOrCreate))
            {
                byte[] vs = new byte[stream.Length];
                stream.Read(vs, 0, vs.Length);
                vs = vs.ToArray();
                str = Encoding.UTF8.GetString(vs);
                if (!String.IsNullOrEmpty(str))
                {
                    string[] arr = str.TrimEnd(']').Split(']');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string[] arr2 = arr[i].Split(';');
                        clients.Add(new Client(arr2[0], arr2[1], arr2[2]));
                    }
                }
            }

        }

        public bool Registration()
        {
            string login = "";
            string pass = "";
            string pass2 = "";
            char[] forbiddenSymbols = { ']', ';', ' ' };
            Console.Clear();
            End:
            Console.Write("НЕЛЬЗЯ использовать ");
            foreach (var item in forbiddenSymbols)
            {
                Console.Write("\"" + item + "\"" + " ");
            }
            Console.WriteLine("\nНЕЛЬЗЯ меньше 7 символов");
            Console.Write("Логин: ");
                login = Console.ReadLine();
            if((login.Length <= 6))
            {
                Console.Clear();
                Console.WriteLine("Логин введен не верно");
                goto End;
            }
            foreach (var item in forbiddenSymbols)
            {
                if (login.Contains(item)) {
                    Console.Clear();
                    Console.WriteLine("Логин введен не верно");
                    goto End;
                }
            }
            Console.Clear();
            End2:
            Console.Write("НЕЛЬЗЯ искользовать ");
            foreach (var item in forbiddenSymbols)
            {
                Console.Write("\"" + item + "\"" + " ");
            }
            Console.WriteLine("\nНЕЛЬЗЯ меньше 7 символов");
            Console.Write("Пароль: ");
            while (true)
            {
                ConsoleKeyInfo ck = Console.ReadKey();
                if (ck.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (ck.Key  == ConsoleKey.Backspace )
                {
                    if (pass.Length != 0)
                    {
                        pass = pass.Remove(pass.Length - 1 , 1);
                        Console.Write(" \b");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                    
                }
                else
                {
                    Console.Write("\b*");
                    pass = pass + ck.KeyChar;
                }
            }
            if ((pass.Length <= 6))
            {
                Console.Clear();
                Console.WriteLine("Пароль введен не верно");
                goto End2;
            }
            foreach (var item in forbiddenSymbols)
            {
                if (pass.Contains(item))
                {
                    Console.Clear();
                    Console.WriteLine("Пароль введен не верно");
                    goto End2;
                }
            }
            Console.Clear();
            Console.Write("Введите пароль еще раз: ");
            
            while (true)
            {
                ConsoleKeyInfo ck = Console.ReadKey();
                if (ck.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (ck.Key == ConsoleKey.Backspace)
                {
                    if (pass2.Length != 0)
                    {
                        pass2 = pass2.Remove(pass2.Length - 1, 1);
                        Console.Write(" \b");
                    }
                    else
                    {
                        Console.Write(" ");
                    }

                }
                else
                {
                    Console.Write("\b*");
                    pass2 = pass2 + ck.KeyChar;
                }
            }
            if(pass != pass2)
            {
                Console.Clear();
                Console.WriteLine("Пароли не похожи");
                goto End2;
            }
            Console.Clear();
            Console.Write("Номер телефона: +7");
            string phoneNumber = Console.ReadLine();
            if (!SendValidationMess(smsSender, phoneNumber))
            {
                return false;
            }
            Client client = new Client(login, pass, phoneNumber);
            clients.Add(client);
                using (FileStream stream = new FileStream("LoginAndPasswords.txt", FileMode.OpenOrCreate))
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        byte[] vs = Encoding.UTF8.GetBytes($"{clients[i].Login};{clients[i].Password};{clients[i].PhoneNumber}]");
                        stream.Write(vs, 0, vs.Length);
                    }
                }
                Console.WriteLine("Создан");
                return true;
            
        }
        public bool LogIn()
        {
            Console.Write("Логин:");
            string login = Console.ReadLine();
            Console.Write("Пароль:");
            string pass = Console.ReadLine();
            Client client = new Client(login, pass);
            

            if (clients.Where(a => a.Login == login && a.Password == pass).FirstOrDefault() != null)// есть ли такие же обьекты
            {
                LogClient = clients.First(a => a.Login == login && a.Password == pass);
                Console.WriteLine("Вход выполнен");
                Console.WriteLine(LogClient.Login);
                Console.WriteLine(LogClient.PhoneNumber);
                Console.ReadLine();
                return true;

            }
            else
            {
                Console.WriteLine("Ошибка");
                return false;
            }
        }

        private void LogTransaction(TransactionMode mode, int value)//логирование транзов
        {
            using (FileStream stream = new FileStream($"{LogClient.Login}/TransLogs.txt", FileMode.Append))//добавляет в конец файла. этот файл создается автоматом
            //создает папку для каждого пользователя с его именем
            {
                byte[] vs = Encoding.UTF8.GetBytes($"{mode}|{LogClient.Login}|{value}|{LogClient.Cash}]");
                stream.Write(vs, 0, vs.Length);
            }
            SendLogMessage(smsSender, $"{mode} на счет {LogClient.Login} послан. Сумма выплаты {value}. Текущий счет равен {LogClient.Cash}!");
        }
        
        public List<string> ListTransactions()//возвращает лист всех транзакций у залогированного пользователя.
        {
            List<string> transList = new List<string>();
            if (File.Exists($"{LogClient.Login}/TransLogs.txt"))
            {
                using (FileStream stream = new FileStream($"{LogClient.Login}/TransLogs.txt", FileMode.Open))
                {
                    byte[] vs = new byte[stream.Length];
                    stream.Read(vs, 0, vs.Length);
                    vs = vs.ToArray();
                    string str = Encoding.UTF8.GetString(vs);
                    if (!String.IsNullOrEmpty(str))
                    {
                        string[] arr = str.TrimEnd(']').Split(']');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            transList.Add(arr[i]);
                        }
                    }
                }
                return transList;
            }
            return null;
        }

        public void Deposit()
        {
            Console.Clear();
            Console.WriteLine($"Здраствуйте,{LogClient.Login}!");
            Console.WriteLine("Сколько вы хотите положить в депозит?");
            int depositValue = 0;
            int.TryParse(Console.ReadLine(), out depositValue);

            if (depositValue <= LogClient.Cash)
            {
                LogClient.Cash -= depositValue;
                if (!Directory.Exists(LogClient.Login))
                {
                    Directory.CreateDirectory(LogClient.Login);
                }
                LogTransaction(TransactionMode.DEPOSIT, depositValue);
                Console.WriteLine("Деньги переведены!");
            }
            else
            {
                Console.WriteLine("У вас не хватает денег...!");
            }
                Console.ReadLine();
        }

        public void Credit()
        {
            Console.Clear();
            Console.WriteLine($"Здраствуйте,{LogClient.Login}!");
            Console.WriteLine("Сколько вы хотите взять в кредит?");

            int creditValue = 0;

            int.TryParse(Console.ReadLine(), out creditValue);

            LogClient.Cash += creditValue;

            if (!Directory.Exists(LogClient.Login))
            {
                Directory.CreateDirectory(LogClient.Login);
            }
            LogTransaction(TransactionMode.CREDIT, creditValue);
            Console.WriteLine("Деньги переведены!");
            Console.ReadLine();
        }
        private bool SendValidationMess(ISmsSender smsSender, string number = "")
        {
            if (number == "")
            {
                Console.WriteLine("Для подтверджения вашей личности, вам отправили СМС");
                Console.ReadLine();
                return smsSender.ValidateSms(LogClient.PhoneNumber);
            }
            else
            {
                return smsSender.ValidateSms(number);
            }
        }

        private void SendLogMessage(ISmsSender smsSender, string value)
        {
            smsSender.LogSms(value, LogClient.PhoneNumber);
        }


    }
}
