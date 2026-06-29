using SubmissionProcessor.Worker.Consumer;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Polly;
using Polly.Extensions.Http;
using System.Web;
using System.Net;
using System.Net.Http;
using SubmissionProcessor.Worker.Services;
using DotNetEnv;

// Load the .env file
DotNetEnv.Env.Load();

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<SubmissionConsumer>();
builder.Configuration.AddEnvironmentVariables(); 

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
 
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Registration
builder.Services.AddHttpClient<TraineeDirectoryClient>("TraineeDirectory.Api", client =>
    {
        client.BaseAddress = new Uri("http://localhost:5190/");
    }).ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new SocketsHttpHandler()
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(15)
        };
    })
    .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
    .AddStandardResilienceHandler(options =>
    {
    
        options.Retry.MaxRetryAttempts = 5;
        options.Retry.BackoffType = DelayBackoffType.Exponential;
        options.Retry.UseJitter = true;
    
        options.Retry.ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
            .Handle<HttpRequestException>()
            .HandleResult(response =>
            response.StatusCode == HttpStatusCode.RequestTimeout ||
            response.StatusCode == HttpStatusCode.ServiceUnavailable ||
            response.StatusCode == HttpStatusCode.TooManyRequests ||
            (int)response.StatusCode >= 500
            );
    
        options.CircuitBreaker.FailureRatio = 0.5;
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(20);
        options.CircuitBreaker.MinimumThroughput = 5;
        options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30);
    
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
    });

var host = builder.Build();
host.Run();
