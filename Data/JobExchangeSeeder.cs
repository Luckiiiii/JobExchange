using JobExchange.Models;
using Microsoft.AspNetCore.Identity;

namespace JobExchange.Data
{
    public class JobExchangeSeeder
    {
        private readonly JobExchangeContext _context;
        private readonly UserManager<StoreUser> _userManager;
        private readonly IServiceProvider _serviceProvider;

        //private readonly IWebHostEnvironment _hosting;

        public JobExchangeSeeder(JobExchangeContext context, IWebHostEnvironment hosting, UserManager<StoreUser> userManager, IServiceProvider serviceProvider)
        {
            _context = context;
            //_hosting = hosting;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
        }
        public async Task SeedAsync()
        {
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var adminRoleExists = await roleManager.RoleExistsAsync("admin");
            if (!adminRoleExists)
            {
                var adminRole = new IdentityRole("admin");
                await roleManager.CreateAsync(adminRole);
            }
            _context.Database.EnsureCreated();

            StoreUser Admin = await _userManager.FindByEmailAsync("Admin@gmail.com");
            if (Admin == null)
            {
                Admin = new StoreUser()
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin@gmail.com"
                };
                var result = await _userManager.CreateAsync(Admin, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    //throw new InvalidOperationException("Could not create new user in seeder");
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Could not create new user in seeder. Errors: {errors}");
                }

            }


            var isInRole = await _userManager.IsInRoleAsync(Admin, "admin");
            if (!isInRole)
            {
                await _userManager.AddToRoleAsync(Admin, "admin");
            }

            StoreUser Admin2 = await _userManager.FindByEmailAsync("Admin2@gmail.com");
            if (Admin2 == null)
            {
                Admin2 = new StoreUser()
                {
                    FirstName = "Admin2",
                    LastName = "Admin2",
                    Email = "Admin2@gmail.com",
                    UserName = "Admin2@gmail.com"
                };
                var result = await _userManager.CreateAsync(Admin2, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    //throw new InvalidOperationException("Could not create new user in seeder");
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Could not create new user in seeder. Errors: {errors}");
                }

            }


            var isInRole2 = await _userManager.IsInRoleAsync(Admin2, "admin");
            if (!isInRole2)
            {
                await _userManager.AddToRoleAsync(Admin2, "admin");
            }

            StoreUser user = await _userManager.FindByEmailAsync("luckyphuocs@gmail.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Lucky",
                    LastName = "Phuoc",
                    Email = "luckyphuocs@gmail.com",
                    UserName = "luckyphuocs@gmail.com"
                };
                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    //throw new InvalidOperationException("Could not create new user in seeder");
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Could not create new user in seeder. Errors: {errors}");
                }
            }
            StoreUser user1 = await _userManager.FindByEmailAsync("user1@gmail.com");
            if (user1 == null)
            {
                user1 = new StoreUser()
                {
                    FirstName = "user1",
                    LastName = "Phuoc",
                    Email = "user1@gmail.com",
                    UserName = "user1@gmail.com"
                };
                var result = await _userManager.CreateAsync(user1, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    //throw new InvalidOperationException("Could not create new user in seeder");
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Could not create new user in seeder. Errors: {errors}");
                }
            }

            
            StoreUser user3 = await _userManager.FindByEmailAsync("user3@gmail.com");
            if (user3 == null)
            {
                user3 = new StoreUser()
                {
                    FirstName = "user3",
                    LastName = "Phuoc",
                    Email = "user3@gmail.com",
                    UserName = "user3@gmail.com"
                };
                var result = await _userManager.CreateAsync(user3, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    //throw new InvalidOperationException("Could not create new user in seeder");
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Could not create new user in seeder. Errors: {errors}");
                }
            }

            if (!_context.Employers.Any() && user != null)
            {

                _context.Employers.AddRange(
                new Employer
                {
                    CompanyName = "Test1",
                    AddressOfCompany = "HaNoi",
                    Email = "luckyphuocs@gmail.com",
                    Phone = "0932403242",
                    User = user

                });

                _context.SaveChanges();
            }
            if (!_context.TypeJobs.Any())
            {
                _context.TypeJobs.AddRange(
                    new TypeJob
                    {
                        NameJob = "Information Techology"
                    },
                    new TypeJob
                    {
                        NameJob = "Business"
                    },
                    new TypeJob
                    {
                        NameJob = "Law"
                    },
                    new TypeJob
                    {
                        NameJob = "Design"
                    },
                    new TypeJob
                    {
                        NameJob = "education industry"
                    });
                _context.SaveChanges();
            }

        }

    }
}
