using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace Attribute.Data.Extension
{
    public static class AccountManagementExtensions
    {
        public static string GetProperty(this Principal principal, String property)
        {
            DirectoryEntry directoryEntry = principal.GetUnderlyingObject() as DirectoryEntry;
            if (directoryEntry.Properties.Contains(property))
                return directoryEntry.Properties[property].Value.ToString();
            else
                return string.Empty;
        }

        public static string GetCompany(this Principal principal)
        {
            return principal.GetProperty("company");
        }

        public static string GetDepartment(this Principal principal)
        {
            return principal.GetProperty("department");
        }
    }
}
