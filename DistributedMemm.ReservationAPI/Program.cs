using DistributedMemm.ReservationAPI;
using DistributedMemm.ReservationAPI.Services.Implementations;
using DistributedMemm.ReservationAPI.Services.Interfaces;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));

builder.Services.AddSingleton<ICacheService, MongoDbCacheService>(sp =>
{
    var dbSettings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    return new MongoDbCacheService(dbSettings.ConnectionString, dbSettings.DatabaseName, dbSettings.CollectionName);
});

builder.Services.AddSingleton<IConsumerService, RabbitMQConsumerService>();
builder.Services.AddHostedService<RabbitMQHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();
app.Run();
