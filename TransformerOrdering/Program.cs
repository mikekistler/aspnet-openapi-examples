using Microsoft.AspNetCore.OpenApi;
using TransformerOrdering;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddOperationTransformer((operation, context, cancellationToken) =>
        {
            System.Console.WriteLine($"Op Transformer 1 running on {operation.OperationId ?? "unknown"}");
            return Task.CompletedTask;
        });
    options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            System.Console.WriteLine($"Doc Transformer 1 running on {context.DocumentName ?? "unknown"}");
            return Task.CompletedTask;
        });
    options.AddOperationTransformer((operation, context, cancellationToken) =>
        {
            System.Console.WriteLine($"Op Transformer 2 running on {operation.OperationId ?? "unknown"}");
            return Task.CompletedTask;
        });
    options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            System.Console.WriteLine($"Doc Transformer 2 running on {context.DocumentName ?? "unknown"}");
            return Task.CompletedTask;
        });
    options.AddSchemaTransformer((schema, context, cancellationToken) =>
        {
            var schemaId = schema.Reference?.Id
                ?? OpenApiOptions.CreateDefaultSchemaReferenceId(context.JsonTypeInfo)
                ?? $"{context.JsonTypeInfo.Type.Name} {context.JsonPropertyInfo?.Name}"
                ?? "unknown";
            System.Console.WriteLine($"Schema Transformer 1 running on {schemaId}");
            return Task.CompletedTask;
        });
    options.AddSchemaTransformer((schema, context, cancellationToken) =>
        {
            var schemaId = schema.Reference?.Id
                ?? OpenApiOptions.CreateDefaultSchemaReferenceId(context.JsonTypeInfo)
                ?? $"{context.JsonTypeInfo.Type.Name} {context.JsonPropertyInfo?.Name}"
                ?? "unknown";
            System.Console.WriteLine($"Schema Transformer 2 running on {schemaId}");
            return Task.CompletedTask;
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapWidgetsApi();
app.MapGadgetsApi();

app.Run();
