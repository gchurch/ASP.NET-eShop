using ApplicationCore.Models;
using ApplicationCore.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

// The repository pattern: https://deviq.com/repository-pattern/

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                return await TryToGetAllProductsAsync();
            }
            catch (SqlException e)
            {
                LogError(e.Message);
                return new List<Product>();
            }
        }

        public async Task<IEnumerable<Product>> TryToGetAllProductsAsync()
        {
            var query = from product in _context.Products select product;
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                return await TryToGetProductByIdAsync(id);
            }
            catch (InvalidOperationException e)
            {
                LogError(e.Message);
                return new Product();
            }
            catch (SqlException e)
            {
                LogError(e.Message);
                return new Product();
            }
        }

        public async Task<Product> TryToGetProductByIdAsync(int id)
        {
            var query = from product in _context.Products where product.ProductId == id select product;
            return await query.AsNoTracking().SingleAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                await TryToAddProductAsync(product);
            }
            catch (SqlException e)
            {
                LogError(e.Message);
            }
        }

        public async Task TryToAddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductByIdAsync(int id)
        {
            try
            {
                await TryToDeleteProductByIdAsync(id);
            }
            catch (SqlException e)
            {
                LogError(e.Message);
            }
        }

        public async Task TryToDeleteProductByIdAsync(int id)
        {
            Product product = new Product()
            {
                ProductId = id
            };
            _context.Products.Attach(product);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                await TryToUpdateProductAsync(product);
            }
            catch (SqlException e)
            {
                LogError(e.Message);
            }
        }

        public async Task TryToUpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public void LogError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
        }
    }
}
