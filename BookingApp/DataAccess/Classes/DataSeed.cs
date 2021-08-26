using Data.Models;
using Data.Models.Committees;
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
                UserManager<User> _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
               // RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                context.Database.EnsureCreated();
                UnitOfWork unitOfWork = new UnitOfWork(context);

                #region Seeding

                #region USER INFO
                // User Info
                //string userName = "SuperAdmin";
                //string firstName = "Super";
                //string lastName = "Admin";
                string email = "superadmin@admin.com";
                string password = "Qwaszx123$";

                //string role = "SuperAdmins";
                if (await _userManager.FindByNameAsync(email) == null)
                {
                    // Create SuperAdmins role if it doesn't exist
                    //if (await roleManager.FindByNameAsync(role) == null)
                    //{
                    //    await roleManager.CreateAsync(new IdentityRole(role));
                    //}

                    // Create user account if it doesn't exist
                    User user = new User
                    {
                        UserName = email,
                        Email = email,
                        Name = "Super Admin",
                        //extended properties
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        TwoFactorEnabled = false,
                        LockoutEnabled = false,
                        AccessFailedCount = 0,
                        IsAdmin = true,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                        Position = "",
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, password);

                    // Assign role to the user
                    if (result.Succeeded)
                    {

                        //SignInManager<ApplicationUser> _signInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
                        //await _signInManager.SignInAsync(user, isPersistent: false);

                      //  await _userManager.AddToRoleAsync(user, role);
                    }
                }
                    #endregion

                    #region  Committee Roles
                    if (!context.CommitteeUserTypes.Any())
                {
                    var committusertypes = new CommitteeUserType()
                    {
                        Type = "Chairman",
                    };
                    var committusertypes1 = new CommitteeUserType()
                    {
                        Type = "Secretary",
                    };
                    var committusertypes2 = new CommitteeUserType()
                    {
                        Type = "Member",
                    };
                    var committusertypes3 = new CommitteeUserType()
                    {
                        Type = "Visitor",
                    };
                    unitOfWork.CommitteeUserTypes.Add(committusertypes);
                    unitOfWork.CommitteeUserTypes.Add(committusertypes1);
                    unitOfWork.CommitteeUserTypes.Add(committusertypes2);
                    unitOfWork.CommitteeUserTypes.Add(committusertypes3);
                    unitOfWork.Save();
                }
                #endregion


                #endregion
            }
        }
    }
}
