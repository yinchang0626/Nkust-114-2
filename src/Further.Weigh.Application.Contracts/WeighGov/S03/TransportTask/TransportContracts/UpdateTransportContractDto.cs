using System;
using System.ComponentModel.DataAnnotations;

namespace Further.WeighGov.S03.TransportTask.TransportContracts;

public class UpdateTransportContractDto
{
    /// <summary>合約名稱</summary>
    [Required]
    [MaxLength(TransportContractConsts.MaxNameLength)]
    public string Name { get; set; } = null!;

    /// <summary>廠商 Id</summary>
    [Required]
    public Guid VendorId { get; set; }

    /// <summary>廠商名稱快照</summary>
    [Required]
    [MaxLength(TransportContractConsts.MaxVendorNameLength)]
    public string VendorName { get; set; } = null!;

    /// <summary>合約類型</summary>
    public ContractType ContractType { get; set; }

    /// <summary>有效起日</summary>
    [Required]
    public DateTime ValidFrom { get; set; }

    /// <summary>有效迄日</summary>
    [Required]
    public DateTime ValidTo { get; set; }

    /// <summary>附件 URL</summary>
    [MaxLength(TransportContractConsts.MaxAttachmentUrlLength)]
    public string? AttachmentUrl { get; set; }

    /// <summary>備註</summary>
    [MaxLength(TransportContractConsts.MaxRemarksLength)]
    public string? Remarks { get; set; }
}
