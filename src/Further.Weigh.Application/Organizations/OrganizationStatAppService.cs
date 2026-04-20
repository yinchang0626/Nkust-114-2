using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
// 1. 補上這一行，讓它去抓你寫在 Domain 與 Contracts 層的檔案
using Further.Weigh.Organizations;

// 2. 確保這裡的命名空間是 Further.Weigh.Organizations
namespace Further.Weigh.Organizations
{
    public class OrganizationStatAppService :
        CrudAppService<
            OrganizationStat,
            OrganizationStatDto,
            Guid,
            PagedAndSortedResultRequestDto>,
        IOrganizationStatAppService
    {
        public OrganizationStatAppService(IRepository<OrganizationStat, Guid> repository)
            : base(repository)
        {
        }
    }
}