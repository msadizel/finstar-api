
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers().AddControllersAsServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
    builder =>
    {
#if DEBUG
        builder
        .SetIsOriginAllowed(origin => true)
        .AllowAnyHeader()
        .WithMethods("PUT", "POST", "GET", "DELETE")
        .AllowCredentials();
#else
            builder
            .WithOrigins
            (
            "http://SOMEDOMAIN.com/",
            "httpS://SOMEDOMAIN.com/ "
            )
            .AllowAnyHeader()
            .WithMethods("PUT", "POST", "GET", "DELETE")
            .AllowCredentials()
            .SetIsOriginAllowedToAllowWildcardSubdomains();
#endif
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var connectionString = string.Empty;
#if DEBUG
connectionString = builder.Configuration.GetConnectionString("DebugConnection");
#else
    connectionString = builder.Configuration.GetConnectionString("ReleaseConnection");
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
    app.UseSwaggerUI(options =>
    {
        //options.RoutePrefix = string.Empty;

        options.EnableTryItOutByDefault();
        options.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
