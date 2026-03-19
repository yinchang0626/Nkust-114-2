using Volo.Abp.Modularity;

namespace Further.Weigh;

public abstract class WeighApplicationTestBase<TStartupModule> : WeighTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
