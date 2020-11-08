using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Common
{
    public class MessageReponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
