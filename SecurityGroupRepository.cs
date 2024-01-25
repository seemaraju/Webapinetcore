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
using Org.BouncyCastle.Asn1.Cms;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using DocumentFormat.OpenXml.Spreadsheet;
using Attribute.Data.Extension;
using System.Security.Claims;

namespace Attribute.Data.Repository
{
    public class SecurityGroupRepository : IUser
    {
        static string userName = System.Environment.UserName;
        static string domainName = System.Environment.UserDomainName;
        static string currentDomain = Domain.GetCurrentDomain().ToString();



        /// <summary>
        /// Gets a list of all Active Directory Groups the specified username is a member of.
        /// </summary>
        /// <param name="userName">The username of the user in DOMAIN\Username format</param>
        /// <param name="domain">The name of the domain server, e.g., server.domain.com (not DOMAIN or DOMAIN.COM or SERVER)</param>
        /// <returns></returns>
        public  List<User> GetUserGroups()
        {
        
           

            User user = new User();
            List<User> userlist = new List<User>();
            int i = 0;
            PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainName);
            UserPrincipal userPc = UserPrincipal.FindByIdentity(pc, userName);

       var department= AccountManagementExtensions.GetDepartment(userPc);
            








            if (userPc == null) return null;

            var src = userPc.GetGroups(pc);
            var result = new List<string>();
            src.ToList().ForEach(sr => result.Add(sr.SamAccountName));            
            foreach (var item in result)
            {
                user = new User();
                user.UserName = userName;
                user.ADGroup = result[i];
                userlist.Add(user);
                i = i + 1;
            }
                return userlist;
        }



        
              public List<User> GetADGroupsBasedonUserName(string useranmeforadgroup)
        {
            User user = new User();
            List<User> userlist = new List<User>();
            int i = 0;
            try
            {
            
                PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainName);
                UserPrincipal userPc = UserPrincipal.FindByIdentity(pc, useranmeforadgroup);

                if (userPc == null) return null;

                var src = userPc.GetGroups(pc);
                var result = new List<string>();
                src.ToList().ForEach(sr => result.Add(sr.SamAccountName));
                foreach (var item in result)
                {
                    user = new User();
                    user.UserName = userName;
                    user.ADGroup = result[i];
                    userlist.Add(user);
                    i = i + 1;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {

            }
           
            return userlist;
        }


        public List<User> GetAllADGroupName()
        {
            List<User> AllADGroupNamelist = new List<User>();
            try
            {
                int j = 0;
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domainName);             
                GroupPrincipal qbeGroup = new GroupPrincipal(ctx);              
                PrincipalSearcher principalsrch = new PrincipalSearcher(qbeGroup);
                var listadgroupnames = principalsrch.FindAll().ToList();
                // find all matches
                foreach (var found in listadgroupnames)
                {
                    GroupPrincipal foundGroup = found as GroupPrincipal;
                    if (foundGroup != null)
                    {
                            User alladgroup = new User();
                            alladgroup.ADGroup = found.Name;
                            AllADGroupNamelist.Add(alladgroup);
                            j = j + 1;
                        
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally { 
            
            }
            return AllADGroupNamelist;
        }
            public List<User> GetUserBasedonADGroupName(string adgroupname)
        {
            List<User> UserBasedonADGroupNamelist = new List<User>();
            int i = 0;
            //var groupname = "TJXU-Jira-IA-Developer";
            try
            {
            using (var context = new PrincipalContext(ContextType.Domain, currentDomain))
            {
                using (var group = GroupPrincipal.FindByIdentity(context, adgroupname))
                {
                    if (group == null)
                    {
                        
                    }
                    else
                    {
                        var users = group.GetMembers(true);
                        var totalcount = users.Count();
                        foreach (UserPrincipal userlst in users)
                        {
                            User clsuser = new User();
                            clsuser.UserName = userlst.Name;

                            UserBasedonADGroupNamelist.Add(clsuser);
                            i = i + 1;
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
               
            }

            return UserBasedonADGroupNamelist;
        }


            public  List<User> GetAdUser(IIdentity identity)
        {
            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain);
                UserPrincipal principal = new UserPrincipal(context);
                if (context != null)
                {
                    principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, identity.Name);
                }
                //return AdUser.CastToAdUser(principal);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving AD User", ex);
            }
        }

        public List<User> GetAdUserSameAccount(string samAccountName)
        {

            List<User> logins = new List<User>();
            PrincipalContext context = new PrincipalContext(ContextType.Domain);
            UserPrincipal principal = new UserPrincipal(context);
            if (context != null)
            {
                principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, samAccountName);
            }
            return logins;
                //Login.CastToAdUser(principal);
        }

        public List<User> GetAdUserFromGuid(Guid guid)
        {
            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain);
                UserPrincipal principal = new UserPrincipal(context);
                if (context != null)
                {
                    principal = UserPrincipal.FindByIdentity(context, IdentityType.Guid, guid.ToString());
                }
                //return AdUser.CastToAdUser(principal);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving AD User", ex);
            }
        }

      public  List<User> GetDomainUsers()
        {
            List<User> a = new List<User>();
            String currentDomain = Domain.GetCurrentDomain().ToString();
            PrincipalContext context = new PrincipalContext(ContextType.Domain, currentDomain);
            UserPrincipal principal = new UserPrincipal(context);
            principal.UserPrincipalName = "*@*";
            principal.Enabled = true;
            PrincipalSearcher searcher = new PrincipalSearcher(principal);
            //var b = searcher.FindAll().ToList();
            var c = searcher.FindAll().AsQueryable().Cast<UserPrincipal>().ToList();
          //  var users = searcher.FindAll().Take(50).AsQueryable().Cast<UserPrincipal>().FilterUsers().SelectAdUsers().OrderBy(x => x.Surname).ToList();
           var users = searcher.FindAll().Take(50).AsQueryable().Cast<UserPrincipal>().ToList();
            return a;
           
        }



         public List<User> FindDomainUser(string search)
        {
            List<User> users = new List<User>();
            PrincipalContext context = new PrincipalContext(ContextType.Domain);
            UserPrincipal principal = new UserPrincipal(context);
            principal.SamAccountName = search;
            principal.Enabled = true;
            PrincipalSearcher searcher = new PrincipalSearcher(principal);
            //var users = searcher.FindAll().AsQueryable().Cast<UserPrincipal>().FilterUsers().SelectAdUsers().OrderBy(x => x.Surname).ToList();
            var users1 = searcher.FindAll().AsQueryable().Cast<UserPrincipal>().ToList();
            return users;
           
        }


        public List<User> getGroupsWithUsers()
        {
            List<User> userwithgrouplist = new List<User>();
            String currentDomain = Domain.GetCurrentDomain().ToString();

            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, currentDomain))
            {
                using (PrincipalSearcher searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {

                    foreach (var result in searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                        //var SID = WindowsIdentity.GetCurrent().User.ToString();
                        //var group1 = WindowsIdentity.GetCurrent().Groups.ToList();

                        // find a user
                        UserPrincipal user = UserPrincipal.FindByIdentity(context, de.Properties["samAccountName"].Value.ToString());

                        if (user != null)
                        {
                            int i = 0;
                            // get the user's groups
                            var groups = user.GetAuthorizationGroups();

                            foreach (GroupPrincipal group in groups)
                            {
                                Console.WriteLine("User: " + user + " is in Group: " + group);

                                User  user1 = new User();
                                    user1.UserName = user.Name;
                                    user1.ADGroup = group.SamAccountName;
                                userwithgrouplist.Add(user1);
                                    i = i + 1;
                               
                            }


                        }
                    }
                }
            }


            return userwithgrouplist;
        }

        public bool AddUserToGroup(string adduserName, string groupName)
        {
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, currentDomain))
                {
                    GroupPrincipal addUserToGroup = GroupPrincipal.FindByIdentity(pc, groupName);
                    addUserToGroup.Members.Add(pc, IdentityType.UserPrincipalName, adduserName);
                    addUserToGroup.Save();
                    return true;
                }
            }
            catch (DirectoryServicesCOMException ex)
            {
                ex.Message.ToString();
                return false;
            }
            finally { 
            
            }
        }

        public bool RemoveUserFromGroup(string removeusername, string groupName)
        {
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, currentDomain))
                {
                    GroupPrincipal removeUserFromGroup = GroupPrincipal.FindByIdentity(pc, groupName);
                    removeUserFromGroup.Members.Remove(pc, IdentityType.UserPrincipalName, removeusername);
                    removeUserFromGroup.Save();
                    return true;
                }
            }
            catch (DirectoryServicesCOMException E)
            {
                E.Message.ToString();
                return false;
            }
        }


        public string CreateADGroup(string groupname)
        {
            try
            {
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, currentDomain, "OU=Mail Users,OU=Users,OU=Trade Secret,DC=corp,DC=tjxcorp,DC=net"))
                {
                    // create a new group principal, give it a name
                    GroupPrincipal group = new GroupPrincipal(ctx, groupname);

                    // optionally set additional properties on the newly created group here....

                    // save the group 
                    group.Save();
                    return "Success";
                }
            }
            catch (Exception E)
            {

                E.Message.ToString();
                return "UnSuccessful";
            }
           
           
          
        }





    }
}
