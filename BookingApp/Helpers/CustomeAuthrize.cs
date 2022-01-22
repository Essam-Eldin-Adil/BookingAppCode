using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp
{
    public class Authrize
    {
        public class AuthrizeAttribute : TypeFilterAttribute
        {
            public AuthrizeAttribute(string claimValue) : base(typeof(AuthrizeFilter))
            {
                Arguments = new object[] { claimValue };
            }
        }

        public class AuthrizeFilter : IAuthorizationFilter
        {
            readonly string _roles;

            public AuthrizeFilter(string roles)
            {
                _roles = roles;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var actionRoles = _roles.Split(",");
                if (!SessionClass.IsAuthentecated(context.HttpContext))
                {
                    if (actionRoles.Contains(nameof(Enums.UserType.EndUser)))
                    {
                        context.Result = new RedirectResult("/User/Join");
                        return;
                    }
                    context.Result = new RedirectResult("/Accounts/Login");
                    return;
                }
                var user = SessionClass.GetUser(context.HttpContext);

                bool access = false;
                if (string.IsNullOrEmpty(_roles))
                {
                    access = true;
                }
                else
                {
                    switch (user.UserType)
                    {
                        case (int)Enums.UserType.Admin:
                            if (actionRoles.Contains(nameof(Enums.UserType.Admin)))
                            {
                                access = true;
                            }
                            break;
                        case (int)Enums.UserType.BookAdmin:
                            if (actionRoles.Contains(nameof(Enums.UserType.BookAdmin)))
                            {
                                access = true;
                            }
                            break;
                        case (int)Enums.UserType.EndUser:
                            if (actionRoles.Contains(nameof(Enums.UserType.EndUser)))
                            {
                                access = true;
                            }
                            break;
                        case (int)Enums.UserType.BookUser:
                            if (actionRoles.Contains(nameof(Enums.UserType.BookUser)))
                            {
                                access = true;
                            }
                            break;
                    }
                    
                }
                if (!access)
                {
                    context.Result = new RedirectResult("/Home/Index");
                    return;
                }
            }
        }
    }
}
