using System;
using System.Linq.Expressions;
using Further.WeighGov.S03.TransportTask.TransportContracts;
using Volo.Abp.Specifications;

namespace Further.WeighGov.S03.TransportTask.TransportContracts.Specifications;

/// <summary>
/// 合約可啟用規格：狀態為 Draft 且有效期尚未過期
/// </summary>
public class ActivatableContractSpecification : Specification<TransportContract>
{
    private readonly DateTime _now;

    public ActivatableContractSpecification(DateTime now)
    {
        _now = now;
    }

    public override Expression<Func<TransportContract, bool>> ToExpression()
    {
        return contract =>
            contract.Status == ContractStatus.Draft &&
            contract.ValidTo > _now;
    }
}
