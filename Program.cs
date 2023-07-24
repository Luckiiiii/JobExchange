using JobExchange.Data;
using JobExchange.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddIdentity<StoreUser, IdentityRole>(cfg =>
{
    cfg.User.RequireUniqueEmail = true;
})
        .AddEntityFrameworkStores<JobExchangeContext>()
        .AddSignInManager<SignInManager<StoreUser>>();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextPool<JobExchangeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("JobExchangeContextDb")));
builder.Services.AddDbContext<JobExchangeContext>();
builder.Services.AddScoped<IJobExchangeRepository, JobExchangeRepository>();
builder.Services.AddTransient<JobExchangeSeeder>();
builder.Services.AddSession();

//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

/*using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}*/
if (args.Length == 1 && args[0].ToLower() == "/seed")
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;

        var seeder = serviceProvider.GetRequiredService<JobExchangeSeeder>();
        seeder.SeedAsync().Wait();
    };

}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=App}/{action=Index}/{id?}");

app.Run();
