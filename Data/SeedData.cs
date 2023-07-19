using JobExchange.Models;
using Microsoft.EntityFrameworkCore;

namespace JobExchange.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new JobExchangeContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<JobExchangeContext>>()))
            {
                if (context.Employers.Any())
                {
                    return;   // DB has been seeded
                }
                context.Employers.AddRange(
                    new Employer
                    {
                        CompanyName = "Test1",
                        AddressOfCompany = "HaNoi",
                        Email = "luckyphuocs@gmail.com",
                        Phone = "0932403242"

                    },
                    new Employer
                    {
                        CompanyName = "Test2",
                        AddressOfCompany = "DaNang",
                        Email = "luckyphuocs@gmail.com",
                        Phone = "0932403242"

                    },
                    new Employer
                    {
                        CompanyName = "Test3",
                        AddressOfCompany = "HCM",
                        Email = "luckyphuocs@gmail.com",
                        Phone = "0932403242"

                    });

                context.SaveChanges();
            }
        }
    }
}
