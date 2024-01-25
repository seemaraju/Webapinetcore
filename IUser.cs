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
    public interface IUser
    {
        
          
        List<User> GetUserGroups();


        List<User> getGroupsWithUsers();
        List<User> GetAdUser(IIdentity identity);
        List<User> GetAdUserSameAccount(string samAccountName);
        List<User> GetAdUserFromGuid(Guid guid);
        List<User> GetDomainUsers();
        List<User> FindDomainUser(string search);
        List<User> GetUserBasedonADGroupName(string adgroupname);
        List<User> GetAllADGroupName();
        List<User> GetADGroupsBasedonUserName(string useranmeforadgroup);
        bool AddUserToGroup(string adduserName, string groupName);

        string CreateADGroup(string groupname);

    }
}
