using Further.Weigh;
using Further.WeighGov.S03.TransportTask.TransportContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Further.Weigh.EntityFrameworkCore.WeighGov.S03.TransportTask.TransportContracts;

public class TransportContractConfiguration : IEntityTypeConfiguration<TransportContract>
{
    public void Configure(EntityTypeBuilder<TransportContract> builder)
    {
        builder.ToTable(WeighConsts.DbTablePrefix + "TransportContracts", WeighConsts.DbSchema);
        builder.ConfigureByConvention();

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(TransportContractConsts.MaxCodeLength);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(TransportContractConsts.MaxNameLength);

        builder.Property(x => x.VendorName)
            .IsRequired()
            .HasMaxLength(TransportContractConsts.MaxVendorNameLength);

        builder.Property(x => x.ContractType)
            .IsRequired();

        builder.Property(x => x.ValidFrom)
            .IsRequired();

        builder.Property(x => x.ValidTo)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.AttachmentUrl)
            .HasMaxLength(TransportContractConsts.MaxAttachmentUrlLength);

        builder.Property(x => x.Remarks)
            .HasMaxLength(TransportContractConsts.MaxRemarksLength);

        // Indexes
        builder.HasIndex(x => new { x.TenantId, x.Code });
        builder.HasIndex(x => new { x.TenantId, x.VendorId });
        builder.HasIndex(x => new { x.TenantId, x.Status });
        builder.HasIndex(x => new { x.TenantId, x.ValidFrom, x.ValidTo });
    }
}
