using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracklet.Domain.Entities;

namespace Tracklet.Infra.Data.Configurations;

public class VisitorConfiguration : IEntityTypeConfiguration<Visitor>
{
    public void Configure(EntityTypeBuilder<Visitor> builder)
    {
        builder.ToTable("TB_VISITORS");
        
        builder.HasKey(v => v.Id).HasName("PK_VISITORS");;
        
        builder.Property(v => v.Id)
            .HasColumnName("ID")
            .HasMaxLength(64);
        
        builder.Property(v => v.FirstSeen)
            .HasColumnName("FIRST_SEEN")
            .IsRequired();
        
        builder.Property(v => v.LastSeen)
            .HasColumnName("LAST_SEEN")
            .IsRequired();
        
        builder.Property(v => v.CreatedAt)
            .HasColumnName("CREATED_AT")
            .IsRequired()
            .HasDefaultValueSql("NOW()");
    }
}