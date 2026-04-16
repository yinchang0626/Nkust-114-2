using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Further.WeighGov.S03.TransportTask.TransportContracts;

public interface ITransportContractAppService : IApplicationService
{
    /// <summary>取得單筆合約</summary>
    Task<TransportContractDto> GetAsync(Guid id);

    /// <summary>分頁查詢合約清單</summary>
    Task<PagedResultDto<TransportContractDto>> GetListAsync(GetTransportContractsInput input);

    /// <summary>新增合約（初始狀態為 Draft）</summary>
    Task<TransportContractDto> CreateAsync(CreateTransportContractDto input);

    /// <summary>更新合約基本資訊（僅限 Draft 狀態）</summary>
    Task<TransportContractDto> UpdateAsync(Guid id, UpdateTransportContractDto input);

    /// <summary>刪除合約</summary>
    Task DeleteAsync(Guid id);

    /// <summary>啟用合約（Draft -> Active）</summary>
    Task<TransportContractDto> ActivateAsync(Guid id);

    /// <summary>停用合約（Active -> Inactive）</summary>
    Task<TransportContractDto> DeactivateAsync(Guid id);

    /// <summary>作廢合約（Draft / Active / Inactive -> Voided）</summary>
    Task<TransportContractDto> VoidAsync(Guid id);
}
