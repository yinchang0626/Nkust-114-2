using System;
using System.Linq.Expressions;
using Further.WeighGov.S03.TransportTask.TransportContracts;
using Volo.Abp.Specifications;

namespace Further.WeighGov.S03.TransportTask.TransportContracts.Specifications;

/// <summary>
/// 合約可停用規格：狀態為 Active
/// </summary>
public class DeactivatableContractSpecification : Specification<TransportContract>
{
    public override Expression<Func<TransportContract, bool>> ToExpression()
    {
        return contract => contract.Status == ContractStatus.Active;
    }
}
