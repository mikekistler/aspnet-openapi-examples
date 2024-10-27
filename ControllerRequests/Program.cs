using Microsoft.AspNetCore.Mvc.Formatters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => {
    // Controller-based APIs have built-in support for XML serialization, but you need
    // to add the XmlSerializerInputFormatter to the input formatters collection.
    options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
    // // Add a custom input formatter for text/plain request bodies.
    // options.InputFormatters.Add(new TextPlainInputFormatter());
    // // Add a custom input formatter for application/octet-stream request bodies.
    // options.InputFormatters.Add(new StreamInputFormatter());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
