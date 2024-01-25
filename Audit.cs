using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attribute.Data.Models
{
    public class Audit
    {
        public int BatchID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ComittedBy { get; set; }
        public DateTime? ComittedDate { get; set; }
        public DateTime? ProcessingDate { get; set; }
        public string? PONumber { get; set; }
        public string? AttributeType { get; set; }
        public string? AttributeStatus { get; set; }




        
    }
}
