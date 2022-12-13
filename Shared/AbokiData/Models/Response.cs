using System;
using System.Collections.Generic;
using System.Text;

namespace AbokiData.Models
{
    public class Response
    {
        /// <summary>
        /// Id
        /// </summary>
        public string RequestId => $"{Guid.NewGuid().ToString()}";
        /// <summary>
        /// Response Code
        /// </summary>
        public string ResponseCode { get; set; }
        /// <summary>
        /// The response Message
        /// </summary>
        public string ResponseMessage { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        public object Data { get; set; }
    }
}
