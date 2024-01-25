using Attribute.Data.Interface;
using Attribute.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using Microsoft.AspNetCore.Authentication;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.DirectoryServices.ActiveDirectory;
using Attribute.Data.Extension;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Bibliography;

namespace Attribute.Data.Repository
{
    public class LoginRepository : ILogin
    {
        static string userName = System.Environment.UserName;
        static string domainName = System.Environment.UserDomainName;
        public bool GetUserNamePassword(string username,string? passowrd)
        {
            bool bResult = false;
            try
            {
                PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainName);
                 bResult = pc.ValidateCredentials(username, passowrd);
                

               
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }
            finally { 
            
            }
            return bResult;
        }
        
         public List<Login> ValidateWindowsLogin(string tokenstring)
        {
          Login result = null;
            List<Login> Logindata = new List<Login>();
            try {
                string domainName;
                domainName = System.Environment.UserDomainName;
                
                bool isUserExpired;
                bool isAccountLocked;

                using (var context = new PrincipalContext(ContextType.Domain, domainName))
                {
                    var user = UserPrincipal.FindByIdentity(context, System.Environment.UserName);
                    if (user != null)
                    {
                        var department = AccountManagementExtensions.GetDepartment(user);

                        if (user.AccountExpirationDate != null)
                        {
                            isUserExpired= false;
                        }
                        else
                        {
                            isUserExpired= true;
                        }
                        if (user.IsAccountLockedOut != null)
                        {
                            isAccountLocked = false;
                        }
                        else
                        {
                            isAccountLocked = true;
                        }
                        result = new Login
                        {
                            UserName = System.Environment.UserName,
                            FirstName = user.GivenName,
                            LastName = user.Surname,
                            Department = department,
                            IsUserExpired = isUserExpired,
                            IsUserExisiting = true,
                            IsAccountLocked = isAccountLocked,
                            Token = tokenstring
                            


                        };
                        Logindata.Add(result);
                    }

                    
                }
                return Logindata;

            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {

            }
            return Logindata;
        }


        
    }
}
