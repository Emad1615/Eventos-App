using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public class AppException(int staus, string title, string? details)
    {
        public int Status { get; set; } = staus;
        public string title { get; set; } = title;
        public string Details { get; set; } = details;
    }
}
