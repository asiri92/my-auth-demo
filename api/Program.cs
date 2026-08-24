using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using my_auth_api_demo.Authorization;
using my_auth_api_demo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{

    var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? Array.Empty<string>();

    options.AddPolicy("AngularDev", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .WithMethods("GET", "POST")
              .WithHeaders("Authorization", "Content-Type");
    });
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"))
    .AddPolicy("RequireWriter", policy => policy.RequireRole("Writer", "Admin"))
    .AddPolicy("RequireReader", policy => policy.RequireRole("Reader", "Writer", "Admin"))
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

builder.Services.AddSingleton<NoteStore>();
builder.Services.AddSingleton<IAuthorizationHandler, NoteAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AngularDev");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
