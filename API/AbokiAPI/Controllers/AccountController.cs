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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly IScopeInformation _scopeInfo;

        public AccountController(IAccountRepository accountRepository,
            IMapper mapper,
            ILogger<AccountController> logger,
            IScopeInformation scopeInfo)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _logger = logger;
            _scopeInfo = scopeInfo;
        }


        /// <summary>
        /// Get account by Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Getting an Account by the Id</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [HttpGet("{accountId}", Name = "GetAccount")]
        public async Task<ActionResult> GetAccount(Guid accountId)
        {
            //Logging
            var userEmail = User.Claims.FirstOrDefault(a => a.Type == "Email")?.Value;
            var userId = User.Claims.FirstOrDefault(a => a.Type == "Sub")?.Value;

            object[] Infos = { User.Claims, userEmail };
            object[] Info = { User.Claims, userId };

            //Additional Info like machine name.
            using (_logger.BeginScope(_scopeInfo.HostScopeInfo))
            {
                _logger.LogInformation(message: "{userEmail} IS inside the get account by Id. {claims}",
                 args: Infos);
                _logger.LogInformation(message: "{userId} is inside get account by Id.{claims}",
                    args: Info);
            }

            var accountFromRepo = await _accountRepository.GetAccountAsync(accountId);

            if (accountFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AccountDto>(accountFromRepo));
        }
        /// <summary>
        /// Register account
        /// </summary>
        /// <param name="newAccount"></param>
        /// <returns></returns>
        //Register New Account
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        [HttpPost]
        public async Task<ActionResult> RegisterNewAccount([FromBody] RegisterNewAccountModel newAccount)
        {

            //Logging
            var userEmail = User.Claims.FirstOrDefault(a => a.Type == "Email")?.Value;
            var loguserId = User.Claims.FirstOrDefault(a => a.Type == "Sub")?.Value;

            object[] Infos = { User.Claims, userEmail };
            object[] Info = { User.Claims, loguserId };

            //Additional Info like machine name.
            using (_logger.BeginScope(_scopeInfo.HostScopeInfo))
            {
                _logger.LogInformation(message: "{userEmail} IS registering new account. {claims}",
                 args: Infos);
                _logger.LogInformation(message: "{loguserId} is inside get new account.{claims}",
                    args: Info);
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(newAccount);
            }

            //map to account
            var accountEntity = _mapper.Map<Account>(newAccount);

            var userId = User.Claims.FirstOrDefault(a => a.Type == "Id")?.Value;
            //var userEmail2 = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Email)?.Value;

            //add
            _accountRepository.Create(accountEntity, newAccount.Pin, newAccount.Confirmpin, userId);

            //Save Changes
            await _accountRepository.SaveChangesAsync();

            //Map
            var accountToReturn = _mapper.Map<AccountDto>(accountEntity);

            // return Ok(accountToReturn);
            return CreatedAtRoute("GetAccount", new
            {
                accountId = accountToReturn.Id
            }, accountToReturn);
        }

        /// <summary>
        /// Get The List of Accounts
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAccountModel>>> GetAllAccounts()
        {
            //Logging
            var userEmail = User.Claims.FirstOrDefault(a => a.Type == "Email")?.Value;
            var loguserId = User.Claims.FirstOrDefault(a => a.Type == "Sub")?.Value;

            object[] Infos = { User.Claims, userEmail };
            object[] Info = { User.Claims, loguserId };

            //Additional Info like machine name.
            using (_logger.BeginScope(_scopeInfo.HostScopeInfo))
            {
                _logger.LogInformation(message: "{userEmail} IS inside the get all account. {claims}",
                 args: Infos);
                _logger.LogInformation(message: "{loguserId} is inside get all account.{claims}",
                    args: Info);
            }

            var allAccounts = await _accountRepository.GetAccountsAsync();

            var accountsToReturn = _mapper.Map<IList<GetAccountModel>>(allAccounts);

            return Ok(accountsToReturn);
        }

        /// <summary>
        /// Get account by Account Number
        /// </summary>
        /// <param name="AccountNumber"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [HttpGet]
        [Route("get_by_account_number")]
        public async Task<IActionResult> GetByAccountNumber(string AccountNumber)
        {
            //Logging
            var userEmail = User.Claims.FirstOrDefault(a => a.Type == "Email")?.Value;
            var loguserId = User.Claims.FirstOrDefault(a => a.Type == "Sub")?.Value;

            object[] Infos = { User.Claims, userEmail };
            object[] Info = { User.Claims, loguserId };

            //Additional Info like machine name.
            using (_logger.BeginScope(_scopeInfo.HostScopeInfo))
            {
                _logger.LogInformation(message: "{userEmail} IS inside the get account by account number. {claims}",
                 args: Infos);
                _logger.LogInformation(message: "{loguserId} is inside get account by account number.{claims}",
                    args: Info);
            }


            if (AccountNumber == null)
            {
                return BadRequest();
            }
            if (!Regex.IsMatch(AccountNumber, @"^[0-9]{10}$"))
            {
                return BadRequest("Account Number must be 10 digits");
            }

            var account = await _accountRepository.GetByAccountNumberAsync(AccountNumber);

            var accountToReturn = _mapper.Map<GetAccountModel>(account);
            return Ok(accountToReturn);
        }

        /// <summary>
        /// Update existing Account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //Update Account.
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [HttpPut("{accountId}")]
        public async Task<ActionResult> UpdateAccount(Guid accountId,
          [FromBody]  UpdateAccountModel model)
        {

            //Logging
            var userEmail = User.Claims.FirstOrDefault(a => a.Type == "Email")?.Value;
            var loguserId = User.Claims.FirstOrDefault(a => a.Type == "Sub")?.Value;

            object[] Infos = { User.Claims, userEmail };
            object[] Info = { User.Claims, loguserId };

            //Additional Info like machine name.
            using (_logger.BeginScope(_scopeInfo.HostScopeInfo))
            {
                _logger.LogInformation(message: "{userEmail} IS inside the Put account. {claims}",
                 args: Infos);
                _logger.LogInformation(message: "{loguserId} is inside put account by Id.{claims}",
                    args: Info);
            }

            if (!await _accountRepository.AccountExistsAsync(accountId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var account = _mapper.Map<Account>(model);
             _accountRepository.Update(account, model.Pin);
            return NoContent();
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //Authenticate
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [HttpPost]
        [Consumes("application/json")]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate ([FromBody] AuthenticateModel model)
        {
            //Logging
            var userEmail = User.Claims.FirstOrDefault(a => a.Type == "Email")?.Value;
            var loguserId = User.Claims.FirstOrDefault(a => a.Type == "Sub")?.Value;

            object[] Infos = { User.Claims, userEmail };
            object[] Info = { User.Claims, loguserId };

            //Additional Info like machine name.
            using (_logger.BeginScope(_scopeInfo.HostScopeInfo))
            {
                _logger.LogInformation(message: "{userEmail} IS inside the authenticate it's account. {claims}",
                 args: Infos);
                _logger.LogInformation(message: "{loguserId} is inside the authenticate account.{claims}",
                    args: Info);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var authResult = await _accountRepository.Authenticate(
                model.AccountNumber, model.Pin);

            if (authResult == null)
            {
                return Unauthorized("Invalid Credentials");
            }

            return Ok(authResult);

        }

    }
}
