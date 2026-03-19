using Further.Weigh.Samples;
using Xunit;

namespace Further.Weigh.EntityFrameworkCore.Applications;

[Collection(WeighTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<WeighEntityFrameworkCoreTestModule>
{

}
