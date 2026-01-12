using HospitalManagementCosmosDB.Application.AutoMapping;
using HospitalManagementCosmosDB.Infrastructure.Injection;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using HospitalManagementCosmosDB.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMap).Assembly);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Get Cosmos options and client once
    var cosmosOptions = services.GetRequiredService<IOptions<CosmosDbOptions>>().Value;
    var cosmosClient = services.GetRequiredService<CosmosClient>();

    // Log info
    Console.WriteLine($"DB = {cosmosOptions.DatabaseId}");
    Console.WriteLine($"Container = {cosmosOptions.ContainerId}");

    // Initialize database and container
    await CosmosInitializer.InitializeAsync(
        cosmosClient,
        cosmosOptions.DatabaseId,
        cosmosOptions.ContainerId,
        cosmosOptions.PartitionKeyPath
    );
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
