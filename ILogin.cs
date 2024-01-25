using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Attribute.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Attribute.Data.Interface
{
    public interface ILogin
    {
        
            bool GetUserNamePassword(string username, string passowrd);
        List<Login> ValidateWindowsLogin(string token);
        
    }
}
