﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ganges.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ganges.FunctionalTests
{
    class CustomWebApplicationFactory<TStartup> 
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<GangesDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<GangesDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName: "FuctionalTestingDatabase");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<GangesDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure that the database is a fresh version ready for testing
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    db.SaveChanges();

                    try
                    {
                        SeedData.Initialize(scopedServices);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occured seeding the DB.");
                    }
                }
            });
        }
    }
}