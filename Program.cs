using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => {
    // Controller-based APIs have built-in support for XML serialization, but you need
    // to add the XmlSerializerInputFormatter to the input formatters collection.
    options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
    // Add a custom input formatter for text/plain request bodies.
    options.InputFormatters.Add(new TextPlainInputFormatter());
    // Add a custom input formatter for application/octet-stream request bodies.
    options.InputFormatters.Add(new StreamInputFormatter());
});

builder.Services.AddOpenApi(options => {
    options.UseTransformer<InfoTransformer>();
    options.UseTransformer((document, context, cancellationToken) =>
    {
        document.Servers = new List<OpenApiServer>
        {
            new OpenApiServer
            {
                Url = "http://localhost:5000",
                Description = "Local development server"
            }
        };
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

app.UseAuthorization();

app.MapControllers();

app.Run();
