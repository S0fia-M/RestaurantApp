using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data.Models.Domain;

namespace RestaurantApp.Data.DataAccess
{
    public class ApplicationDatabase : DbContext
    {
        public ApplicationDatabase()
        {
            Database.EnsureCreated();
        }

        public ApplicationDatabase(DbContextOptions<ApplicationDatabase> options)
            : base(options) 
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Branch> Branches { get; set; }

        public virtual DbSet<Composition> Compositions { get; set; }

        public virtual DbSet<Employer> Employers { get; set; }

        public virtual DbSet<HumanResourcesDepartments> HumanResourcesDepartments { get; set; }

        public virtual DbSet<MenuItem> MenuItems { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<PassportData> PassportDatas { get; set; }

        public virtual DbSet<Position> Positions { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Shift> Shifts { get; set; }

        public virtual DbSet<WarehouseItem> WarehouseItems { get; set; }

        public virtual DbSet<OrdersMenuItems> OrdersMenuItems { get; set; }

        public virtual DbSet<WarehouseItemsProducts> WarehouseItemsProducts { get; set; }

        public virtual DbSet<ShiftsOrders> ShiftsOrders { get; set; }

        public virtual DbSet<MenuItemsCompositions> MenuItemsCompositions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server =.\SQLEXPRESS;Database=RestaurantAppDomainDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrdersMenuItems>().HasKey(x => new {x.OrderId , x.MenuItemId });

            modelBuilder.Entity<OrdersMenuItems>()
                .HasOne<Order>(sc => sc.Order)
                .WithMany(s => s.OrdersMenuItems)
                .HasForeignKey(sc => sc.OrderId);


            modelBuilder.Entity<OrdersMenuItems>()
                .HasOne<MenuItem>(sc => sc.MenuItem)
                .WithMany(s => s.OrdersMenuItems)
                .HasForeignKey(sc => sc.MenuItemId);

            modelBuilder.Entity<WarehouseItemsProducts>().HasKey(x => new { x.WarehouseItemId, x.ProductId });

            modelBuilder.Entity<WarehouseItemsProducts>()
                .HasOne<WarehouseItem>(sc => sc.WarehouseItem)
                .WithMany(s => s.WarehouseItemsProducts)
                .HasForeignKey(sc => sc.WarehouseItemId);


            modelBuilder.Entity<WarehouseItemsProducts>()
                .HasOne<Product>(sc => sc.Product)
                .WithMany(s => s.WarehouseItemsProducts)
                .HasForeignKey(sc => sc.ProductId);

            modelBuilder.Entity<ShiftsOrders>().HasKey(x => new { x.OrderId, x.ShiftId });

            modelBuilder.Entity<ShiftsOrders>()
                .HasOne<Order>(sc => sc.Order)
                .WithMany(s => s.ShiftsOrders)
                .HasForeignKey(sc => sc.OrderId);


            modelBuilder.Entity<ShiftsOrders>()
                   .HasOne<Shift>(sc => sc.Shift)
                   .WithMany(s => s.ShiftsOrders)
                   .HasForeignKey(sc => sc.ShiftId);

            modelBuilder.Entity<MenuItemsCompositions>().HasKey(x => new { x.MenuItemId, x.CompositionId });

            modelBuilder.Entity<MenuItemsCompositions>()
                .HasOne<MenuItem>(sc => sc.MenuItem)
                .WithMany(s => s.MenuItemsCompositions)
                .HasForeignKey(sc => sc.MenuItemId);


            modelBuilder.Entity<MenuItemsCompositions>()
                   .HasOne<Composition>(sc => sc.Composition)
                   .WithMany(s => s.MenuItemsCompositions)
                   .HasForeignKey(sc => sc.CompositionId);
        }
    }
}
