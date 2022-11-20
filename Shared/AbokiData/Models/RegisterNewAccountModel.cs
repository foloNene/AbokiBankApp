using AbokiData.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AbokiData.Models
{
    public class RegisterNewAccountModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public AccountType AccountType { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastUpdated { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "AccountNumber must be 10 digits")]
        public string Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string Confirmpin { get; set; }
    }
}
