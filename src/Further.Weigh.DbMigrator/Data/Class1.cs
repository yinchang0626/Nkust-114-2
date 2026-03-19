using Further.WeighGov.S03.TransportTask.TransportContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace Further.Weigh.DbMigrator.Data
{
    public class DataImportDataSeedContributor : IDataSeedContributor,Volo.Abp.DependencyInjection.ITransientDependency
    {
        private readonly ITransportContractRepository transportContractRepository;

        public DataImportDataSeedContributor(
            Further.WeighGov.S03.TransportTask.TransportContracts.ITransportContractRepository transportContractRepository
            )
        {
            this.transportContractRepository = transportContractRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {


            Further.WeighGov.S03.TransportTask.TransportContracts.TransportContract transportContract = TransportContract.Create(Guid.NewGuid(), "", "", Guid.NewGuid(), "", ContractType.Glass, DateTime.Now, DateTime.Now);


            await transportContractRepository.InsertAsync(transportContract);

        }
    }
}
