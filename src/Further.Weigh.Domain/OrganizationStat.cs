using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Further.Weigh.Organizations
{
    // 繼承 AuditedAggregateRoot 可以自動幫你紀錄建立時間、修改時間等資訊
    public class OrganizationStat : AuditedAggregateRoot<Guid>
    {
        public string GroupType { get; set; } // 民國102年底人民團體會員數_選任職員及工作人員數_按團體別分 (團體別)
        public int GroupCount { get; set; } // 團體數
        public int TotalMembers { get; set; } // 年底會社員數_總會社員數_總計_計
        public int StaffTotal { get; set; } // 工作人員數_人_總計_計_統計
        public int VolunteerTotal { get; set; } // 志工_人_計_統計

        // ... (你可以依據 CSV 欄位，把剩下的 男/女、理事長、秘書長等屬性補齊)
    }
}
