using AbokiData.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AbokiData.Models
{
    public class RegisterNewAccountModel
    {
        /// <summary>
        /// First name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
       /// <summary>
       /// Last name
       /// </summary>
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
       /// <summary>
       /// Phone number
       /// </summary>
        [Required]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Email Address
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// Account type
        /// </summary>
        public AccountType AccountType { get; set; }
        /// <summary>
        /// Date created
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date Updated
        /// </summary>
        public DateTime DateLastUpdated { get; set; }
        /// <summary>
        /// Pin
        /// </summary>
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "AccountNumber must be 10 digits")]
        public string Pin { get; set; }
       /// <summary>
       /// Confirm Pin
       /// </summary>
        [Required]
        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string Confirmpin { get; set; }
    }
}
