using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAC_VALES.Common.Models
{
    public class UserRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public int UserTypeId { get; set; }

        public string PasswordConfirm { get; set; }

    }
}
