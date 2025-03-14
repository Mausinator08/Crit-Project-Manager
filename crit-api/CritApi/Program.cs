using System.Net;
using System.Text;
using CritBusinessLogic;
using CritBusinessLogic.Repositories;
using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDataAccess.Services;
using CritDTO.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

StringBuilder? connectionString = new StringBuilder().Append("mongodb://");
string? userNameEnvVar = builder.Configuration.GetValue<string>("MongoDB:UserName");
string? passwordEnvVar = builder.Configuration.GetValue<string>("MongoDB:Password");
// Can be name of server or an IP address.
string? serverName = builder.Configuration.GetValue<string>("MongoDB:ServerName");
int? port = builder.Configuration.GetValue<int>("MongoDB:Port");
string? database = builder.Configuration.GetValue<string>("MongoDB:Database");

if (userNameEnvVar != null && passwordEnvVar != null)
{
    connectionString.Append(Environment.GetEnvironmentVariable(userNameEnvVar))
    .Append(":")
    .Append(Environment.GetEnvironmentVariable(passwordEnvVar))
    .Append("@");
}
else if (userNameEnvVar != null && passwordEnvVar == null)
{
    connectionString.Append(Environment.GetEnvironmentVariable(userNameEnvVar))
    .Append("@");
}
else if (userNameEnvVar == null && passwordEnvVar != null)
{
    throw new Exception("Cannot connect to Mongo DB with a password and no user name.");
}

if (serverName != null)
{
    connectionString.Append(serverName);

    if (port != null)
    {
        connectionString.Append(":")
        .Append(port.ToString());
    }
}
else
{
    throw new Exception("Cannot connect to Mongo DB with no server name or IP address.");
}

if (database == null)
{
    throw new Exception("A database was not configured.");
}

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

builder.Services.AddDbContext<CritDbContext>(options =>
{
    options.UseMongoDB(connectionString.ToString(), database);
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
.AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(connectionString.ToString(), database).AddSignInManager();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
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

builder.Services.AddScoped<ITenantDbContextService, TenantDbContextService>(x =>
{
    var userManager = x.GetService<UserManager<ApplicationUser>>() ?? throw new ArgumentNullException(nameof(UserManager<ApplicationUser>));
    var critDbContext = x.GetService<CritDbContext>() ?? throw new ArgumentNullException(nameof(CritDbContext));
    return new TenantDbContextService(connectionString.ToString(), userManager, critDbContext);
});
string? logPath = builder.Configuration.GetValue<string>("Logging:File:Path");

if (logPath == null)
{
    throw new Exception("Logging path was not configured.");
}

builder.Services.AddSingleton<CritApi.Logging.ILogger, CritApi.Logging.Logger>(x => new CritApi.Logging.Logger(logPath));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<IPhoneNumberRepository, PhoneNumberRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication? app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
