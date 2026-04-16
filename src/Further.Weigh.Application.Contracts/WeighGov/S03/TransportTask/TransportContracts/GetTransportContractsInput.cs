using System;
using Volo.Abp.Application.Dtos;

namespace Further.WeighGov.S03.TransportTask.TransportContracts;

public class GetTransportContractsInput : PagedAndSortedResultRequestDto
{
    /// <summary>關鍵字（合約代碼 / 合約名稱）</summary>
    public string? Filter { get; set; }

    /// <summary>廠商篩選</summary>
    public Guid? VendorId { get; set; }

    /// <summary>合約狀態篩選</summary>
    public ContractStatus? Status { get; set; }

    /// <summary>合約類型篩選</summary>
    public ContractType? ContractType { get; set; }

    /// <summary>有效起日（起）</summary>
    public DateTime? ValidFromMin { get; set; }

    /// <summary>有效起日（迄）</summary>
    public DateTime? ValidFromMax { get; set; }
}
