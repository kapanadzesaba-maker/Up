var builder = WebApplication.CreateBuilder(args);

// Services will be registered later when Application/Infrastructure are added
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/", () => Results.Ok(new { name = "BookCatalog API", status = "ok" }));

app.Run();


