﻿using ApplicationCore.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data
{
    public class ProductDbContext : ApiAuthorizationDbContext<IdentityUser>
    {
        public DbSet<Product> Products { get; set; }

        public ProductDbContext(
            DbContextOptions<ProductDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }
    }
}