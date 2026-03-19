using Further.Weigh.Samples;
using Xunit;

namespace Further.Weigh.EntityFrameworkCore.Domains;

[Collection(WeighTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<WeighEntityFrameworkCoreTestModule>
{

}
