using Volo.Abp.Modularity;

namespace Further.Weigh;

[DependsOn(
    typeof(WeighApplicationModule),
    typeof(WeighDomainTestModule)
)]
public class WeighApplicationTestModule : AbpModule
{

}
