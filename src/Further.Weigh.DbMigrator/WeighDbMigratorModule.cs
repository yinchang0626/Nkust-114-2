using Further.Weigh.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Further.Weigh.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(WeighEntityFrameworkCoreModule),
    typeof(WeighApplicationContractsModule)
)]
public class WeighDbMigratorModule : AbpModule
{
}
