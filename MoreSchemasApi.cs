using Microsoft.AspNetCore.Mvc;

internal static class MoreSchemasApi
{
    public static RouteGroupBuilder MapMoreSchemas(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/more-schemas");

        group.WithTags("More Schemas");

        group.MapPost("/dict-body",
        (
            DictBody body
        ) =>
        {
            return TypedResults.Ok<DictBody>(body);
        });

        group.MapPost("/dict-property",
        (
            DictProperty body
        ) =>
        {
            return TypedResults.Ok<DictProperty>(body);
        });

        group.MapPost("/extends-dict",
        (
            ExtendsDict body
        ) =>
        {
            return TypedResults.Ok<ExtendsDict>(body);
        });

        group.MapPost("/type-with-extension-data",
        (
            TypeWithExtensionData body
        ) =>
        {
            return TypedResults.Ok<TypeWithExtensionData>(body);
        });

        group.MapPost("/parent",
        (
            Parent body
        ) =>
        {
            return TypedResults.Ok<Parent>(body);
        });

        group.MapPost("/child",
        (
            Child body
        ) =>
        {
            return TypedResults.Ok<Child>(body);
        });

        // Return a circle, triangle, or square based on the query parameter
        group.MapGet("/shapes",
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
        });

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
        });

        group.MapGet("/squares",
        () =>
        {
            return TypedResults.Ok(new Square { Area = 1 });
        });

        group.MapPost("map-type",
        (
            [FromQuery] decimal mapped
        ) =>
        {
            return TypedResults.Ok();
        });

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
        });

        return group;
    }
}
