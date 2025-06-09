using Domain.Contracts;
using Domain.Models;
using Domain.Models.Identity;
using Domain.Models.OrderModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
using System.Linq.Expressions;
using System.Text.Json;
namespace Persistence
{
    public class DbInitializer(StoreDbContext context,StoreIdentityDbContext identityContext,UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager) : IDbInitializer
    {
        private readonly StoreDbContext _context = context;
        private readonly StoreIdentityDbContext _identityContext = identityContext;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

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
                if (!_context.DeliveryMethods.Any())
                {
                    // 1. Read All Data from types.json as string
                    var deliveryMethods = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\delivery.json");
                    // 2. Transform string to C# object [List<ProductType>] (deserialization)
                    var deliveries = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethods);
                    // 3. Add to Database
                    if (deliveries is not null && deliveries.Any())
                    {
                        await _context.DeliveryMethods.AddRangeAsync(deliveries);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task InitializeIdentityAsync()
        {
            // Create Database if it isn't existed
            if (_identityContext.Database.GetPendingMigrations().Any())
            {
                await _identityContext.Database.MigrateAsync();
            }
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "SuperAdmin"
                });
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Admin"
                });
            }
            if (!_userManager.Users.Any())
            {
                var superAdmin = new AppUser()
                {
                    DisplayName = "SuperAdmin",
                    Email = "superadmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789"

                };
                var admin = new AppUser()
                {
                    DisplayName = "Admin",
                    Email = "admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "01234567891"

                };

                await _userManager.CreateAsync(admin, "P@ssW0rd");
                await _userManager.CreateAsync(superAdmin, "P@ssW0rd");

                await _userManager.AddToRoleAsync(admin, "Admin");
                await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
            }

        }
    }
}
