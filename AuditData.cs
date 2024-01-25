using Attribute.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attribute.Data.Entity
{
    public class AuditData
    {
        public List<Audit> listauditHeaders()
        {
            List<Audit> listauditHeaders = new List<Audit>
            {
                new Audit { BatchID = 1, CreatedBy = "Seema", CreatedDate = DateTime.Today, ComittedBy = "Seema", ComittedDate = DateTime.Today, ProcessingDate = DateTime.Today,PONumber="11",AttributeType="Test1", AttributeStatus="SHIPST" },
                new Audit { BatchID = 2, CreatedBy = "Seema", CreatedDate = DateTime.Today, ComittedBy = "Seema", ComittedDate = DateTime.Today, ProcessingDate = DateTime.Today ,PONumber="12",AttributeType="Test2", AttributeStatus="SHIPSP"},
                new Audit { BatchID = 3, CreatedBy = "Seema", CreatedDate = DateTime.Today, ComittedBy = "Seema", ComittedDate = DateTime.Today, ProcessingDate = DateTime.Today ,PONumber="13",AttributeType="Test3", AttributeStatus="SHIPSQ"},
                new Audit { BatchID = 4, CreatedBy = "Seema", CreatedDate = DateTime.Today, ComittedBy = "Seema", ComittedDate = DateTime.Today, ProcessingDate = DateTime.Today ,PONumber="14",AttributeType="Test4", AttributeStatus="SHIPSE"},
                new Audit { BatchID = 5, CreatedBy = "Seema", CreatedDate = DateTime.Today, ComittedBy = "Seema", ComittedDate = DateTime.Today, ProcessingDate = DateTime.Today ,PONumber="15",AttributeType="Test5", AttributeStatus="SHIPSC"},
                new Audit { BatchID = 6, CreatedBy = "Seema", CreatedDate = DateTime.Today, ComittedBy = "Seema", ComittedDate = DateTime.Today, ProcessingDate = DateTime.Today ,PONumber="16",AttributeType="Test6", AttributeStatus="SHIPSD"}
            };
            return listauditHeaders;

        }
        public List<Audit> listauditDetails()
        {
            List<Audit> listauditDetails = new List<Audit>
             {
             new Audit{BatchID=1,PONumber="11",AttributeType="Test1", AttributeStatus="SHIPST",CreatedBy="Seema",CreatedDate=DateTime.Today,ComittedBy ="Seema" ,ComittedDate=DateTime.Today,ProcessingDate=DateTime.Today},
            new Audit{BatchID=2,PONumber="12",AttributeType="Test2", AttributeStatus="SHIPSR" ,CreatedBy="Seema",CreatedDate=DateTime.Today,ComittedBy ="Seema" ,ComittedDate=DateTime.Today,ProcessingDate=DateTime.Today},
            new Audit{BatchID=3,PONumber="13",AttributeType="Test3", AttributeStatus="SHINST",CreatedBy="Seema",CreatedDate=DateTime.Today,ComittedBy ="Seema" ,ComittedDate=DateTime.Today,ProcessingDate=DateTime.Today },
            new Audit{BatchID=4,PONumber="14",AttributeType="Test4", AttributeStatus="SHIKST" ,CreatedBy="Seema",CreatedDate=DateTime.Today,ComittedBy ="Seema" ,ComittedDate=DateTime.Today,ProcessingDate=DateTime.Today},
            new Audit{BatchID=5,PONumber="15",AttributeType="Test5", AttributeStatus="SHYPST" ,CreatedBy="Seema",CreatedDate=DateTime.Today,ComittedBy ="Seema",ComittedDate=DateTime.Today,ProcessingDate=DateTime.Today},
            new Audit{BatchID=6,PONumber="16",AttributeType="Test6", AttributeStatus="SBIPST" ,CreatedBy="Seema",CreatedDate=DateTime.Today,ComittedBy = "Seema",ComittedDate=DateTime.Today,ProcessingDate=DateTime.Today}

        };

            return listauditDetails;
        }
    }
}
