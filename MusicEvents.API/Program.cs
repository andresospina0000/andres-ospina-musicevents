using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using MusicEvents.API.HealthChecks;
using MusicEvents.DataAccess;
using MusicEvents.DataAccess.Repositories;
using MusicEvents.Entities;
using MusicEvents.Services.Implementations;
using MusicEvents.Services.Interfaces;
using MusicEvents.Services.Profiles;
using Serilog;
using System.Text;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

var corsConfiguration = "MusicalEventsAPI";

builder.Logging
    .AddConsole();

builder.Host.UseSerilog((options, logging) =>
{
    // Cuando usamos mas de un Sink esto podria ralentizar el uso de nuestro API.
    logging.WriteTo.Console();
    logging.WriteTo.MSSqlServer(options.Configuration.GetConnectionString("Default"),
        new MSSqlServerSinkOptions
        {
            AutoCreateSqlTable = true,
            TableName = "ApiLogs",
        }, restrictedToMinimumLevel: LogEventLevel.Warning);
});

builder.Services.AddCors(setup =>
{
    setup.AddPolicy(corsConfiguration, x =>
    {
        x.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(options =>
{
    options.AddMaps(typeof(Genre));
    options.AddMaps(typeof(Concert));
    options.AddProfile<SaleProfile>();
});

// Vamos a configurar una clase que mapee las propiedades de nuestro de archivo de configuracion
builder.Services.Configure<AppSettings>(builder.Configuration);

// Registro la dependencia (Inyeccion de Dependencias)
builder.Services.AddScoped<IFileUploader, AzureBlobStorageUploader>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleService, SaleService>();

builder.Services.AddControllers();

builder.Services.AddSqlServer<MusicalEventsDbContext>(builder.Configuration.GetConnectionString("Default"),
    optionsAction: options => options.EnableSensitiveDataLogging(false));
builder.Services.AddIdentity<MusicEventsUserIdentity, IdentityRole>(setup =>
{
    setup.Password.RequireNonAlphanumeric = false;
    setup.Password.RequiredUniqueChars = 0;
    setup.Password.RequireUppercase = false;
    setup.Password.RequireLowercase = false;
    setup.Password.RequireDigit = false;
    setup.Password.RequiredLength = 8;
    setup.Lockout = new LockoutOptions()
    {
        MaxFailedAccessAttempts = 2,
        AllowedForNewUsers = false,
        DefaultLockoutTimeSpan = new TimeSpan(0, 0, 40)
    };
    setup.User.RequireUniqueEmail = true;
    setup.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<MusicalEventsDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddHealthChecks()
    .AddCheck("MusicalEventsAPI", _ => HealthCheckResult.Healthy(), new[] { "servicio" })
    .AddTypeActivatedCheck<PingHealthCheck>("Google", HealthStatus.Healthy, new[] { "internet" }, "google.com")
    .AddTypeActivatedCheck<PingHealthCheck>("Amazon", HealthStatus.Healthy, new[] { "internet" }, "amazon.com")
    .AddTypeActivatedCheck<PingHealthCheck>("Azure", HealthStatus.Healthy, new[] { "internet" }, "azure.com")
    .AddTypeActivatedCheck<DiskHealthCheck>("Almacenamiento", HealthStatus.Healthy, new[] { "servicio" }, builder.Configuration)
    .AddDbContextCheck<MusicalEventsDbContext>("EF Core", HealthStatus.Healthy, new[] { "basedatos" });

var key = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:SigningKey"));

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

var app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// HABILITAMOS EL CORS (El frontend lo agradecerá)
app.UseCors(corsConfiguration);

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        //Predicate = x => x.Tags.Contains("servicio")
    }).RequireAuthorization();

    endpoints.MapHealthChecks("/health/externos", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        Predicate = x => x.Tags.Contains("internet")
    }).RequireAuthorization();

    endpoints.MapHealthChecks("/health/internos", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        Predicate = x => x.Tags.Contains("basedatos")
    }).RequireAuthorization();
});

//app.MapGet("/api/genre", async (MusicalEventsDbContext db) => await db.Genres.ToListAsync());

//app.MapPost("/api/genre", async (DtoGenre request, MusicalEventsDbContext db, HttpContext context) =>
//{
//    var entity = new Genre
//    {
//        Description = request.Description,
//        Status = true
//    };

//    db.Genres.Add(entity);
//    await db.SaveChangesAsync();

//    context.Response.Headers.Add("location", $"/api/genre/{entity.Id}");
//});

app.MapControllers();

app.Run();