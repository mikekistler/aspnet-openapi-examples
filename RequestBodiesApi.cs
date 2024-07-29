using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

internal static class RequestBodiesApi
{
    public static RouteGroupBuilder MapRequestBodies(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/request-bodies");

        // Tags can be set at the group (RouteGroupBuilder) level
        group.WithTags("Request Bodies");

        group.MapPost("/json-body",
        (
        [Description("A Json request body")] [FromBody] JsonBody body
        ) =>
        {
            return TypedResults.Ok("Good to go");
        });

        group.MapPost("/optional-body",
        (
        [Description("An optional Json request body")] JsonBody? body // implicitly FromBody
        ) =>
        {
            return TypedResults.Ok("Good to go");
        });

        group.MapPost("/non-json-body",
        (
        [Description("A non-json request body")] byte[] body
        ) =>
        {
            return TypedResults.Ok("Good to go");
        })
        .Accepts<byte[]>("application/octet-stream");

        group.MapPost("/multi-content-type",
        (
        [Description("A multi-content-type request body")] byte[] body
        ) =>
        {
            return TypedResults.Ok("Good to go");
        })
        .Accepts<byte[]>("image/jpeg", "image/png", "image/tiff");

        group.MapPost("/multipart",
        (
        [Description("A multipart request body")] JsonBody? body
        ) =>
        {
            return TypedResults.Ok(body);
        })
        .Accepts<byte[]>("multipart/form-data");

        group.MapPost("/from-form",
        (
            [Description("Name")][FromForm] string name,
            [Description("Age")][FromForm] int age,
            [Description("Photo")][FromForm] byte[] photo
        ) =>
        {
            // Create a dynamic object to return the values
            return TypedResults.Ok(new { name, age, photo });
        });

        return group;
    }
}

internal record JsonBody
{
    public string prop1 { get; set; }
    public long? prop2 {  get; set; }
    public bool? prop3 {  get; set; }
}
