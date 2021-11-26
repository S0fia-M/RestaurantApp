//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using RestaurantApp.Data.Models.Domain;

//namespace RestaurantApp.Data.Mappings
//{
//    public class EmployerMapping : IEntityTypeConfiguration<Employer>
//    {
//        public void Configure(EntityTypeBuilder<Employer> builder)
//        {
//            builder.ToTable("Employers", "dbo")
//                .HasKey(x => x.Id);

//            builder
//                .HasOne(x => x.PassportData)
//                .WithOne(x => x.Employer);
//        }
//    }
//}
