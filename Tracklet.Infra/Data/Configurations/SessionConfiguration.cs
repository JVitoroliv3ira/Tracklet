using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracklet.Domain.Entities;

namespace Tracklet.Infra.Data.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("TB_SESSIONS");
        
        builder.HasKey(s => s.Id)
            .HasName("PK_SESSIONS");
        
        builder.Property(s => s.Id)
            .HasColumnName("ID")
            .HasMaxLength(64);

        builder.Property(s => s.VisitorId)
            .HasColumnName("VISITOR_ID")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(s => s.DeviceId)
            .HasColumnName("DEVICE_ID")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(s => s.StartTime)
            .HasColumnName("START_TIME")
            .IsRequired();

        builder.Property(s => s.EndTime)
            .HasColumnName("END_TIME")
            .IsRequired();

        builder.Property(s => s.Duration)
            .HasColumnName("DURATION")
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .HasColumnName("CREATED_AT")
            .IsRequired()
            .HasDefaultValueSql("NOW()");
        
        builder.HasOne(s => s.Visitor)
            .WithMany()
            .HasForeignKey(s => s.VisitorId)
            .HasConstraintName("FK_SESSIONS_VISITORS")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Device)
            .WithMany()
            .HasForeignKey(s => s.DeviceId)
            .HasConstraintName("FK_SESSIONS_DEVICES")
            .OnDelete(DeleteBehavior.Restrict);
    }
}