using ApplicationCore.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data
{
    // A DbContext instance can be used to query from a database and group together changes that will then be written 
    // back to the store as a unit. Using Code First migrations, you can create an SQL Server database
    // from this DbContext. A table in the database is created for each DbSet instance. Including a DbSet of a type on 
    // your context means that it is included in EF Core's model; we usually refer to such a type as an entity. EF Core 
    // can read and write entity instances from/to the database. Each DbContext instance tracks changes made to entities. 
    // These tracked entities in turn drive the changes to the database when SaveChanges is called.
    public class ProductDbContext : ApiAuthorizationDbContext<IdentityUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        public ProductDbContext(
            DbContextOptions<ProductDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }
    }
}