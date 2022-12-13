using AbokiCore;
using AbokiData.Models;
using System;
using System.Threading.Tasks;

namespace AbokiAPI.Services
{
    public interface ITransactionRepository
    {
        Task<Response> CreateNewTransaction(Transaction transaction);
        Task<Response> FindTransactionByDate(DateTime date);
        Task<Response> MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin);
        Task<Response> MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin);
        Task<Response> MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin);
    }
}