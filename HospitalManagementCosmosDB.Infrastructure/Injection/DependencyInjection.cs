using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.Azure.Cosmos;
using HospitalManagementCosmosDB.Application.Interfaces;
using HospitalManagementCosmosDB.Infrastructure.Repository;
using HospitalManagementCosmosDB.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace HospitalManagementCosmosDB.Infrastructure.Injection
{
    public class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CosmosDbOptions>(
                configuration.GetSection("CosmosDb"));

            //services.AddSingleton(sp =>
            //{
            //    var opt = sp.GetRequiredService<IOptions<CosmosDbOptions>>().Value;
            //    return new CosmosClient(opt.AccountEndpoint, opt.AccountKey);
            //});

            services.AddSingleton(sp =>
            {
                var opt = sp.GetRequiredService<IOptions<CosmosDbOptions>>().Value;

                return new CosmosClient(
                    opt.AccountEndpoint,
                    opt.AccountKey,
                    new CosmosClientOptions
                    {
                        ConnectionMode = ConnectionMode.Gateway,
                        HttpClientFactory = () =>
                        {
                            var handler = new HttpClientHandler
                            {
                                ServerCertificateCustomValidationCallback =
                                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                            };
                            return new HttpClient(handler);
                        }
                    });
            });



            services.AddSingleton(sp =>
            {
                var opt = sp.GetRequiredService<IOptions<CosmosDbOptions>>().Value;
                var client = sp.GetRequiredService<CosmosClient>();

                return client.GetContainer(opt.DatabaseId, opt.ContainerId);
            });

            services.AddSingleton<IPatientRepository, PatientRepository>();
            services.AddSingleton<IPatientService, PatientService>();

            return services;
        }
    }
}
