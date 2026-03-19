using Volo.Abp.Modularity;

namespace Further.Weigh;

[DependsOn(
    typeof(WeighDomainModule),
    typeof(WeighTestBaseModule)
)]
public class WeighDomainTestModule : AbpModule
{

}
