using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SERVER_store.Models.Repository
{
    public class EFDbContext : IdentityDbContext<User>
    {
        public EFDbContext(DbContextOptions<EFDbContext> options)
          : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Category_has_Category> Category_Has_Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Atribute> Atributes { get; set; }
        public DbSet<Atribute_in_product> Atribute_In_Products { get; set; }
        public DbSet<Product_in_cart> Product_In_Carts { get; set; }
        public DbSet<Product_in_order> Product_In_Orders { get; set; }

    }
}
