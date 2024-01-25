using Attribute.Data.Interface;
using Attribute.Data.Models;
using Attribute.Data.Repository;
using Microsoft.AspNetCore.Mvc;


namespace PO1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditController : ControllerBase
    {
        private IAudit Audit = new AuditRepository();



        [Route("GetAuditHeaders")]
        [HttpPost]
        public List<Audit> GetAuditHeaders()
        {
            try
            {
                return Audit.GetAuditHeaders().ToList();
            }
            catch
            {
                throw new NotImplementedException();
            }
            finally
            {
            }

        }
        [Route("GetAuditDetails")]
        [HttpPost]
        public List<Audit> GetAuditDetails()
        {

            try
            {
                return Audit.GetAuditDetails().ToList();


            }
            catch
            {
                throw new NotImplementedException();
            }
            finally
            {
            }

        }
    }
}
