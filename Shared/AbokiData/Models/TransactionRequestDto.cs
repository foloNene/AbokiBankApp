using AbokiData.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbokiData.Models
{
    public class TransactionRequestDto
    {
        /// <summary>
        /// Transaction Amount
        /// </summary>
        public decimal TransactionAmount { get; set; }
        /// <summary>
        /// The Souce Account
        /// </summary>
        public string TransactionSourceAccount { get; set; }
        /// <summary>
        /// Distination Account
        /// </summary>
        public string TransactionDestinationAccount { get; set; }
        /// <summary>
        /// Transaction Type
        /// </summary>
        public TranType TransactionType { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        public DateTime TransactionDate { get; set; }
    }
}
