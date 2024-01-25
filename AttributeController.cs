using Microsoft.AspNetCore.Http;
using Attribute.Data.Interface;
using Attribute.Data.Models;
using Attribute.Data.Repository;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Http.Cors;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting.Server;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
//using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Net;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
using Attribute.Data.Entity;

namespace PO1.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AttributeController : ControllerBase
    {

        // GET: AttributeController
        ////private IHostingEnvironment Environment;
        //private IAttribute Attributes = new AttributesRepository();

        //private readonly IConfiguration _configuration;
        private readonly IAttribute _attributeService;

        public AttributeController(IAttribute attributeService)
        {
            this._attributeService = attributeService;
        }

        //string sqlConnectionString = "";
        //public AttributeController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    sqlConnectionString = _configuration.GetConnectionString("POAttributeUpdateConnection");
        //}
      

        [Route("AttributeTypeDetails")]
        [HttpGet]
        public ActionResult<IEnumerable<AttributeType>> BindAttributeType()
        {
            return _attributeService.GetAttributeType();
        }
        [Route("AttributeStatusDetails/{attributetypeId}")]
        [HttpGet]
        public ActionResult<List<AttributeStatus>> BindAttributeStatusByAttributeType(int attributetypeId)
        {
            return _attributeService.GetAttributeStatusByAttributeType(attributetypeId).ToList();
        }
        [Route("UpdatePOAData/{attributetypeId}")]
        [HttpPost]
        public ActionResult UpdatePOAData()
        {
            return null;
            //return Attributes.UpdatePOAData(Attributes,"Attributetype").ToList();
        }
        [Route("ReadExcelFile/{file}")]
        [HttpPost]
        public List<Attributes> ReadExcelFile(IFormFile postedFile)
        {
          return _attributeService.UploadFile(postedFile).ToList();
           //return Attributes.ReadExcelFile(postedFile).ToList();
            
        }
        [Route("Commitdata/{savedata}")]
        [HttpPost]
        public async Task<ActionResult<bool>> CommitdataAsync(List<Attributes> savedata)
        {


            //bool Commitdata = Attributes.Commitdatasaved(savedata, sqlConnectionString);
            bool Commitdata = await _attributeService.Commitdatasaved(savedata);


            if (Commitdata == true)
            {
                return true;
            }
            return false;
           

        }

        
    }
       

    }

