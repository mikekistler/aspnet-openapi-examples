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

        //group.MapPost("/parent",
        //(
        //    Parent body
        //) =>
        //{
        //    return TypedResults.Ok<Parent>(body);
        //});

        group.MapPost("/child",
        (
            Child body
        ) =>
        {
            return TypedResults.Ok<Child>(body);
        });

        group.MapPost("/shapes",
        (
        ) =>
        {
            return TypedResults.Ok<Shape>(null);
        });

        return group;
    }

}
