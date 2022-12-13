using AbokiData.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbokiData.Models
{
    public class TransactionDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Transaction Reference
        /// </summary>
        public string TransactionUniqueReference { get; set; } //this will generate in every instance off the class
        /// <summary>
        /// Amount
        /// </summary>
        public decimal TransactionAmount { get; set; }
        /// <summary>
        /// Transaction status
        /// </summary>
        public TranStatus TransactionStatus { get; set; }  // An enum
        /// <summary>
        /// Transaction Success
        /// </summary>
        public bool IsSuccessful => TransactionStatus.Equals(TranStatus.Success); //depends on the value of transactionStatus
        /// <summary>
        /// Source Account
        /// </summary>
        public string TransactionSourceAccount { get; set; }
        /// <summary>
        /// Destination Account
        /// </summary>
        public string TransactionDestinationAccount { get; set; }
        /// <summary>
        /// Transaction Particular
        /// </summary>
        public string TransactionParticulars { get; set; }
        /// <summary>
        /// Transaction type
        /// </summary>
        public TranType TransactionType { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        public DateTime TransactionDate { get; set; }
        /// <summary>
        /// Account Id
        /// </summary>
        public Guid AccountId { get; set; }
    }
}
