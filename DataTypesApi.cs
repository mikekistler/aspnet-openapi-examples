using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

internal static class DataTypesApi
{
    public static RouteGroupBuilder MapDataTypes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/data-types");

        group.WithTags("Data Types");

        // List all DataTypes
        group.MapGet("/", Ok<DataTypes> () =>
        {
            var dataTypes = new DataTypes();
            return TypedResults.Ok(dataTypes);
        });

        group.MapPost("/", Ok<DataTypes> (DataTypes ) =>
        {
            var dataTypes = new DataTypes();
            return TypedResults.Ok(dataTypes);
        });

        return group;
    }
}