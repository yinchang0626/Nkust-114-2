using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Further.Weigh.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class WeighDbContextFactory : IDesignTimeDbContextFactory<WeighDbContext>
{
    public WeighDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        WeighEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<WeighDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new WeighDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Further.Weigh.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
