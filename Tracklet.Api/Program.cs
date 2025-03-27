using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Tracklet.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var applicationConnectionString = builder.Configuration.GetConnectionString("ApplicationConnection") ??
                                  throw new Exception("Application connection string not found");
var migrationConnectionString = builder.Configuration.GetConnectionString("MigrationConnection") ??
                                throw new NullReferenceException("Migration connection string not found");

builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseNpgsql(applicationConnectionString, op => op.EnableRetryOnFailure())
);

builder.Services.AddFluentMigrator(migrationConnectionString);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

app.UseAuthorization();

app.MapControllers();

app.Run();