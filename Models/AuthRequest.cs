using System.ComponentModel.DataAnnotations;

namespace LibraryMS.Model
{
    public class AuthRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
