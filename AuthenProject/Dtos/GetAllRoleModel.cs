using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Dtos
{
    public class GetAllRoleModel
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}
