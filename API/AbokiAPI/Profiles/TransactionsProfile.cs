using AbokiCore;
using AbokiData.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbokiAPI.Profiles
{
    public class TransactionsProfile : Profile
    {
        public TransactionsProfile()
        {
            CreateMap<TransactionRequestDto, Transaction>();

            CreateMap<Transaction, TransactionDto>();
                
        }
    }
}
