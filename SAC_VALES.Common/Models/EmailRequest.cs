using System.ComponentModel.DataAnnotations;

namespace SAC_VALES.Common.Models
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
