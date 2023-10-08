using DistributedMemm.ReservationAPI;
using DistributedMemm.ReservationAPI.Services.Implementations;
using DistributedMemm.ReservationAPI.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.Configure<RabbitMQSettings>(ops => builder.Configuration.GetSection("RabbitMqSettings").Bind(ops));

builder.Services.AddSingleton<ICacheService, MongoDbCacheService>(sp =>
{
    var dbSettings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    return new MongoDbCacheService(
        dbSettings.ConnectionString, 
        dbSettings.DatabaseName, 
        dbSettings.CollectionName);
});

builder.Services.AddSingleton<IConsumerService, RabbitMQConsumerService>(sp =>
{
    var rabbitMqSettings = sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
    var cacheService = sp.GetRequiredService<ICacheService>();
    return new RabbitMQConsumerService(cacheService, rabbitMqSettings);
});
builder.Services.AddHostedService<RabbitMQHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.Run();
