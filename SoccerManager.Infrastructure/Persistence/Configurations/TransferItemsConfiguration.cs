using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerManager.Application.Entities;

namespace SoccerManager.Infrastructure.Persistence.Configurations;

public class TransferItemsConfiguration : IEntityTypeConfiguration<TransferItem>
{
    public void Configure(EntityTypeBuilder<TransferItem> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Price)
            .IsRequired();

        builder.HasOne(t => t.SoccerPlayer)
            .WithOne(p => p.TransferItem)
            .HasForeignKey<SoccerPlayer>(p => p.TransferItemId);
    }
}