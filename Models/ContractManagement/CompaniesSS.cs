using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCMAPI.Models.ContractManagement
{
    public class CompaniesSSModel
    {
        public CompaniesSSModel()
        {
            companies = new List<CompaniesSS>();
        }
        List<CompaniesSS> companies { get; set; }        
    }

    public class CompaniesSS
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyName2L { get; set; }
        public int Blocked { get; set; }
    }
}