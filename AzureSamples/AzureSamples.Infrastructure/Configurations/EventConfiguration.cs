using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AzureSamples.Infrastructure.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<EventEntity>
    {
        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.ToTable("Event");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Property(e => e.UserId)
                .IsRequired();
            builder.Property(e => e.EventType)
                .IsRequired();
            builder.Property(e => e.CreatedAtUtc)
                .IsRequired();

            builder.HasIndex(x => x.CreatedAtUtc);
            builder.HasIndex(x => x.UserId);
        }
    }
}