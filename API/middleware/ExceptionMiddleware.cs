using Application.Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.middleware
{
    public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment env) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var response = env.IsDevelopment()
                ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace) : new AppException(context.Response.StatusCode, ex.Message, null);
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }

        private async Task HandleValidationException(HttpContext context, ValidationException ex)
        {
            var validationErrors = new Dictionary<string, string[]>();
            if (ex.Errors is not null)
            {
                foreach (var error in ex.Errors)
                {
                    if (validationErrors.TryGetValue(error.PropertyName, out var existingError))
                    {
                        validationErrors[error.PropertyName] = [.. existingError, error.ErrorMessage];
                    }
                    else
                    {
                        validationErrors[error.PropertyName] = [error.ErrorMessage];
                    }
                }
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var validationProplemDetails = new ValidationProblemDetails(validationErrors)
                {
                    Type = "ValidationFailure",
                    Status = StatusCodes.Status400BadRequest,
                    Title = "ValidationError",
                    Detail = "one or more error has occured"
                };
                await context.Response.WriteAsJsonAsync(validationProplemDetails);
            }
        }
    }
}
