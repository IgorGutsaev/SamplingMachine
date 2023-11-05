using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPT.Vending.Domains.Products.Infrastructure.Entities;

namespace MPT.Vending.Domains.Products.Infrastructure.Configurations
{
    public class PictureConfiguration : IEntityTypeConfiguration<PictureEntity>
    {
        public void Configure(EntityTypeBuilder<PictureEntity> builder) {
            builder.ToTable("Picture");
            builder.HasKey(x => x.Id);
        }
    }
}