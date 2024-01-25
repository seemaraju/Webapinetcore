using Microsoft.AspNetCore.Http;
using Attribute.Data.Interface;
using Attribute.Data.Models;
using Attribute.Data.Repository;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Http.Cors;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;

namespace PO1.Controllers
{
    
    [ApiController]
    [Route("[controller]")]    
    public class SecurityGroupController : ControllerBase
    {
        // GET: AttributeController
        private IUser user = new SecurityGroupRepository();
        
        [Route("GetUserGroups")]
        [Authorize]
        [HttpPost]
        
        public ActionResult <List<User>> GetUserGroups()
        {
            //List<User> a = new List<User>();
            //a = user.GetUserGroups().ToList();
            return user.GetUserGroups().ToList();
           
        }        

        [Route("GetDomainUsers")]
        [HttpPost]
        public ActionResult<List<User>> GetDomainUsers()
        {
            //List<User> a = new List<User>();
            //a = user.GetDomainUsers().ToList();
            return user.GetDomainUsers().ToList();

        }
        [Route("GetGroupsWithUsers")]
        [HttpPost]
        public ActionResult<List<User>> GetGroupsWithUsers()
        {
            //List<User> a = new List<User>();
            //a = user.GetDomainUsers().ToList();
            return user.getGroupsWithUsers().ToList();

        }
        [Route("GetUserBasedonADGroupName/{adgroupname}")]
        [HttpPost]
        public ActionResult<List<User>> GetUserBasedonADGroupName(string adgroupname)
        {
            return user.GetUserBasedonADGroupName(adgroupname).ToList();

        }


        [Route("GetADGroupsBasedonUserName/{useranmeforadgroup}")]
        [HttpPost]
        public ActionResult<List<User>> GetADGroupsBasedonUserName(string useranmeforadgroup)
        {
            return user.GetADGroupsBasedonUserName(useranmeforadgroup).ToList();

        }


        [Route("GetAllADGroupName")]
        [Authorize]
        [HttpPost]
        public ActionResult<List<User>> GetAllADGroupName()
        {
            return user.GetAllADGroupName().ToList();

        }

        
        [Route("AddUserToGroup/{adduserName}/{groupName}")]
        [HttpPost]
        public bool AddUserToGroup(string adduserName, string groupName)
        {
            return user.AddUserToGroup(adduserName, groupName);

        }
        [Route("CreateADGroup/{groupname}")]
        [HttpPost]
        public string CreateADGroup(string groupname)
        {
            return user.CreateADGroup(groupname);

        }


    }
}
