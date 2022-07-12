using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]

        public string SurName { get; set; }
        public bool IsAdmin { get; set; }
    }
   
}
