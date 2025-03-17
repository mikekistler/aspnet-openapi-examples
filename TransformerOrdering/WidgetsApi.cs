using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace TransformerOrdering;

public record Widget
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public static class WidgetsApi
{
    private static readonly List<Widget> _widgets = new();

    public static void MapWidgetsApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/widgets");

        group.MapGet("/", () => _widgets);

        group.MapGet("/{id}", (int id) =>
        {
            var found = _widgets.FirstOrDefault(w => w.Id == id);
            return found is not null ? Results.Ok(found) : Results.NotFound();
        });

        group.MapPost("/", (Widget widget) =>
        {
            widget.Id = _widgets.Count > 0 ? _widgets.Max(w => w.Id) + 1 : 1;
            _widgets.Add(widget);
            return Results.Created($"/api/widgets/{widget.Id}", widget);
        });

        group.MapPut("/{id}", (int id, Widget updatedWidget) =>
        {
            var index = _widgets.FindIndex(w => w.Id == id);
            if (index == -1) return Results.NotFound();
            _widgets[index] = updatedWidget with { Id = id };
            return Results.Ok(_widgets[index]);
        });

        group.MapDelete("/{id}", (int id) =>
        {
            var found = _widgets.FirstOrDefault(w => w.Id == id);
            if (found is null) return Results.NotFound();
            _widgets.Remove(found);
            return Results.NoContent();
        });
    }
}