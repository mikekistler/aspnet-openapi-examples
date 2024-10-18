using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;

internal static class RequestBodiesApi
{
    public static RouteGroupBuilder MapRequests(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/").WithTags("Request Bodies");

        // Route handlers can define parameters annotated with [FromBody] to bind to the request body
        group.MapPost("/json-body",
        (
            [Description("A Json request body")] [FromBody] JsonBody body
        ) =>
        {
            return TypedResults.Ok(body);
        });

        // If the parameter is nullable, the request body is optional; required is omitted and defaults to false.
        group.MapPost("/optional-body",
        (
            [Description("An optional Json request body")] JsonBody? body // implicitly FromBody
        ) =>
        {
            return TypedResults.Ok("Good to go - " + (body == null ? "no body" : "body present"));
        });

        // <snippet_allow_empty_body>
        // If the FromBody attribute specifies an EmptyBodyBehavior of Allow, the body is optional; required is omitted and defaults to false.
        group.MapPost("/allow-empty-body",
        (
            [Description("An optional Json request body")] [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] JsonBody body
        ) =>
        {
            return TypedResults.Ok("Good to go - " + (body == null ? "no body" : "body present"));
        });
        // </snippet_allow_empty_body>

        // For non-json request bodies, the route handler can read and parse the request body directly
        // from the HttpContext. In this case, use the Accepts extension method to specify the allowed
        // content types and the expected type of the request body.
        group.MapPost("/accepts",
        (
            HttpContext context
        ) =>
        {
            return TypedResults.Ok("Good to go - " + context.Request.ContentType);
        })
        .Accepts<byte[]>("image/jpeg", "image/png", "image/tiff");

        // Another option for non-json request bodies is to define a custom type that implements
        // IBindableFromHttpContext<T> and IEndpointParameterMetadataProvider. This allows the framework
        // to bind the parameter from the request body in the HTTP context.
        group.MapPost("/todos/{id}", (int id, Todo todo) =>
        {
            return TypedResults.Ok(todo);
        });

        return group;
    }
}

internal record JsonBody
{
    public string prop1 { get; set; } = String.Empty;
    public long? prop2 {  get; set; }
    public bool? prop3 {  get; set; }
}
