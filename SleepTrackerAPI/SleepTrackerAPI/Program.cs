using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddApplicationInsightsTelemetry();

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new() { Title = "SleepTrackerAPI", Version = "v1" });

    config.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key Needed",
        In = ParameterLocation.Header,
        Name = "X-Api-Key",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<ISleepService, SleepService>();
builder.Services.AddScoped<ISleepUseCase, SleepUseCase>();

builder.Services.AddScoped<IAccountService, AccountService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated(); // Creates DB and tables if they don't exist
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // User Secrets config for API key in local development
    builder.Configuration.AddUserSecrets<Program>();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<APIKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
