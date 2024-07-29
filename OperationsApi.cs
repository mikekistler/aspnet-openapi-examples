using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

internal static class OperationsApi
{
    public static RouteGroupBuilder MapOperations(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/operations");

        // Tags can be set at the group (RouteGroupBuilder) level
        group.WithTags("Operations");

        // summary, description, and operationId are commonly set on the operation (RouteHandlerBuilder)

        // summary, description, and operationId can be set using extension methods
        group.MapPost("/extension-methods", () =>
        {
            return TypedResults.Ok("Good to go");
        })
        .WithSummary("Operation summary from extension method")
        .WithDescription("Operation description from extension method")
        .WithName("FromMethods");

        // summary, description, and operationId can be set using attributes
        // Descriptions can be set on parameters using attributes
        group.MapPost("/attributes",
        [EndpointSummary("Operation summary from attribute")]
        [EndpointDescription("Operation description from attribute")]
        [EndpointName("FromAttributes")]
        (
            [Description("Parameter description from attribute")][FromQuery] string? q
        ) =>
        {
            return TypedResults.Ok("Good to go");
        });

        // XML Doc is currently not supported for summary, description, and operationId
        group.MapPost("/xmldoc", StaticRouteHandlerWithXmlDoc);

        // Try all forms of parameters with varying naming conventions and requiredness

        group.MapPost("/{id}/parameters",
        (
            // name
            [Description("Pascal case")][FromQuery] string ? Pascal,
            [Description("My kebab case header")][FromHeader(Name = "my-kebab-header")] string ? myKebabHeader,
            // in
            [Description("The id")] string id, // implicitly [FromPath]
            [Description("A query string")][FromQuery] string? q,
            [Description("My special header")][FromHeader] string? myCustomHeader,
            // schema
            [Description("non-nullable value type")] int value,
            [Description("nullable value type")] long? maybeValue,
            [Description("non-nullable reference type")][FromQuery] string version,
            [Description("nullable reference type")][FromQuery] string ? option,
            [Description("array")][FromQuery] string[]? tags,
            // required
            [Description("Required parameter")][Required] string required
        ) =>
        {
            var result = new
            {
                Pascal,
                myKebabHeader,
                id,
                q,
                myCustomHeader,
                value,
                maybeValue,
                version,
                option,
                tags,
                required
            };
            return TypedResults.Ok(result);
        });

        // Try various schema assertions

        group.MapPost("/{id}/parameter-schemas",
        (
            // minLength, maxLength, pattern
            [MinLength(1), MaxLength(63), RegularExpression(@"^[A-Za-z0-9-]*$")] string id,

            // minimum, maximum
            [Range(0, 100)][FromQuery] int Confidence,

            // default
            [DefaultValue(42)][FromQuery] int Default
        ) =>
        {
            return TypedResults.Ok("Good to go");
        });

        return group;
    }

    /// <summary>
    /// Operation summary from XML doc
    /// </summary>
    /// <remarks>
    /// Operation description from XML doc
    /// </remarks>
    /// <param name="q">Parameter description from XML doc</param>
    private static IResult StaticRouteHandlerWithXmlDoc(
        [FromQuery] string? q
    )
    {
        return TypedResults.Ok("Good to go");
    }
}
