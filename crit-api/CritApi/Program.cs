using Microsoft.EntityFrameworkCore;
using CritDataAccess.Context;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<CritSQLContext>(options =>
{
    options.UseSqlServer(new StringBuilder().Append("Server=")
    .Append(builder.Configuration.GetValue<string>("SQLServerInstanceName"))
    .Append("Database=")
    .Append(builder.Configuration.GetValue<string>("SQLDatabaseName"))
    .Append("Uid=")
    .Append(Environment.GetEnvironmentVariable("APP_CONTEXT_USER"))
    .Append("Pwd=")
    .Append(Environment.GetEnvironmentVariable(variable: "APP_CONTEXT_PASSWORD"))
    .ToString(), sqlServerOptionsAction =>
    {
        sqlServerOptionsAction.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
