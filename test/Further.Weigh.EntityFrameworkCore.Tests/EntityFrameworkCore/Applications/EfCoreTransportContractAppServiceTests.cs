using Further.WeighGov.S03.TransportTask.TransportContracts;
using Xunit;

namespace Further.Weigh.EntityFrameworkCore.Applications;

[Collection(WeighTestConsts.CollectionDefinitionName)]
public class EfCoreTransportContractAppServiceTests
    : TransportContractAppServiceTests<WeighEntityFrameworkCoreTestModule>
{
}