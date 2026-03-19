using System;
using System.Linq.Expressions;
using Further.WeighGov.S03.TransportTask.TransportContracts;
using Volo.Abp.Specifications;

namespace Further.WeighGov.S03.TransportTask.TransportContracts.Specifications;

/// <summary>
/// 指定廠商在指定日期有有效合約規格：Status=Active 且有效期涵蓋該日
/// </summary>
public class ActiveContractSpecification : Specification<TransportContract>
{
    private readonly Guid _vendorId;
    private readonly DateTime _date;

    public ActiveContractSpecification(Guid vendorId, DateTime date)
    {
        _vendorId = vendorId;
        _date = date;
    }

    public override Expression<Func<TransportContract, bool>> ToExpression()
    {
        return contract =>
            contract.VendorId == _vendorId &&
            contract.Status == ContractStatus.Active &&
            contract.ValidFrom <= _date &&
            contract.ValidTo >= _date;
    }
}
