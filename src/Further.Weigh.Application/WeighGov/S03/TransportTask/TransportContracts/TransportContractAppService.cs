using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Further.Weigh.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Further.WeighGov.S03.TransportTask.TransportContracts;

[Authorize(WeighPermissions.TransportContracts.Default)]
public class TransportContractAppService : Further.Weigh.WeighAppService, ITransportContractAppService
{
    private readonly ITransportContractRepository _repository;
    private readonly IGuidGenerator _guidGenerator;

    public TransportContractAppService(
        ITransportContractRepository repository,
        IGuidGenerator guidGenerator)
    {
        _repository = repository;
        _guidGenerator = guidGenerator;
    }

    /// <inheritdoc/>
    public async Task<TransportContractDto> GetAsync(Guid id)
    {
        var contract = await _repository.GetAsync(id);
        return ObjectMapper.Map<TransportContract, TransportContractDto>(contract);
    }

    /// <inheritdoc/>
    public async Task<PagedResultDto<TransportContractDto>> GetListAsync(GetTransportContractsInput input)
    {
        var queryable = await _repository.GetQueryableAsync();

        queryable = queryable
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                x => x.Code.Contains(input.Filter!) || x.Name.Contains(input.Filter!))
            .WhereIf(input.VendorId.HasValue, x => x.VendorId == input.VendorId!.Value)
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value)
            .WhereIf(input.ContractType.HasValue, x => x.ContractType == input.ContractType!.Value)
            .WhereIf(input.ValidFromMin.HasValue, x => x.ValidFrom >= input.ValidFromMin!.Value)
            .WhereIf(input.ValidFromMax.HasValue, x => x.ValidFrom <= input.ValidFromMax!.Value);

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        var items = await AsyncExecuter.ToListAsync(
            queryable
                .OrderBy(input.Sorting.IsNullOrWhiteSpace()
                    ? nameof(TransportContract.Code)
                    : input.Sorting)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount));

        return new PagedResultDto<TransportContractDto>(
            totalCount,
            ObjectMapper.Map<List<TransportContract>, List<TransportContractDto>>(items));
    }

    /// <inheritdoc/>
    [Authorize(WeighPermissions.TransportContracts.Create)]
    public async Task<TransportContractDto> CreateAsync(CreateTransportContractDto input)
    {
        var existing = await _repository.FindByCodeAsync(input.Code);
        if (existing != null)
        {
            throw new BusinessException(TransportContractErrorCodes.CodeAlreadyExists)
                .WithData("Code", input.Code);
        }

        var contract = TransportContract.Create(
            _guidGenerator.Create(),
            input.Code,
            input.Name,
            input.VendorId,
            input.VendorName,
            input.ContractType,
            input.ValidFrom,
            input.ValidTo);

        contract.SetRemarks(input.Remarks);
        contract.SetAttachmentUrl(input.AttachmentUrl);

        await _repository.InsertAsync(contract, autoSave: true);

        return ObjectMapper.Map<TransportContract, TransportContractDto>(contract);
    }

    /// <inheritdoc/>
    [Authorize(WeighPermissions.TransportContracts.Edit)]
    public async Task<TransportContractDto> UpdateAsync(Guid id, UpdateTransportContractDto input)
    {
        var contract = await _repository.GetAsync(id);

        contract.UpdateBasicInfo(
            input.Name,
            input.VendorId,
            input.VendorName,
            input.ContractType,
            input.ValidFrom,
            input.ValidTo);

        contract.SetRemarks(input.Remarks);
        contract.SetAttachmentUrl(input.AttachmentUrl);

        await _repository.UpdateAsync(contract, autoSave: true);

        return ObjectMapper.Map<TransportContract, TransportContractDto>(contract);
    }

    /// <inheritdoc/>
    [Authorize(WeighPermissions.TransportContracts.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id, autoSave: true);
    }

    /// <inheritdoc/>
    [Authorize(WeighPermissions.TransportContracts.Edit)]
    public async Task<TransportContractDto> ActivateAsync(Guid id)
    {
        var contract = await _repository.GetAsync(id);
        contract.Activate(Clock.Now);
        await _repository.UpdateAsync(contract, autoSave: true);
        return ObjectMapper.Map<TransportContract, TransportContractDto>(contract);
    }

    /// <inheritdoc/>
    [Authorize(WeighPermissions.TransportContracts.Edit)]
    public async Task<TransportContractDto> DeactivateAsync(Guid id)
    {
        var contract = await _repository.GetAsync(id);
        contract.Deactivate();
        await _repository.UpdateAsync(contract, autoSave: true);
        return ObjectMapper.Map<TransportContract, TransportContractDto>(contract);
    }

    /// <inheritdoc/>
    [Authorize(WeighPermissions.TransportContracts.Edit)]
    public async Task<TransportContractDto> VoidAsync(Guid id)
    {
        var contract = await _repository.GetAsync(id);
        contract.Void();
        await _repository.UpdateAsync(contract, autoSave: true);
        return ObjectMapper.Map<TransportContract, TransportContractDto>(contract);
    }
}
