using System;
using Further.WeighGov.S03.TransportTask.TransportContracts.Specifications;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Further.WeighGov.S03.TransportTask.TransportContracts;

public class TransportContract : FullAuditedAggregateRoot<Guid>
{
    /// <summary>合約代碼</summary>
    public string Code { get; private set; } = null!;

    /// <summary>合約名稱</summary>
    public string Name { get; private set; } = null!;

    /// <summary>廠商</summary>
    public Guid VendorId { get; private set; }

    /// <summary>廠商名稱快照</summary>
    public string VendorName { get; private set; } = null!;

    /// <summary>合約類型</summary>
    public ContractType ContractType { get; private set; }

    /// <summary>有效起日</summary>
    public DateTime ValidFrom { get; private set; }

    /// <summary>有效迄日</summary>
    public DateTime ValidTo { get; private set; }

    /// <summary>合約狀態</summary>
    public ContractStatus Status { get; private set; }

    /// <summary>附件 URL</summary>
    public string? AttachmentUrl { get; private set; }

    /// <summary>備註</summary>
    public string? Remarks { get; private set; }

    /// <summary>Phase 2 保留</summary>
    public Guid? FormDocumentId { get; private set; }

    public Guid? TenantId { get; private set; }

    protected TransportContract()
    {
        // Required by ORM
    }

    private TransportContract(Guid id) : base(id)
    {
        // Required by Create factory method
    }

    public static TransportContract Create(
        Guid id,
        string code,
        string name,
        Guid vendorId,
        string vendorName,
        ContractType contractType,
        DateTime validFrom,
        DateTime validTo)
    {
        var contract = new TransportContract(id)
        {
            Code = code,
            Name = name,
            VendorId = vendorId,
            VendorName = vendorName,
            ContractType = contractType,
            ValidFrom = validFrom,
            ValidTo = validTo,
            Status = ContractStatus.Draft
        };

        contract.ValidateInvariants(code, name, vendorId, validFrom, validTo);
        return contract;
    }

    public void Activate(DateTime now)
    {
        var specification = new ActivatableContractSpecification(now);
        if (!specification.IsSatisfiedBy(this))
        {
            throw new BusinessException(TransportContractErrorCodes.InvalidStatusTransition);
        }

        Status = ContractStatus.Active;
    }

    public void Deactivate()
    {
        var specification = new DeactivatableContractSpecification();
        if (!specification.IsSatisfiedBy(this))
        {
            throw new BusinessException(TransportContractErrorCodes.InvalidStatusTransition);
        }

        Status = ContractStatus.Inactive;
    }

    public void Void()
    {
        var specification = new VoidableContractSpecification();
        if (!specification.IsSatisfiedBy(this))
        {
            throw new BusinessException(TransportContractErrorCodes.InvalidStatusTransition);
        }

        Status = ContractStatus.Voided;
    }

    public bool IsActiveOn(DateTime date)
    {
        var specification = new ActiveContractSpecification(VendorId, date);
        return specification.IsSatisfiedBy(this);
    }

    /// <summary>更新基本資訊（僅限 Draft 狀態）</summary>
    public void UpdateBasicInfo(
        string name,
        Guid vendorId,
        string vendorName,
        ContractType contractType,
        DateTime validFrom,
        DateTime validTo)
    {
        if (Status != ContractStatus.Draft)
        {
            throw new BusinessException(TransportContractErrorCodes.InvalidStatusTransition);
        }

        Name = name;
        VendorId = vendorId;
        VendorName = vendorName;
        ContractType = contractType;
        ValidFrom = validFrom;
        ValidTo = validTo;

        ValidateInvariants(Code, name, vendorId, validFrom, validTo);
    }

    /// <summary>更新備註</summary>
    public void SetRemarks(string? remarks)
    {
        Remarks = remarks?.Length > TransportContractConsts.MaxRemarksLength
            ? remarks[..TransportContractConsts.MaxRemarksLength]
            : remarks;
    }

    /// <summary>更新附件 URL</summary>
    public void SetAttachmentUrl(string? url)
    {
        AttachmentUrl = url?.Length > TransportContractConsts.MaxAttachmentUrlLength
            ? url[..TransportContractConsts.MaxAttachmentUrlLength]
            : url;
    }

    private void ValidateInvariants(string code, string name, Guid vendorId, DateTime validFrom, DateTime validTo)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new BusinessException(TransportContractErrorCodes.CodeRequired);
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessException(TransportContractErrorCodes.NameRequired);
        }

        if (vendorId == Guid.Empty)
        {
            throw new BusinessException(TransportContractErrorCodes.VendorIdRequired);
        }

        if (validFrom >= validTo)
        {
            throw new BusinessException(TransportContractErrorCodes.InvalidDateRange)
                .WithData("StartDate", validFrom)
                .WithData("EndDate", validTo);
        }
    }
}
