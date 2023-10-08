using DistributedMemm.Lib;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var rabbitConfig = builder.Configuration.GetValue<string?>("Rabbit:RabbitMQHost");
if (rabbitConfig != null) builder.Services.AddDistributedMemm(builder.Configuration);

var loggerConfig = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .ReadFrom.Configuration(builder.Configuration);

Log.Logger = loggerConfig.CreateLogger();
builder.Services.AddLogging(v => v.AddSerilog(Log.Logger));
var loggerFactory = new SerilogLoggerFactory(null, true);
builder.Services.AddSingleton<ILoggerFactory>(loggerFactory);
builder.Services.AddLogging(v => v.AddSerilog(Log.Logger));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();