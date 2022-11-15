using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbokiCore
{
    public class ApiDbContext : IdentityDbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
           : base(options)
        {

        }

        //dbSet
        public virtual DbSet<RefreshToken> RefreshTokens { get; set;}
    }
}
