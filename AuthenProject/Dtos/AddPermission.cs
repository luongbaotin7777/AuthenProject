using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Dtos
{
    public class AddPermission
    {
        public string RoleName { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
