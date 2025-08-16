using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class UserApplication : IdentityUser
    {
        public string? FullName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public bool Geneder { get; set; } = true;  // 1 Male , 0 Female
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
