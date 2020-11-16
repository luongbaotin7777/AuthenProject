using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Dtos
{
    public class RemoveRoleUser
    {
        public string UserName { get; set; }
        public List<string> ListRole { get; set; }
    }
}
