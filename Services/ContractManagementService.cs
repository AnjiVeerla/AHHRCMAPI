using RCMAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RCMAPI.DAL;
using RCMAPI.Models.ContractManagement;
using System.Threading.Tasks;

namespace RCMAPI.Services
{
    public class ContractManagementService
    {
        public async Task<(int,string)> SaveCompany(Company company)
        {
            ContractManagementDAL objContractMgmtDal = new ContractManagementDAL();
            var returnValue = await objContractMgmtDal.SaveCompany(company);
            return (returnValue.Item1, returnValue.Item2);
        }
        public Company FetchCompany(int id)
        {
            ContractManagementDAL objContractMgmtDal = new ContractManagementDAL();
            Company returnValue = objContractMgmtDal.FetchCompany(id);
            return returnValue;
        }

        public List<Company> FetchAllCompanies()
        {
            ContractManagementDAL objContractMgmtDal = new ContractManagementDAL();
            List<Company> returnValue = objContractMgmtDal.FetchAllCompanies();
            return returnValue;
        }

        public List<CompaniesSS> FetchCompaniesSS(string value)
        {
            ContractManagementDAL objContractMgmtDal = new ContractManagementDAL();
            List<CompaniesSS> returnValue = objContractMgmtDal.FetchCompaniesSS(value);
            return returnValue;
        }
    }
}