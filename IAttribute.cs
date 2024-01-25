using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute.Data.Models;
using Microsoft.AspNetCore.Http;

namespace Attribute.Data.Interface
{
    public interface IAttribute
    {
        
            List<AttributeType> GetAttributeType();
        List<AttributeStatus> GetAttributeStatusByAttributeType(int id);
       void  UpdatePOAData(List<Attributes> Attributes, string TableName);

        //Attributes UploadFile(IFormFile postedFile);
        //List<Attributes> ReadExcelFile(IFormFile file);
        List<Attributes> UploadFile(IFormFile file);
        bool Commitdata(List<Attributes> savedata);
        Task<bool> Commitdatasaved(List<Attributes> savedata);


    }
}
