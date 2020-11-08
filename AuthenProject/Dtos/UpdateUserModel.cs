using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Dtos
{
    public class UpdateUserModel
    {
      
        [Required]
        public string Email { get; set; }
        public DateTime? Dob { get; set; }
       
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string PhoneNumber { get; set; }
    }
}
