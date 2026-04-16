using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Further.WeighGov.S03.TransportTask.TransportContracts;

[Mapper]
public partial class TransportContractMapper : MapperBase<TransportContract, TransportContractDto>
{
    public override partial TransportContractDto Map(TransportContract source);

    public override partial void Map(TransportContract source, TransportContractDto destination);
}
