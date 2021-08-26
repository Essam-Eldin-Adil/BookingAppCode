using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Microsoft.AspNetCore.Http;
public abstract class SessionClass
{

    public static User Identity { get; set; }

    public static void Clear(HttpContext httpContext)
    {
        httpContext.Session.Clear();
    }


}