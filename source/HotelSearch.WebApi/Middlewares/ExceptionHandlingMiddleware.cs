using System.Net;
using HotelSearch.Domain.Exceptions;

namespace HotelSearch.WebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (ValidationFailedException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new BadRequestResponse
            {
                Errors = ex.ValidationErrors.SelectMany(x => x.Value).ToList()
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (HotelNotFoundException ex)
        {
            var errorId = Guid.NewGuid().ToString();
            _logger.LogError(ex, "Hotel not found exception with errorId {ErrorId}", errorId);

            context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = $"resource not found. Error id {errorId}",
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            var errorId = Guid.NewGuid().ToString();
            _logger.LogError(ex, "Unhandled exception with errorId {ErrorId}", errorId);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = $"An unexpected error occurred. Error id {errorId}",
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
public class BadRequestResponse
{
    public  string Type { get; private set; } = "VALIDATION_FAILED";
    public List<string> Errors { get; set; }
}
