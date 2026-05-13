using Core.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Interceptors;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            var validator = serviceProvider.GetService(validatorType);

            if (validator is null)
                continue;

            var method = validatorType.GetMethod("ValidateAsync");

            var task = (Task<ValidationResult>)
                method.Invoke(validator, new object[] { argument, default(CancellationToken) });

            var result = await task;

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );

                throw new ValidationAppException(errors);
            }
        }

        await next();
    }
}