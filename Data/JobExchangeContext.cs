using JobExchange.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobExchange.Data
{
    public class JobExchangeContext : IdentityDbContext
    {
        /*private readonly IConfiguration _config;

        public JobExchangeContext(IConfiguration configuration) 
        {
            _config = configuration;
        }*/
        public JobExchangeContext(DbContextOptions<JobExchangeContext> options)
            : base(options)
        {

        }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<JobInfo> JobInfos { get; set; }
        public DbSet<TypeJob> TypeJobs{ get; set; }

        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Cấu hình mối quan hệ một-một giữa StoreUser và Employer
            builder.Entity<Employer>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employers)
                .HasForeignKey<Employer>(e => e.User);
        }*/
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:JobExchangeContextDb"]);
        }*/
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employer>().HasData(new Employer()
            {
                Id = 1,
                CompanyName = "Test",
                Address ="HaNoi",
                Email ="luckyphuocs@gmail.com",
                Phone ="0932403242"    
                
            });
        }*/
    }
}
