using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using System.Reflection;
using System.Text;

/// <summary>
/// An <see cref="OkTextPlain"/> that when executed
/// will produce a response with content.
/// </summary>
public partial class OkTextPlain : IResult, IEndpointMetadataProvider
{
    private static readonly Encoding Encoding = Encoding.UTF8;

    /// <summary>
    /// Initializes a new instance of the <see cref="OkTextPlain"/> class with the text
    /// </summary>
    /// <param name="text">The value to format in the entity body.</param>
    internal OkTextPlain(string? text)
    {
        ResponseContent = text;
    }

    /// <summary>
    /// Gets the HTTP status code: <see cref="StatusCodes.Status200OK"/>
    /// </summary>
    public int StatusCode => StatusCodes.Status200OK;

    /// <summary>
    /// Gets the value for the <c>Content-Type</c> header: <c>application/problem+json</c>
    /// </summary>
    public static string ContentType => "text/plain; charset=utf-8";

    /// <summary>
    /// Gets the content representing the body of the response.
    /// </summary>
    public string? ResponseContent { get; internal init; }

    /// <summary>
    /// Writes the content to the HTTP response.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    public Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        // Creating the logger with a string to preserve the category after the refactoring.
        var loggerFactory = httpContext.RequestServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("OkTextPlain");

        // Remaining code modeled after HttpResultsHelper.cs in aspnetcore
        var response = httpContext.Response;
        response.StatusCode = StatusCode;
        response.ContentType = ContentType;

        Log.WritingResultAsContent(logger, ContentType);

        if (ResponseContent != null)
        {
            response.ContentLength = Encoding.GetByteCount(ResponseContent);
            return response.WriteAsync(ResponseContent, Encoding);
        }

        return Task.CompletedTask;
    }
    internal static partial class Log
    {
        [LoggerMessage(2, LogLevel.Information,
            "Write content with HTTP Response ContentType of {ContentType}",
            EventName = "WritingResultAsContent")]
        public static partial void WritingResultAsContent(ILogger logger, string contentType);
    }

    /// <inheritdoc/>
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(builder);

        builder.Metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status200OK, typeof(string), new[] { ContentType }));
    }
}
