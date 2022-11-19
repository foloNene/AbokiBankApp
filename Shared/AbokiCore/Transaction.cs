using AbokiData.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AbokiCore
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        //this will generate in every instance off the class
        public string TransactionUniqueReference { get; set; }

        public decimal TransactionAmount { get; set; }
        public TranStatus TransactionStatus { get; set; }  // An enum
        public bool IsSuccessful => TransactionStatus.Equals(TranStatus.Success); //depends on the value of transactionStatus
        public string TransactionSourceAccount { get; set; }

        public string TransactionDestinationAccount { get; set; }

        public string TransactionParticulars { get; set; }

        public TranType TransactionType { get; set; }

        public DateTime TransactionDate { get; set; }

        public Guid AccountId { get; set; }




        public Transaction()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 27)}"; //Generate Refrence with Guid
        }


    }
}
