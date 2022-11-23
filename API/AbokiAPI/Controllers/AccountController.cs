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

namespace AbokiAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "AppUser")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountController(IAccountRepository accountRepository,
            IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }



        [HttpGet("{accountId}", Name = "GetAccount")]
        public async Task<IActionResult> GetAccount(Guid accountId)
        {
            var accountFromRepo = await _accountRepository.GetAccountAsync(accountId);

            if (accountFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AccountDto>(accountFromRepo));
        }

        //Register New Account
        [HttpPost]
        public async Task<ActionResult> RegisterNewAccount([FromBody] RegisterNewAccountModel newAccount)
        {
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



        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAccountModel>>> GetAllAccounts()
        {
            var allAccounts = await _accountRepository.GetAccountsAsync();

            var accountsToReturn = _mapper.Map<IList<GetAccountModel>>(allAccounts);

            return Ok(accountsToReturn);
        } 

        [HttpGet]
        [Route("get_by_account_number")]
        public async Task<IActionResult> GetByAccountNumber(string AccountNumber)
        {
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

        //Update Account.
        [HttpPut("{accountId}")]
        public async Task<IActionResult> UpdateAccount(Guid accountId,
          [FromBody]  UpdateAccountModel model)
        {
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


        //Authenticate
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate ([FromBody] AuthenticateModel model)
        {
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
