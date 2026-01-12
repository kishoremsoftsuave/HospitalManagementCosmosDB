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
    var opt = scope.ServiceProvider
                   .GetRequiredService<IOptions<CosmosDbOptions>>().Value;

    Console.WriteLine("DB = " + opt.DatabaseId);
    Console.WriteLine("Container = " + opt.ContainerId);
}


using (var scope = app.Services.CreateScope())
{
    var client = scope.ServiceProvider.GetRequiredService<CosmosClient>();
    var opt = scope.ServiceProvider.GetRequiredService<IOptions<CosmosDbOptions>>().Value;

    await CosmosInitializer.InitializeAsync(
        client,
        opt.DatabaseId,
        opt.ContainerId,
        opt.PartitionKeyPath);
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
