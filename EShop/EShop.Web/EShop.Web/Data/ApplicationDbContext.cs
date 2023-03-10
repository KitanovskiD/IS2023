using EShop.Web.Models.DomainModels;
using EShop.Web.Models.IdentityModels;
using EShop.Web.Models.Relationship;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<EShopApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<ProductInShoppingCart> ProductInShoppingCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EShopApplicationUser>()
            .HasOne(s => s.UserShoppingCart)
            .WithOne(ad => ad.Owner)
            .HasForeignKey<ShoppingCart>(ad => ad.OwnerId);

            builder.Entity<Product>()
               .Property(z => z.Id)
               .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ProductInShoppingCart>().HasKey(psc => new { psc.ProductId, psc.ShoppingCartId});

            builder.Entity<ProductInShoppingCart>()
                .HasOne(p => p.Product)
                .WithMany(sc => sc.ProductInShoppingCarts)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<ProductInShoppingCart>()
              .HasOne(sc => sc.ShoppingCart)
              .WithMany(sc => sc.ProductInShoppingCarts)
              .HasForeignKey(z => z.ProductId);
        }
    }
}
