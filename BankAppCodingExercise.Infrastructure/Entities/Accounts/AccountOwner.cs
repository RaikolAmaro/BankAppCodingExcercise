using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankAppCodingExcercise.Infrastructure.Entities
{
    public class AccountOwner
    {
        [Key]
        public int AccountOwnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
