using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels.AccountViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
