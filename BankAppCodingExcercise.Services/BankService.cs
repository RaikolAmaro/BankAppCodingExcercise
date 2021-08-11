using BankAppCodingExcercise.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankAppCodingExcercise.Services
{
    public class BankService
    {
        private readonly BankDbContext _context;

        public BankService(BankDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Makes a deposit.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="accountId"></param>
        /// <returns>New Balance</returns>
        public async Task<decimal> MakeDepositAsync(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) throw new ApplicationException($"Account Id {accountId} not found");

            var newBalance = account.MakeDeposit(amount);
            
            await _context.SaveChangesAsync();

            return newBalance;
        }

        /// <summary>
        /// Makes a withdrawal.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="accountId"></param>
        /// <returns>New Balance</returns>
        public async Task<decimal> MakeWithdrawalAsync(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) throw new ApplicationException($"Account Id {accountId} not found");

            var newBalance = account.MakeWithdrawal(amount);

            await _context.SaveChangesAsync();

            return newBalance;
        }

        /// <summary>
        /// Makes a transfer, which is treated as a withdraw transaction from account1 and a deposit to account2.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="accountId"></param>
        /// <returns>New Balance</returns>
        public async Task<(decimal, decimal)> MakeTransferAsync(int accountFromId, int accountToId, decimal amount)
        {
            var accountFrom = await _context.Accounts.FindAsync(accountFromId);
            if (accountFrom == null) throw new ApplicationException($"Account id {accountFromId} not found");

            var accountTo = await _context.Accounts.FindAsync(accountToId);
            if (accountTo == null) throw new ApplicationException($"Account id {accountToId} not found");

            var newFromBalance = accountFrom.MakeWithdrawal(amount);
            var newToBalance = accountTo.MakeDeposit(amount);

            await _context.SaveChangesAsync();

            return (newFromBalance, newToBalance);
        }
    }
}
