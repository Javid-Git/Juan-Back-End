using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:255)]
        public string Key { get; set; }
        [Required]
        [StringLength(maximumLength: 255)]
        public string Value { get; set; }
    }
}
