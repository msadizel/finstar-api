
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);



string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers().AddControllersAsServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
    builder =>
    {
#if DEBUG || LOCALRELEASE
        builder
        .SetIsOriginAllowed(origin => true)
        .AllowAnyHeader()
        .WithMethods("PUT", "POST", "GET", "DELETE")
        .AllowCredentials();
#else
                    builder.WithOrigins(
                    "http://SOMEDOMAIN.com/  ,                
                    "httpS://SOMEDOMAIN.com/                  
                    )
        .AllowAnyHeader()
        .WithMethods("PUT", "POST", "GET", "DELETE")
        .AllowCredentials()
        .SetIsOriginAllowedToAllowWildcardSubdomains();
#endif
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = string.Empty;
#if DEBUG
connectionString = "DebugConnection";
#else
connectionString = "ReleaseConnection";
#endif

builder.Services.AddDbContext<FinStarEntity.Models.FinStarContext>(
             options => options.UseSqlServer(connectionString),
             contextLifetime: ServiceLifetime.Transient,
             optionsLifetime: ServiceLifetime.Singleton);
builder.Services.AddDbContextFactory<FinStarEntity.Models.FinStarContext>(options => options.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
