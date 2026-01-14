using HospitalManagementCosmosDB.Application.Helpers;
using HospitalManagementCosmosDB.Domain.Entities;
using HospitalManagementCosmosDB.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagementCosmosDB.API.Middlewares
{
    public class IdempotencyMiddleware
    {
        private readonly RequestDelegate _next;

        public IdempotencyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IdempotencyRepository repo)
        {
            if (context.Request.Method != HttpMethods.Post && context.Request.Method != HttpMethods.Put)
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("Idempotency-Key", out var key))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Idempotency-Key header is required");
                return;
            }

            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            var requestHash = RequestHashHelper.ComputeHash(body);

            var existing = await repo.GetAsync(key!);

            if (existing != null && existing.RequestHash != requestHash)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsync("Idempotency-Key reuse with different body");
                return;
            }

            if (existing != null)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(existing.ResponseJson);
                return;
            }

            var originalBody = context.Response.Body;
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            await _next(context);

            memStream.Position = 0;
            var responseBody = await new StreamReader(memStream).ReadToEndAsync();
            memStream.Position = 0;
            await memStream.CopyToAsync(originalBody);

            if (context.Response.StatusCode is >= 200 and < 300)
            {
                await repo.SaveAsync(new Idempotency
                {
                    Id = key!,
                    RequestHash = requestHash,
                    ResponseJson = responseBody
                });
            }
        }

    }
}
