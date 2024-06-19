var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/todo-dict", (Todo todo) => { });

app.Run();

 public class Todo
{
    public int Id { get; set; }
    public Dictionary<string, string> KeyValuePairs { get; set; } = new();
}
