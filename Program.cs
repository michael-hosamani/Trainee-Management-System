using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApiDocument(config =>
{
   config.DocumentName = "v1";
   config.Title = "Training Management api"; 
});

builder.Services.AddSingleton<ITraineeService, TraineeService>();

var app = builder.Build();

app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "Hello World!");


app.UseOpenApi();
app.UseSwaggerUi();


app.Run();