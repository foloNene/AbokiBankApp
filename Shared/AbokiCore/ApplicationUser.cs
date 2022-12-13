using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbokiCore
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Account> Accounts { get; set; }
            = new List<Account>();
    }
}
