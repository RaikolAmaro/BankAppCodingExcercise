using System;
using BankAppCodingExcercise.Infrastructure.Entities;

namespace BankAppCodingExcercise
{
    class Program
    {
        static void Main(string[] args)
        {
            var acc = new Account();
            Console.WriteLine(acc.GetHashCode());
            Console.ReadLine();
        }
    }
}
