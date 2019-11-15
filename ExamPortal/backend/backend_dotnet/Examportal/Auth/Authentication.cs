using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
namespace Examportal.Auth
{
    public class Authentication
    {
        public Dictionary<string,string> getAllClaims(HttpContext httpContext)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            var data = httpContext.User.Identity as ClaimsIdentity;
            if(data != null)
            {
                String email = data.FindFirst("Email").Value;
                header.Add("Email",email);
            }
            return header;
        }
    }
}
