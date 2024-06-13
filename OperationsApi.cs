using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

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
        // The Description attribute does not add a description on parameters
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

        // summary, description, operationId, and parameter descriptions can be set with Swashbuckle annotations

        group.MapPost("/annotations",
        [SwaggerOperation(
            Summary = "Operation summary from annotation",
            Description = "Operation description from annotation",
            OperationId = "FromAnnotations"
        )]
        (
            [SwaggerParameter("Parameter description from annotation")][FromQuery] string? q
        ) =>
        {
            return TypedResults.Ok("Good to go");
        });

        // summary, description, and parameter descriptions can be set on xml doc comments
        // when the endpoint handler is a static method
        group.MapPost("/xmldoc", StaticRouteHandlerWithXmlDoc);

        // Try all forms of parameters with varying naming conventions and requiredness

        group.MapPost("/{id}/parameters",
        [EndpointSummary("Operation summary from attribute")]
        [EndpointDescription("Operation description from attribute")]
        [EndpointName("Parameters")]
        (
            // name
            [FromQuery] string ? Pascal,       // parameter with Pascal-case name
            [FromHeader] string ? myCustomHeader,
            [FromHeader(Name = "my-kebab-header")] string ? myKebabHeader,
            // in
            string id, // implicitly [FromPath]
            [FromQuery] string? q,
            [FromHeader] string? header,
            // schema
            int value,
            long? maybeValue,
            [FromQuery] string version,
            [FromQuery] string? option,
            [FromQuery] string[]? tags
         ) =>
        {
            return TypedResults.Ok("Good to go");
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
