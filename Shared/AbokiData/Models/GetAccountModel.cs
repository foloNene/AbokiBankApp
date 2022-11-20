using System;
using System.Collections.Generic;
using System.Text;

namespace AbokiData.Models
{
    public class GetAccountModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AccountNumberGenerated { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public decimal CurrentAccountBalance { get; set; }
    }
}
