using AbokiCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbokiAPI.Services
{
    public interface IAccountRepository
    {
        Task<bool> AccountExistsAsync(Guid accountId);
        Account Create(Account account, string Pin, string ConfirmPin, string userId);
        Task<Account> GetAccountAsync(Guid accountId);
        Task<IEnumerable<Account>> GetAccountsAsync();
        Task<bool> SaveChangesAsync();
        Task<Account> GetByAccountNumberAsync(string AccountNumber);

        void Update(Account account, string Pin = null);

        Task<Account> Authenticate(string AccountNumber, string Pin);
    }
}