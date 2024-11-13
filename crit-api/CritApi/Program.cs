using Microsoft.EntityFrameworkCore;
using CritDataAccess.Contexts;
using System.Text;
using CritDTO.Identity;
using CritDataAccess.Services;

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

builder.Services.AddDbContext<CritDbContext>(options =>
{
    options.UseMongoDB(connectionString.ToString(), database);
});

builder.Services.AddSingleton<ITenantDbContextService, TenantDbContextService>(x => new TenantDbContextService(connectionString.ToString()));
builder.Services.AddSingleton<CritApi.Logging.ILogger, CritApi.Logging.Logger>(x => new CritApi.Logging.Logger(null));

builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin();
        });
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(setup =>
{
    setup.Password.RequireDigit = true;
    setup.Password.RequiredLength = 8;
    setup.Password.RequireLowercase = true;
    setup.Password.RequireUppercase = true;
    setup.Password.RequireNonAlphanumeric = true;
})
.AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(connectionString.ToString(), database);

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
