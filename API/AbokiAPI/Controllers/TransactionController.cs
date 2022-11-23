using AbokiAPI.Services;
using AbokiCore;
using AbokiData.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AbokiAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "AppUser")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private IMapper _mapper;

        public TransactionController(ITransactionRepository transactionRepository,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create_new_transaction")]
        public async Task<IActionResult> CreateNewTransaction([FromBody] TransactionRequestDto transactionRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(transactionRequest);
            }

            var transaction = _mapper.Map<Transaction>(transactionRequest);

            return Ok(await _transactionRepository.CreateNewTransaction(transaction));

        }

        [HttpPost]
        [Route("make_deposit")]
        public async Task<IActionResult> MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
            {
                return BadRequest("Account Number must be 10 digits");
            }

            return Ok(await _transactionRepository.MakeDeposit(AccountNumber, Amount, TransactionPin));
        }

        [HttpPost]
        [Route("make_withdrawal")]
        public async Task<IActionResult> MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
            {
                return BadRequest("Account Number must be 10 digits");
            }

            return Ok(await _transactionRepository.MakeWithdrawal(AccountNumber, Amount, TransactionPin));

        }

        [HttpPost]
        [Route("make_funds_transfer")]
        public async Task<IActionResult> MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            if ((!Regex.IsMatch(FromAccount, @"^[0-9]{10}$")) || (!Regex.IsMatch(ToAccount, @"^[0-9]{10}$")))
            {
                return BadRequest("Account Number must be 10 digits");
            }

            if (FromAccount.Equals(ToAccount))
            {
                return BadRequest("You cannot transfer money to yourself");
            }

            return Ok(await _transactionRepository.MakeFundTransfer(FromAccount, ToAccount, Amount, TransactionPin));
        }
    }
}
