using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Further.Weigh.Organizations
{
    // 定義這支 API 擁有標準的 CRUD 功能
    public interface IOrganizationStatAppService :
        ICrudAppService<
            OrganizationStatDto,
            Guid,
            PagedAndSortedResultRequestDto>
    {
    }
}