using AbokiCore;
using AbokiData.Enums;
using AbokiData.Models;
using AbokiData.Utilis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbokiAPI.Services
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApiDbContext _dbcontext;
        private readonly ILogger<TransactionRepository> _logger;
        private AppSettings _settings;
        private static string _ourBankSettlementAccount;
        private readonly IAccountRepository _accountRepository;


        public TransactionRepository(ApiDbContext dbcontext,
             ILogger<TransactionRepository> logger,
             IOptions<AppSettings> settings,
             IAccountRepository accountRepository
             )
        {
            _dbcontext = dbcontext ??
                throw new ArgumentNullException(nameof(_dbcontext));
            _logger = logger;
            _settings = settings.Value;
            _ourBankSettlementAccount = _settings.OurBankSettlementAccount;
            _accountRepository = accountRepository;
        }

        public async Task<Response> CreateNewTransaction(Transaction transaction)
        {
            _logger.LogInformation(message: "Inside the respository about to create Transaction");


            Response response = new Response();

            try
            {
                _dbcontext.Transactions.Add(transaction);
                await _dbcontext.SaveChangesAsync();
                response.ResponseCode = "00";
                response.ResponseMessage = "Transaction created successfully!";
                response.Data = null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED => {ex.Message}");
            }

            return response;
        }

        public async Task<Response> FindTransactionByDate(DateTime date)
        {
            _logger.LogInformation(message: "Inside the respository about to Find Transaction");

            Response response = new Response();
            var transaction = await _dbcontext.Transactions.Where(x => x.TransactionDate == date).ToListAsync();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successfully!";
            response.Data = transaction;

            return response;
        }

        //make deposit
        public async Task<Response> MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            //Make a deposit...
            Response response = new Response();
            Account sourceAccount = default; //The bank Settlement account
            Account destinationAccount; // Individual.
            Transaction transaction = new Transaction();

            //first check that user account owner is valid
            //aunthenticate in UserService, by injecting IUserService here
            var authUser = await _accountRepository.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null)
            {
                throw new ArgumentNullException("Invalid credential");
            }

            // validation pass
            try
            {
                sourceAccount = await _accountRepository.GetByAccountNumberAsync(_ourBankSettlementAccount);
                destinationAccount = await _accountRepository.GetByAccountNumberAsync(AccountNumber);

                //let's update the account balance
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there is updates
                if ((_dbcontext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_dbcontext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfully";
                    response.Data = null;
                }
                else
                {
                    //so transaction is unsuccessful
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURERD.... =>{ ex.Message}");

            }

            //set other props of transaction here
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = TranType.Deposit;
            transaction.AccountId = sourceAccount.Id;
            transaction.TransactionAmount = Amount;
            transaction.TransactionSourceAccount = _ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionParticulars = $"NEW Transaction FROM SOURCE {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $"TO DESTINATION => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON {transaction.TransactionDate} " +
                $"TRAN_TYPE =>  {transaction.TransactionType} TRAN_STATUS => {transaction.TransactionStatus}";
            response.Data = transaction;

            _dbcontext.Transactions.Add(transaction);
            await _dbcontext.SaveChangesAsync();

            return response;
        }



        //Make transfer
        public async Task<Response> MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {

            _logger.LogInformation(message: "Inside the respository about to Make Fund Transfer Transaction");

            //make withdraw
            Response response = new Response();
            Account sourceAccount = default;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //check if valid user
            var authUser = await _accountRepository.Authenticate(FromAccount, TransactionPin);

            if (authUser == null)
            {
                throw new ArgumentNullException("Invalid credentials");
            }

            //validation pass

            try
            {
                sourceAccount = await _accountRepository.GetByAccountNumberAsync(FromAccount);
                destinationAccount = await _accountRepository.GetByAccountNumberAsync(ToAccount);

                //update their account balance
                //let's update their account balance
                sourceAccount.CurrentAccountBalance -= Amount; //reduce the transfer amount from the customer's balance.
                destinationAccount.CurrentAccountBalance += Amount; //addd transfer amount to our target customer's balance


                //check if there is update
                if ((_dbcontext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_dbcontext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfully";
                    response.Data = null;
                }
                else
                {
                    //so transaction is unsuccessful
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURERD..=>{ ex.Message}");
            }

            //set other props of transaction here
            transaction.TransactionType = TranType.Transfer;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.AccountId = sourceAccount.Id;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW Transaction FROM SOURCE {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
               $"TO DESTINATION => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON {transaction.TransactionDate} " +
               $"TRAN_TYPE =>  {transaction.TransactionType} TRAN_STATUS => {transaction.TransactionStatus}";
            response.Data = transaction;

            _dbcontext.Transactions.Add(transaction);
            await _dbcontext.SaveChangesAsync();

            return response;

        }

        public async Task<Response> MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            _logger.LogInformation(message: "Inside the respository about to Make withdraw Transaction");

            //make withdrawal....
            Response response = new Response();
            Account sourceAccount = default; //individual
            Account destinationAccount; //the bank settlement Account
            Transaction transaction = new Transaction();

            //Validate the user
            var authUser = await _accountRepository.Authenticate(AccountNumber, TransactionPin);

            if (authUser == null)
            {
                throw new ArgumentNullException("Invalid credential");
            }

            //validate passess
            try
            {
                //for deposit, our banksettlement is the distination getting money from the user's account
                //let's update their account balance
                sourceAccount = await _accountRepository.GetByAccountNumberAsync(AccountNumber);
                destinationAccount = await _accountRepository.GetByAccountNumberAsync(_ourBankSettlementAccount);

                //update the account balanace
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there is updates
                if ((_dbcontext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_dbcontext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfully";
                    response.Data = null;
                }
                else
                {
                    //so transaction is unsuccessful
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURERD..=>{ ex.Message}");
            }

            //set other props of transaction here
            transaction.TransactionType = TranType.Withdrawal;
            transaction.TransactionSourceAccount = AccountNumber;
            transaction.AccountId = sourceAccount.Id;
            transaction.TransactionDestinationAccount = _ourBankSettlementAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW Transaction FROM SOURCE {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $"TO DESTINATION => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON {transaction.TransactionDate} " +
                $"TRAN_TYPE =>  {transaction.TransactionType} TRAN_STATUS => {transaction.TransactionStatus}";
            response.Data = transaction;


            _dbcontext.Transactions.Add(transaction);
            await _dbcontext.SaveChangesAsync();

            return response;

        }
    }

}
