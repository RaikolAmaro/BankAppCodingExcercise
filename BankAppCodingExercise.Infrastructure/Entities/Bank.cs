using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankAppCodingExcercise.Infrastructure.Entities
{
    public class Bank
    {
        [Key]
        public int BankId { get; set; }
        public string Name { get; private set; }
        public List<Account> Accounts { get; private set; }
    }
}
