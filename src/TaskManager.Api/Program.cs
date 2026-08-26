using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskManager.Api.Filters;
using TaskManager.Api.OpenApi;
using TaskManager.Api.Services;
using TaskManager.Application.DependencyInjection;
using TaskManager.Application.Interfaces;
using TaskManager.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        var configuration = builder.Configuration;

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],

            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(
                    configuration["Jwt:SecretKey"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                Console.WriteLine($"JWT HEADER: {context.Request.Headers.Authorization}");
                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"JWT ERROR: {context.Exception.Message}");
                return Task.CompletedTask;
            },

            OnChallenge = context =>
            {
                Console.WriteLine($"JWT CHALLENGE: {context.Error} - {context.ErrorDescription}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddScoped<ExceptionFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    options.AddOperationTransformer((operation, context, cancellationToken) =>
    {
        var hasAuthorizeAttribute =
            context.Description.ActionDescriptor.EndpointMetadata
                .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>()
                .Any();

        if (hasAuthorizeAttribute)
        {
            operation.Security ??= [];

            operation.Security.Add(
                new Microsoft.OpenApi.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer"),
                        []
                    }
                });
        }

        return Task.CompletedTask;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}