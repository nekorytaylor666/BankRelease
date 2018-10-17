using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public enum Currancy
    {
        KZT, EUR, USD
    }
    public enum AccountType
    {
        loan, debit
    }
    public class Account
    {
        public double Balance { get; set; }
        public Currancy Currancy { get; set; }
        public string NoAccount { get; set; }
        public AccountType AccountType { get; set; }
        public List<Transaction> Tranzaktions { get; set; }

    }
    public class Transaction
    {
        public DateTime dateTransaction { get; set; }
        public int NoTransaction { get; set; }
        public double SumTransaction { get; set; }

    }
}
