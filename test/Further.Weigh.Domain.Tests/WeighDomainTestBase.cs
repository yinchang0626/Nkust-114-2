using Volo.Abp.Modularity;

namespace Further.Weigh;

/* Inherit from this class for your domain layer tests. */
public abstract class WeighDomainTestBase<TStartupModule> : WeighTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
