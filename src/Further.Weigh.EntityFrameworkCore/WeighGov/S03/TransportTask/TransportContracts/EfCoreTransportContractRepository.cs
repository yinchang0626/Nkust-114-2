using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Further.Weigh.EntityFrameworkCore;
using Further.WeighGov.S03.TransportTask.TransportContracts;
using Further.WeighGov.S03.TransportTask.TransportContracts.Specifications;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Further.Weigh.EntityFrameworkCore.WeighGov.S03.TransportTask.TransportContracts;

public class EfCoreTransportContractRepository :
    EfCoreRepository<WeighDbContext, TransportContract, Guid>,
    ITransportContractRepository
{
    public EfCoreTransportContractRepository(
        IDbContextProvider<WeighDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<TransportContract?> FindByCodeAsync(string code)
    {
        return await (await GetQueryableAsync())
            .FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<TransportContract?> FindActiveContractAsync(Guid vendorId, DateTime date)
    {
        var spec = new ActiveContractSpecification(vendorId, date);
        return await (await GetQueryableAsync())
            .Where(spec.ToExpression())
            .FirstOrDefaultAsync();
    }

    public async Task<List<TransportContract>> GetByVendorIdAsync(
        Guid vendorId,
        int skipCount = 0,
        int maxResultCount = 10)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.VendorId == vendorId)
            .OrderByDescending(x => x.CreationTime)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<List<TransportContract>> GetByStatusAsync(
        ContractStatus status,
        int skipCount = 0,
        int maxResultCount = 10)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.Status == status)
            .OrderByDescending(x => x.CreationTime)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }
}
