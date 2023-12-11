
using ApiN5now;
using Serilog;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfig) =>
        loggerConfig.ReadFrom.Configuration(hostingContext.Configuration));

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);


app.Run();
