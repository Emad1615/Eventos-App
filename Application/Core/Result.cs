using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public class Result<T>
    {
        public int Status { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; } = string.Empty;
        public T? Value { get; set; }
        public static Result<T> Success(T value) => new Result<T>
        {
            IsSuccess = true,
            Value = value
        };
        public static Result<T> Failure(int status, string error) => new Result<T>
        {
            IsSuccess = false,
            Status = status,
            Error = error
        };
    }
}
