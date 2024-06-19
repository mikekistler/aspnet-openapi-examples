using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Reflection;
using System.Text;

/// <summary>
/// An <see cref="OkImage"/> that when executed
/// will produce a response with content.
/// </summary>
public partial class OkImage : IResult, IEndpointMetadataProvider
{
    public class ImageTypes
    {
        private ImageTypes(string value) { Value = value; }

        public string Value { get; private set; }

        public static ImageTypes Jpeg { get { return new ImageTypes("Image/Jpeg"); } }
        public static ImageTypes Png { get { return new ImageTypes("Image/Png"); } }
        public static ImageTypes Tiff { get { return new ImageTypes("Image/Tiff"); } }

        public override string ToString()
        {
            return Value;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OkImage"/> class with the text
    /// </summary>
    /// <param name="image">The value to format in the entity body.</param>
    internal OkImage(ReadOnlyMemory<byte> image, ImageTypes imageType)
    {
        ResponseContent = image;
        ImageType = imageType;
    }

    /// <summary>
    /// The HTTP status code: <see cref="StatusCodes.Status200OK"/>
    /// </summary>
    public int StatusCode => StatusCodes.Status200OK;

    /// <summary>
    /// The content type of the image.
    /// </summary>
    public ReadOnlyMemory<byte> ResponseContent { get; internal init; }

    /// <summary>
    /// The 
    /// </summary>
    public ImageTypes ImageType { get; internal init; }

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
        var logger = loggerFactory.CreateLogger("OkImage");

        var response = httpContext.Response;
        response.StatusCode = StatusCode;
        response.ContentType = ImageType.ToString();

        Log.WritingResultAsContent(logger, response.ContentType);

        response.ContentLength = ResponseContent.Length;
        return response.Body.WriteAsync(ResponseContent).AsTask();
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

        var contentTypes = new string[] { "image/jpeg", "image/png", "image/tiff" };
        builder.Metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status200OK, typeof(byte[]), contentTypes));
    }
}
