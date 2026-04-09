using System;
using System.Linq.Expressions;
using Further.WeighGov.S03.TransportTask.TransportContracts;
using Volo.Abp.Specifications;

namespace Further.WeighGov.S03.TransportTask.TransportContracts.Specifications;

/// <summary>
/// 合約可作廢規格：狀態不為 Voided
/// </summary>
public class VoidableContractSpecification : Specification<TransportContract>
{
    public override Expression<Func<TransportContract, bool>> ToExpression()
    {
        return contract => contract.Status != ContractStatus.Voided;
    }
}
