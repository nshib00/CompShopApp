using System.Data.Entity;

namespace DAL.Entity
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=compshop")
        {
            Database.SetInitializer<AppDbContext>(null);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<Product>()
                .HasOptional(p => p.Cart)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CartId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasOptional(p => p.OrderDetail)
                .WithMany()
                .HasForeignKey(p => p.OrderDetailId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}