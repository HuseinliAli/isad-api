using Core.Domain.ErrorModels;
using Core.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace PaymentService.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";

        var statusCode = (int)HttpStatusCode.InternalServerError;
        object? responseObj = null;

        switch (exception)
        {
            case NotFoundException notFoundEx:
                statusCode = StatusCodes.Status404NotFound;
                responseObj = new ErrorDetails
                {
                    StatusCode = statusCode,
                    Message = notFoundEx.Message
                };
                break;

            case BusinessRuleViolationException businessEx:
                statusCode = StatusCodes.Status422UnprocessableEntity;
                responseObj = new ErrorDetails
                {
                    StatusCode = statusCode,
                    Message = businessEx.Message
                };
                break;

            case BadRequestException badRequestEx:
                statusCode = StatusCodes.Status400BadRequest;
                responseObj = new ErrorDetails
                {
                    StatusCode = statusCode,
                    Message = badRequestEx.Message
                };
                break;

            case PermissionDeniedException permissionEx:
                statusCode = StatusCodes.Status403Forbidden;
                responseObj = new ErrorDetails
                {
                    StatusCode = statusCode,
                    Message = permissionEx.Message
                };
                break;

            case UnauthorizedException unauthorizedEx:
                statusCode = StatusCodes.Status401Unauthorized;
                responseObj = new ErrorDetails
                {
                    StatusCode = statusCode,
                    Message = unauthorizedEx.Message
                };
                break;

            case ValidationAppException validationEx:
                statusCode = StatusCodes.Status400BadRequest;

                responseObj = new ErrorDetails
                {
                    StatusCode = statusCode,
                    Message = validationEx.Message,
                    Errors = validationEx.Errors
                };

                break;

            default:
                responseObj = new ErrorDetails
                {
                    StatusCode = statusCode,
                    Message = "Internal Server Error"
                };
                break;
        }

        httpContext.Response.StatusCode = statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        var jsonResponse = JsonSerializer.Serialize(responseObj, options);

        await httpContext.Response.WriteAsync(jsonResponse);

        return true;
    }
}