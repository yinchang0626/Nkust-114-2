using Further.Weigh.EntityFrameworkCore.WeighGov.S03.TransportTask.TransportContracts;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Further.Weigh.EntityFrameworkCore;

public static class WeighGovS03TransportTaskModelBuilderExtensions
{
    public static void ConfigureWeighGovS03TransportTask(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ApplyConfiguration(new TransportContractConfiguration());
    }
}
