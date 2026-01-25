using FrenchRevolution.Application.Abstractions;
using MediatR;

namespace FrenchRevolution.Application.Behaviours;

public sealed partial class LoggingPipelineBehaviour<TRequest, TResponse>(
    ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Starting request: {RequestName}, {DateTimeUtc}")]
    private partial void LogRequestStarting(string requestName, DateTime dateTimeUtc);
    
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Completed request: {RequestName}, {DateTimeUtc}")]
    private partial void LogRequestCompleted(string requestName, DateTime dateTimeUtc);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Request failure: {RequestName}, {Error}, {DateTimeUtc}")]
    private partial void LogRequestFailure(string requestName, string? error, DateTime dateTimeUtc);

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken
        )
    {
        LogRequestStarting(typeof(TRequest).Name, DateTime.UtcNow);
        
        var result = await next(cancellationToken);

        if (result is IResultType { IsFailure: true } failedResult)
        {
            LogRequestFailure(typeof(TRequest).Name, failedResult.Error, DateTime.UtcNow);
        }
        
        LogRequestCompleted(typeof(TRequest).Name, DateTime.UtcNow);

        return result;
    }
}