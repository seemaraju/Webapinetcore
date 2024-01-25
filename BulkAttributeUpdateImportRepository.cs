using Attribute.Data.Entity;
using Attribute.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attribute.Data.Repository
{
    public class BulkAttributeUpdateImportRepository
    {
        private readonly AppDbContext context;
        public BulkAttributeUpdateImportRepository(AppDbContext _context)
        {
            this.context = _context;
        }
        public bool Commitdatasaved(List<Attributes> savedata)
        {
            try
            {
                BulkAttributeUpdateImport clsbulkattributeupdate = new BulkAttributeUpdateImport();


                foreach (var value in savedata)
                {
                    clsbulkattributeupdate.PO_No = value.PONumber;
                    clsbulkattributeupdate.NewAttributeSetCode = value.AttributeStatus;
                    clsbulkattributeupdate.NewAttributeCode = value.AttributeType;
                    clsbulkattributeupdate.TJXUser = "Test";


                    context.BulkAttributeUpdateImport.Add(clsbulkattributeupdate);
                }
                context.SaveChanges();
                return true;


                //context.BulkAttributeUpdateImport.Add(savedata);
                //context.SaveChanges();


            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return true;

        }
    }
}
