using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerManager.Application.Entities;

namespace SoccerManager.Infrastructure.Persistence.Configurations;

public class SoccerPlayerConfiguration : IEntityTypeConfiguration<SoccerPlayer>
{
    public void Configure(EntityTypeBuilder<SoccerPlayer> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.FirstName)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(p => p.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Country)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(p => p.Position)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Age)
            .IsRequired();
        
        builder.Property(p => p.MarketValue)
            .IsRequired();
        
        builder.HasOne(p => p.SoccerTeam)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.SoccerTeamId)
            .IsRequired();

    }
}