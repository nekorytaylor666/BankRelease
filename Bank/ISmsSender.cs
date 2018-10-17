using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    interface ISmsSender
    {
        bool ValidateSms(string phoneNumber);

        void LogSms(string value, string phoneNumber);
    }
}
