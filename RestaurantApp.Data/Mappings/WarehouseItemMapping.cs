//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using RestaurantApp.Data.Models.Domain;

//namespace RestaurantApp.Data.Mappings
//{
//    public class WarehouseItemMapping : IEntityTypeConfiguration<WarehouseItem>
//    {
//        public void Configure(EntityTypeBuilder<WarehouseItem> builder)
//        {
//            builder
//                .ToTable("WarehouseItems", "dbo")
//                .HasKey(x => x.Id);

//            builder.HasMany(x => x.Products)
//                .WithOne(x => x.WarehouseItem)
//                .HasForeignKey(x => x.WarehouseItemId);  
//        }
//    }
//}
