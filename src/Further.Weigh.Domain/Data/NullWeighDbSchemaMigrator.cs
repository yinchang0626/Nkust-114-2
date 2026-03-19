using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Further.Weigh.Data;

/* This is used if database provider does't define
 * IWeighDbSchemaMigrator implementation.
 */
public class NullWeighDbSchemaMigrator : IWeighDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
