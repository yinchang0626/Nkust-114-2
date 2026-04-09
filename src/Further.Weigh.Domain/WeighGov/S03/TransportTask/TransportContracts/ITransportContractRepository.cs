using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Further.WeighGov.S03.TransportTask.TransportContracts;

public interface ITransportContractRepository : IRepository<TransportContract, Guid>
{
    Task<TransportContract?> FindByCodeAsync(string code);

    Task<TransportContract?> FindActiveContractAsync(Guid vendorId, DateTime date);

    Task<List<TransportContract>> GetByVendorIdAsync(
        Guid vendorId,
        int skipCount = 0,
        int maxResultCount = 10);

    Task<List<TransportContract>> GetByStatusAsync(
        ContractStatus status,
        int skipCount = 0,
        int maxResultCount = 10);
}
