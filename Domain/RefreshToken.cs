using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class RefreshToken
    {
        public string ID { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresDate { get; set; }
        public bool IsExpire => DateTime.UtcNow >= ExpiresDate;
        public DateTime CreatedDateTime { get; set; }
        public string CreatedByIP { get; set; } = string.Empty;
        public DateTime? RevokedDateTime { get; set; }
        public string RevokedByIP { get; set; } = string.Empty;
        public bool IsActive => RevokedDateTime == null && !IsExpire;
        //FK To User
        public string UserId { get; set; } = string.Empty;
        public UserApplication User { get; set; }   
    }
}
