//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using RestaurantApp.Data.Models.Domain;

//namespace RestaurantApp.Data.Mappings
//{
//    public class ShiftOrdersMapping : IEntityTypeConfiguration<ShiftOrders>
//    {
//        public void Configure(EntityTypeBuilder<ShiftOrders> builder)
//        {
//            builder
//            .ToTable("ShiftOrders", "dbo")
//                .HasKey(x => x.Id);

//            builder.HasMany(x => x.Orders)
//                .WithOne(x => x.ShiftOrders)
//                .HasForeignKey(x => x.ShiftOrdersId);

//            builder.HasOne(x => x.Employer)
//                .WithMany(x => x.ShiftOrders)
//                .HasForeignKey(x => x.EmployerId);
//        }
//    }
//}
