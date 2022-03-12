using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerManager.Application.Entities;

namespace SoccerManager.Infrastructure.Persistence.Configurations;

public class SoccerTeamConfiguration : IEntityTypeConfiguration<SoccerTeam>
{
    public void Configure(EntityTypeBuilder<SoccerTeam> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Name)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(t => t.Country)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(t => t.Budget)
            .IsRequired();
        
        builder.HasMany(t => t.Players)
            .WithOne(p => p.SoccerTeam)
            .HasForeignKey(p => p.SoccerTeamId)
            .IsRequired();

        builder.HasOne(t => t.User)
            .WithOne(u => u.SoccerTeam)
            .HasForeignKey<SoccerTeam>(t => t.UserId)
            .IsRequired();
    }
}