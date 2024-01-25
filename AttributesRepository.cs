using Attribute.Data.Interface;
using Attribute.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;

using System.Net;

using DataTable = System.Data.DataTable;
using System.Reflection;

using System.Data;

using System.IO;

using GemBox.Spreadsheet;

using ClosedXML.Excel;
using Attribute.Data.Entity;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Office.Interop.Excel;

namespace Attribute.Data.Repository
{
    public class AttributesRepository : IAttribute
    {
        private readonly AppDbContext _dbContext;

        public AttributesRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        List<AttributeType> lisattributes = new List<AttributeType>
        {
            new AttributeType{AttributeTypeId=1, AttributeTypeCode="SHIPST" },
            new AttributeType{AttributeTypeId=2, AttributeTypeCode="SHIPSR" },
            new AttributeType{AttributeTypeId=3, AttributeTypeCode="SHINST" },
            new AttributeType{AttributeTypeId=4, AttributeTypeCode="SHIKST" },
            new AttributeType{AttributeTypeId=5, AttributeTypeCode="SHYPST" },
            new AttributeType{AttributeTypeId=6, AttributeTypeCode="SBIPST" },
        };

        List<AttributeStatus> lisattributestatus = new List<AttributeStatus>
        {
            new AttributeStatus{AttributeStatusId=1,  AttributeStatusCode="0DEPPD" ,AttributeTypeId=1 },
            new AttributeStatus{AttributeStatusId=2,  AttributeStatusCode="0DEPPD" ,AttributeTypeId=2},
            new AttributeStatus{AttributeStatusId=3,  AttributeStatusCode="1ITWHS",AttributeTypeId=5 },
            new AttributeStatus{AttributeStatusId=4,  AttributeStatusCode="1ITWHS",AttributeTypeId=1 },
            new AttributeStatus{AttributeStatusId=5, AttributeStatusCode="5REORG" ,AttributeTypeId=6 },
            new AttributeStatus{AttributeStatusId=6,  AttributeStatusCode="5REORG" ,AttributeTypeId=4},
             new AttributeStatus{AttributeStatusId=7,  AttributeStatusCode="8REORG" ,AttributeTypeId=1},
        };


        public List<AttributeType> GetAttributeType()
        {
            try
            {
                return lisattributes.ToList();
            }
            catch
            {
                throw new NotImplementedException();
            }
            finally
            {
            }

        }

        public List<AttributeStatus> GetAttributeStatusByAttributeType(int id)
        {

            try
            {
                return lisattributestatus.Where(y => y.AttributeTypeId == id).ToList();


            }
            catch
            {
                throw new NotImplementedException();
            }
            finally
            {
            }

        }

        public void UpdatePOAData(List<Attributes> list, string TableName)
        {
            DataTable dt = new DataTable("MyTable");
            dt = ConvertToDataTable(list);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolSoulDataEntitiesForReport"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("", conn))
                {
                    try
                    {
                        conn.Open();

                        //Creating temp table on database
                        command.CommandText = "CREATE TABLE #TmpTable(...)";
                        command.ExecuteNonQuery();

                        //Bulk insert into temp table
                        using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                        {
                            bulkcopy.BulkCopyTimeout = 660;
                            bulkcopy.DestinationTableName = "#TmpTable";
                            bulkcopy.WriteToServer(dt);
                            bulkcopy.Close();
                        }

                        // Updating destination table, and dropping temp table
                        command.CommandTimeout = 300;
                        command.CommandText = "UPDATE T SET ... FROM " + TableName + " T INNER JOIN #TmpTable Temp ON ...; DROP TABLE #TmpTable;";
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Handle exception properly
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public List<Attributes> UploadFile(IFormFile postedFile)
        {
            List<Attributes> lstAttributes = new List<Attributes>();
            var path = @"C:\Users\see00166\Project\";
            string fileName = Path.GetFileName(postedFile.FileName);
            string filePath = Path.Combine(path, fileName);

            string extension = Path.GetExtension(filePath);
            string connString = string.Empty;
            var conString = string.Empty;
            DataTable dtxlscsv = new DataTable();
            string strconnString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
            switch (extension)
            {
                case ".xls": //Excel 97-03.
                             //  connString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                             // conString = string.Format(connString, filePath);
                    conString = string.Format(strconnString, filePath);

                    dtxlscsv = ConvertXLStoDataTable(conString);
                    lstAttributes = ConvertDatatableIntoList(dtxlscsv);

                    // lstAttributes =ConvertDataTable<lstAttributes>(dtxlscsv);
                    break;
                case ".xlsx": //Excel 07 and above.
                              //connString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;


                    // Create new XLSX file.
                    var xlsxFile = new ExcelFile();

                    // Load data from XLSX file.
                    xlsxFile.LoadXlsx(fileName + ".xls", XlsxOptions.PreserveMakeCopy);

                    // Save XLSX file to XLS file.
                    xlsxFile.SaveXls(fileName + ".xls");

                    string filePath1 = Path.Combine(path, fileName);
                    conString = string.Format(strconnString, filePath1);

                    dtxlscsv = ConvertXLStoDataTable(conString);
                    break;
                case ".csv":
                    dtxlscsv = ConvertCSVtoDataTable(filePath);
                    break;
            }
            return lstAttributes;
        }
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dte = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dte.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dte.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dte.Rows.Add(dr);
                    }
                }
            }

            return dte;
        }

        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            //OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dtt = new DataTable();
            try
            {









                //oledbConn.Open();
                //using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
                //{
                //    OleDbDataAdapter oleda = new OleDbDataAdapter();
                //    oleda.SelectCommand = cmd;
                //    DataSet ds = new DataSet();
                //    oleda.Fill(ds);

                //    dtt = ds.Tables[0];
                //}
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {

                //oledbConn.Close();
            }

            return dtt;

        }
        public static DataTable ConvertXLStoDataTable(string xlsconnString)
        {
            DataTable dtxlssheetdata = new DataTable();
            try
            {
                using (OleDbConnection connExcel = new OleDbConnection(xlsconnString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            if (connExcel.State == ConnectionState.Closed)
                            {
                                connExcel.Open();
                            }

                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dtxlssheetdata);
                            connExcel.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {


            }

            return dtxlssheetdata;

        }
        public List<Attributes> ConvertDatatableIntoList(DataTable dt)
        {
            int i = 1;
            var convertedList = (from rw in dt.AsEnumerable()
                                 select new Attributes()
                                 {
                                     PONumber = rw["PONumber"].ToString(),
                                     AttributeStatus = rw["AttributeStatus"].ToString(),
                                     AttributeType = rw["AttributeType"].ToString(),
                                     AttributeId = dt.Rows.Count > 0 ? i++ : 1

                                 }).ToList();

            return convertedList;
        }


        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                list.Add(item);
            }
            return list;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }



        public bool Commitdata(List<Attributes> savedata)
        {

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Users");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "PO Number";
                    worksheet.Cell(currentRow, 2).Value = "Attribute Type";
                    worksheet.Cell(currentRow, 3).Value = "Attribute Status";
                    foreach (var sd in savedata)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = sd.PONumber;
                        worksheet.Cell(currentRow, 2).Value = sd.AttributeType;
                        worksheet.Cell(currentRow, 3).Value = sd.AttributeStatus;
                    }
                    workbook.SaveAs("PO.xlsx");

                }
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
            finally
            {

            }

        }

        public DataTable ConvertToDataTables<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        public async Task<bool> Commitdatasaved(List<Attributes> attributes)
        {
            try
            {
                 List<BulkAttributeUpdateImport> batchid = await GetBatchID();
                if (batchid[0].BatchID == "")
                {
                    batchid[0].BatchID = "1";
                }
                else
                {
                    int incrementbatchid = Convert.ToInt32(batchid[0].BatchID) + 1;
                    batchid[0].BatchID = incrementbatchid.ToString();
                }
               
                if (batchid != null)
                {
                
                for (int i = 0; i < attributes.Count; i++)
                {

                    var parameter = new List<SqlParameter>();
                    parameter.Add(new SqlParameter("@PO_No", attributes[i].PONumber));
                    parameter.Add(new SqlParameter("@NewAttributeCode", attributes[i].AttributeStatus));
                    parameter.Add(new SqlParameter("@NewAttributeSetCode", attributes[i].AttributeType));
                    parameter.Add(new SqlParameter("@TJXUser", "Test"));
                    parameter.Add(new SqlParameter("@BatchID", batchid[0].BatchID));

                    var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRaw(@"exec InsertBulkAttributeUpdateImport @PO_No, @NewAttributeCode, @NewAttributeSetCode, @TJXUser,@BatchID", parameter.ToArray()));
                }
               
            }
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }

        }

        public async Task<List<BulkAttributeUpdateImport>> GetBatchID1()
        {
            return await _dbContext.BulkAttributeUpdateImport
                .FromSqlRaw<BulkAttributeUpdateImport>("GetBatchID")
                .ToListAsync();
        }

        public async Task<List<BulkAttributeUpdateImport>> GetBatchID()
        {
            List<BulkAttributeUpdateImport> lstBatchid = new List<BulkAttributeUpdateImport>();
            var conString = _dbContext.Database.GetDbConnection();
            using (SqlConnection con = new SqlConnection(conString.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetBatchID", con);
                cmd.CommandType = CommandType.StoredProcedure;             
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    BulkAttributeUpdateImport bulkAttributeUpdateImport = new BulkAttributeUpdateImport();

                    bulkAttributeUpdateImport.BatchID = rdr["BatchID"].ToString();

                    lstBatchid.Add(bulkAttributeUpdateImport);
                }
                con.Close();
            }
            return lstBatchid;
        }


    }
    
}  

    


