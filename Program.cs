using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<ITraineeService, TraineeService>();

var app = builder.Build();

// This seeds the database by creating one user in the User table
using(var scope = app.Services.CreateAsyncScope()) 
{
   var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

   if (!db.Users.Any())
   {
      var admin = new User
      {
         Username = "michael",
         Email = "michael@gmail.com",
         PasswordHash = "pass",
         Role = Role.Admin
      };
      Console.WriteLine("Seeding user: " + admin);
      db.Users.Add(admin);
      db.SaveChanges();
   }
}

app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "Hello World!");


app.UseOpenApi();
app.UseSwaggerUi();


app.Run();