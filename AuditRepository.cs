using Attribute.Data.Interface;
using Attribute.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using DataTable = System.Data.DataTable;
using System.Data;
using Attribute.Data.Entity;



//using DocumentFormat.OpenXml.Spreadsheet;

namespace Attribute.Data.Repository
{
    public class AuditRepository : IAudit
    {

        AuditData auditdata = new AuditData();
       

        public List<Audit> GetAuditDetails()
        {
            
            return auditdata.listauditDetails();

        }

        public List<Audit> GetAuditHeaders()
        {
            return  auditdata.listauditHeaders();
        }
    }
}  

    


