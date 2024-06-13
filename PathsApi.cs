using Microsoft.AspNetCore.Http.HttpResults;

internal static class PathsApi
{
    public static RouteGroupBuilder MapPaths(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/paths/{id}");

        group.WithTags("Paths");

        // Setting summary and description on the group does not set the summary or description
        // on the pathItem.
        //group.WithSummary("Summary of this Path");
        //group.WithDescription("Description of this Path");

        // get property of pathItem
        group.MapGet("/", (string id) =>
        {
            return Results.Ok();
        });

        // put property of pathItem
        group.MapPut("/", (string id) =>
        {
            return Results.Ok();
        });

        // post property of pathItem
        group.MapPost("/", (string id) =>
        {
            return Results.Ok();
        });

        // delete property of pathItem
        group.MapDelete("/", (string id) =>
        {
            return Results.Ok();
        });

        // options - use  DocumentFilter

        // head - use  DocumentFilter

        // patch property of pathItem
        group.MapPatch("/", (string id) =>
        {
            return Results.Ok();
        });

        // trace - use  DocumentFilter

        // parameters - use  DocumentFilter

        return group;
    }
}
