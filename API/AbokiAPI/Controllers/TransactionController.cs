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
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace AbokiAPI.Controllers
{
    [Produces("application/json", "application/xml")]
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "AppUser")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private IMapper _mapper;
        private readonly ILogger<TransactionController> _logger;
        private readonly IScopeInformation _scopeInfo;

        public TransactionController(ITransactionRepository transactionRepository,
            IMapper mapper,
            ILogger<TransactionController> logger,
            IScopeInformation scopeInfo)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _logger = logger;
            _scopeInfo = scopeInfo;
        }

        /// <summary>
        /// Create new Transaction
        /// </summary>
        /// <param name="transactionRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create_new_transaction")]
        public async Task<IActionResult> CreateNewTransaction([FromBody] TransactionRequestDto transactionRequest)
        {
            //Logging
            var userEmail = User.Claims.FirstOrDefault(a => a.Type == "Email")?.Value;
            var userId = User.Claims.FirstOrDefault(a => a.Type == "Sub")?.Value;

            object[] Infos = { User.Claims, userEmail };
            object[] Info = { User.Claims, userId };

            //Additional Info like machine name.
            using (_logger.BeginScope(_scopeInfo.HostScopeInfo))
            {
                _logger.LogInformation(message: "{userEmail} IS inside the Create New Transaction by Id. {claims}",
                 args: Infos);
                _logger.LogInformation(message: "{userId} is inside the Create New Transactiony Id.{claims}",
                    args: Info);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(transactionRequest);
            }

            var transaction = _mapper.Map<Transaction>(transactionRequest);

            return Ok(await _transactionRepository.CreateNewTransaction(transaction));

        }
        /// <summary>
        /// Deposit funds
        /// </summary>
        /// <param name="AccountNumber"></param>
        /// <param name="Amount"></param>
        /// <param name="TransactionPin"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [HttpPost]
        [Route("make_deposit")]
        public async Task<IActionResult> MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {

            //Logging
            var userEmail = User.Claims.FirstOrDefault(a => a.Type == "Email")?.Value;
            var userId = User.Claims.FirstOrDefault(a => a.Type == "Sub")?.Value;

            object[] Infos = { User.Claims, userEmail };
            object[] Info = { User.Claims, userId };

            //Additional Info like machine name.
            using (_logger.BeginScope(_scopeInfo.HostScopeInfo))
            {
                _logger.LogInformation(message: "{userEmail} IS inside the Make deposit Transaction by Id. {claims}",
                 args: Infos);
                _logger.LogInformation(message: "{userId} is inside the make deposit Id.{claims}",
                    args: Info);
            }

            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
            {
                return BadRequest("Account Number must be 10 digits");
            }

            return Ok(await _transactionRepository.MakeDeposit(AccountNumber, Amount, TransactionPin));
        }

        /// <summary>
        /// MAke withderawal
        /// </summary>
        /// <param name="AccountNumber"></param>
        /// <param name="Amount"></param>
        /// <param name="TransactionPin"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [HttpPost]
        [Route("make_withdrawal")]
        public async Task<IActionResult> MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {

            //Logging
            var userEmail = User.Claims.FirstOrDefault(a => a.Type == "Email")?.Value;
            var userId = User.Claims.FirstOrDefault(a => a.Type == "Sub")?.Value;

            object[] Infos = { User.Claims, userEmail };
            object[] Info = { User.Claims, userId };

            //Additional Info like machine name.
            using (_logger.BeginScope(_scopeInfo.HostScopeInfo))
            {
                _logger.LogInformation(message: "{userEmail} IS inside the make withdrawal by Id. {claims}",
                 args: Infos);
                _logger.LogInformation(message: "{userId} is inside the make withdraw Id.{claims}",
                    args: Info);
            }

            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
            {
                return BadRequest("Account Number must be 10 digits");
            }

            return Ok(await _transactionRepository.MakeWithdrawal(AccountNumber, Amount, TransactionPin));

        }
        /// <summary>
        /// Make fund Transafer
        /// </summary>
        /// <param name="FromAccount"></param>
        /// <param name="ToAccount"></param>
        /// <param name="Amount"></param>
        /// <param name="TransactionPin"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [HttpPost]
        [Route("make_funds_transfer")]
        public async Task<IActionResult> MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {

            //Logging
            var userEmail = User.Claims.FirstOrDefault(a => a.Type == "Email")?.Value;
            var userId = User.Claims.FirstOrDefault(a => a.Type == "Sub")?.Value;

            object[] Infos = { User.Claims, userEmail };
            object[] Info = { User.Claims, userId };

            //Additional Info like machine name.
            using (_logger.BeginScope(_scopeInfo.HostScopeInfo))
            {
                _logger.LogInformation(message: "{userEmail} IS inside the make Fund Transfer {claims}",
                 args: Infos);
                _logger.LogInformation(message: "{userId} is inside the make Fund Transfer.{claims}",
                    args: Info);
            }

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
