using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Get JWT settings
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? 
    throw new InvalidOperationException("JWT Secret is not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? 
    throw new InvalidOperationException("JWT Issuer is not configured");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? 
    throw new InvalidOperationException("JWT Audience is not configured");

// Add services to the container.
builder.Services.AddControllers();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add Application Layer Dependencies
builder.Services.AddApplication();

// Add Infrastructure Layer Dependencies
builder.Services.AddInfrastructure(builder.Configuration);

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSecret))
    };
});

// Add Authorization
builder.Services.AddAuthorization();

// Add EndpointsApiExplorer and Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// Configure CORS
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();