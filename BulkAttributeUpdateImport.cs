using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attribute.Data.Models
{
    [Table("Bulk_Attribute_Update_Import")]
    [Keyless]
    public class BulkAttributeUpdateImport
    {
        public string? PO_No { get; set; }
        public string? NewAttributeCode { get; set; }
        public string? NewAttributeSetCode { get; set; }
        public string? TJXUser { get; set; }
        public string? BatchID { get; set; }
    }
}
