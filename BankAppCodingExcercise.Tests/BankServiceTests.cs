using BankAppCodingExcercise.Infrastructure;
using BankAppCodingExcercise.Infrastructure.Entities;
using BankAppCodingExcercise.Infrastructure.Entities.Transactions;
using BankAppCodingExcercise.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace BankAppCodingExcercise.Tests
{
    public class BankServiceTests
    {
        private int _accountId1 = 1;
        private int _accountId2 = 2;
        private int _accountIdIndividual = 3;
        private DbContextOptions<BankDbContext> _options;

        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<BankDbContext>();
            builder.UseInMemoryDatabase("bankDb");
            var options = builder.Options;

            using (var context = new BankDbContext(options))
            {
                context.Accounts.Add(new Infrastructure.Entities.Account() { AccountId = _accountId1 });
                context.Accounts.Add(new Infrastructure.Entities.Account(500) { AccountId = _accountId2 });
                context.Accounts.Add(new Infrastructure.Entities.IndividualAccount(1000) { AccountId = _accountIdIndividual });

                context.SaveChanges();
            }

            _options = options;
        }

        [TearDown]
        public void TearDown()
        {
            using (var context = new BankDbContext(_options))
            {
                context.Accounts.Remove(context.Accounts.Find(_accountId1));
                context.Accounts.Remove(context.Accounts.Find(_accountId2));
                context.Accounts.Remove(context.Accounts.Find(_accountIdIndividual));
                context.SaveChanges();
            }
        }

        [Test]
        public async Task MakeDeposit_IsSuccessful()
        {
            using (var context = new BankDbContext(_options))
            {
                var sut = new BankService(context);

                var amount = 330;

                await sut.MakeDepositAsync(_accountId1, amount);

                var account = await context.Accounts.FindAsync(_accountId1);

                Assert.AreEqual(amount, account.Balance);
                Assert.AreEqual(account.Transactions.Count, 1);
                Assert.AreEqual(account.Transactions[0].Amount, amount);
                Assert.AreEqual(account.Transactions[0].GetType(), typeof(DepositTransaction));
            }
        }

        [Test]
        public async Task MakeWithdrawal_IsSuccessful()
        {
            using (var context = new BankDbContext(_options))
            {
                var sut = new BankService(context);

                var amount = 330;

                await sut.MakeWithdrawalAsync(_accountId2, amount);

                var account = await context.Accounts.FindAsync(_accountId2);

                Assert.AreEqual(500 - 330, account.Balance);
                Assert.AreEqual(account.Transactions.Count, 1);
                Assert.AreEqual(account.Transactions[0].Amount, amount);
                Assert.AreEqual(account.Transactions[0].GetType(), typeof(WithdrawTransaction));
            }
        }

        [Test]
        public async Task MakeWithdrawal_FailsWhenGoingNegative()
        {
            using (var context = new BankDbContext(_options))
            {
                var sut = new BankService(context);

                var amount = 600;

                var msg = Assert.ThrowsAsync<ApplicationException>
                    (async () => await sut.MakeWithdrawalAsync(_accountId2, amount)).Message;
                Assert.AreEqual("Not enough funds to withdraw the requested amount.", msg);
            }
        }

        [Test]
        public void MakeWithdrawal_FailsOnWithdrawalLimit()
        {
            using (var context = new BankDbContext(_options))
            {
                var sut = new BankService(context);

                var amount = 600;

                var msg = Assert.ThrowsAsync<ApplicationException>(async 
                    () => await sut.MakeWithdrawalAsync(_accountIdIndividual, amount)).Message;
                Assert.AreEqual("Cannot withdraw from than 500 from an Individual Account", msg);
            }
        }

        [Test]
        public async Task MakeTransfer_IsSuccessful()
        {
            using (var context = new BankDbContext(_options))
            {
                var sut = new BankService(context);

                var amount = 200;

                await sut.MakeTransferAsync(_accountId2, _accountId1, amount);

                var fromAccount = await context.Accounts.FindAsync(_accountId2);

                Assert.AreEqual(500 - amount, fromAccount.Balance);
                Assert.AreEqual(fromAccount.Transactions.Count, 1);
                Assert.AreEqual(fromAccount.Transactions[0].Amount, amount);
                Assert.AreEqual(fromAccount.Transactions[0].GetType(), typeof(WithdrawTransaction));

                var toAccount = await context.Accounts.FindAsync(_accountId1);

                Assert.AreEqual(amount, toAccount.Balance);
                Assert.AreEqual(toAccount.Transactions.Count, 1);
                Assert.AreEqual(toAccount.Transactions[0].Amount, amount);
                Assert.AreEqual(toAccount.Transactions[0].GetType(), typeof(DepositTransaction));
            }
        }
    }
}