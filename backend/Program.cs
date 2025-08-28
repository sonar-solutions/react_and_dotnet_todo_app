var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


var repo = new TodoRepository();

app.MapGet("/api/todos", () => repo.GetAll());
app.MapGet("/api/todos/{id}", (int id) =>
{
    var todo = repo.Get(id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});
app.MapPost("/api/todos", (TodoItem item) => Results.Ok(repo.Add(item)));
app.MapPut("/api/todos/{id}", (int id, TodoItem item) =>
    repo.Update(id, item) ? Results.Ok() : Results.NotFound());
app.MapDelete("/api/todos/{id}", (int id) =>
    repo.Delete(id) ? Results.Ok() : Results.NotFound());


app.Run();

