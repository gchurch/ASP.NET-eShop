using Ganges.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ganges.IntegrationTests.Repositories
{
    [TestClass]
    public class ProductRepositoryTest
    {

        private DbContextOptions<GangesDbContext> _dbOptions;

        public ProductRepositoryTest()
        {
            _dbOptions = new DbContextOptionsBuilder<GangesDbContext>()
                .UseInMemoryDatabase(databaseName: "IntegrationTestingDatabase")
                .Options;
        }

        [TestMethod]
        public void ()
        {
            Assert.AreEqual(1,1);
        }
    }
}
