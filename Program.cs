using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
   .AddJsonOptions(options =>
   {
      options.JsonSerializerOptions.Converters.Add(
         new JsonStringEnumConverter() 
      );
   });

builder.Services.AddOpenApiDocument(config =>
{
   config.DocumentName = "v1";
   config.Title = "Training Management api"; 
});

builder.Services.AddDbContext<AppDbContext>(
   options => 
   {
      options.UseInMemoryDatabase("TraineeDb");
   }
);

builder.Services.AddScoped<ITraineeService, TraineeService>();

var app = builder.Build();

app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "Hello World!");


app.UseOpenApi();
app.UseSwaggerUi();


app.Run();