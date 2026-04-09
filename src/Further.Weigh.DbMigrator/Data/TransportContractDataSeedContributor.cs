using System;
using System.Linq;
using System.Threading.Tasks;
using Further.WeighGov.S03.TransportTask.TransportContracts;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Further.Weigh.DbMigrator.Data;

/// <summary>
/// 負責 TransportContract（清運合約）的初始測試資料植入。
/// 採冪等設計：依合約代碼判斷是否已存在，避免重複插入。
/// </summary>
public class TransportContractDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ITransportContractRepository _transportContractRepository;

    // ── 固定廠商 ID（測試用）──────────────────────────────────────────────
    private static readonly Guid VendorIdYongShun = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000001");
    private static readonly Guid VendorIdXinGang = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000002");
    private static readonly Guid VendorIdLvHuan = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000003");

    public TransportContractDataSeedContributor(
        ITransportContractRepository transportContractRepository)
    {
        _transportContractRepository = transportContractRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        // ── Glass 類型合約：2026 年有效（Active）─────────────────────────
        await SeedContractAsync(
            id: Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001"),
            code: "TC-GLASS-2026-001",
            name: "玻璃資源回收清運合約 2026",
            vendorId: VendorIdYongShun,
            vendorName: "永順環保清潔有限公司",
            contractType: ContractType.Glass,
            validFrom: new DateTime(2026, 1, 1),
            validTo: new DateTime(2026, 12, 31),
            activate: true,
            remarks: "玻璃瓶罐類資源物清運，每週三次定期收運。"
        );

        // ── Metal 類型合約：2026 年有效（Active）─────────────────────────
        await SeedContractAsync(
            id: Guid.Parse("a1b2c3d4-0001-0001-0001-000000000002"),
            code: "TC-METAL-2026-001",
            name: "金屬廢料回收清運合約 2026",
            vendorId: VendorIdXinGang,
            vendorName: "鑫鋼資源回收股份有限公司",
            contractType: ContractType.Metal,
            validFrom: new DateTime(2026, 1, 1),
            validTo: new DateTime(2026, 12, 31),
            activate: true,
            remarks: "鐵鋁銅等金屬廢料清運，依磅單重量計費。"
        );

        // ── General 類型合約：2026 年有效（Active）───────────────────────
        await SeedContractAsync(
            id: Guid.Parse("a1b2c3d4-0001-0001-0001-000000000003"),
            code: "TC-GEN-2026-001",
            name: "一般廢棄物清運合約 2026",
            vendorId: VendorIdLvHuan,
            vendorName: "綠環清潔服務有限公司",
            contractType: ContractType.General,
            validFrom: new DateTime(2026, 1, 1),
            validTo: new DateTime(2026, 12, 31),
            activate: true,
            remarks: "廠區一般事業廢棄物清運，含紙類、塑膠等混合廢棄物。"
        );

        // ── Glass 類型合約：2027 年草稿（Draft，尚未啟用）───────────────
        await SeedContractAsync(
            id: Guid.Parse("a1b2c3d4-0001-0001-0001-000000000004"),
            code: "TC-GLASS-2027-001",
            name: "玻璃資源回收清運合約 2027（草稿）",
            vendorId: VendorIdYongShun,
            vendorName: "永順環保清潔有限公司",
            contractType: ContractType.Glass,
            validFrom: new DateTime(2027, 1, 1),
            validTo: new DateTime(2027, 12, 31),
            activate: false,
            remarks: "2027 年度合約草稿，待主管審核後啟用。"
        );

        // ── Metal 類型合約：2025 年已停用（Inactive）─────────────────────
        await SeedContractAsync(
            id: Guid.Parse("a1b2c3d4-0001-0001-0001-000000000005"),
            code: "TC-METAL-2025-001",
            name: "金屬廢料回收清運合約 2025",
            vendorId: VendorIdXinGang,
            vendorName: "鑫鋼資源回收股份有限公司",
            contractType: ContractType.Metal,
            validFrom: new DateTime(2025, 1, 1),
            validTo: new DateTime(2025, 12, 31),
            activate: true,
            deactivate: true,
            remarks: "2025 年度合約，已於年末停用。"
        );
    }

    private async Task SeedContractAsync(
        Guid id,
        string code,
        string name,
        Guid vendorId,
        string vendorName,
        ContractType contractType,
        DateTime validFrom,
        DateTime validTo,
        bool activate = false,
        bool deactivate = false,
        string? remarks = null)
    {
        // 冪等：依合約代碼確認是否已植入
        if (await _transportContractRepository.FindByCodeAsync(code) is not null)
        {
            return;
        }

        var contract = TransportContract.Create(
            id, code, name, vendorId, vendorName, contractType, validFrom, validTo);

        if (!string.IsNullOrWhiteSpace(remarks))
        {
            contract.SetRemarks(remarks);
        }

        if (activate)
        {
            contract.Activate(validFrom);   // 以有效起日作為啟用時間點
        }

        if (deactivate)
        {
            contract.Deactivate();
        }

        await _transportContractRepository.InsertAsync(contract);
    }
}
