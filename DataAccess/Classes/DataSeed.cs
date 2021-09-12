using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;

namespace DataAccess.Classes
{
    public static class DataSeed
    {
        public static object CustomClaimTypes { get; private set; }

        public static async System.Threading.Tasks.Task SeedAsync(IServiceProvider serviceProvider)
        {
            IServiceScopeFactory scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DataContext>();
               // RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                context.Database.EnsureCreated();
                UnitOfWork unitOfWork = new UnitOfWork(context);
                    unitOfWork.Save();
            }
        }
    }
}
