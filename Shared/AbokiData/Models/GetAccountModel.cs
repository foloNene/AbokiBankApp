using System;
using System.Collections.Generic;
using System.Text;

namespace AbokiData.Models
{
    public class GetAccountModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// FirstName
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// LastName
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Phone number
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Account Number
        /// </summary>
        public string AccountNumberGenerated { get; set; }
        /// <summary>
        /// Date Created
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date Updated
        /// </summary>
        public DateTime DateLastUpdated { get; set; }
        /// <summary>
        /// Account balance
        /// </summary>
        public decimal CurrentAccountBalance { get; set; }
    }
}
