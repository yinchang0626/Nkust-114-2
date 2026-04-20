using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Further.Weigh.Organizations
{
    public class OrganizationStatDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<OrganizationStat, Guid> _repository;

        public OrganizationStatDataSeederContributor(IRepository<OrganizationStat, Guid> repository)
        {
            _repository = repository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // 如果資料庫已經有資料就跳過
            if (await _repository.GetCountAsync() > 0) return;

            // 讀取你的 CSV 檔案 (記得把 CSV 複製到 DbMigrator 專案的輸出目錄下)
            var lines = File.ReadAllLines("民國102年底人民團體會員數、選任職員及工作人員數_按團體別分-145042.csv");

            // 跳過第一行標題，逐行讀取
            for (int i = 1; i < lines.Length; i++)
            {
                var columns = lines[i].Split(',');
                if (columns.Length < 35) continue;

                await _repository.InsertAsync(new OrganizationStat
                {
                    GroupType = columns[0],
                    GroupCount = int.TryParse(columns[1], out var gc) ? gc : 0,
                    TotalMembers = int.TryParse(columns[2], out var tm) ? tm : 0,
                    StaffTotal = int.TryParse(columns[20], out var st) ? st : 0,
                    VolunteerTotal = int.TryParse(columns[32], out var vt) ? vt : 0
                }, autoSave: true);
            }
        }
    }
}
