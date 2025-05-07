using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;
using System.Text.Json;
namespace Persistence
{
    public class DbInitializer(StoreDbContext context) : IDbInitializer
    {
        private readonly StoreDbContext _context = context;

        public async Task InitializeAsync()
        {
            try {
                // create db if it doesn't exist & apply any pending migrations
                if (_context.Database.GetPendingMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }
                // Data Seeding
                // Seeding ProductTypes
                if (!_context.ProductTypes.Any())
                {
                    // 1. Read All Data from types.json as string
                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");
                    // 2. Transform string to C# object [List<ProductType>] (deserialization)
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    // 3. Add to Database
                    if (types is not null && types.Any())
                    {
                        await _context.ProductTypes.AddRangeAsync(types);
                        await _context.SaveChangesAsync();
                    }
                }
                // Seeding ProductBrands
                if (!_context.ProductBrands.Any())
                {
                    // 1. Read All Data from types.json as string
                    var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");
                    // 2. Transform string to C# object [List<ProductType>] (deserialization)
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    // 3. Add to Database
                    if (brands is not null && brands.Any())
                    {
                        await _context.ProductBrands.AddRangeAsync(brands);
                        await _context.SaveChangesAsync();
                    }
                }
                // Seeding Products
                if (!_context.Products.Any())
                {
                    // 1. Read All Data from types.json as string
                    var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");
                    // 2. Transform string to C# object [List<ProductType>] (deserialization)
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    // 3. Add to Database
                    if (products is not null && products.Any())
                    {
                        await _context.Products.AddRangeAsync(products);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
