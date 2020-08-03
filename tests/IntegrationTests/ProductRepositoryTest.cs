using Ganges.ApplicationCore.Interfaces;
using Ganges.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ganges.IntegrationTests
{
    [TestClass]
    public class ProductRepositoryTest
    {

        private readonly GangesDbContext _context;
        private readonly IProductRepository _productRepository;

        public ProductRepositoryTest()
        {
            var dbOptions = new DbContextOptionsBuilder<GangesDbContext>()
                .UseInMemoryDatabase(databaseName: "TestCatalog")
                .Options;
            _context = new GangesDbContext(dbOptions);
            _productRepository = new ProductRepository(_context);
        }


        [TestMethod]
        public void Test()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
