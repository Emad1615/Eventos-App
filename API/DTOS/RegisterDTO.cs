using System.ComponentModel.DataAnnotations;

namespace API.DTOS
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
        public bool Geneder { get; set; } = true;  // 1 Male , 0 Female
        public DateTime? BirthDate { get; set; }
        public string? Bio { get; set; } = string.Empty;

    }
}
