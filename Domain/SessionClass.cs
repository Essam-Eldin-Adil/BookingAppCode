using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public abstract class SessionClass
{

    public static User Identity { get; set; }

    public static void Clear(HttpContext httpContext)
    {
        httpContext.Session.Clear();
    }
    public static bool IsAuthentecated(HttpContext httpContext)
    {
        var json = httpContext.Session.GetString("User");
        if (string.IsNullOrEmpty(json))
        {
            return false;
        }
        return true;
    }
    public static User GetUser(HttpContext httpContext)
    {
        var json = httpContext.Session.GetString("User");
        if (string.IsNullOrEmpty(json))
        {
            return new User();
        }
        return JsonConvert.DeserializeObject<User>(json);
    }

    public static void Remove(HttpContext httpContext)
    {
        httpContext.Session.Remove("User");
    }

    public static void SetUser(HttpContext httpContext,User user)
    {
        httpContext.Session.SetString("User",JsonConvert.SerializeObject(user));
    }
}