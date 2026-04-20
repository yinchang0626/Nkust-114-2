using System;
using Volo.Abp.Application.Dtos;

namespace Further.Weigh.Organizations
{
    // 這個是用來傳給前端的資料格式，繼承 AuditedEntityDto 會自動包含 Id 與建立時間
    public class OrganizationStatDto : AuditedEntityDto<Guid>
    {
        public string GroupType { get; set; }
        public int GroupCount { get; set; }
        public int TotalMembers { get; set; }
        public int StaffTotal { get; set; }
        public int VolunteerTotal { get; set; }
    }
}
