using AbokiCore;
using AbokiData.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbokiAPI.Profiles
{
    public class AccountsProfile : Profile
    {
        public AccountsProfile()
        {
            CreateMap<Account, AccountDto>();
            CreateMap<RegisterNewAccountModel, Account>();
            CreateMap<Account, GetAccountModel>();
            CreateMap<UpdateAccountModel, Account>();
        }
    }
}
