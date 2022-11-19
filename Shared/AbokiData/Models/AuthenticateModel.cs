﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AbokiData.Models
{
    public class AuthenticateModel
    {
        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage ="AccountNumber must be 10-digits")]
        public string AccountNumber { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must be 4-digit")]
        public string Pin { get; set; }

    }
}
