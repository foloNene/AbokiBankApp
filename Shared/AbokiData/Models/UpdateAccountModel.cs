using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AbokiData.Models
{
    public class UpdateAccountModel
    {
        /// <summary>
        /// The Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The Phone Number
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Pin
        /// </summary>
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pins must be 4-digits")]
        public string Pin { get; set; }
        /// <summary>
        /// Confirm Pin
        /// </summary>
        [Required]
        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string ConfirmPin { get; set; }
    }
}
