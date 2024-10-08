using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

internal static class DataTypesApi
{
    public static RouteGroupBuilder MapDataTypes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/");

        group.MapPost("/data-types", Ok<DataTypes> (DataTypes body) =>
        {
            return TypedResults.Ok(body);
        })
        .WithTags("Data Types");

        group.MapPost("/metadata", Ok<Metadata> (Metadata body) =>
        {
            return TypedResults.Ok(body);
        })
        .WithTags("Metadata");

        group.MapPost("/more-metadata", Ok<MoreMetadata> (MoreMetadata body) =>
        {
            return TypedResults.Ok(body);
        })
        .WithTags("More Metadata");

        group.MapPost("/enums", Ok<Enums> (Enums body) =>
        {
            return TypedResults.Ok(body);
        })
        .WithTags("Enums");

        // Shapes

        // Return a circle, triangle, or square based on the query parameter
        group.MapGet("/shape",
        (
            [FromQuery] string type
        ) =>
        {
            Shape shape = type switch
            {
                "circle" => new Circle { Radius = 1 },
                "triangle" => new Triangle { Hypotenuse = 1 },
                "square" => new Square { Area = 1 },
                _ => null
            };
            return TypedResults.Ok<Shape>(shape);
        })
        .WithTags("Shapes");

        // Receives a shape and returns the name of the type of shape
        group.MapPost("/shape-type",
        (
              Shape shape
        ) =>
        {
            // use reflection to get the type of the shape
            Type shapeType = shape.GetType();
            string shapeTypeName = shapeType.Name;
            return TypedResults.Ok<string>(shapeTypeName);
        }).
        WithTags("Shapes");

        group.MapGet("/squares",
        () =>
        {
            return TypedResults.Ok(new Square { Area = 1 });
        })
        .WithTags("Shapes");

        // Pets

        group.MapGet("/pets/{type}",
        (
            [FromRoute] string type
        ) =>
        {
            Pet pet = type switch
            {
                "dog" => new Dog { Name = "Fido", Age = 3, Breed = "Golden Retriever" },
                "cat" => new Cat { Name = "Whiskers", Age = 2 },
                "fish" => new Fish { Name = "Bubbles", Age = 1 },
                _ => null
            };
            return TypedResults.Ok<Pet>(pet);
        })
        .WithTags("Pets");

        return group;
    }
}