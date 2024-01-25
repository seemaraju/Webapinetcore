using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute.Data.Models;
using Microsoft.AspNetCore.Http;

namespace Attribute.Data.Interface
{
    public interface IAudit
    {
        
            List<Audit> GetAuditHeaders();
        List<Audit> GetAuditDetails();
      
        
    }
}
