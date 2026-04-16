using Further.Weigh;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;

namespace Further.WeighGov.S03.TransportTask.TransportContracts;

public abstract class TransportContractAppServiceTests<TStartupModule> : WeighApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private static int _sequence;

    private readonly ITransportContractAppService _appService;
    private readonly ITransportContractRepository _repository;

    protected TransportContractAppServiceTests()
    {
        _appService = GetRequiredService<ITransportContractAppService>();
        _repository = GetRequiredService<ITransportContractRepository>();
    }

    [Fact]
    public async Task GetAsync_Should_Return_Contract()
    {
        var contract = await CreatePersistedContractAsync();

        var result = await _appService.GetAsync(contract.Id);

        result.Id.ShouldBe(contract.Id);
        result.Code.ShouldBe(contract.Code);
        result.Name.ShouldBe(contract.Name);
        result.Status.ShouldBe(contract.Status);
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_And_Sort_Contracts()
    {
        var vendorId = Guid.NewGuid();
        var keyword = NextCode("LIST");

        await CreatePersistedContractAsync(
            code: $"{keyword}-01",
            name: $"{keyword}-Alpha",
            vendorId: vendorId,
            contractType: ContractType.Glass,
            validFrom: new DateTime(2026, 1, 1),
            validTo: new DateTime(2026, 12, 31));

        await CreatePersistedContractAsync(
            code: $"{keyword}-02",
            name: $"{keyword}-Beta",
            vendorId: vendorId,
            contractType: ContractType.Glass,
            validFrom: new DateTime(2026, 1, 5),
            validTo: new DateTime(2026, 12, 31));

        await CreatePersistedContractAsync(
            code: $"{keyword}-XX",
            name: "Other Vendor",
            vendorId: Guid.NewGuid(),
            contractType: ContractType.Glass,
            validFrom: new DateTime(2026, 1, 5),
            validTo: new DateTime(2026, 12, 31));

        var result = await _appService.GetListAsync(new GetTransportContractsInput
        {
            Filter = keyword,
            VendorId = vendorId,
            Status = ContractStatus.Draft,
            ContractType = ContractType.Glass,
            Sorting = $"{nameof(TransportContract.Code)} desc",
            MaxResultCount = 10,
            SkipCount = 0
        });

        result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
        result.Items.Select(x => x.Code).ShouldContain($"{keyword}-01");
        result.Items.Select(x => x.Code).ShouldContain($"{keyword}-02");
        result.Items.First().Code.ShouldBe($"{keyword}-02");
    }

    [Fact]
    public async Task CreateAsync_Should_Create_Draft_Contract()
    {
        var input = BuildCreateInput();

        var result = await _appService.CreateAsync(input);
        var contract = await _repository.GetAsync(result.Id);

        result.Code.ShouldBe(input.Code);
        result.Name.ShouldBe(input.Name);
        result.Status.ShouldBe(ContractStatus.Draft);
        contract.Code.ShouldBe(input.Code);
        contract.Remarks.ShouldBe(input.Remarks);
        contract.AttachmentUrl.ShouldBe(input.AttachmentUrl);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Code_Already_Exists()
    {
        var input = BuildCreateInput();
        await CreatePersistedContractAsync(code: input.Code);

        var exception = await Assert.ThrowsAsync<BusinessException>(() => _appService.CreateAsync(input));

        exception.Code.ShouldBe(TransportContractErrorCodes.CodeAlreadyExists);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Draft_Contract()
    {
        var contract = await CreatePersistedContractAsync();
        var input = BuildUpdateInput();

        var result = await _appService.UpdateAsync(contract.Id, input);
        var updated = await _repository.GetAsync(contract.Id);

        result.Name.ShouldBe(input.Name);
        result.VendorId.ShouldBe(input.VendorId);
        result.Status.ShouldBe(ContractStatus.Draft);
        updated.Name.ShouldBe(input.Name);
        updated.VendorName.ShouldBe(input.VendorName);
        updated.ContractType.ShouldBe(input.ContractType);
        updated.Remarks.ShouldBe(input.Remarks);
        updated.AttachmentUrl.ShouldBe(input.AttachmentUrl);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_For_Non_Draft_Contract()
    {
        var contract = await CreatePersistedContractAsync(status: ContractStatus.Active);

        var exception = await Assert.ThrowsAsync<BusinessException>(
            () => _appService.UpdateAsync(contract.Id, BuildUpdateInput()));

        exception.Code.ShouldBe(TransportContractErrorCodes.InvalidStatusTransition);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Contract()
    {
        var contract = await CreatePersistedContractAsync();

        await _appService.DeleteAsync(contract.Id);

        await Assert.ThrowsAsync<EntityNotFoundException<TransportContract>>(() => _repository.GetAsync(contract.Id));
    }

    [Fact]
    public async Task ActivateAsync_Should_Change_Status_To_Active()
    {
        var contract = await CreatePersistedContractAsync();

        var result = await _appService.ActivateAsync(contract.Id);
        var updated = await _repository.GetAsync(contract.Id);

        result.Status.ShouldBe(ContractStatus.Active);
        updated.Status.ShouldBe(ContractStatus.Active);
    }

    [Fact]
    public async Task DeactivateAsync_Should_Change_Status_To_Inactive()
    {
        var contract = await CreatePersistedContractAsync(status: ContractStatus.Active);

        var result = await _appService.DeactivateAsync(contract.Id);
        var updated = await _repository.GetAsync(contract.Id);

        result.Status.ShouldBe(ContractStatus.Inactive);
        updated.Status.ShouldBe(ContractStatus.Inactive);
    }

    [Fact]
    public async Task VoidAsync_Should_Change_Status_To_Voided()
    {
        var contract = await CreatePersistedContractAsync(status: ContractStatus.Active);

        var result = await _appService.VoidAsync(contract.Id);
        var updated = await _repository.GetAsync(contract.Id);

        result.Status.ShouldBe(ContractStatus.Voided);
        updated.Status.ShouldBe(ContractStatus.Voided);
    }

    private async Task<TransportContract> CreatePersistedContractAsync(
        string? code = null,
        string? name = null,
        Guid? vendorId = null,
        string? vendorName = null,
        ContractType contractType = ContractType.General,
        DateTime? validFrom = null,
        DateTime? validTo = null,
        ContractStatus status = ContractStatus.Draft)
    {
        var now = DateTime.UtcNow;
        var contract = TransportContract.Create(
            Guid.NewGuid(),
            code ?? NextCode("TC"),
            name ?? $"Contract-{Guid.NewGuid():N}",
            vendorId ?? Guid.NewGuid(),
            vendorName ?? $"Vendor-{Guid.NewGuid():N}",
            contractType,
            validFrom ?? now.AddDays(-1),
            validTo ?? now.AddDays(30));

        contract.SetRemarks($"Remarks-{Guid.NewGuid():N}");
        contract.SetAttachmentUrl($"https://example.com/{Guid.NewGuid():N}");

        switch (status)
        {
            case ContractStatus.Active:
                contract.Activate(now);
                break;
            case ContractStatus.Inactive:
                contract.Activate(now);
                contract.Deactivate();
                break;
            case ContractStatus.Voided:
                contract.Void();
                break;
        }

        await _repository.InsertAsync(contract, autoSave: true);
        return contract;
    }

    private static CreateTransportContractDto BuildCreateInput()
    {
        var code = NextCode("CRT");

        return new CreateTransportContractDto
        {
            Code = code,
            Name = $"Name-{code}",
            VendorId = Guid.NewGuid(),
            VendorName = $"Vendor-{code}",
            ContractType = ContractType.Metal,
            ValidFrom = DateTime.UtcNow.Date,
            ValidTo = DateTime.UtcNow.Date.AddDays(30),
            Remarks = $"Remarks-{code}",
            AttachmentUrl = $"https://example.com/{code}"
        };
    }

    private static UpdateTransportContractDto BuildUpdateInput()
    {
        var code = NextCode("UPD");

        return new UpdateTransportContractDto
        {
            Name = $"Updated-{code}",
            VendorId = Guid.NewGuid(),
            VendorName = $"UpdatedVendor-{code}",
            ContractType = ContractType.Glass,
            ValidFrom = DateTime.UtcNow.Date.AddDays(1),
            ValidTo = DateTime.UtcNow.Date.AddDays(45),
            Remarks = $"UpdatedRemarks-{code}",
            AttachmentUrl = $"https://example.com/updated/{code}"
        };
    }

    private static string NextCode(string prefix)
    {
        var value = Interlocked.Increment(ref _sequence);
        return $"{prefix}-{value:D6}";
    }
}