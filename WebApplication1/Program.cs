using HospitalManagementCosmosDB.API.Middlewares;
using HospitalManagementCosmosDB.Application.AutoMapping;
using HospitalManagementCosmosDB.Infrastructure.Injection;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hospital Management API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Idempotency-Key", new OpenApiSecurityScheme
    {
        Name = "Idempotency-Key",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Unique key to make POST/PUT requests idempotent"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Idempotency-Key"
                }
            },
            Array.Empty<string>()
        }
    });
});


// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMap).Assembly);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


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

    // Initialize database and container
    foreach (var c in cosmosOptions.Containers)
    {
        Console.WriteLine($"Container = {c.ContainerId}, PK = {c.PartitionKeyPath}");
    }

    await CosmosInitializer.InitializeAsync(cosmosClient, cosmosOptions);
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseMiddleware<IdempotencyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
