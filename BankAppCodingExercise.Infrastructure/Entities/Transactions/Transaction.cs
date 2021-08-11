using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankAppCodingExcercise.Infrastructure.Entities
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public Account Account { get; set; }
        public int AccountId { get; set; }
    }
}
