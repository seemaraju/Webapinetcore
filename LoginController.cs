using Microsoft.AspNetCore.Http;
using Attribute.Data.Interface;
using Attribute.Data.Models;
using Attribute.Data.Repository;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Http.Cors;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PO1.Models;
using static System.Net.WebRequestMethods;
using DocumentFormat.OpenXml.Spreadsheet;

namespace PO1.Controllers
{
    
    [ApiController]
    [Route("[controller]")]    
    public class LoginController : ControllerBase
    {
        // GET: AttributeController
        private ILogin Login = new LoginRepository();
        [Route("ValidateUserNamePassword/{username}/{password}")]
        [HttpGet]
        public bool GetUserNamePassword(string username, string password)
        {
            bool result =  Login.GetUserNamePassword(username, password);
            return result;
        }
       

        [Route("WindowsLogin")]
        [HttpGet]
        public List<Login> WindowsLogin()
        {
           string token="";
            var generatedtoken = GenerateToken();
          
                var tokenvalue = ((ObjectResult)generatedtoken).Value;
                token = Convert.ToString(tokenvalue);
            
           
            List<Login> result = Login.ValidateWindowsLogin(token);
            return result;
        }

       


        [HttpPost, Route("LoginWithUserNamePassword/{username}/{password}")]
        public List<Login> LoginWithUserNamePassword(string username, string password)
        {
            LoginModel model = new LoginModel();
            List<Login> result = new List<Login>();
            string token = "";
            model.UserName = username;
            model.Password= password;

            bool validusernamepasswordresult = Login.GetUserNamePassword(username, password);


            if (model == null)
            {
                result = new List<Login>
                {
                    new Login { ErrorStatusCode = "Invalid client request" }
                };
               
               
            }
          
            if(validusernamepasswordresult == true)
            {
              
              var generatedtoken =  GenerateToken();

              if (((ObjectResult)generatedtoken).StatusCode == 200)
                {
                    var tokenvalue = ((ObjectResult)generatedtoken).Value;
                    token = Convert.ToString(tokenvalue);
                }

                result = Login.ValidateWindowsLogin(token);
                result[0].ErrorStatusCode= "OK";
              


            }
            else
            {
                result = new List<Login>
                {
                    new Login { ErrorStatusCode = "Unauthorized" }
                };
              
            }

            return result;
        }

        [HttpPost, Route("GenerateToken")]
        public IActionResult GenerateToken()
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Su*p9e_r+S2e/c4r6e7t*0K/e8y"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:44369/",
                audience: "https://localhost:44369",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            /* return Ok(new { Token = tokenString })*/

            return Ok(tokenString);
        }



        }
}
