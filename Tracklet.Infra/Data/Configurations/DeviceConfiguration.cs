using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracklet.Domain.Entities;

namespace Tracklet.Infra.Data.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("TB_DEVICES");
        
        builder.HasKey(d => d.Id)
            .HasName("PK_DEVICES");
        
        builder.Property(d => d.Id)
            .HasColumnName("ID")
            .HasMaxLength(64);

        builder.Property(d => d.UserAgent)
            .HasColumnName("USER_AGENT")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.ScreenWidth)
            .HasColumnName("SCREEN_WIDTH")
            .IsRequired();

        builder.Property(d => d.ScreenHeight)
            .HasColumnName("SCREEN_HEIGHT")
            .IsRequired();

        builder.Property(d => d.OperatingSystem)
            .HasColumnName("OS")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.CreatedAt)
            .HasColumnName("CREATED_AT")
            .IsRequired()
            .HasDefaultValueSql("NOW()");
    }
}