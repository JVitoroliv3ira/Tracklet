using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Tracklet.Infra.Data.Migrations;

public static class MigrationRunner
{
    public static void AddFluentMigrator(this IServiceCollection services, string connectionString)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(MigrationRunner).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());
    }
}