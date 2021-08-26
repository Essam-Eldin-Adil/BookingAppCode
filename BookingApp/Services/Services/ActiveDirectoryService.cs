using Data.Models;
using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Services.Models;
using System.Linq;

namespace Services
{
    public static class ActiveDirectoryService
    {
        public static bool IsAuthenticated(HttpContext httpContext, string email, string password)
        {
            var result = false;
            try
            {
                var activeDirectoryInfo = GetActiveDirectoryInfo(httpContext);
                if(!string.IsNullOrEmpty(activeDirectoryInfo.Domain) && !string.IsNullOrEmpty(activeDirectoryInfo.UserName) && !string.IsNullOrEmpty(activeDirectoryInfo.Password))
                {
                    var searchResult = DirectorySearcher(httpContext, email);
                    if (searchResult != null)
                    {
                        if (searchResult.GetDirectoryEntry().Properties["samaccountname"].Value != null)
                        {
                            using (var pc = new PrincipalContext(ContextType.Domain, activeDirectoryInfo.Domain))
                            {
                                var username = searchResult.GetDirectoryEntry().Properties["samaccountname"].Value.ToString();
                                var isValid = pc.ValidateCredentials(username, password);
                                if (isValid)
                                {
                                    result = true;
                                }
                            }
                        }
                    }
                }                       
            }
            catch
            {

            }
            return result;
        }

        public static User GetUserInfo(HttpContext httpContext, string email)
        {
            var user = new User();
            try
            {
                var searchResult = DirectorySearcher(httpContext,email);
                if (searchResult != null)
                {
                    user.UserName = email;
                    user.Email = email;
                    user.Name = searchResult.GetDirectoryEntry().Properties["cn"].Value?.ToString();
                    //user.EmployeeNumber = searchResult.GetDirectoryEntry().Properties["physicalDeliveryOfficeName"].Value?.ToString();
                    //user.IsActive = true;
                }
            }
            catch
            {

            }
            return user;
        }

        private static SearchResult DirectorySearcher(HttpContext httpContext, string email)
        {
            try
            {
                DirectorySearcher dirSearch = null;
                var activeDirectoryInfo = GetActiveDirectoryInfo(httpContext);
                dirSearch = new DirectorySearcher(new DirectoryEntry("LDAP://" + activeDirectoryInfo.Domain, activeDirectoryInfo.UserName, activeDirectoryInfo.Password))
                {
                    Filter = ("mail=" + email),
                    SearchScope = SearchScope.Subtree,
                    ServerTimeLimit = TimeSpan.FromSeconds(90)
                };
                return dirSearch.FindOne();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static ActiveDirectoryInfo GetActiveDirectoryInfo(HttpContext httpContext)
        {
            var activeDirectoryInfo = new ActiveDirectoryInfo();
            var settingRepository = (IRepository<Data.Models.Setting>)httpContext.RequestServices.GetService(typeof(IRepository<Data.Models.Setting>));
            var settings = settingRepository.Table.ToList();
            activeDirectoryInfo.Domain = settings.FirstOrDefault(f => f.Key == "ADDomain")?.Value;
            activeDirectoryInfo.UserName = settings.FirstOrDefault(f => f.Key == "ADUserName")?.Value;
            activeDirectoryInfo.Password = settings.FirstOrDefault(f => f.Key == "ADPassword")?.Value;
            return activeDirectoryInfo;
        }

        
    }
}
