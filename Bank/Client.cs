using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Client
    {

        public string Login { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int Cash { get; set; }
        public List<Account> accounts;
        public Client() : this("", "")
        {
        }
        public Client(string login, string pass) : this(login, pass, "")
        {
        }
        public Client(string login, string pass, string phoneNumber)
        {
            Login = login;
            Password = pass;
            PhoneNumber = phoneNumber;
        }
    }
}
