using System.Text;
using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Contexts;
using Crit.Domain.Identity;
using Crit.Domain.Models;
using Crit.Infrastructure.Repositories;
using CritApi.Logging;
using CritApi.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

StringBuilder? connectionString = new StringBuilder();
string? userNameEnvVar = builder.Configuration.GetValue<string>("PostgreSQL:Username");
string? passwordEnvVar = builder.Configuration.GetValue<string>("PostgreSQL:Password");
// Can be name of server or an IP address.
string? serverName = builder.Configuration.GetValue<string>("PostgreSQL:ServerName");
int? port = builder.Configuration.GetValue<int>("PostgreSQL:Port");
string? database = builder.Configuration.GetValue<string>("PostgreSQL:Database");

if (serverName == null)
{
    throw new Exception("Cannot connect to PostgreSQL DB with no server name or IP address.");
}

if (database == null)
{
    throw new Exception("A database was not configured.");
}

if (userNameEnvVar != null && passwordEnvVar != null)
{
    connectionString.Append("Host=")
    .Append(serverName)
    .Append(port.HasValue ? ";Port=" : "")
    .Append(port.HasValue ? port.Value.ToString() : "")
    .Append(";Database=")
    .Append(database)
    .Append(";Username=")
    .Append(Environment.GetEnvironmentVariable(userNameEnvVar))
    .Append(";Password=")
    .Append(Environment.GetEnvironmentVariable(passwordEnvVar));
}
else if (userNameEnvVar != null && passwordEnvVar == null)
{
    connectionString.Append("Host=")
    .Append(serverName)
    .Append(port.HasValue ? ";Port=" : "")
    .Append(port.HasValue ? port.Value.ToString() : "")
    .Append(";Database=")
    .Append(database)
    .Append(";Username=")
    .Append(Environment.GetEnvironmentVariable(userNameEnvVar));
}
else if (userNameEnvVar == null && passwordEnvVar != null)
{
    throw new Exception("Cannot connect to PostgreSQL DB with a password and no user name.");
}

if (builder.Environment.IsDevelopment())
{
    connectionString.Append(";Include Error Detail=true");
}

Console.WriteLine(connectionString.ToString());

builder.Services.AddDbContext<CritDbContext>(options =>
{
    options.UseNpgsql(connectionString.ToString(), options =>
    {
        options.UseAdminDatabase("postgres");
    });
    options.EnableSensitiveDataLogging();
});

builder.Services.AddIdentityCore<ApplicationUser>(setupAction =>
{
    setupAction.Password.RequireDigit = true;
    setupAction.Password.RequireLowercase = true;
    setupAction.Password.RequireUppercase = true;
    setupAction.Password.RequireNonAlphanumeric = true;
    setupAction.Password.RequiredLength = 8;
})
.AddRoles<ApplicationRole>()
.AddEntityFrameworkStores<CritDbContext>()
.AddSignInManager();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.OnAppendCookie = context =>
    {
        context.CookieOptions.Extensions.Add("Partitioned");
    };
});

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy", policy =>
        {
            string? origins = builder.Configuration.GetValue<string>("Security:AllowedOrigins");

            if (origins == null)
            {
                throw new Exception("Allowed origins were not configured.");
            }

            if (!origins.Any())
            {
                throw new Exception("Allowed origins is empty.");
            }

            policy.WithOrigins(origins.Split(";"))
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

string? logPath = builder.Configuration.GetValue<string>("Logging:File:Path");

if (logPath == null)
{
    throw new Exception("Logging path was not configured.");
}

builder.Services.AddSingleton<IFileLogger, FileLogger>(x => new FileLogger(logPath));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<IPhoneNumberRepository, PhoneNumberRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IPriorityRepository, PriorityRepository>();
builder.Services.AddScoped<ICustomFieldTypeRepository, CustomFieldTypeRepository>();
builder.Services.AddScoped<ICustomFieldRepository, CustomFieldRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication? app = builder.Build();

using (IServiceScope? scope = app.Services.CreateScope())
{
    CritDbContext critDbContext = scope.ServiceProvider.GetRequiredService<CritDbContext>();

    if (critDbContext.Database.GetPendingMigrations().Any())
    {
        critDbContext.ConfigureDefault();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
