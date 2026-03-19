using Xunit;

namespace Further.Weigh.EntityFrameworkCore;

[CollectionDefinition(WeighTestConsts.CollectionDefinitionName)]
public class WeighEntityFrameworkCoreCollection : ICollectionFixture<WeighEntityFrameworkCoreFixture>
{

}
