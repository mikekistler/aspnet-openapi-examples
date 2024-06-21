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

        group.MapGet("/shapes",
        (
            [FromQuery] string type
        ) =>
        {
            // Return a circle, triangle, or square based on the query parameter
            Shape shape = type switch
            {
                "circle" => new Circle { Radius = 1 },
                "triangle" => new Triangle { Hypotenuse = 1},
                "square" => new Square { Area = 1},
                _ => null
            };
            return TypedResults.Ok<Shape>(shape);
        });

        // An endpoint that receives a shape and returns the name of the type of shape
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
        return group;
    }
}
