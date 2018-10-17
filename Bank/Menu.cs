using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Menu
    {
        Bank bank = new Bank();
        public void Print()
        {
            bool isLog = true;
            int choice = 0;
            while (isLog)
            {
                Console.WriteLine("1.Регистрация");
                Console.WriteLine("2.Вход");
                
                int.TryParse(Console.ReadLine(), out choice);
                switch (choice)
                {
                    case 1:
                        bank.Registration();
                        break;
                    case 2:
                        isLog = !bank.LogIn();
                        break;
                    default:
                        break;
                }
                Console.WriteLine("Нажмите ENTER чтобы продолжить...");
                Console.ReadLine();
                Console.Clear();
            }
            bank.Credit();
            bank.Deposit();
        }
    }
}
