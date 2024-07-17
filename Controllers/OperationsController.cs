using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace aspnet_openapi_examples.Controllers;

[ApiController]
[Route("/operations")]
public class OperationsController : ControllerBase
{
    // summary, description, and operationId can be set using attributes
    // parameter descriptions can be set using the Description attribute

    [EndpointSummary("Operation summary from attribute")]
    [EndpointDescription("Operation description from attribute")]
    [EndpointName("FromAttributes")]
    [Tags(["Tag1", "Tag2"])]
    [HttpPost("attributes")]
    public IResult Attributes([Description("Parameter description from attribute")][FromQuery] string? q)
    {
        return Results.Ok();
    }

    // xml doc comments are not currently supported to set
    // summary, description, operationId, or descriptions on parameters

    /// <summary>
    /// Operation summary from XML doc
    /// </summary>
    /// <remarks>
    /// Operation description from XML doc
    /// </remarks>
    /// <param name="q">Parameter description from XML doc</param>
    [HttpPost("xmldoc")]
    public IResult FromXmlDoc(
        [FromQuery] string? q
    )
    {
        return TypedResults.Ok("Good to go");
    }

    // Try all forms of parameters with varying naming conventions and requiredness

    [HttpPost("{id}/parameters")]
    public Ok<string> parameters(
        // name
        [Description("Pascal case")][FromQuery] string ? Pascal,
        [Description("My kebab case header")][FromHeader(Name = "my-kebab-header")] string ? myKebabHeader,
        // in
        [Description("The id")] string id, // implicitly [FromPath]
        [Description("A query string")][FromQuery] string? q,
        [Description("My special header")][FromHeader] string? mySpecialHeader,
        // schema
        [Description("non-nullable value type")] int value,
        [Description("nullable value type")] long? maybeValue,
        [Description("non-nullable reference type")][FromQuery] string version,
        [Description("nullable reference type")][FromQuery] string ? option,
        [Description("array")][FromQuery] string[]? tags,
        // required
        [Description("required")] [Required] string required
    )
    {
        return TypedResults.Ok("Good to go");
    }
}