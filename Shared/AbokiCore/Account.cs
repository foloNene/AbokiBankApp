using AbokiData.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace AbokiCore
{
    [Table("Accounts")]
    public class Account
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Guid id
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
       /// <summary>
       /// First Name
       /// </summary>
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
       /// <summary>
       /// Last Name
       /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// Account number
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Phone number
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
       /// <summary>
       /// Email
       /// </summary>
        public decimal CurrentAccountBalance { get; set; }
       /// <summary>
       /// Account balance
       /// </summary>
        public AccountType AccountType { get; set; }
       /// <summary>
       /// Account type
       /// </summary>
        public string AccountNumberGenerated { get; set; }
       /// <summary>
       /// Account number
       /// </summary>
        [JsonIgnore]
        public byte[] PinHash { get; set; }
       /// <summary>
       /// Pin
       /// </summary>
        [JsonIgnore]
        public byte[] PinSalt { get; set; }
       /// <summary>
       /// Pin
       /// </summary>
        public DateTime DateCreated { get; set; }
       /// <summary>
       /// Date Created
       /// </summary>
        public DateTime DateLastUpdated { get; set; }
/// <summary>
/// Collection of Transaction by the account
/// </summary>
        public ICollection<Transaction> Transactions { get; set; }
  = new List<Transaction>();
/// <summary>
/// Application user
/// </summary>
        public Guid ApplicationUserId { get; set; }



        //Generate Account Number from the Constructor

        //first, generate random obj
        Random rand = new Random();

        public Account()
        {
            //Generate account number
            AccountNumberGenerated = Convert.ToString((long)Math.Floor(rand.NextDouble() * 9_000_000_000L + 1_000_000_000L));

            //also AccountName property = FirstName + LastName;
            AccountName = $"{FirstName} {LastName}";
        }

    }
}
