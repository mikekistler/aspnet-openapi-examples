using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

public record Gadget
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
}

public static class GadgetsApi
{
    private static readonly List<Gadget> _gadgets = new();

    public static void MapGadgetsApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/gadgets");

        group.MapGet("/", () => _gadgets)
            .WithName("GetAllGadgets");

        group.MapGet("/{id}", (int id) =>
        {
            var found = _gadgets.FirstOrDefault(w => w.Id == id);
            return found is not null ? Results.Ok(found) : Results.NotFound();
        }).WithName("GetGadgetById");

        group.MapPost("/", (Gadget gadget) =>
        {
            gadget.Id = _gadgets.Count > 0 ? _gadgets.Max(w => w.Id) + 1 : 1;
            _gadgets.Add(gadget);
            return Results.Created($"/api/gadgets/{gadget.Id}", gadget);
        }).WithName("CreateGadget");

        group.MapPut("/{id}", (int id, Gadget updatedGadget) =>
        {
            var index = _gadgets.FindIndex(w => w.Id == id);
            if (index == -1) return Results.NotFound();
            _gadgets[index] = updatedGadget with { Id = id };
            return Results.Ok(_gadgets[index]);
        }).WithName("UpdateGadget");

        group.MapDelete("/{id}", (int id) =>
        {
            var found = _gadgets.FirstOrDefault(w => w.Id == id);
            if (found is null) return Results.NotFound();
            _gadgets.Remove(found);
            return Results.NoContent();
        }).WithName("DeleteGadget");
    }
}
