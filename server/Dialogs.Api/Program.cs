using System.ComponentModel.Design;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Dialogs.Api;
using Dialogs.Infrastructure;
using Dialogs.Application;
using Dialogs.Api.Grpc;
using MediatR;
using Dialogs.Api.Behavior;
using Dialogs.Api.Middleware;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

builder.Services.AddOpenTelemetry()
    .WithMetrics(x =>
    {
        x.AddPrometheusExporter();
        
        x.AddRuntimeInstrumentation();
        x.AddProcessInstrumentation();

        x.AddMeter(
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel");

        x.AddView("request-duration", new ExplicitBucketHistogramConfiguration
        {
            Boundaries = new double[] { 0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10 }
        });
    });

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestLoggingBehavior<,>));
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();
app.UseMiddleware<ActiveRequestMiddleware>();
app.UseMiddleware<ErrorTrackingMiddleware>();
app.UseMiddleware<RequestIdMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapPrometheusScrapingEndpoint();
app.MapControllers();
app.MapGrpcService<DialogServiceImpl>();
app.MapGrpcReflectionService();
app.Run();