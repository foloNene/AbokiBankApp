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
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public string AccountName { get; set; }

        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }
        [JsonIgnore]
        public byte[] PinHash { get; set; }
        [JsonIgnore]
        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
  = new List<Transaction>();

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
