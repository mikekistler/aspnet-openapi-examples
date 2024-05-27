using Microsoft.AspNetCore.Http.HttpResults;

internal static class SchemasApi
{
    public static RouteGroupBuilder MapSchemas(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Schemas");

        group.WithTags("Schemas");

        // List all Schemas
        group.MapGet("/", Ok<Schemas> () =>
        {
            var Schemas = new Schemas()
            {
                RequiredModifier = 42
            };
            return TypedResults.Ok(Schemas);
        });

        return group;
    }
}
