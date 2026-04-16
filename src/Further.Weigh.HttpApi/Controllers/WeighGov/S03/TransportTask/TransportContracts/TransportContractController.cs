using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Further.WeighGov.S03.TransportTask.TransportContracts;

[RemoteService]
[Area("app")]
[Route("api/app/transport-contracts")]
public class TransportContractController : Further.Weigh.Controllers.WeighController, ITransportContractAppService
{
    private readonly ITransportContractAppService _appService;

    public TransportContractController(ITransportContractAppService appService)
    {
        _appService = appService;
    }

    /// <summary>取得單筆合約</summary>
    [HttpGet]
    [Route("{id}")]
    public Task<TransportContractDto> GetAsync(Guid id)
        => _appService.GetAsync(id);

    /// <summary>分頁查詢合約清單</summary>
    [HttpGet]
    public Task<PagedResultDto<TransportContractDto>> GetListAsync(GetTransportContractsInput input)
        => _appService.GetListAsync(input);

    /// <summary>新增合約</summary>
    [HttpPost]
    public Task<TransportContractDto> CreateAsync([FromBody] CreateTransportContractDto input)
        => _appService.CreateAsync(input);

    /// <summary>更新合約基本資訊（僅限 Draft 狀態）</summary>
    [HttpPut]
    [Route("{id}")]
    public Task<TransportContractDto> UpdateAsync(Guid id, [FromBody] UpdateTransportContractDto input)
        => _appService.UpdateAsync(id, input);

    /// <summary>刪除合約</summary>
    [HttpDelete]
    [Route("{id}")]
    public Task DeleteAsync(Guid id)
        => _appService.DeleteAsync(id);

    /// <summary>啟用合約（Draft -> Active）</summary>
    [HttpPost]
    [Route("{id}/activate")]
    public Task<TransportContractDto> ActivateAsync(Guid id)
        => _appService.ActivateAsync(id);

    /// <summary>停用合約（Active -> Inactive）</summary>
    [HttpPost]
    [Route("{id}/deactivate")]
    public Task<TransportContractDto> DeactivateAsync(Guid id)
        => _appService.DeactivateAsync(id);

    /// <summary>作廢合約</summary>
    [HttpPost]
    [Route("{id}/void")]
    public Task<TransportContractDto> VoidAsync(Guid id)
        => _appService.VoidAsync(id);
}
