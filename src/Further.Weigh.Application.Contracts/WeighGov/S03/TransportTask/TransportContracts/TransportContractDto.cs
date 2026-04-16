using System;
using Volo.Abp.Application.Dtos;

namespace Further.WeighGov.S03.TransportTask.TransportContracts;

public class TransportContractDto : FullAuditedEntityDto<Guid>
{
    /// <summary>合約代碼</summary>
    public string Code { get; set; } = null!;

    /// <summary>合約名稱</summary>
    public string Name { get; set; } = null!;

    /// <summary>廠商 Id</summary>
    public Guid VendorId { get; set; }

    /// <summary>廠商名稱快照</summary>
    public string VendorName { get; set; } = null!;

    /// <summary>合約類型</summary>
    public ContractType ContractType { get; set; }

    /// <summary>有效起日</summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>有效迄日</summary>
    public DateTime ValidTo { get; set; }

    /// <summary>合約狀態</summary>
    public ContractStatus Status { get; set; }

    /// <summary>附件 URL</summary>
    public string? AttachmentUrl { get; set; }

    /// <summary>備註</summary>
    public string? Remarks { get; set; }

    /// <summary>Phase 2 保留</summary>
    public Guid? FormDocumentId { get; set; }

    public Guid? TenantId { get; set; }
}
