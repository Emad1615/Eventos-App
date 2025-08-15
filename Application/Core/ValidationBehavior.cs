using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public class ValidationBehavior<TRquest, TResponse>(IValidator<TRquest>? validator = null) : IPipelineBehavior<TRquest, TResponse> where TRquest : notnull
    {
        public async Task<TResponse> Handle(TRquest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validator == null) return await next();
            var validationResult = await validator!.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            return await next();
        }
    }
}
