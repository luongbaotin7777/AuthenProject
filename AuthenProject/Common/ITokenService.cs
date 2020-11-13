using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Common
{
    public interface ITokenService
    {
        Task<MessageReponse> GenerateJWTToken(string UserName, int expDay);
    }
}
