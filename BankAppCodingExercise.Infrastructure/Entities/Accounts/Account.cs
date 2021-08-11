using BankAppCodingExcercise.Infrastructure.Entities.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankAppCodingExcercise.Infrastructure.Entities
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public decimal Balance { get; protected set; }
        
        public AccountOwner AccountOwner { get; private set; }
        public int AccountOwnerId { get; set; }

        public List<Transaction> Transactions { get; private set; } = new List<Transaction>();

        public Account(decimal initialBalance)
        {
            Balance = initialBalance;
        }

        public Account()
        {
        }

        public virtual decimal MakeDeposit(decimal amount)
        {
            Balance += amount;
            Transactions.Add(new DepositTransaction() { Amount = amount });
            return Balance;
        }

        public virtual decimal MakeWithdrawal(decimal amount)
        {
            if (Balance < amount) throw new ApplicationException("Not enough funds to withdraw the requested amount.");

            Balance -= amount;
            Transactions.Add(new WithdrawTransaction() { Amount = amount });
            return Balance;
        }
    }
}
