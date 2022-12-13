using AbokiData.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbokiData.Models
{
    public class AccountDto
    {
        /// <summary>
        /// Guid Id
        /// </summary>
        public Guid Id { get; set; }
       /// <summary>
       /// First Name
       /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }
       /// <summary>
       /// Account Name
       /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// Phone Number
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Account Balance
        /// </summary>
        public decimal CurrentAccountBalance { get; set; }
       /// <summary>
       /// Account Type
       /// </summary>
        public AccountType AccountType { get; set; }
       /// <summary>
       /// Account Number
       /// </summary>
        public string AccountNumberGenerated { get; set; }
       /// <summary>
       /// Date created
       /// </summary>
        public DateTime DateCreated { get; set; }
       /// <summary>
       /// Date Updated
       /// </summary>
        public DateTime DateLastUpdated { get; set; }
    }
}
