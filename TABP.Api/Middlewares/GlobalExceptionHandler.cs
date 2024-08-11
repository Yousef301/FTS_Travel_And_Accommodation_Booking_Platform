using System.Security.Authentication;
using FluentValidation;
using Microsoft.Data.SqlClient;
using TABP.Domain.Exceptions;

namespace TABP.Web.Middlewares;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var traceId = context.TraceIdentifier;
            _logger.LogError(ex,
                "An unhandled exception occurred. Trace ID: {TraceId}, Request Path: {RequestPath}, Method: {Method}",
                traceId, context.Request.Path, context.Request.Method);


            await HandleExceptionAsync(context, ex, traceId);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, string traceId)
    {
        var statusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            NotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            BadRequestException => StatusCodes.Status400BadRequest,
            InternalServerErrorException => StatusCodes.Status500InternalServerError,
            PaymentRequiredException => StatusCodes.Status402PaymentRequired,
            InvalidCredentialException => StatusCodes.Status401Unauthorized,
            SqlException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError,
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = exception is ValidationException validationException
                ? string.Join("; ", validationException.Errors.Select(e => e.ErrorMessage))
                : exception.Message,
            TraceId = traceId
        };

        context.Response.Headers.Add("X-Trace-ID", traceId);

        return context.Response.WriteAsJsonAsync(response);
    }
}