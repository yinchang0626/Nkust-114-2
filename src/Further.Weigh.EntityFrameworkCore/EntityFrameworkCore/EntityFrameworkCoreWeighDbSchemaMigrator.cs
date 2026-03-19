using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Further.Weigh.Data;
using Volo.Abp.DependencyInjection;

namespace Further.Weigh.EntityFrameworkCore;

public class EntityFrameworkCoreWeighDbSchemaMigrator
    : IWeighDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreWeighDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the WeighDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<WeighDbContext>()
            .Database
            .MigrateAsync();
    }
}
