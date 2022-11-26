using AbokiCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbokiAPI.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApiDbContext _dbcontext;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(ApiDbContext dbcontext,
            ILogger<AccountRepository> logger)
        {
            _dbcontext = dbcontext ??
                throw new ArgumentNullException(nameof(_dbcontext));
            _logger = logger;

        }

        //Getting the list of Accounts
        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            _logger.LogInformation(message: "Inside the respository about to call all Accounts");

            return await _dbcontext.Accounts.ToListAsync();
        }

        //To check if Account exist
        public async Task<bool> AccountExistsAsync(Guid accountId)
        {
            _logger.LogInformation(message: "Inside the respository about to call ExistAccount");
            return await _dbcontext.Accounts.AnyAsync(a => a.Id == accountId);
        }

        //Get one Account
        public async Task<Account> GetAccountAsync(Guid accountId)
        {
            _logger.LogInformation(message: "Inside the respository about to call Get account by Id");

            if (accountId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(accountId));
            }
            return await _dbcontext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        }

        //Create an Account
        public Account Create(Account account, string Pin, string ConfirmPin, string userId)
        {
            //log
            _logger.LogInformation(message: "Inside the respository about to create Accounts");

            //check if pin isn't empty
            if (string.IsNullOrWhiteSpace(Pin))
            {
                throw new ArgumentNullException("Pin cannot be empty");
            }
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }
            //if (_dbcontext.Accounts.Any(x => x.Email == account.Email))
            //{
            //    throw new ArgumentNullException("An account already exist with the email");
            //}
            //Validate Pin
            if (!Pin.Equals(ConfirmPin))
            {
                throw new ArgumentException("Pins do not match", "Pin");
            }


            //Hashing Pin
            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);

            account.PinHash = pinHash;
            account.PinSalt = pinSalt;
            account.AccountName = $"{account.FirstName} {account.LastName}";
            account.ApplicationUserId = Guid.Parse(userId);

            //after being Harsged
            _dbcontext.Accounts.Add(account);
            //_dbcontext.SaveChanges();

            return account;

        }

        //Create PinHarsh Method
        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(pin))
            {
                throw new ArgumentNullException("pin");
            }
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        //To save Account
        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbcontext.SaveChangesAsync() > 0);
        }

        public async Task<Account> GetByAccountNumberAsync(string AccountNumber)
        {
            var account = await _dbcontext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefaultAsync();

            if (account == null)
            {
                return null;
            }

            return account;
        }

        public void Update(Account account, string Pin = null)
        {

            //log
            _logger.LogInformation(message: "Inside the respository about to Update Accounts");

            // fnd userr
            var accountToBeUpdated = _dbcontext.Accounts.Find(account.Id);
            if (accountToBeUpdated == null) throw new ApplicationException("Account not found");
            ////so we have a match
            if (!string.IsNullOrWhiteSpace(account.Email) && account.Email != accountToBeUpdated.Email)
            {
                //throw error because email passeed doesn't matc wiith
                if (_dbcontext.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("Email " + account.Email + " has been taken");
                accountToBeUpdated.Email = account.Email;
            }

            if (!string.IsNullOrWhiteSpace(account.PhoneNumber) && account.Email != accountToBeUpdated.PhoneNumber)
            {
                //throw error because email passeed doesn't matc wiith
                if (_dbcontext.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber)) throw new ApplicationException("PhoneNumber " + account.PhoneNumber + " has been taken");
                accountToBeUpdated.PhoneNumber = account.PhoneNumber;
            }


            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);

                accountToBeUpdated.PinHash = pinHash;
                accountToBeUpdated.PinSalt = pinSalt;
            }

            _dbcontext.Accounts.Update(accountToBeUpdated);
            _dbcontext.SaveChanges();

        }

        public async Task<Account> Authenticate(string AccountNumber, string Pin)
        {
            //log
            _logger.LogInformation(message: "Inside the respository about to Auntheticate Accounts");


            if (string.IsNullOrEmpty(AccountNumber) || string.IsNullOrEmpty(Pin))
            {
                return null;
            }

            //does acount exist 
            var account = await _dbcontext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefaultAsync();

            if (account == null)
            {
                return null;
            }

            //so if match
            //verify Pin
            if (!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
            {
                return null;
            }

            //Aunthicate success
            return account;

        }

        private static bool VerifyPinHash(string Pin, byte[] pinHash,
            byte[] pinSalt)
        {
            if (string.IsNullOrEmpty(Pin))
            {
                throw new ArgumentNullException("Pin");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pin));

                for (int i = 0; i < computedPinhash.Length; i++)
                {
                    if (computedPinhash[i] != pinHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
