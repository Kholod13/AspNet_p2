using Microsoft.EntityFrameworkCore;
using Data.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UseThi.Models;

namespace Data.Data
{
    public class ShopDbContext : IdentityDbContext<User>
    {
        public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }
        public DbSet<UserCryptoCurrency> UserCryptoCurrencies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ShopDbContext()
        {

        }

        public ShopDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var connStr = "Data Source=DESKTOP-JALEVS8;Initial Catalog=web-app-UseThi;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            optionsBuilder.UseSqlServer(connStr)
                  .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CryptoCurrency>()
                .HasKey(cc => cc.Id);  // Вказуємо, що Id є первинним ключем

            modelBuilder.Entity<UserCryptoCurrency>()
                .HasKey(uc => new { uc.UserId, uc.CryptoCurrencyId });

            modelBuilder.Entity<UserCryptoCurrency>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCryptoCurrencies)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserCryptoCurrency>()
                .HasOne(uc => uc.CryptoCurrency)
                .WithMany()
                .HasForeignKey(uc => uc.CryptoCurrencyId);

            modelBuilder.Entity<Product>().Property(x => x.Quantity).HasDefaultValue(1);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)  // Кожен продукт має одну категорію
                .WithMany()  // А категорія може бути пов'язана з багатьма продуктами
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Category>().HasData(new[]
            {
        new Category() { Id = 1, Name = "Electronics" },
        new Category() { Id = 2, Name = "Sport" },
        new Category() { Id = 3, Name = "Fashion" },
        new Category() { Id = 4, Name = "Home & Garden" },
        new Category() { Id = 5, Name = "Transport" },
        new Category() { Id = 6, Name = "Toys & Hobbies" },
        new Category() { Id = 7, Name = "Musical Instruments" },
        new Category() { Id = 8, Name = "Art" },
        new Category() { Id = 9, Name = "Other" }
    });

            modelBuilder.Entity<Product>().HasData(new[]
            {
        new Product() { Id = 1, Name = "iPhone X", CategoryId = 1, Quantity = 3, Status = true, Location = "Rivne", Contact = "sdjfh@gmail.com", Discount = 10, Price = 650, ImageUrl = "https://applecity.com.ua/image/cache/catalog/0iphone/ipohnex/iphone-x-black-1000x1000.png" },
        new Product() { Id = 2, Name = "iPhone X", CategoryId = 1, Status = true, Location = "Rivne", Contact = "sdjfh@gmail.com", Discount = 10, Price = 650, ImageUrl = "https://applecity.com.ua/image/cache/catalog/0iphone/ipohnex/iphone-x-black-1000x1000.png" },
        new Product() { Id = 3, Name = "Nike T-Shirt", CategoryId = 3, Discount = 15, Price = 189, Location = "Prague", Status = true, Contact = "+380439850090", ImageUrl = "https://www.seekpng.com/png/detail/316-3168852_nike-air-logo-t-shirt-nike-t-shirt.png" },
        new Product() { Id = 4, Name = "Samsung S23", CategoryId = 1, Discount = 0, Price = 1200, Location = "Zatec", Status = false, Contact = "+420849953978", ImageUrl = "https://sota.kh.ua/image/cache/data/Samsung-2/samsung-s23-s23plus-blk-01-700x700.webp" },
        new Product() { Id = 5, Name = "MacBook Pro 2019", CategoryId = 1, Discount = 10, Location = "Lviv", Status = true, Price = 700, Contact = "lalalalad@gmail.com", ImageUrl = "https://newtime.ua/image/import/catalog/mac/macbook_pro/MacBook-Pro-16-2019/MacBook-Pro-16-Space-Gray-2019/MacBook-Pro-16-Space-Gray-00.webp" },
        new Product() { Id = 6, Name = "MacBook Pro 2019", CategoryId = 1, Discount = 10, Location = "Lviv", Status = true, Price = 700, Contact = "lalalalad@gmail.com", ImageUrl = "https://newtime.ua/image/import/catalog/mac/macbook_pro/MacBook-Pro-16-2019/MacBook-Pro-16-Space-Gray-2019/MacBook-Pro-16-Space-Gray-00.webp" },
    });
        }

    }
}
