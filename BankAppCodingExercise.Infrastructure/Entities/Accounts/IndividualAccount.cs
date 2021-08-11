using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppCodingExcercise.Infrastructure.Entities
{
    public class IndividualAccount : InvestmentAccount
    {
        public decimal WithdrawalLimit { get; set; } = 500;

        public IndividualAccount(decimal initialBalance)
        {
            Balance = initialBalance;
        }

        public IndividualAccount() { }

        public override decimal MakeWithdrawal(decimal amount)
        {
            if (amount > WithdrawalLimit) 
                throw new ApplicationException($"Cannot withdraw from than {WithdrawalLimit} from an Individual Account");

            return base.MakeWithdrawal(amount);
        }
    }
}
