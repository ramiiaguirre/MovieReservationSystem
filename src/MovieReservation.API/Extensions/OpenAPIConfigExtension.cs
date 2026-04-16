using System;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace MovieReservation.API.Extensions;

public static class OpenAPIConfigExtension
{

    public static IServiceCollection AddOpenApiCustomConfig(this IServiceCollection services)
    {

        services.AddOpenApi((options) =>
        {
            
            options.AddOperationTransformer((operation, context, ct) =>
            {
                var problemDetailsSchema = new OpenApiSchemaReference("ProblemDetails");               

                var problemResponse = (string description) => new OpenApiResponse
                {
                    Description = description,
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/problem+json"] = new OpenApiMediaType
                        {
                            Schema = problemDetailsSchema
                        }
                    }
                };

                var hasAuthorize = context.Description.ActionDescriptor.EndpointMetadata
                    .OfType<AuthorizeAttribute>()
                    .Any();

                if (hasAuthorize)
                {
                    operation.Responses.TryAdd("401", problemResponse("Unauthorized"));
                    operation.Responses.TryAdd("403", problemResponse("Forbidden"));
                }

                operation.Responses.TryAdd("500", problemResponse("Internal Server Error"));

                return Task.CompletedTask;
            });
        
            options.AddSchemaTransformer((schema, context, ct) =>
            {
                if (schema.Metadata is null)
                    return Task.CompletedTask;

                schema.Metadata.TryGetValue("x-schema-id", out object? value);               

                if (value is not null && value.ToString().StartsWith("ApiResponse"))
                {
                    // var reference = new OpenApiSchemaReference(value.ToString());

                    string newName = value.ToString().EndsWith("Response") 
                        ? value.ToString()[..^"Response".Length] 
                        : value.ToString();

                    schema.Title = newName;
                }
                
                return Task.CompletedTask;
            });
        });
                
        return services;
    }

}
